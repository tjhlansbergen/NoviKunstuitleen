/*
    HomeController.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 15 feb 2020
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Extensions;
using NoviKunstuitleen.Models.HomeViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using NoviKunstuitleen.Services;

namespace NoviKunstuitleen.Controllers
{
    /// <summary>
    /// MVC Controller klasse voor de Home pagina's,  alleen beschikbaar voor ingelogde gebruikers
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NoviArtDbContext _dbcontext;
        private readonly UserManager<NoviArtUser> _userManager;
        private readonly IPaymentService _paymentService;

        // constructor
        public HomeController(ILogger<HomeController> logger, NoviArtDbContext dbcontext, UserManager<NoviArtUser> userManager, IPaymentService paymentservice)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _userManager = userManager;
            _paymentService = paymentservice;

            // verwerk vestreken huurperiodes voor ieder sessie, op de achtergrond
            new Action(async () => await ReleaseArtPiecesAsync())();
        }

        /// <summary>
        /// Asynchrone methode voor het verwijderen van de lener bij kunstwerken waarvan de huurperiode verstreken is
        /// </summary>
        public async Task ReleaseArtPiecesAsync()
        {
            // haal alle kunstwerken op die beschikbaar zijn maar toch nog een lener hebben
            foreach (var artpiece in _dbcontext.NoviArtPieces.Include(a => a.Lessee).Where(a => a.AvailableFrom < DateTime.UtcNow && a.Lessee != null))
            {
                // verwijder de lener
                artpiece.Lessee = null;

                // logging
                _logger.LogInformation("Lessee automatically removed for artpiece with id: {0}", artpiece.Id);
            }
            // en verwerk in database
            await _dbcontext.SaveChangesAsync();
        }


        /// <summary>
        /// Methode voor tonen collectiepagina, haalt kunstwerken en bijbehorende gebruikers op uit database
        /// </summary>
        public IActionResult Index()
        {
            return View(new IndexViewModel(_dbcontext.NoviArtPieces.Include(a => a.Lesser).Include(a => a.Lessee).ToList()));
        }

        /// <summary>
        /// Methode voor tonen aanmaakformulier kunstwerk, alleen toegestaan voor Medewerkers
        /// </summary>
        [Authorize(Policy = "MedewerkerOnly")]
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Asynchrone methode voor toevoegen van kunstwerk uit webformulier, alleen toegstaan voor Medewerkers, asynv vanwege database calls
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "MedewerkerOnly")]
        public async Task<IActionResult> Create(CreateViewModel input)
        {
            // verwerk input vanuit webformulier
            NoviArtPiece piece = new NoviArtPiece { Artist = input.Artist, Description = input.Description, Dimensions = input.Dimensions, Frame = input.Frame, Price = input.Price, Title = input.Title };

            // voeg aanmaakdatum, beschikbaarheidsinfo, en aanbieder toe aan item
            piece.Lesser = await _userManager.GetUserAsync(HttpContext.User);
            piece.CreationDate = piece.AvailableFrom = DateTime.UtcNow;

            // upload de afbeelding
            using (var memoryStream = new MemoryStream())
            {
                // lees bestand in
                input.Image.CopyTo(memoryStream);

                // verifieer bestand
                if (!FileHelperExtensions.IsValidFile(input.Image.FileName, memoryStream, new string[] { ".gif", ".png", ".jpg", ".jpeg" }, 2097152))
                {
                    // Toon foutmelding indien afbeelding niet valide
                    ModelState.AddModelError("Image", Localization.MSG_IMAGE_FORMAT);
                }

                // controleer of er zich geen problemen hebben voorgedaan, 
                if (!ModelState.IsValid)
                {
                    return View("Create", piece);
                }

                // alles ok voeg afbeelding toe aan item
                piece.ImageContent = memoryStream.ToArray();

                // file extension opslaan
                piece.ImageType = Path.GetExtension(input.Image.FileName).ToLowerInvariant().TrimStart('.');

            }

            // Voeg resultaat toe aan de database
            _dbcontext.Add<NoviArtPiece>(piece);
            _dbcontext.SaveChanges();

            // logging
            _logger.LogInformation("User created a new artpiece with id: {0}", piece.Id);

            // en keer terug naar de collectie-pagina
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Methode voor tonen van detailpagina voor kunstwerken, asynchroon hier niet nuttig omdat gebruiker sowieso moet wachten op resutaat.
        /// </summary>
        public IActionResult Detail(int id)
        {
            var piece = _dbcontext.NoviArtPieces.Include(a => a.Lesser).Where(a => a.Id == id).FirstOrDefault();
            return View(new DetailViewModel { ArtPiece = piece });
        }


        /// <summary>
        /// Asynchrone methode voor verwerken van bestelling van kunstwerk, alleen toegestaand voor Studenten, async vanwege database calls
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "StudentOnly")]
        public async Task<IActionResult> Order(DetailViewModel input)
        {
            // haal en kunstwerk user op
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var artpiece = _dbcontext.NoviArtPieces.Include(a => a.Lesser).FirstOrDefault(a => a.Id == input.ArtPiece.Id);

            if (artpiece != null && user != null)
            {
                // bereken totaalprijs in decimal
                decimal amount = Convert.ToDecimal(input.Months * artpiece.Price);
                
                // probeer betaling
                var receipt = _paymentService.SendFunds(user.Id, amount, artpiece.Lesser.Id).Result;

                // onvoldoende saldo
                if (receipt == null) return View("Message", new MessageViewModel { Message = Localization.MSG_INSUFFICIENT_ETH, ReturnToController = "Home", ReturnToAction = "Index" });

                // betaling mislukt
                if (!receipt.Succeeded()) return View("Message", new MessageViewModel { Message = Localization.MSG_PAYMENT_FAILED, ReturnToController = "Home", ReturnToAction = "Index" });

                // zet huurder en beschikbaarheidsinfo in database
                artpiece.Lessee = user;
                artpiece.AvailableFrom = DateTime.UtcNow.AddMonths(input.Months);
                _dbcontext.SaveChanges();

                // logging
                _logger.LogInformation("User reserved artpiece with id: {0}", artpiece.Id);

                // toon bestellingsinfo
                return View("Message", new MessageViewModel { Messages = new string[] { Localization.MSG_ORDER_SUCCEEDED, $"Email verhuurder: {artpiece.Lesser.Email}" , $"Kunstwerk: {artpiece.Title}", $"Huur eindigd op: {artpiece.AvailableFrom.ToString("dd-MM-yyyy")}", $"Overeengekomen prijs: ETH: {amount}" }, ReturnToController = "Home", ReturnToAction = "Index" });
            }

            // val terug op de collectie-pagina
            return RedirectToAction("Index");
        }
    }
}

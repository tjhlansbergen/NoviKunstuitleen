/*
    HomeController.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
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

        // constructor
        public HomeController(ILogger<HomeController> logger, NoviArtDbContext dbcontext, UserManager<NoviArtUser> userManager)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _userManager = userManager;

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
        /// Methode voor het tonen van (fout)melding
        /// </summary>
        /// <param name="message">De te tonen melding</param>
        /// <param name="returncontroller">optionele controler voor terug-link</param>
        /// <param name="returnaction">optionele actie voor terug-link</param>
        /// <returns></returns>
        public IActionResult Error(string message, string returncontroller, string returnaction)
        {
            return View(new ErrorViewModel { Message = message, ReturnToController = returncontroller, ReturnToAction = returnaction });
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
        /// Asynchrone methode voor toevoegen van kunstwerk uit webformulier, alleen toegstaan voor Medewerkers
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
        /// Asynchrone methode voor verwerken van bestelling van kunstwerk, alleen toegestaand voor Studenten
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "StudentOnly")]
        public async Task<IActionResult> Order(DetailViewModel input)
        {
            // haal kunstwerk op uit database
            NoviArtPiece entity = _dbcontext.NoviArtPieces.FirstOrDefault(a => a.Id == input.ArtPiece.Id);
            if (entity != null)
            {
                // zet huurder en beschikbaarheidsinfo in database
                entity.Lessee = await _userManager.GetUserAsync(HttpContext.User);
                entity.AvailableFrom = DateTime.UtcNow.AddMonths(input.Months);
                _dbcontext.SaveChanges();

                // logging
                _logger.LogInformation("User reserved artpiece with id: {0}", entity.Id);
            }

            // en keer terug naar de collectie-pagina
            return RedirectToAction("Index");
        }
    }
}

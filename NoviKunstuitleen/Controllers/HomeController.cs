using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Models.HomeViewModels;
using NoviKunstuitleen.Extensions;
using Microsoft.AspNetCore.Identity;

namespace NoviKunstuitleen.Controllers
{
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

            // TODO housekeeping
            foreach(var artpiece in _dbcontext.NoviArtPieces.Where(a => a.AvailableFrom < DateTime.UtcNow))
            {
                    artpiece.Lessee = null;
            }
            _dbcontext.SaveChanges();
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel(_dbcontext.Users.ToList(), _dbcontext.NoviArtPieces.ToList()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Policy = "DocentOnly")]  // Toevoegen van kunstobjecten alleen toegestaan voor de rol docent
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "DocentOnly")]  // Toevoegen van kunstobjecten alleen toegestaan voor de rol docent
        public async Task<IActionResult> Create(CreateViewModel input)
        {
            // verwerk input vanuit webformulier
            NoviArtPiece piece = new NoviArtPiece{ Artist = input.Artist, Description = input.Description, Dimensions = input.Dimensions, Frame = input.Frame, Price = input.Price, Title = input.Title };

            // voeg aanmaakdatum, beschikbaarheidsinfo, en aanbieder toe aan item
            piece.CreationDate = piece.AvailableFrom = DateTime.UtcNow;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            piece.Lesser = user.Id;

            // upload de afbeelding
            using (var memoryStream = new MemoryStream())
            {
                // lees bestand in
                input.Image.CopyTo(memoryStream);

                // verifieer bestand
                if (!FileHelperExtensions.IsValidFile(input.Image.FileName, memoryStream, new string[] { ".gif", ".png", ".jpg", ".jpeg" }, 2097152))
                {
                    // Toon foutmelding indien afbeelding niet valide
                    ModelState.AddModelError("Image", "Ongeldige foto, het bestand mag maximaal 2mb groot zijn, en moet van het type 'gif', 'png' of 'jpg' zijn!");
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

            // Maak entry in log
            _logger.LogInformation("User created a new artpiece with id: {0}", piece.Id);

            // en keer terug naar de collectie-pagina
            return RedirectToAction("Index");
        }
        
        public IActionResult Detail(int id)
        {
            var piece = _dbcontext.NoviArtPieces.Where(a => a.Id == id).FirstOrDefault();
            var lesser = _dbcontext.Users.Where(u => u.Id == piece.Lesser).FirstOrDefault();
            return View(new DetailViewModel(piece , lesser));
        }
    }
}

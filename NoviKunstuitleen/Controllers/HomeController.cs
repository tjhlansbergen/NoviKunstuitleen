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
using NoviKunstuitleen.Models;
using NoviKunstuitleen.Extensions;

namespace NoviKunstuitleen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NoviArtDbContext _dbcontext;

        public HomeController(ILogger<HomeController> logger, NoviArtDbContext dbcontext)
        {
            _logger = logger;
            _dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel(_dbcontext.NoviArtCollection.ToList()));
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

        [Authorize(Policy = "DocentOnly")]  // Toevoegen van kunstobjecten alleen toegestaan voor de rol docent
        public IActionResult AddArtPiece(NoviArtPiece piece)
        {
            // voeg beschikbaarheidsinfo, en aanbieder toe aan item
            piece.AvailableFrom = DateTime.UtcNow;
            piece.Lender = User.FindFirst("DisplayName").Value;

            // upload de afbeelding
            using (var memoryStream = new MemoryStream())
            {
                // lees bestand in
                piece.Image.CopyTo(memoryStream);

                // verifieer bestand
                if (!FileHelper.IsValidFile(piece.Image.FileName, memoryStream, new string[] { ".gif", ".png", ".jpg", ".jpeg" }, 2097152))
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
                piece.ImageType = Path.GetExtension(piece.Image.FileName).ToLowerInvariant().TrimStart('.');

            }

            // Voeg resultaat toe aan de database
            _dbcontext.Add<NoviArtPiece>(piece);
            _dbcontext.SaveChanges();

            // Maak entry in log
            _logger.LogInformation("User created a new artpiece with id: {0}", piece.Id);

            // en keer terug naar de collectie-pagina
            return View("Index", new IndexViewModel(_dbcontext.NoviArtCollection.ToList()));
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Models;

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

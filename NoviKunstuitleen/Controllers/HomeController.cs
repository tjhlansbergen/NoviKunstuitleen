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

        public IActionResult NewArtPiece()
        {
            _logger.LogInformation("User created a new ArtPiece");

            var piece = new NoviArtPiece();
            piece.Title = "Victory Boogy Woogie";
            _dbcontext.Add<NoviArtPiece>(piece);
            _dbcontext.SaveChanges();

            return View("Index", new IndexViewModel(_dbcontext.NoviArtCollection.ToList()));
        }
    }
}

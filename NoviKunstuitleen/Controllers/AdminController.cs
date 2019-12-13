using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Models.AdminViewModels;
using System;
using System.Linq;

namespace NoviKunstuitleen.Controllers
{
    
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly NoviArtDbContext _dbcontext;
        private readonly UserManager<NoviUser> _userManager;

        // constructor
        public AdminController(ILogger<AdminController> logger, NoviArtDbContext dbcontext)
        {
            _logger = logger;
            _dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View("Admin", new AdminViewModel(dbusers: _dbcontext.Users.ToList<NoviUser>(), dbartpieces: _dbcontext.NoviArtPieces.ToList<NoviArtPiece>()));
        }

        /// <summary>
        /// Methode voor het verwijderen van de opgegeven NoviArtPiece uit de database
        /// Method-overloading helaas onmogelijk omdat de http route (actionname) uniek moet zijn. 
        /// </summary>
        public IActionResult DeleteArtPiece(int id)
        {

            NoviArtPiece entity = new NoviArtPiece();
            entity.Id = id;

            // verwijder item uit database
            _dbcontext.NoviArtPieces.Remove(entity);
            _dbcontext.SaveChanges();

            // TODO logging

            // en herlaadt pagina
            return View("Admin", new AdminViewModel(dbusers: _dbcontext.Users.ToList<NoviUser>(), dbartpieces: _dbcontext.NoviArtPieces.ToList<NoviArtPiece>()));
        }


        /// <summary>
        /// Methode voor het verwijderen van de opgegeven NoviUser uit de database
        /// Method-overloading helaas onmogelijk omdat de http route (actionname) uniek moet zijn. 
        /// </summary>
        public IActionResult DeleteUser(string id)
        {
            
            // TODO remove from usermanager
            

            // TODO logging

            // en herlaadt pagina
            return View("Admin", new AdminViewModel(dbusers: _dbcontext.Users.ToList<NoviUser>(), dbartpieces: _dbcontext.NoviArtPieces.ToList<NoviArtPiece>()));
        }
    }
}
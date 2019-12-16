using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Models.AdminViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Controllers
{

    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly NoviArtDbContext _dbcontext;
        private readonly UserManager<NoviArtUser> _userManager;

        // constructor
        public AdminController(ILogger<AdminController> logger, NoviArtDbContext dbcontext, UserManager<NoviArtUser> userManager)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View("Admin", new AdminViewModel(dbusers: _dbcontext.Users.ToList<NoviArtUser>(), dbartpieces: _dbcontext.NoviArtPieces.ToList<NoviArtPiece>()));
        }

        /// <summary>
        /// Methode voor het verwijderen van de opgegeven NoviArtPiece uit de database
        /// Method-overloading helaas onmogelijk omdat de http route (actionname) uniek moet zijn. 
        /// </summary>
        public IActionResult DeleteArtPiece(int id)
        {
            // maak een nieuw ArtPiece instantie met alleen de primary key
            NoviArtPiece entity = new NoviArtPiece { Id = id };

            // verwijder item uit database
            _dbcontext.NoviArtPieces.Remove(entity);
            _dbcontext.SaveChanges();

            // TODO logging

            // en herlaadt pagina
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Methode voor het verwijderen van de opgegeven NoviUser uit de database
        /// Method-overloading helaas onmogelijk omdat de http route (actionname) uniek moet zijn. 
        /// </summary>
        public async Task<IActionResult> DeleteUser(string id)
        {
            // zoek de gebruiker en verwijder deze
            NoviArtUser user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);

            // TODO logging

            // en herlaadt pagina
            return RedirectToAction("Index");
        }
    }
}
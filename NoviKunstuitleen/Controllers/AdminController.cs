using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Models.AdminViewModels;
using NoviKunstuitleen.Models.HomeViewModels;
using System.Linq;
using System.Threading.Tasks;


namespace NoviKunstuitleen.Controllers
{

    /// <summary>
    /// Controller voor de Admin pagina
    /// </summary>
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


        /// <summary>
        /// Toon adminpagina, haal users en artpieces op uit database bij laden
        /// </summary>
        public IActionResult Index()
        {
            return View("Admin", new AdminViewModel(dbusers: _dbcontext.Users.ToList<NoviArtUser>(), dbartpieces: _dbcontext.NoviArtPieces.ToList<NoviArtPiece>()));
        }

        /// <summary>
        /// Methode voor het verwijderen van de opgegeven NoviArtPiece uit de database 
        /// </summary>
        public async Task<IActionResult> DeleteArtPiece(int id)
        {
            // haal artpiece op uit database
            NoviArtPiece entity = await _dbcontext.NoviArtPieces.Include(a => a.Lesser).Where(a => a.Id == id).FirstOrDefaultAsync();

            if (entity != null)
            {
                // controleer of het object momenteel niet verhuurd is
                if (!entity.Available) return View("Error", new ErrorViewModel { Message = "Het kunstwerk wat u wilt verwijderen is momenteel verhuurd, u kunt deze pas verwijderen als de huurperiode verstreken is.", ReturnToController = "Admin", ReturnToAction = "Index" });

                // verwijder item uit database
                _dbcontext.NoviArtPieces.Remove(entity);
                await _dbcontext.SaveChangesAsync();

                // logging
                _logger.LogInformation("An admin deleted artpiece with id: {0}", id);
            }

            // en herlaadt pagina
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Methode voor het verwijderen van de opgegeven NoviUser uit de database
        /// </summary>
        public async Task<IActionResult> DeleteUser(string id)
        {
            // zoek de gebruiker
            NoviArtUser user = await _userManager.FindByIdAsync(id);


            if (user != null)
            {
                // controleer of user momenteel niets huurt
                if (await _dbcontext.NoviArtPieces.Where(a => a.Lessee.Id == user.Id).AnyAsync()) return View("Error", new ErrorViewModel { Message = "De gebruiker die u wilt verwijderen huurt op dit moment één of meerdere kunstwerken. U kunt deze gebruiker pas verwijderen als de huurperiode verstreken is.", ReturnToController = "Admin", ReturnToAction = "Index" });

                // controleer of user momenteel niets verhuurd
                if (await _dbcontext.NoviArtPieces.Where(a => a.Lesser.Id == user.Id).AnyAsync()) return View("Error", new ErrorViewModel { Message = "De gebruiker die u wilt verwijderen biedt nog kunstwerken te leen aan. U kunt de gebruiker pas verwijderen als al zijn/haar kunstwerken verwijderd zijn.", ReturnToController = "Admin", ReturnToAction = "Index" });

                // verwijder user
                await _userManager.DeleteAsync(user);

                // logging
                _logger.LogInformation("An admin deleted a user with id: {0}", id);
            }


            // en herlaadt pagina
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Methode voor het bevestigen van gebruikersaccounts door een admin
        /// </summary>
        public async Task<IActionResult> ConfirmAccount(string id)
        {
            // zoek de gebruiker
            NoviArtUser user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
                // zet bevestigd, update database
                user.EmailConfirmed = true;
                await _dbcontext.SaveChangesAsync();

                // logging
                _logger.LogInformation("An admin confirmed an account with id: {0}", id);
            }

            // en herlaadt pagina
            return RedirectToAction("Index");
        }
    }
}
/*
    ManageController.cs
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
using NoviKunstuitleen.Models.HomeViewModels;
using NoviKunstuitleen.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using NoviKunstuitleen.Services;

namespace NoviKunstuitleen.Controllers
{

    /// <summary>
    /// MVC Controller klasse voor gebruikers profiel pagina's, alleen beschikbaar voor ingelogde gebruikers
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<NoviArtUser> _userManager;
        private readonly SignInManager<NoviArtUser> _signInManager;
        private readonly ILogger _logger;
        private readonly NoviArtDbContext _dbcontext;
        private readonly IPaymentService _paymentservice;

        // constructor
        public ManageController(UserManager<NoviArtUser> userManager, SignInManager<NoviArtUser> signInManager, ILogger<ManageController> logger, NoviArtDbContext dbcontext, IPaymentService paymentservice)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbcontext = dbcontext;
            _paymentservice = paymentservice;
        }

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Toon profielpagina, haal users en artpieces op uit database bij laden, async vanwege database call
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            NoviArtUser user = await _userManager.GetUserAsync(User);
            var wallets = new List<NoviArtWallet>();

            if (user.Type == NoviUserType.Admin || user.Type == NoviUserType.Root)
            {
                // laad alle wallets (voor studenten en medewerkers, admin hebben geen zichtbare wallet)
                foreach (var noviuser in _dbcontext.Users.ToList<NoviArtUser>().Where(u => u.Type == NoviUserType.Medewerker || u.Type == NoviUserType.Student))
                {
                    wallets.Add(new NoviArtWallet { UserID = noviuser.Id, UserName = noviuser.DisplayName, Address = _paymentservice.GetAddress(noviuser.Id), Balance = await _paymentservice.GetBalance(noviuser.Id) });
                }
            }
            else
            {
                wallets.Add(new NoviArtWallet { UserID = user.Id, UserName = user.DisplayName, Address = _paymentservice.GetAddress(user.Id), Balance = await _paymentservice.GetBalance(user.Id) });
            }

            return View(new ManageViewModel { User = await _userManager.GetUserAsync(User), DBContext = _dbcontext, Wallets = wallets });
        }

        /// <summary>
        /// Methode voor het verwijderen van de opgegeven NoviArtPiece uit de database, alleen toegestaan voor Medewerkers, async vanwege database calls
        /// </summary>
        [Authorize(Policy = "MedewerkerOnly")]
        public async Task<IActionResult> DeleteArtPiece(int id)
        {
            // haal artpiece op uit database
            NoviArtPiece entity = await _dbcontext.NoviArtPieces.Include(a => a.Lesser).Where(a => a.Id == id).FirstOrDefaultAsync();
            NoviArtUser user = await _userManager.GetUserAsync(User);

            if (entity != null)
            {
                // controleer of gebruiker het object mag verwijderen
                if (user.Type != NoviUserType.Admin && user.Type != NoviUserType.Root && entity.Lesser.Id != user.Id)
                {
                    return View("Message", new MessageViewModel { Message = Localization.MSG_UNAUTHORIZED_REMOVAL, ReturnToController = "Manage", ReturnToAction = "Manage" });
                }

                // controleer of het object momenteel niet verhuurd is
                if (!entity.Available) return View("Message", new MessageViewModel { Message = Localization.MSG_ARTPIECE_LOCKED, ReturnToController = "Manage", ReturnToAction = "Manage" });

                // verwijder item uit database
                _dbcontext.NoviArtPieces.Remove(entity);
                await _dbcontext.SaveChangesAsync();

                // logging
                _logger.LogInformation("A user deleted artpiece with id: {0}", id);
            }

            // en herlaadt pagina
            return RedirectToAction("Manage");
        }

        /// <summary>
        /// Methode voor het verwijderen van de NoviUser door een admin, async vanwege database calls
        /// </summary>
        public async Task<IActionResult> DeleteUser(string id)
        {
            // zoek de gebruiker
            NoviArtUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                // controleer of user momenteel niets huurt
                if (await _dbcontext.NoviArtPieces.Where(a => a.Lessee.Id == user.Id).AnyAsync()) return View("Message", new MessageViewModel { Message = Localization.MSG_LESSEE_LOCKED, ReturnToController = "Manage", ReturnToAction = "Manage" });

                // controleer of user momenteel niets verhuurd
                if (await _dbcontext.NoviArtPieces.Where(a => a.Lesser.Id == user.Id).AnyAsync()) return View("Message", new MessageViewModel { Message = Localization.MSG_LESSER_LOCKED, ReturnToController = "Manage", ReturnToAction = "Manage" });

                // log gebruiker eerst uit
                await _signInManager.SignOutAsync();

                // verwijder user
                await _userManager.DeleteAsync(user);

                // logging
                _logger.LogInformation("An admin deleted a user with id: {0}", id);
            }


            // en herlaadt pagina
            return RedirectToAction("Manage");
        }

        /// <summary>
        /// Methode voor het bevestigen van gebruikersaccounts door een admin, async vanwege database calls
        /// </summary>
        public async Task<IActionResult> ConfirmAccount(string id)
        {
            // zoek de gebruiker
            NoviArtUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                // zet bevestigd, update database
                user.EmailConfirmed = true;
                await _dbcontext.SaveChangesAsync();

                // logging
                _logger.LogInformation("An admin confirmed an account with id: {0}", id);
            }

            // en herlaadt pagina
            return RedirectToAction("Manage");
        }


        /// <summary>
        /// Toon wachtwoord wijzigen pagina, async vanwege async usermanager call
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }


        /// <summary>
        /// Verwerk wachtwoord wijziging, async vanwege database call
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // user ophalen
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // wachtwoord wijzigen
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            // en opnieuw inloggen
            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = Localization.MSG_PASSWORD_CHANGED;

            return RedirectToAction(nameof(ChangePassword));
        }

        /// <summary>
        /// Methode voor het verwijderen van de NoviUser door zichzelf, async vanwege database calls
        /// </summary>
        public async Task<IActionResult> DeleteSelf()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // controleer of user momenteel niets huurt
                if (await _dbcontext.NoviArtPieces.Where(a => a.Lessee.Id == user.Id).AnyAsync()) return View("Message", new MessageViewModel { Message = Localization.MSG_LESSEE_LOCKED, ReturnToController = "Manage", ReturnToAction = "Manage" });

                // controleer of user momenteel niets verhuurd
                if (await _dbcontext.NoviArtPieces.Where(a => a.Lesser.Id == user.Id).AnyAsync()) return View("Message", new MessageViewModel { Message = Localization.MSG_LESSER_LOCKED, ReturnToController = "Manage", ReturnToAction = "Manage" });

                // gebruiker uitloggen
                await _signInManager.SignOutAsync();

                // verwijder user
                await _userManager.DeleteAsync(user);

                // logging
                _logger.LogInformation("A user deleted itself");
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Verwerk wachtwoord wijziging, async vanwege database call
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(WalletViewModel input)
        {
            // adres controleren
            if (!EthereumPaymentService.IsValidAddress(input.WithdrawAddress)) return View("Message", new MessageViewModel { Message = Localization.MSG_ETHADDRESS_NOT_VALID, ReturnToController = "Manage", ReturnToAction = "Manage" });

            // bedrag controleren
            decimal amount;
            try
            {
                amount = Convert.ToDecimal(input.WithdrawAmount);
            }
            catch
            {
                return View("Message", new MessageViewModel { Message = Localization.MSG_ETHAMOUNT_NOT_VALID, ReturnToController = "Manage", ReturnToAction = "Manage" });
            }

            // user ophalen
            NoviArtUser user = await _userManager.GetUserAsync(User);

            // voer betaling uit
            var result = _paymentservice.SendFunds(user.Id, amount, input.WithdrawAddress).Result;

            // onvoldoende saldo
            if (result == null) return View("Message", new MessageViewModel { Message = Localization.MSG_INSUFFICIENT_ETH, ReturnToController = "Manage", ReturnToAction = "Manage" });

            // betaling gelukt
            if (result.Succeeded()) return View("Message", new MessageViewModel { Message = Localization.MSG_PAYMENT_SUCCEEDED, ReturnToController = "Manage", ReturnToAction = "Manage" });

            // betaling mislukt om onbekende reden
            return View("Message", new MessageViewModel { Message = Localization.MSG_PAYMENT_FAILED, ReturnToController = "Manage", ReturnToAction = "Manage" });

        }

        /// <summary>
        /// Helper voor het toevoegen van error aan errorstate van de pagina
        /// </summary>
        /// <param name="result"></param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}

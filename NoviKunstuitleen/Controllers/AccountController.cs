/*
    AccountController.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 15 feb 2020
*/

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Extensions;
using NoviKunstuitleen.Models.AccountViewModels;
using NoviKunstuitleen.Services;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace NoviKunstuitleen.Controllers
{

    /// <summary>
    /// MVC Controller klasse voor gebruikersbeheer, alleen voor ingelogde gebruikers muv. de methode met AllowAnonymous-annotation
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<NoviArtUser> _userManager;
        private readonly SignInManager<NoviArtUser> _signInManager;
        private readonly IEmailService _emailSender;
        private readonly ILogger _logger;

        // constructor
        public AccountController(UserManager<NoviArtUser> userManager, SignInManager<NoviArtUser> signInManager, IEmailService emailSender, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        //[TempData]
        //public string ErrorMessage { get; set; }


        /// <summary>
        /// Toon login pagina (na eerst geforceerd uit te loggen), async vanwege async uitloggen
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        /// <summary>
        /// Verwerk inlog actie van gebruiker, async vanwege database calls
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // haal gebruiker op uit database
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogInformation("Login attempt for non existing user");
                    ModelState.AddModelError(string.Empty, Localization.MSG_LOGIN_FAILED);
                    return View(model);
                }

                // controleer of account bevestigd is
                var confirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!confirmed)
                {
                    _logger.LogInformation("Login attempt for unconfirmed user");
                    return View("EmailNotConfirmed");
                }

                // log de gebruiker in
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Account locked-out");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    _logger.LogWarning("Unsuccesful login attempt");
                    ModelState.AddModelError(string.Empty, Localization.MSG_LOGIN_FAILED);
                    return View(model);
                }
            }

            // Toon pagina opnieuw bij eerdere foutmeldingen
            return View(model);
        }


        /// <summary>
        /// Toon locked-out melding
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        /// <summary>
        /// Toon registratiepagina voor nieuwe gebruiker
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        /// <summary>
        /// Verwerk registratie door nieuwe gebruiker, async vanwege database calls
        /// </summary>        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // maak user aan met formulier data
                var user = new NoviArtUser { UserName = model.Email, Email = model.Email, NoviNumber = model.Number, Type = model.Type, DisplayName = model.DisplayName };
                var result = await _userManager.CreateAsync(user, model.Password);

                // controleer resultaat
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // verstuur verificatiemail
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, Localization.EML_CONFIRM_ACCOUNT, Localization.EML_CONFIRM_CLICK + $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>" + Localization.EML_CONFIRM + "</a>");

                    // gebruiker terugsturen naar inlogpagina
                    return View("EmailNotConfirmed");
                }
                AddErrors(result);
            }

            // pagina opnieuw laden bij eerdere errors
            return View(model);
        }


        /// <summary>
        /// Toon registratie pagina voor nieuwe gebruikers voor Admins
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Verwerk registratie pagina voor nieuwe gebruikers voor Admins, async vanwege database calls
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminRegister(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // maak gebruiker aan met data uit formulier
                var user = new NoviArtUser { UserName = model.Email, Email = model.Email, NoviNumber = "0", Type = model.Type, DisplayName = model.DisplayName };

                // zet account direct bevestigd (apart van de actie hierboven omwille van leesbaarheid)
                user.EmailConfirmed = true;

                // maak account aan
                var result = await _userManager.CreateAsync(user, model.Password);

                // verwerk resultaat
                if (result.Succeeded)
                {
                    _logger.LogInformation("An admin created a new account with password.");
                    return RedirectToAction("Manage", "Manage");
                }

                // resultaat niet succesvol, 
                AddErrors(result);
            }

            // Toon formulier opnieuw met eerdere errors
            return View(model);
        }

        /// <summary>
        /// Gebruiker uitloggen
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // uitloggen
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            // stuur gebruiker terug naar inlog pagina
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Gebruiker bevestigen, asynv vanwege database calls
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // controleer input
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // haal user op
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation("Unable to load user");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // bevestig user en toon resultaat
            var result = await _userManager.ConfirmEmailAsync(user, code);
            _logger.LogInformation("User confirmed account");
            return View(result.Succeeded ? "ConfirmEmail" : "Message");
        }

        /// <summary>
        /// Toon wacthwoord vergeten formulier
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Verwerk wacthwoord vergeten formulier, asynv vanwege database calls
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // haal user op
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Gebruiker NIET tonen dat sdeze niet bestaat omwille van security!
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // reset mail sturen
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset uw wachtwoord", $"Reset uw wachtwoord door op de volgende link te klikken: <a href='{callbackUrl}'>reset</a>");

                // resultaat tonen
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // Toon formulier opnieuw met eerdere errors
            return View(model);
        }


        /// <summary>
        /// Toon reset wachtwoord bevestiging
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Toon reset wachtwoord formulier
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        /// <summary>
        /// Verwerk reset wachtwoord formulier, async vanwege database calls
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // user ophaken
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Gebruiker NIET tonen dat sdeze niet bestaat omwille van security!
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            // password resetten
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);

            // en resultaat tonen
            return View();
        }

        /// <summary>
        /// Toon reset wachtwoord bevestiging
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Toon toegang geweigerd pagina
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}

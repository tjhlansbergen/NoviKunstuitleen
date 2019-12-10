using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NoviKunstuitleen.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Automatisch gegenereerde klasse (dmv scaffolding) voor het registreren van een gebruiker
    /// Bron: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.0
    /// </summary>
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<NoviUser> _signInManager;
        private readonly UserManager<NoviUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<NoviUser> userManager,
            SignInManager<NoviUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        // verwijderen?
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Dit veld is verplicht")]
            [EmailAddress]
            [Display(Name = "Email adres")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Dit veld is verplicht")]
            [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} maximaal {1} tekens lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bevestig wachtwoord")]
            [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Dit veld is verplicht")]
            [Display(Name = "Weergavenaam")]
            public string DisplayName { get; set; }

            [Required(ErrorMessage = "Dit veld is verplicht")]
            [Display(Name = "Novi student/docent nummer")]
            public string Number { get; set; }

            [Required(ErrorMessage ="Dit veld is verplicht")]
            [Display(Name = "Ik ben een")]
            public NoviUserType Type { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            // verwijderen?
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new NoviUser { UserName = Input.Email, Email = Input.Email, NoviNumber = Input.Number, Type = Input.Type, DisplayName = Input.DisplayName };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    /*
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    */

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}

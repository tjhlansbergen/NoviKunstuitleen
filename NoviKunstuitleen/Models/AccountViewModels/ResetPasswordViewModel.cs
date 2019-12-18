using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Dit veld is verplicht")]
        [EmailAddress]
        [Display(Name = "E-mail adres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Dit veld is verplicht")]
        [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        [Display(Name = "Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Dit veld is verplicht")]
        [DataType(DataType.Password)]
        [Display(Name = "Huidig wachtwoord")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Dit veld is verplicht")]
        [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("NewPassword", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}

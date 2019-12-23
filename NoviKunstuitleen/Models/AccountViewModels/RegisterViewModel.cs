using NoviKunstuitleen.Data;
using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [EmailAddress]
        [Display(Name = Localization.FLD_USER_EMAIL)]
        public string Email { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [StringLength(100, ErrorMessage = Localization.VLD_USER_PASSWORD_LENGTH_6, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = Localization.FLD_USER_PASSWORD)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = Localization.FLD_USER_PASSWORD_CONFIRM)]
        [Compare("Password", ErrorMessage = Localization.VLD_USER_PASSWORD_NOMATCH)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [StringLength(25, ErrorMessage = Localization.VLD_MAX_CHARS_25)]
        [Display(Name = Localization.FLD_USER_DISPLAYNAME)]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [Display(Name = Localization.FLD_USER_NUMBER)]
        public string Number { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [Display(Name = Localization.FLD_USER_TYPE)]
        public NoviUserType Type { get; set; }
    }
}

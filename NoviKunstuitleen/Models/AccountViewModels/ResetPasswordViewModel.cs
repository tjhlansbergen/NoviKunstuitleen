/*
    ResetPasswordViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.AccountViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [EmailAddress]
        [Display(Name = Localization.FLD_USER_EMAIL)]
        public string Email { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [StringLength(100, ErrorMessage = Localization.VLD_USER_PASSWORD_LENGTH_6, MinimumLength = 6)]
        [Display(Name = Localization.FLD_USER_PASSWORD)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = Localization.FLD_USER_PASSWORD_CONFIRM)]
        [Compare("Password", ErrorMessage = Localization.VLD_USER_PASSWORD_NOMATCH)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}

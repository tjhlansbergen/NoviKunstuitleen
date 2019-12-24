/*
    ChangePasswordViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.ManageViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [DataType(DataType.Password)]
        [Display(Name = Localization.FLD_USER_PASSWORD_CURRENT)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [StringLength(100, ErrorMessage = Localization.VLD_USER_PASSWORD_LENGTH_6, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = Localization.FLD_USER_PASSWORD_NEW)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = Localization.FLD_USER_PASSWORD_CONFIRM)]
        [Compare("NewPassword", ErrorMessage = Localization.VLD_USER_PASSWORD_NOMATCH)]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}

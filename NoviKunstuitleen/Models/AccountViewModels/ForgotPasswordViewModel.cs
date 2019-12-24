/*
    ForgotPasswordViewModel.cs
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
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [EmailAddress]
        [Display(Name = Localization.FLD_USER_EMAIL)]
        public string Email { get; set; }
    }
}

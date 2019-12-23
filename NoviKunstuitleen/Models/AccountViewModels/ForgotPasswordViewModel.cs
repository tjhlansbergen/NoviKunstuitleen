using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [EmailAddress]
        [Display(Name = Localization.FLD_USER_EMAIL)]
        public string Email { get; set; }
    }
}

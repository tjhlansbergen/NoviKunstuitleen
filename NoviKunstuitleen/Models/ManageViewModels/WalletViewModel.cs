using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.ManageViewModels
{
    public class WalletViewModel
    {
        public string Address { get; set; }
        public decimal Balance { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [Display(Name = Localization.FLD_WITHDRAWAL_AMOUNT)]
        public double WithdrawAmount { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [StringLength(42, ErrorMessage = Localization.VLD_ETH_ADDRESS_LENGTH, MinimumLength = 42)]
        [Display(Name = Localization.FLD_WITHDRAWAL_ADDRESS)]
        public string WithdrawAddress { get; set; }
    }
}

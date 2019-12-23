using NoviKunstuitleen.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class DetailViewModel
    {
        // weergave properties
        public NoviArtPiece ArtPiece { get; set; }

        // formulier properties
        [Required]
        [Range(1, 12, ErrorMessage = Localization.VLD_MIN_MAX_MONTHS_3_12)]
        [Display(Name = Localization.FLD_ORDER_MONTHS)]
        public int Months { get; set; } = 3;
    }
}

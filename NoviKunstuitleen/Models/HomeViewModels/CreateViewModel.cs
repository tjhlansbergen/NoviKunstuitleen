using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class CreateViewModel
    {
        // Properties die door de gebruiker in het webformulier worden gezet

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [Display(Name = Localization.FLD_ARTPIECE_TITLE)]
        public string Title { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [Display(Name = Localization.FLD_ARTPIECE_ARTIST)]
        public string Artist { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Localization.FLD_ARTPIECE_PRICE_FORMAT)]
        [RegularExpression("([0-9]+)", ErrorMessage = Localization.FLD_ARTPIECE_PRICE_FORMAT)]
        [Display(Name = Localization.FLD_ARTPIECE_PRICE)]
        public int Price { get; set; }

        [Display(Name = Localization.FLD_ARTPIECE_MEASUREMENTS)]
        public string Dimensions { get; set; }

        [Display(Name = Localization.FLD_ARTPIECE_FRAME)]
        public string Frame { get; set; }

        [Required(ErrorMessage = Localization.VLD_REQUIRED)]
        [Display(Name = Localization.FLD_ARTPIECE_IMAGE)]
        public IFormFile Image { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(140, ErrorMessage = Localization.VLD_MAX_CHARS_140)]
        [Display(Name = Localization.FLD_ARTPIECE_DESCRIPTION)]
        public string Description { get; set; }

    }
}

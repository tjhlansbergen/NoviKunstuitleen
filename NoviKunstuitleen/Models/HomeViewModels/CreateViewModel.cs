using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class CreateViewModel
    {
        // Properties die door de gebruiker in het webformulier worden gezet

        [Required]
        [Display(Name = "Titel:")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Kunstenaar:")]
        public string Artist { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Minimaal 1, in hele euro's")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Minimaal 1, in hele euro's")]
        [Display(Name = "Huurprijs:")]
        public int Price { get; set; }

        [Display(Name = "Afmetingen:")]
        public string Dimensions { get; set; }

        [Display(Name = "Type lijst:")]
        public string Frame { get; set; }

        [Required]
        [Display(Name = "Foto:")]
        public IFormFile Image { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(140, ErrorMessage = "Maximaal 140 karakters")]
        [Display(Name = "Omschrijving:")]
        public string Description { get; set; }

    }
}

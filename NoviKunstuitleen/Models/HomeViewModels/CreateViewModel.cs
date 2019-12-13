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
        [Display(Name = "Prijs:")]
        public double Price { get; set; }

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

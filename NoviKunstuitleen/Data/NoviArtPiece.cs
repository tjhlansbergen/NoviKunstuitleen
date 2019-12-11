using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{

    /// <summary>
    /// Klasse voor kunstwerk wat te leen wordt aangeboden
    /// </summary>
    public class NoviArtPiece
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
        [NotMapped]     // uitsluiten van ORM
        [Display(Name = "Foto:")]
        public IFormFile Image { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(140, ErrorMessage = "Maximaal 140 karakters")]
        [Display(Name = "Omschrijving:")]
        public string Description { get; set; }


        // Properties die vanuit de code worden gezet
        public int Id { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string Lender { get; set; }
        public byte[] ImageContent { get; set; }
        public string ImageType { get; set; }
        
    }
}

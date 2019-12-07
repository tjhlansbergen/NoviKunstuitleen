using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{

    /// <summary>
    /// Klasse voor kunstwerk wat te leen wordt aangeboden
    /// </summary>
    public class NoviArtPiece
    {
        // Properties
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Titel:")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Kunstenaar:")]
        public string Artist { get; set; }

        [Required]
        [Display(Name = "Aanbieder:")]
        public string Lender { get; set; }

        [Required]
        [Display(Name = "Prijs:")]
        public double Price { get; set; }
        [Display(Name = "Afmetingen:")]
        public string Dimensions { get; set; }
        [Display(Name = "Type lijst:")]
        public string Frame { get; set; }

        [Required]
        public string Image { get; set; }
        public DateTime AvailableFrom { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Omschrijving:")]
        public string Description { get; set; }

    }
}

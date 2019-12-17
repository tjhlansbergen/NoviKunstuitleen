using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class DetailViewModel
    {
        // weergave properties
        public NoviArtPiece ArtPiece { get; set; }
        public NoviArtUser Lesser { get; set; }

        // formulier properties
        [Required]
        [Range(1, 12, ErrorMessage = "Minimaal 3, maximaal 12 maanden")]
        [Display(Name = "Aantal maanden:")]
        public int Months { get; set; } = 3;
    }
}

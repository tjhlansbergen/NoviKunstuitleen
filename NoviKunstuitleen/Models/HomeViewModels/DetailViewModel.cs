/*
    DetailViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using NoviKunstuitleen.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
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

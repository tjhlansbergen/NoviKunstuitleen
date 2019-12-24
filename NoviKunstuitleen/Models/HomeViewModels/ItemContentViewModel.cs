/*
    ItemContentViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using NoviKunstuitleen.Data;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class ItemContentViewModel
    {
        public NoviArtPiece ArtPiece { get; set; }
        public bool ShowTitle { get; set; }
        public bool ShowDescription { get; set; }
    }
}

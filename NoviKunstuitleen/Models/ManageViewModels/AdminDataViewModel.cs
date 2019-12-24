/*
    AdminDataViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using NoviKunstuitleen.Data;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models.ManageViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class AdminDataViewModel
    {
        public List<NoviArtUser> Users { get; set; }
        public List<NoviArtPiece> ArtPieces { get; set; }

    }
}

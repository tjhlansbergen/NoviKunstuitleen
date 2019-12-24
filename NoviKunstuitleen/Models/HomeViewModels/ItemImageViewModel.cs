/*
    ItemImageViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using System;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class ItemImageViewModel
    {
        public byte[] ImageContent { get; set; }
        public string ImageType { get; set; }
        public bool ShowHighlight { get; set; }

    }
}

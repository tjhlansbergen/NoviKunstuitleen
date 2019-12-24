/*
    IndexViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class IndexViewModel
    {
        // properties
        public List<NoviArtPiece> ArtPieces { get; private set; }
        public NoviArtPiece HighlightedArtPiece { get; private set; }

        public readonly string Build = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version.Split('.').Last();

        // random voor 'uitgelicht' selectie
        static readonly Random rnd = new Random();

        // constructor
        public IndexViewModel(List<NoviArtPiece> dbpieces)
        {
            // initialisatie
            ArtPieces = dbpieces;

            // willekeurig item selecteren uit de beschikbare items als highlighted item
            var availableArtPieces = ArtPieces.Where(a => a.Available).ToList();
            if (availableArtPieces.Count > 0)
                HighlightedArtPiece = availableArtPieces[rnd.Next(availableArtPieces.Count)];
            else
                HighlightedArtPiece = null;
        }
    }
}

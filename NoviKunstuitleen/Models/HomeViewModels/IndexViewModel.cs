using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class IndexViewModel
    {
        // properties
        public List<NoviArtUser> Users { get; private set; }
        public List<NoviArtPiece> ArtPieces { get; private set; }
        public NoviArtPiece HighlightedArtPiece { get; private set; }
        

        // random voor 'uitgelicht' selectie
        static readonly Random rnd = new Random();

        // constructor
        public IndexViewModel(List<NoviArtUser> dbusers, List<NoviArtPiece> dbpieces)
        {
            // initialisatie
            ArtPieces = dbpieces;
            Users = dbusers;

            // willekeurig item selecteren uit de beschikbare items als highlighted item
            var availableArtPieces = ArtPieces.Where(a => a.Available).ToList();
            if (availableArtPieces.Count > 0)
                HighlightedArtPiece = availableArtPieces[rnd.Next(availableArtPieces.Count)];
            else
                HighlightedArtPiece = null;
        }
    }
}

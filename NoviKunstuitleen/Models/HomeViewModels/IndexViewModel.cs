using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class IndexViewModel
    {
        // properties
        public List<NoviArtPiece> ArtPieces { get; private set; }
        public NoviArtPiece HighlightedArtPiece { get; private set; }      // TODO van degenen die beschikbaar zijn

        // random voor 'uitgelicht' selectie
        static readonly Random rnd = new Random();

        // constructor
        public IndexViewModel(List<NoviArtPiece> dbpieces)
        {
            // initialisatie
            ArtPieces = dbpieces;

            // willekeurig item selecteren als highlighted item
            if (ArtPieces.Count > 0)
                HighlightedArtPiece = ArtPieces[rnd.Next(ArtPieces.Count)];
            else
                HighlightedArtPiece = null;
        }
    }
}

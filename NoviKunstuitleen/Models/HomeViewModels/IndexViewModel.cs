using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class IndexViewModel
    {
        // properties
        public List<NoviArtPiece> Pieces { get; private set; }
        public NoviArtPiece HighlightedPiece { get; private set; }      // TODO van degenen die beschikbaar zijn

        // random voor 'uitgelicht' selectie
        static readonly Random rnd = new Random();

        // constructor
        public IndexViewModel(List<NoviArtPiece> dbpieces)
        {
            // initialisatie
            Pieces = dbpieces;

            // willekeurig item selecteren als highlighted item
            if (Pieces.Count > 0)
                HighlightedPiece = Pieces[rnd.Next(Pieces.Count)];
            else
                HighlightedPiece = null;
        }
    }
}

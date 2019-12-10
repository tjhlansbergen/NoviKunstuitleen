using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models
{
    public class IndexViewModel
    {
        // properties
        public List<NoviArtPiece> Pieces { get; private set; }
        public NoviArtPiece HighlightedPiece { get; private set; }

        // random voor 'uitgelicht' selectie
        static readonly Random rnd = new Random();

        // constructor
        public IndexViewModel(List<NoviArtPiece> dbpieces)
        {
            // initialisatie
            Pieces = dbpieces;

            // willekeurig item selecteren als highlighted item
            HighlightedPiece = Pieces[rnd.Next(Pieces.Count)];
        }
    }
}

using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models
{
    public class IndexViewModel
    {
        // properties
        public List<NoviArtPiece> Pieces { get; private set; }

        // constructor
        public IndexViewModel(List<NoviArtPiece> dbpieces)
        {
            // initialiseer
            Pieces = dbpieces;
        }
    }
}

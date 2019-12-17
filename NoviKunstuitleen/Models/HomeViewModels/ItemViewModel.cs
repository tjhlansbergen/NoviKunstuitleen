using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class ItemViewModel
    {
        public NoviArtPiece ArtPiece { get; set; }
        public NoviArtUser Lesser { get; set; }

        public ItemViewModel(NoviArtPiece dbartpiece, NoviArtUser dblesser)
        {
            ArtPiece = dbartpiece;
            Lesser = dblesser;
        }
    }
}

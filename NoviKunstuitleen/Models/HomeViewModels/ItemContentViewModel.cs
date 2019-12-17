using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class ItemContentViewModel
    {
        public NoviArtPiece ArtPiece { get; set; }
        public NoviArtUser Lesser { get; set; }
        public bool ShowTitle { get; set; }
        public bool ShowDescription { get; set; }
    }
}

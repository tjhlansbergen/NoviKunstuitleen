using NoviKunstuitleen.Data;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models.ManageViewModels
{
    public class ManageViewModel
    {
        public NoviArtUser User { get; set; }
        public List<NoviArtPiece> ArtPieces { get; set; }
    }
}

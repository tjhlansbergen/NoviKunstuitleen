using NoviKunstuitleen.Data;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models.ManageViewModels
{
    public class AdminDataViewModel
    {
        public List<NoviArtUser> Users { get; set; }
        public List<NoviArtPiece> ArtPieces { get; set; }

    }
}

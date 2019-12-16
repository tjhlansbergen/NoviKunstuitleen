using NoviKunstuitleen.Data;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models.AdminViewModels
{
    public class AdminViewModel
    {
        // properties
        public List<NoviArtUser> Users { get; private set; }
        public List<NoviArtPiece> ArtPieces { get; private set; }
        

        public AdminViewModel(List<NoviArtUser> dbusers, List<NoviArtPiece> dbartpieces)
        {
            Users = dbusers;
            ArtPieces = dbartpieces;
        }
    }
}

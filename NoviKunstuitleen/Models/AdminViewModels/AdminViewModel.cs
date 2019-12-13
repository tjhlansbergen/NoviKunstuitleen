using NoviKunstuitleen.Data;
using System.Collections.Generic;

namespace NoviKunstuitleen.Models.AdminViewModels
{
    public class AdminViewModel
    {
        // properties
        public List<NoviUser> Users { get; private set; }
        public List<NoviArtPiece> ArtPieces { get; private set; }
        

        public AdminViewModel(List<NoviUser> dbusers, List<NoviArtPiece> dbartpieces)
        {
            Users = dbusers;
            ArtPieces = dbartpieces;
        }
    }
}

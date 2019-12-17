using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class DetailViewModel
    {
        public NoviArtPiece ArtPiece { get; set; }
        public List<NoviArtUser> Users { get; set; }

        public DetailViewModel(NoviArtPiece dbartpiece, List<NoviArtUser> dbusers)
        {
            ArtPiece = dbartpiece;
            Users = dbusers;
        }
    }
}

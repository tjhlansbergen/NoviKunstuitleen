/*
    ManageViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using System.Collections.Generic;
using NoviKunstuitleen.Data;

namespace NoviKunstuitleen.Models.ManageViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class ManageViewModel
    {
        public NoviArtUser User { get; set; }
        public NoviArtDbContext DBContext { get; set; }
        public List<NoviArtWallet> Wallets { get; set; }
    }
}

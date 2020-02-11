using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{
    public class NoviArtWallet
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
    }
}

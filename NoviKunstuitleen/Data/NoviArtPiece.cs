using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{

    /// <summary>
    /// Klasse voor kunstwerk wat te leen wordt aangeboden
    /// </summary>
    public class NoviArtPiece
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public double Price { get; set; }
        public string Dimensions { get; set; }
        public string Frame { get; set; }
        public string Description { get; set; }
        public string Lender { get; set; }
        public string ImageType { get; set; }
        public byte[] ImageContent { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

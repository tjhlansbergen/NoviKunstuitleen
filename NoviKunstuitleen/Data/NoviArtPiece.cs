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
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Artist { get; set; }
        [Required]
        public int Price { get; set; }
        public string Dimensions { get; set; }
        public string Frame { get; set; }
        public string Description { get; set; }
        [Required]
        public NoviArtUser Lesser { get; set; }
        public NoviArtUser Lessee { get; set; }
        [Required]
        public string ImageType { get; set; }
        [Required]
        public byte[] ImageContent { get; set; }
        [Required]
        public DateTime AvailableFrom { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public bool Available => (AvailableFrom < DateTime.UtcNow);
    }
}

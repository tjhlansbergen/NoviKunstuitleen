using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    public class ItemImageViewModel
    {
        public byte[] ImageContent { get; set; }
        public string ImageType { get; set; }
        public bool ShowHighlight { get; set; }

        public ItemImageViewModel(byte[] content, string type, bool highlight)
        {
            ImageContent = content;
            ImageType = type;
            ShowHighlight = highlight;
        }
    }
}

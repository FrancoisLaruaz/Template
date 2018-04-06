using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Search
{
    public  class SearchItem
    {
        public string  Name { get; set; }
        public string Url { get; set; }

        public string ImageSrc { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public SearchItem()
        {
            
        }
    }
}

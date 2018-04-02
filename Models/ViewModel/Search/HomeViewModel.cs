using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Search
{
    public class SearchViewModel
    {

        public int? SearchId { get; set; }

        public string Pattern { get; set; }

        public SearchViewModel()
        {
           
        }
    }
}

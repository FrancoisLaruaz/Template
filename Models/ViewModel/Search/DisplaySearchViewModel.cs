using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;
using Models.Class.Search;

namespace Models.ViewModels.Search
{
    public class DisplaySearchViewModel : BaseModelPager
    {
        public List<SearchItem> SearchList { get; set; }

        public string Pattern { get; set; }


        public DisplaySearchViewModel()
        {
            SearchList = new List<SearchItem>();
        }
    }
}

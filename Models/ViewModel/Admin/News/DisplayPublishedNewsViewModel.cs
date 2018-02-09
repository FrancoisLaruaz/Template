using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;
using Models.Class;


namespace Models.ViewModels
{
    public class DisplayPublishedNewsViewModel : BaseModelPager
    {
        public List<News> NewsList { get; set; }

        public string Pattern { get; set; }


        public DisplayPublishedNewsViewModel()
        {
            NewsList = new List<News>();
        }
    }
}

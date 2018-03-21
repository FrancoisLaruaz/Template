using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;
using Models.Class;
using Models.ViewModels;

namespace Models.ViewModels.Admin.News
{
    public class DisplayPublishedNewsViewModel : BaseModelPager
    {
        public List<Models.BDDObject.News> NewsList { get; set; }

        public string Pattern { get; set; }


        public DisplayPublishedNewsViewModel()
        {
            NewsList = new List<Models.BDDObject.News>();
        }
    }
}

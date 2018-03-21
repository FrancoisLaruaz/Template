using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels.Admin.News
{
    public class NewsViewModel 
    {

        public List<Models.BDDObject.News> NotPublishedNews { get; set; }


        public NewsViewModel()
        {
            NotPublishedNews = new List<Models.BDDObject.News>();
        }
    }
}

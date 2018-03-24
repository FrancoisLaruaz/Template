using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DataEntities.Model;

namespace Models.ViewModels.Admin.News
{
    public class NewsViewModel 
    {

        public List<DataEntities.Model.News> NotPublishedNews { get; set; }


        public NewsViewModel()
        {
            NotPublishedNews = new List<DataEntities.Model.News>();
        }
    }
}

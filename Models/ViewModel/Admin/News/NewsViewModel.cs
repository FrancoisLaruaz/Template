using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class NewsViewModel 
    {

        public List<News> NotPublishedNews { get; set; }


        public NewsViewModel()
        {
            NotPublishedNews = new List<News>();
        }
    }
}

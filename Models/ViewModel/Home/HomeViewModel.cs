using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class HomeViewModel
    {
        public bool SignUp { get; set; }

        public bool PromptLogin { get; set; }

        public string RedirectTo { get; set; }

        public HomeViewModel()
        {
           
        }
    }
}

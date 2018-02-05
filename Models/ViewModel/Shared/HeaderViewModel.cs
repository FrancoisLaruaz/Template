using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class HeaderViewModel
    {
        public string UserFirstName { get; set; }

        public string UserNameDecrypt { get; set; }

        public string PictureSrc { get; set; }

        public HeaderViewModel()
        {
            UserNameDecrypt = "User Profile";
        }
    }
}

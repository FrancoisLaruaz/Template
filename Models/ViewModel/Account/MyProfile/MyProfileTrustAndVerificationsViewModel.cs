using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class MyProfileTrustAndVerificationsViewModel
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public MyProfileTrustAndVerificationsViewModel()
        {
            
        }
    }
}

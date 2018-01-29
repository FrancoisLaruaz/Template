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
    public class DisplayUserRolesViewModel : BaseModelPager
    {
        public List<UserRoleItem> UserRolesList { get; set; }

        public string Pattern { get; set; }


        public DisplayUserRolesViewModel()
        {
            UserRolesList = new List<UserRoleItem>();
        }
    }
}

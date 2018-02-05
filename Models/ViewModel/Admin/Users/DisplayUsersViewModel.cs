﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;
using Models.Class;


namespace Models.ViewModels
{
    public class DisplayUsersViewModel : BaseModelPager
    {
        public List<UserRoleItem> UserRolesList { get; set; }

        public string Pattern { get; set; }


        public DisplayUsersViewModel()
        {
            UserRolesList = new List<UserRoleItem>();
        }
    }
}
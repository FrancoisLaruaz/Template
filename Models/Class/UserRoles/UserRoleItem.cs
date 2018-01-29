using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;
using Models.Class;

namespace Models.Class
{
    public  class UserRoleItem
    {
        public string UserLastNameDecrypt { get; set; }
        public string UserFirstNameDecrypt { get; set; }
        public string UserNameDecrypt { get; set; }
        public int UserId { get; set; }
        public string UseridentityId { get; set; }

        public List<UserRoles> UserRolesList { get; set; }

        public List<UserRoles> UserNotInRoleList { get; set; }

        public UserRoleItem()
        {
            UserRolesList = new List<UserRoles>();
            UserNotInRoleList = new List<UserRoles>();
        }
    }
}

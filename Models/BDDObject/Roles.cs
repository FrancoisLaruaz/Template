using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class Roles
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Roles()
        {

        }
    }
}

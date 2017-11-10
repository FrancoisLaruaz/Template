using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateModification { get; set; }
        public int RoleId { get; set; }
    }
}

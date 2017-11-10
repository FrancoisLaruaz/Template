using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateModification { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
        public int CategoryTypeId { get; set; }
    }
}

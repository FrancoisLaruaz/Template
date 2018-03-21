using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BDDObject
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Order { get; set; }

        public int CountryId { get; set; }

    }
}

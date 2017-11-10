using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.Class
{
    public class DisplayEmailAuditViewModel : BaseModelPager
    {
        public List<EMailAudit> AuditsList { get; set; }

        public string Pattern { get; set; }


        public DisplayEmailAuditViewModel()
        {
            AuditsList = new List<EMailAudit>();
        }
    }
}

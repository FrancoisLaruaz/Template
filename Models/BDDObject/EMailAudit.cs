using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class EMailAudit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string EMailFrom { get; set; }
        public string EMailTo { get; set; }
        public int? UserId { get; set; }

        public int AttachmentNumber { get; set; }
        public int EMailTypeId { get; set; }

    }
}

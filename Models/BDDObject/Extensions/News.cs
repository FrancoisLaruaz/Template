using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class News
    {

        public string TypeName { get; set; }

        public string TypeUserMailingName { get; set; }

        public int? ScheduledTaskId { get; set; }

        public bool? HasScheduledTaskBeenExecuted { get; set; }

        public int MailSentNumber { get; set; }


        public string LastModificationUserFullNameDecrypt { get; set; }

    }
}

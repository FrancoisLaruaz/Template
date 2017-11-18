using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class ScheduledTask
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime? ExecutionDate { get; set; }

        public DateTime ExpectedExecutionDate { get; set; }
        public int? UserId { get; set; }

        public string CallbackUrl { get; set; }

        public string CallbackId { get; set; }


    }
}

﻿using System;
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

        public string GroupName { get; set; }

        public string CallbackId { get; set; }

        public int? EmailTypeId { get; set; }

        public int? NewsId { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class ScheduledTask
    {

        public string EMailTypeName { get; set; }

        public string UserFirstNameDecrypt { get; set; }

        public string UserLastNameDecrypt { get; set; }

        public string UserEMailDecrypt { get; set; }

    }
}
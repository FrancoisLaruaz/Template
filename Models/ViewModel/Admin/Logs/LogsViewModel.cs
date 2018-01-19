﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;
using Models.Class;

namespace Models.ViewModels
{
    public class LogsViewModel :IAdminViewModel
    {


        public string Title { get; set; }

        public string Description { get; set; }

        public string InfoConnectionBBD { get; set; }

        public LogsViewModel()
        {
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class PreviewNewsMailViewModel
    {
        public PreviewNewsMailViewModel()
        {
        }


        public string Body { get; set; }

    }
}
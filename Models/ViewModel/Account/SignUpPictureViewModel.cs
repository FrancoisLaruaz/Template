﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class SignUpPictureViewModel
    {

        public string PictureSrc { get; set; }

        public string PicturePreviewSrc { get; set; }

        public SignUpPictureViewModel()
        {
            
        }
    }
}
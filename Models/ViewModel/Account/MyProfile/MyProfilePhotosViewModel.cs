using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class MyProfilePhotosViewModel
    {
        public int UserId { get; set; }

        public string PictureSrc { get; set; }

        public string PictureThumbnailSrc { get; set; }

        public string PictureDecryptSrc { get; set; }


        public MyProfilePhotosViewModel()
        {
            
        }
    }
}

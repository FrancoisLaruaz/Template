using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Models.Class
{
    public class FileUpload
    {
        public FileUpload()
        {
            ConvertPdf = false;
            CreateThumbnail = false;
            EncryptFile = false;
        }
        public string UploadName { get; set; }
        public string SavedName { get; set; }
        public string Location { get; set; }

        public string UrlPath { get; set; }
        public string DiskPath { get; set; }

        public HttpPostedFileBase File { get; set; }
        public byte[] FileBytes { get; set; }
        public byte[] ThumbBytes { get; set; }
        public bool ConvertPdf { get; set; }
        public bool CreateThumbnail { get; set; }
        public bool EncryptFile { get; set; }
    }
}

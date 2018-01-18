using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.ExternalAuthentification
{
    public class ExternalAuthentificationResult
    {
        public ExternalAuthentificationResult(bool success, string returnUrl, string error, string media, string imageSrc, bool isSignUp, string language)
        {
            Success = success;
            ReturnUrl = returnUrl;
            Error = error;
            Media = media;
            IsSignUp = isSignUp;
            ImageSrc = imageSrc;
            Language = language;
        }

        public bool Success { get; set; }
        public string ReturnUrl { get; set; }

        public string ImageSrc { get; set; }

        public string Media { get; set; }

        public string Error { get; set; }

        public bool IsSignUp { get; set; }

        public string Language { get; set; }
    }
}

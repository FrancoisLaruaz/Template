using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{

    public partial class User
    {
        public string CountryName { get; set; }

        public string ProvinceName { get; set; }

        public string LanguageName { get; set; }

        public string LanguageCode { get; set; }

        public string FirstNameDecrypt { get; set; }

        public string LastNameDecrypt { get; set; }

        public string EmailDecrypt { get; set; }

        public string PasswordDecrypt { get; set; }

        public string UserFullNameDecrypt { get { return (FirstNameDecrypt + " " + LastNameDecrypt).Trim(); } }
    }
}

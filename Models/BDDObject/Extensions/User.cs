using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{

    public partial class User
    {
        public string UserIdentityId { get; set; }
        public DateTime DateLastConnection { get; set; }
        public string CountryName { get; set; }

        public string ProvinceName { get; set; }

        public string LanguageName { get; set; }

        public string LanguageCode { get; set; }

        public string FirstNameDecrypt { get; set; }

        public string LastNameDecrypt { get; set; }

        public string EMailDecrypt { get; set; }

       public string UserNameDecrypt { get; set; }

        public string ResetPasswordToken { get; set; }

        public int AccessFailedCount { get; set; }


        public bool EmailConfirmed { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string UserFullNameDecrypt { get { return (FirstNameDecrypt + " " + LastNameDecrypt).Trim(); } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class EMailAudit
    {
        public int LanguageId { get; set; }

        public int EMailTypeId { get; set; }

        public string LanguageName { get; set; }

        public string UserFirstNameDecrypt { get; set; }
        public string UserLastNameDecrypt { get; set; }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string EMailFromDecrypt { get; set; }
        public string EMailTypeName { get; set; }
        public string EMailToDecrypt { get; set; }

        public int? NewsId { get; set; }
        public string NewsTitle { get; set; }

        public string Comment
        {
            get
            {
                string Result = "";

                if(!String.IsNullOrWhiteSpace(NewsTitle))
                {
                    Result = "[[[News : ]]]" + NewsTitle;
                }

                return Result;
            }
        }

    }
}

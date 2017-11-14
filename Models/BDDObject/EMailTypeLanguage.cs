using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class EMailTypeLanguage
    {
        public int Id { get; set; }
        public int EMailTypeId { get; set; }
        public int LanguageId { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
    }
}

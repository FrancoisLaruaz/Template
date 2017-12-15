using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class News
    {
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public int? LastModificationUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string MailSubject { get; set; }

        public int TypeId { get; set; }

        public bool Active { get; set; }

        public int? TypeUserMailingId { get; set; }



    }
}

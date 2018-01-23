using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class SocialMediaConnection
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
  

        public string LoginProvider { get; set; }

        public string ProviderKeyUserSignedUp { get; set; }

        public string ProviderKeyUserFriend { get; set; }

    }
}

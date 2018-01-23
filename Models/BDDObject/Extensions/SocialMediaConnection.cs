using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class SocialMediaConnection
    {
        public int UserFriendId { get; set; }

        public string UserFriendFirstNameDecrypt { get; set; }

        public string UserFriendLastNameDecrypt { get; set; }

        public string UserFriendUserName { get; set; }

        public string UserFriendEmailDecrypt { get; set; }

        public int UserSignedUpId { get; set; }

        public string UserSignedUpFirstNameDecrypt { get; set; }

        public string UserSignedUpLastNameDecrypt { get; set; }

        public string UserSignedUpUserName { get; set; }

        public string UserSignedUpEmailDecrypt { get; set; }

    }
}

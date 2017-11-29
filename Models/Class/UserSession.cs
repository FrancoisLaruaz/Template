using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class
{
    [Serializable]
    public class UserSession
    {
        public string UserName { get; set; }

        public string UserNameDecrypt { get; set; }

        public string UserIdentityId { get; set; }

        public int UserId { get; set; }

        public string FirstNameDecrypt { get; set; }

       public string LastNameDecrypt { get; set; }


        public UserSession()
        {

        }
    }
}

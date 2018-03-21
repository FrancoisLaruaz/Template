using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.SignUp
{

    public class UserSignUp
    {
        public string UserName { get; set; }

        public string UserNameDecrypt { get; set; }

        public string UserIdentityId { get; set; }

        public int UserId { get; set; }

        public string FirsName { get; set; }

       public string LastName { get; set; }

        public string LanguageTag { get; set; }

        public string PictureThumbnailSrc { get; set; }




        public UserSignUp()
        {

        }
    }
}

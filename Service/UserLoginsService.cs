using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using Models.Class.ExternalAuthentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class UserLoginsService
    {

        /// <summary>
        /// Create an external login if a registered user decide to use a social media to log in
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public static bool CreateExternalLogin(ExternalSignUpInformation Info)
        {
            bool result = false;
            try
            {
                User user = UserService.GetUserByUserName(EncryptHelper.EncryptToString(Info.Email));
                if (user != null)
                {

                    string UserId = user.UserIdentityId;

                    Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                    Columns.Add("ProviderKey", Info.ProviderKey);
                    Columns.Add("LoginProvider", Info.LoginProvider);
                    Columns.Add("UserId", UserId);
                    int InsertedId = GenericDAL.InsertRow("userlogins", Columns);

                    if (InsertedId >= 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + Info.Email);
            }
            return result;
        }



    }
}

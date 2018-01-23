using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Models.ViewModels;

namespace Service
{
    public static class SocialMediaConnectionService
    {

        /// <summary>
        /// Insert a connection between user of the website
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static int InsertSocialMediaConnection(SocialMediaConnection Connection)
        {
            int Result = -1;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("LoginProvider", Connection.LoginProvider);
                Columns.Add("ProviderKeyUserSignedUp", Connection.ProviderKeyUserSignedUp);
                Columns.Add("ProviderKeyUserFriend", Connection.ProviderKeyUserFriend);
                Columns.Add("Date", DateTime.UtcNow);
                Result = GenericDAL.InsertRow("socialmediaconnection", Columns);
            }
            catch (Exception e)
            {
                Result = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Result;
        }

        /// <summary>
        /// Get a list of social connections with informations concerning the users.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static List<SocialMediaConnection> GetSocialMediaConnectionsList(int? UserId = null)
        {
            List<SocialMediaConnection> Result = new List<SocialMediaConnection>();
            try
            {
                Result = SocialMediaConnectionDAL.GetSocialMediaConnectionsList(UserId);
            }
            catch (Exception e)
            {

                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return Result;
        }



    }
}

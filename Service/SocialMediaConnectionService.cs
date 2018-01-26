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
        /// Insert connections between users of the website
        /// </summary>
        /// <param name="FriendsList"></param>
        /// <param name="ProviderKey"></param>
        /// <param name="LoginProvider"></param>
        /// <returns></returns>
        public static bool InsertSocialMediaConnections(List<string> FriendsList,string ProviderKey, string LoginProvider)
        {
            bool Result =true;
            try
            {

                if (FriendsList != null && FriendsList.Count > 0)
                {
                    foreach (string Friend in FriendsList)
                    {
                        Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                        Columns.Add("LoginProvider", LoginProvider);
                        Columns.Add("ProviderKeyUserSignedUp", ProviderKey);
                        Columns.Add("ProviderKeyUserFriend", Friend);
                        Columns.Add("Date", DateTime.UtcNow);
                        Result = Result && (GenericDAL.InsertRow("socialmediaconnection", Columns) >0?true:false);
                    }
                }
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProviderKey = "+ ProviderKey+ " and LoginProvider ="+ LoginProvider);
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

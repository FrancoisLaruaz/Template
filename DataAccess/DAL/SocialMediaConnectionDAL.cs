using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Commons;

namespace DataAccess
{
    public class SocialMediaConnectionDAL
    {
        public SocialMediaConnectionDAL()
        {

        }

        /// <summary>
        /// Get a list of social connections with informations concerning the users.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static List<SocialMediaConnection> GetSocialMediaConnectionsList(int? UserId=null)
        {
            List<SocialMediaConnection> result = new List<SocialMediaConnection>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select s.Id, s.Date, s.LoginProvider, s.ProviderKeyUserSignedUp, s.ProviderKeyUserFriend  ";
                Query = Query + ", uf.Id as 'UserFriendId', uf.username as 'UserFriendUserName', uf.FirstName as 'UserFriendFirstName', uf.LastName as 'UserFriendLastName', uf.EMail as 'UserFriendEMail'";
                Query = Query + ", us.Id as 'UserSignedUpId', us.username as 'UserSignedUpUserName', us.FirstName as 'UserSignedUpFirstName', us.LastName as 'UserSignedUpLastName', us.EMail as 'UserSignedUpEMail'";
                Query = Query + "from socialmediaconnection s ";
                Query = Query + "inner join userlogins lf on lf.ProviderKey=s.ProviderKeyUserFriend ";
                Query = Query + "inner join useridentity if on if.UserId=lf.UserId ";
                Query = Query + "inner join user uf on uf.username=if.username ";
                Query = Query + "inner join userlogins ls on ls.ProviderKey=s.ProviderKeyUserSignedUp ";
                Query = Query + "inner join useridentity is on is.UserId=ls.UserId ";
                Query = Query + "inner join user us on us.username=is.username ";
                Query = Query + " where 1=1 ";
                if (UserId != null && UserId.Value > 0)
                {
                    Query = Query + " and s.UserId=" + UserId.Value.ToString();
                }


                Query = Query + " order by s.ProviderKeyUserSignedUp, s.Date desc";
                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    SocialMediaConnection element = new SocialMediaConnection();
                    element.Id = Convert.ToInt32(dr["Id"]);
                    element.Date = MySQLHelper.GetDateFromMySQL(dr["Date"]).Value;
                    element.LoginProvider = MySQLHelper.GetStringFromMySQL(dr["LoginProvider"]);
                    element.ProviderKeyUserSignedUp = MySQLHelper.GetStringFromMySQL(dr["ProviderKeyUserSignedUp"]);
                    element.ProviderKeyUserFriend = Commons.MySQLHelper.GetStringFromMySQL(dr["ProviderKeyUserFriend"]);

                    element.UserFriendId = Convert.ToInt32(dr["UserFriendId"]);
                    element.UserFriendUserName = MySQLHelper.GetStringFromMySQL(dr["UserFriendUserName"]);
                    element.UserFriendFirstNameDecrypt = EncryptHelper.DecryptString(MySQLHelper.GetStringFromMySQL(dr["UserFriendFirstName"]));
                    element.UserFriendEmailDecrypt = EncryptHelper.DecryptString(MySQLHelper.GetStringFromMySQL(dr["UserFriendEMail"]));
                    element.UserFriendFirstNameDecrypt = EncryptHelper.DecryptString(MySQLHelper.GetStringFromMySQL(dr["UserFriendLastName"]));

                    element.UserSignedUpId = Convert.ToInt32(dr["UserSignedUpId"]);
                    element.UserSignedUpUserName = MySQLHelper.GetStringFromMySQL(dr["UserSignedUpUserName"]);
                    element.UserSignedUpFirstNameDecrypt = EncryptHelper.DecryptString(MySQLHelper.GetStringFromMySQL(dr["UserSignedUpFirstName"]));
                    element.UserSignedUpEmailDecrypt = EncryptHelper.DecryptString(MySQLHelper.GetStringFromMySQL(dr["UserSignedUpEMail"]));
                    element.UserSignedUpFirstNameDecrypt = EncryptHelper.DecryptString(MySQLHelper.GetStringFromMySQL(dr["UserSignedUpLastName"]));

                    result.Add(element);
                }

            }
            catch (Exception e)
            {
                string StrUserId = "NULL";
                if (UserId != null)
                    StrUserId = UserId.ToString();

                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + StrUserId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }




    }
}

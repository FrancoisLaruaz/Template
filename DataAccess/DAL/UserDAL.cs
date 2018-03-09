using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace DataAccess
{
    public class UserDAL
    {
        public UserDAL()
        {

        }



        /// <summary>
        /// Modification of the language preference of the user
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool UpdateLanguageUser(string Language, string UserName)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "update User";
                Query = Query + " set LanguageId=IFNULL(( ";
                Query = Query + " select  id  from category where CategoryTypeId="+ CommonsConst.CategoryTypes.Language.ToString()+ " and code=@Language limit 1 ),LanguageId) ";
                Query = Query + "where LOWER(username) =  LOWER(@username) and id>=1";
                parameters.Add("@UserName", UserName);
                parameters.Add("@Language", Language);
                result = db.ExecuteQuery(Query, parameters);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Language = " + Language+ " and UserName=" + UserName);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Check if the user is registered
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public static bool IsUserRegistered(string UserName)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "select count(*) ";
                Query = Query + "from User where LOWER(UserName)=LOWER(@UserName) ";
                parameters.Add("@UserName", UserName);
                result = GenericDAL.GetSingleNumericData(Query, parameters).Value>0?true:false;

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }



        /// <summary>
        ///  Delete a user
        /// </summary>
        /// <param name="UserToDelete"></param>
        /// <returns></returns>
        public static bool DeleteUserById(User UserToDelete)
        {
            bool result = true;
            DBConnect db = null;
            try
            {
                if (UserToDelete != null)
                {
                    int UserId = UserToDelete.Id;
                    string UserName = UserToDelete.UserName;
                    string UserIdentityId=  UserToDelete.UserIdentityId;

                    db = new DBConnect();
                    db.BeginTransaction();
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    string Query = "delete from scheduledtask where UserId=@UserId;";
                    Query = Query + "update log4net set username=null where username=@UserName;";
                    Query = Query + "update news set LastModificationUserId=null where LastModificationUserId=@UserId;";
                    Query = Query + "delete from emailaudit where UserId=@UserId;";
                    Query = Query + "delete from user where Id=@UserId;";
                    Query = Query + "delete from userclaims where UserId=@UserIdentityId;";
                    Query = Query + "delete from userroles where UserId=@UserIdentityId;";
                    Query = Query + "delete from socialmediaconnection where ProviderKeyUserFriend in  (select providerKey from userlogins where UserId=@UserIdentityId);";
                    Query = Query + "delete from socialmediaconnection where ProviderKeyUserSignedUp in  (select providerKey from userlogins where UserId=@UserIdentityId);";
                    Query = Query + "delete from userlogins where UserId=@UserIdentityId;";
                    Query = Query + "delete from useridentity where Id=@UserIdentityId;";
                     parameters.Add("@UserIdentityId", UserIdentityId);
                   // Query = Query.Replace("@UserIdentityId", MySQLHelper.GetStringToInsert(UserIdentityId));
                    parameters.Add("@UserId", UserId);
                    parameters.Add("@UserName", UserName);
                    result = db.ExecuteQuery(Query, parameters);

                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserToDelete.Id);
            }
            finally
            {
                if (result)
                    result = db.CommitTransaction();
                else
                    db.RollbackTransaction();
                db.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Get the list of the users
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public static List<User> GetUsersList(int? Id=null,string UserName = null)
        {
            List<User> result = new List<User>();
            DBConnect db = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            try
            {
                db = new DBConnect();
                string Query = "select U.Id, U.PictureSrc, U.FirstName, U.LastName ,U.DateOfBirth, U.DateCreation, U.DateModification, UI.DateLastConnection, U.PictureThumbnailSrc ";
                Query = Query + ", U.ReceiveNews,U.GenderId,U.Adress1,U.Adress2,U.Adress3,U.Description, UI.PasswordHash, UI.Email, U.CountryId ";
                Query = Query + ", C.Name as CountryName, UI.EmailConfirmationToken ";
                Query = Query + ", UI.UserName, UI.Id as UserIdentityId, UI.ResetPasswordToken,UI.EmailConfirmed,UI.AccessFailedCount,UI.LockoutEnabled,UI.LockoutEndDateUtc ";
                Query = Query + ",P.Id as ProvinceId, P.Name as ProvinceName ";
                Query = Query + ",L.Id as LanguageId, L.Name as LanguageName, L.Code as LanguageCode ";
                Query = Query + "from user U ";
                Query = Query + "inner join category L on L.Id=U.LanguageId ";
                Query = Query + "inner join useridentity UI on U.username=UI.username ";
                Query = Query + "left join country C on C.Id=U.CountryId ";
                Query = Query + "left join province P on P.Id=U.ProvinceId ";
                Query = Query + " where 1=1 ";
                if (Id!=null && Id.Value>0)
                {
                    Query = Query + " and U.Id=@Id";
                    parameters.Add("@Id", Id);
                }
                if (!String.IsNullOrWhiteSpace(UserName))
                {
                    Query = Query + " and LOWER(U.UserName)=LOWER(@UserName)";
                    parameters.Add("@UserName", UserName);
                }
                Query = Query + " order by U.Id desc";
                
                
                DataTable data = db.GetData(Query, parameters);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    User User = new User();
                    User.FirstName = Convert.ToString(dr["FirstName"]);
                    User.LastName = Convert.ToString(dr["LastName"]);
                    User.EmailConfirmationToken= Convert.ToString(dr["EmailConfirmationToken"]);
                    User.UserIdentityId = Convert.ToString(dr["UserIdentityId"]);
                    User.Id = Convert.ToInt32(dr["Id"]);
                    User.Email = Convert.ToString(dr["Email"]);
                    User.AccessFailedCount = Convert.ToInt32(dr["AccessFailedCount"]);
                    User.DateOfBirth = Commons.MySQLHelper.GetDateFromMySQL(dr["DateOfBirth"]);
                    User.DateModification = Commons.MySQLHelper.GetDateFromMySQL(dr["DateModification"]).Value;
                    User.DateCreation = Commons.MySQLHelper.GetDateFromMySQL(dr["DateCreation"]).Value;
                    User.DateLastConnection = Commons.MySQLHelper.GetDateFromMySQL(dr["DateLastConnection"]).Value;
                    User.LockoutEndDateUtc = Commons.MySQLHelper.GetDateFromMySQL(dr["LockoutEndDateUtc"]);
                    User.LockoutEnabled = Commons.MySQLHelper.GetBoolFromMySQL(dr["LockoutEnabled"]).Value;
                    User.EmailConfirmed = Commons.MySQLHelper.GetBoolFromMySQL(dr["EmailConfirmed"]).Value;
                    User.Adress1 = MySQLHelper.GetStringFromMySQL(dr["Adress1"]);
                    User.PictureSrc = MySQLHelper.GetStringFromMySQL(dr["PictureSrc"]);
                    User.PictureThumbnailSrc = MySQLHelper.GetStringFromMySQL(dr["PictureThumbnailSrc"]);
                    User.Adress2 = MySQLHelper.GetStringFromMySQL(dr["Adress2"]);
                    User.Adress3 = MySQLHelper.GetStringFromMySQL(dr["Adress3"]);
                    User.PasswordHash = Convert.ToString(dr["PasswordHash"]);
                    User.ResetPasswordToken = Convert.ToString(dr["ResetPasswordToken"]);
                    User.UserName = Convert.ToString(dr["UserName"]);
                    User.Description = Convert.ToString(dr["Description"]);
                    User.CountryId= MySQLHelper.GetIntFromMySQL(dr["CountryId"]);
                    User.CountryName = Convert.ToString(dr["CountryName"]);
                    User.ProvinceId= MySQLHelper.GetIntFromMySQL(dr["ProvinceId"]);
                    User.ProvinceName = MySQLHelper.GetStringFromMySQL(dr["ProvinceName"]);
                    User.LanguageId = MySQLHelper.GetIntFromMySQL(dr["LanguageId"]).Value;
                    User.LanguageCode = MySQLHelper.GetStringFromMySQL(dr["LanguageCode"]);
                    User.LanguageName = MySQLHelper.GetStringFromMySQL(dr["LanguageName"]);
                    User.GenderId = MySQLHelper.GetIntFromMySQL(dr["GenderId"]);
                    User.ReceiveNews = MySQLHelper.GetBoolFromMySQL(dr["ReceiveNews"]).Value;
                    User.FirstNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["FirstName"]));
                    User.LastNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["LastName"]));
                    User.EMailDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["EMail"]));
                    User.UserNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["UserName"]));

                    result.Add(User);
                }

            }
            catch (Exception e)
            {
                result = null;
                string strUserName = "NULL";
                if (UserName != null)
                    strUserName = UserName;
                string strId = "NULL";
                if (Id != null)
                    strId = Id.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + strId+ " and UserName = " + strUserName);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }



    }
}

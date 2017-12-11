using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class UserService
    {

        /// <summary>
        /// Check if the user is registered
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public static bool IsUserRegistered(string EMail)
        {
            bool result = false;
            try
            {
                result = UserDAL.IsUserRegistered(EMail);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMail = " + EMail);
            }
            return result;
        }





        public static bool UpdateLanguageUser(string Language, string UserName)
        {
            bool result = false;
            try
            {

                result = UserDAL.UpdateLanguageUser(Language, UserName);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Language = " + Language+ " and UserName = " + UserName);
            }
            return result;
        }

        public static List<User> GeListUsers()
        {
            List<User> result = null;
            try
            {
                result = UserDAL.GetUsersList();
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Get an object to put in the session
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static UserSession GetUserSession(string UserName)
        {
            UserSession result = new UserSession();
            try
            {
                List<User> ListResult = UserDAL.GetUsersList(null, UserName);
                if (ListResult != null && ListResult.Count > 0)
                {
                    User UserLogged = ListResult[0];
                    if(UserLogged!=null)
                    {
                        result.FirstNameDecrypt = UserLogged.FirstNameDecrypt;
                        result.LastNameDecrypt = UserLogged.LastNameDecrypt;
                        result.UserName = UserLogged.UserName;
                        result.UserNameDecrypt = UserLogged.UserNameDecrypt;
                        result.UserIdentityId = UserLogged.UserIdentityId;
                        result.UserId = UserLogged.Id;
                        result.LanguageTag = UserLogged.LanguageCode;
                    }
                }
            }
            catch (Exception e)
            {
                result = new UserSession();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }

        public static User GetUserByUserName(string UserName)
        {
            User result = null;
            try
            {
                List<User> ListResult = UserDAL.GetUsersList(null, UserName);
                if (ListResult != null && ListResult.Count > 0)
                {
                    result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool DeleteUserById(int UserId)
        {
            bool result = false;
            try
            {
                ScheduledTaskService.CancelTaskByUserId(UserId);
                User UserToDelete = GetUserById(UserId);
                if(UserToDelete!=null)
                {
                    result = UserDAL.DeleteUserById(UserToDelete);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return result;
        }



        /// <summary>
        /// Return the specified user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static User GetUserById(int UserId)
        {
            User result = null;
            try
            {
                List<User> ListResult = UserDAL.GetUsersList(UserId);
                if (ListResult != null && ListResult.Count > 0)
                {
                    result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }

    }
}

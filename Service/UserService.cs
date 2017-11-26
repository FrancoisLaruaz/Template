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

        /// <summary>
        /// Check if the user can login
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static bool IsPasswordAndUserMatching(string EMail, string Password)
        {
            bool result = false;
            try
            {
                result = UserDAL.IsPasswordAndUserMatching(EMail, Password);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMail = " + EMail);
            }
            return result;
        }

        /// <summary>
        /// Modification of DateLastConnection
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool UpdateDateLastConnection(int UserId)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("DateLastConnection", DateTime.UtcNow);

                Task.Factory.StartNew(() => GenericDAL.UpdateById("user", UserId, Columns));
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId );
            }
            return result;
        }

        public static bool UpdateLanguageUser(string Language, string UserId)
        {
            bool result = false;
            try
            {

                result = UserDAL.UpdateLanguageUser(Language, Convert.ToInt32(UserId));
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Language = " + Language+ " and UserId = "+ UserId);
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
                result = UserDAL.DeleteUserById(UserId);
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
        public static User GetUserById(string UserId)
        {
            User result = null;
            try
            {
                result = GetUserById(Convert.ToInt32(UserId));
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
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

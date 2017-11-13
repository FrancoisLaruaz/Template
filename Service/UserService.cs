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

        public static User GetUserByEMail(string EMail)
        {
            User result = null;
            try
            {
                List<User> ListResult = UserDAL.GetUsersList(null, EMail);
                if (ListResult != null && ListResult.Count > 0)
                {
                    result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMail = " + EMail);
            }
            return result;
        }

        public static bool DeleteUserById(int UserId)
        {
            bool result = false;
            try
            {
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

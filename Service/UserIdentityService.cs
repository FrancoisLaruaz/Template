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
    public static class UserIdentityService
    {

        /// <summary>
        /// Update the user after an unsuccessfull login
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool UpdateUserIdentityLoginFailure(string UserName)
        {
            bool result = false;
            try
            {
                Task.Factory.StartNew(() => UserIdentityDAL.UpdateUserIdentityLoginFailure(UserName));
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }


    /// <summary>
    /// Set the token of a user 
    /// </summary>
    /// <param name="UserIdentityId"></param>
    /// <param name="Passcode"></param>
    /// <returns></returns>
        public static bool SetPasswordToken(string UserIdentityId,string Token)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("ResetPasswordToken", Token);

                Task.Factory.StartNew(() => GenericDAL.UpdateById("useridentity", UserIdentityId, Columns));
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserIdentityId = " + UserIdentityId);
            }
            return result;
        }

        /// <summary>
        /// Update the user after a successfull login
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool UpdateUserIdentityLoginSuccess(string Id)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("DateLastConnection", DateTime.UtcNow);
                Columns.Add("AccessFailedCount", 0);

                Task.Factory.StartNew(() => GenericDAL.UpdateById("useridentity", Id, Columns));
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id);
            }
            return result;
        }

        /// <summary>
        /// Set theemail of the user as confirmed
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool SetUserEmailAsConfirmed(string Id)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("EmailConfirmationToken", null);
                Columns.Add("EmailConfirmed", true);

                result=GenericDAL.UpdateById("useridentity", Id, Columns);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id);
            }
            return result;
        }



    }
}

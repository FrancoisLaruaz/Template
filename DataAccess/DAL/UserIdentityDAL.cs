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
    public class UserIdentityDAL
    {
        public UserIdentityDAL()
        {

        }


        /// <summary>
        /// Update the user after an unsuccessfull login
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool UpdateUserIdentityLoginFailure(string UserName)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "update Useridentity";
                Query = Query + " set AccessFailedCount= AccessFailedCount + 1";
                Query = Query + " where LOWER(username) =  LOWER(@username)";
                parameters.Add("@UserName", UserName);
                result = db.ExecuteQuery(Query, parameters);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName );
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }


    }
}

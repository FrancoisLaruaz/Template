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
        /// Check if the user is registered
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public static bool IsUserRegistered(string EMail)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select count(*) ";
                Query = Query + "from User where LOWER(Email)=LOWER('"+ EMail + "') ";

                result = GenericDAL.GetSingleNumericData(Query).Value>0?true:false;

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMail = " + EMail);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Check if the user can login
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static bool IsPasswordAndUserMatching(string EMail,string Password)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select count(*) ";
                Query = Query + "from User where Email='" + EMail + "' ";
                Query = Query + " and Password='" + Password + "' ";

                result = GenericDAL.GetSingleNumericData(Query).Value > 0 ? true : false;

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMail = " + EMail);
            }
            finally
            {
                db.Dispose();
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
            bool result = true;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                db.BeginTransaction();
                string Query = "delete from userrole where UserId=" + UserId;
                result= result && db.ExecuteQuery(Query);

                Query = "delete from user where Id=" + UserId;
                result = result && db.ExecuteQuery(Query);

                if (result)
                    db.CommitTransaction();
                else
                    db.RollbackTransaction();
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public static List<User> GetUsersList(int? Id=null,string EMail=null)
        {
            List<User> result = new List<User>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select U.Id, U.FirstName, U.LastName ,U.DateOfBirth, U.DateCreation, U.DateModification ";
                Query = Query + ",U.Adress1,U.Adress2,U.Adress3,U.Description, U.Password, U.Email, U.CountryId ";
                Query = Query + ", C.Name as CountryName";
                Query = Query + "from user U ";
                Query = Query + "left join country C on C.Id=U.CountryId ";
                Query = Query + " where 1=1 ";
                if (Id!=null && Id.Value>0)
                {
                    Query = Query + " and U.Id="+Id.Value.ToString();
                }
                if (!String.IsNullOrWhiteSpace(EMail))
                {
                    Query = Query + " and LOWER(U.EMail)=LOWER('" + EMail+"')";
                }
                Query = Query + " order by U.Id desc";
                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    User User = new User();
                    User.FirstName = Convert.ToString(dr["FirstName"]);
                    User.LastName = Convert.ToString(dr["LastName"]);
                    User.Id = Convert.ToInt32(dr["Id"]);
                    User.Email = Convert.ToString(dr["Email"]);
                    User.DateOfBirth = Commons.MySQLHelper.GetDateFromMySQL(dr["DateOfBirth"]);
                    User.DateModification = Commons.MySQLHelper.GetDateFromMySQL(dr["DateModification"]).Value;
                    User.DateCreation = Commons.MySQLHelper.GetDateFromMySQL(dr["DateCreation"]).Value;
                    User.Adress1 = Convert.ToString(dr["Adress1"]);
                    User.Adress2 = Convert.ToString(dr["Adress2"]);
                    User.Adress3 = Convert.ToString(dr["Adress3"]);
                    User.Password = Convert.ToString(dr["Password"]);
                    User.ResetPasswordToken = Convert.ToString(dr["ResetPasswordToken"]);
                    User.Description = Convert.ToString(dr["Description"]);
                    User.CountryId= MySQLHelper.GetIntFromMySQL(dr["CountryId"]);

                    User.CountryName = Convert.ToString(dr["CountryName"]);
                    User.EmailDecrypt = Commons.EncryptHelper.DecryptString(Convert.ToString(dr["Email"]));
                    User.PasswordDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["Password"]));
                    User.FirstNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["FirstName"]));
                    User.LastNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["LastName"]));

                    result.Add(User);
                }

            }
            catch (Exception e)
            {
                result = null;
                string strEmail = "NULL";
                if (EMail != null)
                    strEmail = EMail;
                string strId = "NULL";
                if (Id != null)
                    strId = Id.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + strId+" and Email = "+strEmail);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }



    }
}

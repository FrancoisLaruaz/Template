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
    public class UserRoleDAL
    {
        public UserRoleDAL()
        {

        }

        /// <summary>
        /// Add a role to the user
        /// </summary>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public static bool  AddUserRole(UserRole UserRole)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "insert into userrole (UserId,RoleId,DateModification) values ( ";
                Query = Query +MySQLHelper.GetIntToInsert(UserRole.UserId);
                Query = Query +","+ MySQLHelper.GetIntToInsert(UserRole.RoleId);
                Query = Query + "," + MySQLHelper.GetDateTimeToInsert(UserRole.DateModification)+")";
                result = db.ExecuteQuery(Query);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserRole.UserId+" and RoleId = "+UserRole.RoleId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }


        /// <summary>
        /// Return a list of user roles
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static List<UserRole> GetUserRoles(int? UserId=null,int? RoleId=null)
        {
            List<UserRole> result = new List<UserRole>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select UR.Id, UR.RoleId, UR.UserId , UR.DateModification ";
                Query = Query + ", C.Name, U.FirstName, U.LastName, U.Email";
                Query = Query + "from userrole UR ";
                Query = Query + "inner  join category C on C.Id=UR.RoleId ";
                Query = Query + "inner  join user U on U.Id=UR.UserId ";
                Query = Query + " where 1=1 " ;
                if(UserId != null && UserId.Value>0)
                    Query = Query + " and UserId = " + UserId.ToString();
                if (RoleId != null && RoleId.Value > 0)
                    Query = Query + " and RoleId = " + RoleId.ToString();

                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    UserRole userRole = new UserRole();
                    userRole.Id = Convert.ToInt32(dr["Id"]);
                    userRole.RoleId = Convert.ToInt32(dr["RoleId"]);
                    userRole.UserId = Convert.ToInt32(dr["UserId"]);
                    userRole.RoleName = Convert.ToString(dr["Name"]);
                    userRole.UserFirstNameDecrypt =  EncryptHelper.DecryptString(Convert.ToString(dr["FirstName"]));
                    userRole.DateModification = Commons.MySQLHelper.GetDateFromMySQL(dr["DateModification"]).Value;
                    userRole.UserLastNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["LastName"]));
                    userRole.UserEMailDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["EMail"]));
                    result.Add(userRole);
                }

            }
            catch (Exception e)
            {
                string strUserId = "NULL";
                if (UserId != null)
                    strUserId = UserId.ToString();
                string strRoleId = "NULL";
                if (RoleId != null)
                    strRoleId = RoleId.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "RoleId = " + strRoleId + " and UserId = " + strUserId);
            }
            finally
            {
                db.Dispose();
            }
            return result;

        }

        /// <summary>
        /// Delete a userrole by UserId and RoleId
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static bool  DeleteUserRoleByUserIdAndRoleId(int UserId,int  RoleId)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "delete from userrole where UserId = " + UserId+" and RoleId = "+RoleId;
                result = db.ExecuteQuery(Query);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Delete all the userroles of the specified user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool DeleteUserRolesByUserId(int UserId)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "delete from userrole where UserId = "+UserId;
                result = db.ExecuteQuery(Query);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }


    }
}

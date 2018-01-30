using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Models.ViewModels;
using Models.Class;

namespace DataAccess
{
    public class UserRolesDAL
    {
        public UserRolesDAL()
        {

        }

        /// <summary>
        /// Add a role to the user
        /// </summary>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public static int AddUserRole(UserRoles UserRoles)
        {
            int insertedId = -1;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "insert into userroles (UserId,RoleId) values ( ";
                Query = Query + MySQLHelper.GetStringToInsert(UserRoles.UserId);
                Query = Query + "," + MySQLHelper.GetStringToInsert(UserRoles.RoleId) + ")";
                insertedId = db.ExecuteInsertQuery(Query);
            }
            catch (Exception e)
            {
                insertedId = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserRoles.UserId + " and RoleId = " + UserRoles.RoleId);
            }
            finally
            {
                db.Dispose();
            }
            return insertedId;
        }


        /// <summary>
        /// Get the list of the roles of a specific user
        /// </summary>
        /// <param name="UserIdentityId"></param>
        /// <returns></returns>
        public static List<UserRoles> GetUserRolesListByUserIdentityId(string UserIdentityId)
        {
            List<UserRoles> result = new List<UserRoles>();
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "select r.id, ur.userid,r.name  ";
                Query = Query + "from userroles ur ";
                Query = Query + " inner join roles r on r.id=ur.roleid ";
                Query = Query + " where userid = " + MySQLHelper.GetStringToInsert(UserIdentityId);
                Query = Query + " order by r.Name";
                DataTable data = db.GetData(Query, parameters);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    UserRoles element = new UserRoles();
                    element.RoleId = Convert.ToString(dr["id"]);
                    element.RoleName = Convert.ToString(dr["name"]);
                    element.UserId = Convert.ToString(dr["UserId"]);

                    result.Add(element);
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserIdentityId = " + UserIdentityId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Get the list of all the roles
        /// </summary>
        /// <returns></returns>
        public static List<Roles> GetRolesList()
        {
            List<Roles> result = new List<Roles>();
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "select r.id, r.name  ";
                Query = Query + "from  roles r ";
                Query = Query + " order by r.Name";
                DataTable data = db.GetData(Query, parameters);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    Roles element = new Roles();
                    element.Id = Convert.ToString(dr["id"]);
                    element.Name = Convert.ToString(dr["name"]);

                    result.Add(element);
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }


        /// <summary>
        /// Return a list of users with their roles in the website
        /// </summary>
        /// <param name="UserIdentityId"></param>
        /// <param name="Pattern"></param>
        /// <param name="StartAt"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DisplayUsersViewModel GetUserRolesList(string UserIdentityId, string Pattern, int StartAt = -1, int PageSize = -1)
        {
            DisplayUsersViewModel model = new DisplayUsersViewModel();
            List<UserRoleItem> ListUserRoles = new List<UserRoleItem>();
            DBConnect db = null;
            try
            {
                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower();

                List<Roles> AllRoles = GetRolesList();

                db = new DBConnect();
                string Query = "select u.username, u.firstname,u.lastname, ui.id as useridentityid, u.id ";
                Query = Query + "from user u ";
                Query = Query + " inner join useridentity ui on ui.username=u.username ";
                Query = Query + "where 1=1 ";
                if (!String.IsNullOrWhiteSpace(UserIdentityId))
                {
                    Query = Query + " and  ui.Id=" + MySQLHelper.GetStringToInsert(UserIdentityId);
                }
                Query = Query + " order by u.id desc";
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    model.Count = db.GetData(Query).Rows.Count;
                    Query = Query + " LIMIT " + PageSize.ToString() + " OFFSET " + StartAt.ToString();
                }

                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    UserRoleItem Item = new UserRoleItem();

                    Item.UserId = Convert.ToInt32(dr["id"]);
                    Item.UseridentityId = Convert.ToString(dr["useridentityid"]);
                    Item.UserNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["userName"]))?.ToLower().Trim();
                    Item.UserFirstNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["FirstName"]))?.ToLower().Trim();
                    Item.UserLastNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["LastName"]))?.ToLower().Trim();

                    Item.UserRolesList = GetUserRolesListByUserIdentityId(Item.UseridentityId);


                    List<UserRoles> UserNotInRoleList = new List<UserRoles>();
                    foreach (Roles role in AllRoles)
                    {
                        string strRole = role.Name;
                        if(!Item.UserRolesList.Where(a => a.RoleName== role.Name).Any())
                        {
                            UserRoles userRole = new UserRoles();
                            userRole.RoleName = role.Name;
                            userRole.RoleId = role.Id;
                            userRole.UserId = Item.UseridentityId;
                            UserNotInRoleList.Add(userRole);
                        }
                    }
                    Item.UserNotInRoleList = UserNotInRoleList;



                    ListUserRoles.Add(Item);
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<UserRoleItem> resultIEnumerable = ListUserRoles as IEnumerable<UserRoleItem>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.UserRolesList.ToList().Where(r => r.RoleName.ToLower().Contains(Pattern.ToLower())).Any()) || (a.UserFirstNameDecrypt != null && a.UserFirstNameDecrypt.Contains(Pattern)) || a.UserLastNameDecrypt.Contains(Pattern) || (a.UserNameDecrypt != null && a.UserNameDecrypt.Contains(Pattern)));
                    model.Count = resultIEnumerable.ToList().Count;
                    ListUserRoles = resultIEnumerable.Take(PageSize).Skip(StartAt).OrderByDescending(a => a.UserFirstNameDecrypt).OrderByDescending(a => a.UserLastNameDecrypt).ToList();
                }

                model.UserRolesList = ListUserRoles;
            }
            catch (Exception e)
            {
                string strPattern = "NULL";
                if (Pattern != null)
                {
                    strPattern = Pattern;
                }
                string strUserIdentityId = "NULL";
                if (UserIdentityId != null)
                {
                    strUserIdentityId = UserIdentityId;
                }
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + strPattern + " and UserIdentityId= " + strUserIdentityId);
            }
            finally
            {
                db.Dispose();
            }
            return model;
        }

        /// <summary>
        /// Delete a userrole by UserId and RoleId
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static bool DeleteUserRoleByUserIdAndRoleId(string UserId, string RoleId)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "delete from userroles where UserId = " + MySQLHelper.GetStringToInsert(UserId) + " and RoleId = " + MySQLHelper.GetStringToInsert(RoleId);
                result = db.ExecuteQuery(Query);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId + " and RoleId = " + RoleId);
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
        public static bool DeleteUserRolesByUserId(string UserId)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "delete from userroles where UserId = " + MySQLHelper.GetStringToInsert(UserId);
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

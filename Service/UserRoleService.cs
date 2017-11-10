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
    public static class UserRoleService
    {

  
        public static List<UserRole> GeListUserRoles()
        {
            List<UserRole> result = null;
            try
            {
                result = UserRoleDAL.GetUserRoles();
            }
            catch (Exception e)
            {
                result = new List<UserRole>();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Return the list of all the roles of the specified user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static List<UserRole> GeListUserRolesByUserId(int UserId)
        {
            List<UserRole> result = null;
            try
            {
                result = UserRoleDAL.GetUserRoles(UserId,null);
            }
            catch (Exception e)
            {
                result = new List<UserRole>();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = "+ UserId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Add a role to the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static bool AddUserRole(int UserId,int RoleId)
        {
            bool result = false;
            try
            {
                UserRole UserRole = new UserRole();
                UserRole.UserId = UserId;
                UserRole.RoleId = RoleId;
                UserRole.DateModification = DateTime.UtcNow;
                result=DeleteUserRoleByUserIdAndRoleId(UserId, RoleId);
                if(result)
                    result = UserRoleDAL.AddUserRole(UserRole);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = "+ UserId+" and RoleId = " + RoleId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Return a list of all the user having the specified role
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static List<UserRole> GeListUserRolesByRoleId(int RoleId)
        {
            List<UserRole> result = null;
            try
            {
                result = UserRoleDAL.GetUserRoles(null, RoleId);
            }
            catch (Exception e)
            {
                result = new List<UserRole>();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "RoleId = " + RoleId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Delete all the role of the specified user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool DeleteUserRolesByUserId(int UserId)
        {
            bool result = false;
            try
            {
                result = UserRoleDAL.DeleteUserRolesByUserId(UserId);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Delete a userrole by userid and roleid
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static bool DeleteUserRoleByUserIdAndRoleId(int UserId,int RoleId)
        {
            bool result = false;
            try
            {
                result = UserRoleDAL.DeleteUserRoleByUserIdAndRoleId(UserId, RoleId);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString()+ " and RoleId = "+ RoleId);
            }
            return result;
        }

        /// <summary>
        /// Delete the specified user role
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DeleteUserRoleById(int Id)
        {
            bool result = false;
            try
            {
                result = GenericDAL.DeleteById("userrole", Id);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return result;
        }

    }
}

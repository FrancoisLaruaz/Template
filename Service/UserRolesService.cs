using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class UserRolesService
    {

        /// <summary>
        /// Get the list of the users with their roles
        /// </summary>
        /// <param name="Pattern"></param>
        /// <param name="StartAt"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DisplayUserRolesViewModel GetDisplayUserRolesViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayUserRolesViewModel model = new DisplayUserRolesViewModel();
            try
            {
                model = UserRolesDAL.GetUserRolesList(null,Pattern, StartAt, PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }

        /// <summary>
        /// Get the user roles informations for one user
        /// </summary>
        /// <param name="UserIdentity"></param>
        /// <returns></returns>
        public static UserRoleItem GetUserRolesByUseridentityId(string UserIdentity)
        {
            DisplayUserRolesViewModel model = new DisplayUserRolesViewModel();
            try
            {
                model = UserRolesDAL.GetUserRolesList(UserIdentity, null,0,1);
                if(model!=null)
                {
                    return model.UserRolesList[0];
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserIdentity = " + UserIdentity);
            }
            return new UserRoleItem() ;
        }

        /// <summary>
        /// Add a role to the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static bool AddUserRole(string UserId,string RoleId)
        {
            bool result = false;
            try
            {
                UserRoles UserRole = new UserRoles();
                UserRole.UserId = UserId;
                UserRole.RoleId = RoleId;

                result=DeleteUserRoleByUserIdAndRoleId(UserId, RoleId);
                if(result)
                    result = UserRolesDAL.AddUserRole(UserRole) >= 0?true:false;
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = "+ UserId+" and RoleId = " + RoleId.ToString());
            }
            return result;
        }




        /// <summary>
        /// Delete a userrole by userid and roleid
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static bool DeleteUserRoleByUserIdAndRoleId(string UserId, string RoleId)
        {
            bool result = false;
            try
            {
                result = UserRolesDAL.DeleteUserRoleByUserIdAndRoleId(UserId, RoleId);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString()+ " and RoleId = "+ RoleId);
            }
            return result;
        }



    }
}

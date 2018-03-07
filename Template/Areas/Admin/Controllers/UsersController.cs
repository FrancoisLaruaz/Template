﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Commons;
using Models.Class;
using Service;
using Models.ViewModels;
using Quartz;
using Quartz.Impl;
using Service.TaskClasses;
using Quartz.Impl.Matchers;
using Models.Class.TaskSchedule;
using Website.Controllers;

namespace Website.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            UsersViewModel Model = new UsersViewModel();
            try
            {
                ViewBag.Title = "[[[Users]]]";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult _DisplayUsers(DisplayUsersViewModel Model)
        {
            try
            {
                Model = UserRolesService.GetDisplayUsersViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView(Model);
        }


        [HttpPost]
        public ActionResult AddUserRole(string roleid, string userid)
        {
            bool _success = false;
            string _error = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(roleid) && !String.IsNullOrWhiteSpace(userid))
                {
                    _success = UserRolesService.AddUserRole(userid, roleid);
                }
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "roleid = " + roleid + " and userid = " + userid);
            }
            return Json(new { Result = _success, Error = _error });
        }

        [HttpPost]
        public ActionResult DeleteUserRole(string roleid, string userid)
        {
            bool _success = false;
            string _error = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(roleid) && !String.IsNullOrWhiteSpace(userid))
                {
                    _success = UserRolesService.DeleteUserRoleByUserIdAndRoleId(userid, roleid);
                }
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "roleid = " + roleid + " and userid = " + userid);
            }
            return Json(new { Result = _success, Error = _error });
        }

        [HttpPost]
        public ActionResult DeleteUser(int UserId)
        {
            bool _success = false;
            string _error = "";
            try
            {

                _success = UserService.DeleteUserById(UserId);
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return Json(new { Result = _success, Error = _error });
        }


        [HttpPost]
        public ActionResult _DisplayUsersModifications(string UserIdentityId)
        {
            UserRoleItem Model = new UserRoleItem();
            try
            {
                Model.UseridentityId = UserIdentityId;
                Model = UserRolesService.GetUserRolesByUseridentityId(UserIdentityId);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserIdentityId = " + UserIdentityId);
                return Content("ERROR");
            }
            return PartialView(Model);
        }
    }
}
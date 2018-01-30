using System;
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

namespace Website.Controllers
{
    public class AdminController : BaseController
    {



        [HttpGet]
        public ActionResult Index()
        { 
            return RedirectToAction("Logs");
        }

        #region UserRoles

        [HttpGet]
        public ActionResult Users()
        {
            UsersViewModel Model = new UsersViewModel();

            try
            {
                ViewBag.Title = "[[[User Roles]]]";
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
            return PartialView("~/Views/Admin/Users/_DisplayUsers.cshtml", Model);
        }


        [HttpPost]
        public ActionResult AddUserRole(string roleid,string userid)
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
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "roleid = " + roleid+" and userid = "+userid);
            }
            return Json(new { Result = _success, Error =_error });
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
            return PartialView("~/Views/Admin/Users/_DisplayUsersModifications.cshtml", Model);
        }



        #endregion

        #region NewsLetter

        [HttpGet]
        public ActionResult News()
        {
            NewsViewModel Model = new NewsViewModel();

            try
            {
                ViewBag.Title = "[[[News Letter]]]";
                Model = NewsService.GetNewsViewModel();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View(Model);
        }


        [HttpGet]
        public ActionResult EditNews(int? Id)
        {
            NewsEditViewModel Model = new NewsEditViewModel();

            try
            {
                if (Id!=null && Id > 0)
                {
                    ViewBag.Title = "[[[News Letter Edit]]]";
                }
                else
                {
                    ViewBag.Title = "[[[News Letter Creation]]]";
                }
                ViewBag.NewsId = Id;
                Model = NewsService.GetNewsEditViewModel(Id);
                Model.NewsDescription = "<p style='color:red'>hahah</p>";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = "+ Id);
            }
            return View("~/Views/Admin/News/EditNews.cshtml", Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditNews(NewsEditViewModel Model)
        {
            bool _success = false;
            string _Error = "";
            try
            {
                if (ModelState.IsValid)
                {
                    _success = true;
                }
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Model.Id);
            }
            return Json(new { Result = _success,  Error = _Error});
        }



        #endregion

        #region EmailAudits

        [HttpGet]
        public ActionResult EmailAudits()
        {
            EmailAuditViewModel model = new EmailAuditViewModel();
            try
            {

                ViewBag.Title = "Email Audits";

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult _DisplayEmailAudits(DisplayEmailAuditViewModel Model)
        {
            try
            {
                Model = EMailService.GetDisplayEmailAuditViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Model.Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView("~/Views/Admin/EMailAudits/_DisplayEmailAudits.cshtml", Model);
        }



        #endregion

        #region Logs

        [HttpGet]
        public ActionResult Logs()
        {
            LogsViewModel Model = new LogsViewModel();

            try
            {
                ViewBag.Title = "Logs";
                Model = LogService.GetLogsViewModel();
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult _DisplayLogs(DisplayLogsViewModel Model)
        {
            try
            {
                Model = LogService.GetDisplayLogsViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Model.Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView("~/Views/Admin/Logs/_DisplayLogs.cshtml", Model);
        }

        #endregion

        #region Task
        [HttpGet]
        public ActionResult Tasks()
        {
            SchedulerStatusViewModel Model = new SchedulerStatusViewModel();
            try
            {
                ViewBag.Title = "[[[Tasks]]]";
                //  ScheduledTaskService.ScheduleEMailUserTask(21, CommonsConst.EmailType.UserWelcome, TimeSpan.FromSeconds(200));
               //   ScheduledTaskService.ScheduleEMailUserTask(21, CommonsConst.EmailType.UserWelcome, TimeSpan.FromSeconds(300));

                Model = TaskHelper.GetSchedulerInformation();
                Model.ScheduledTasksNumberInDatabase = ScheduledTaskService.GetActiveScheduledTasksNumber();
                Model.ScheduledTasksProblemsNumber = ScheduledTaskService.GetNotExecutedTasksNumber();

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View(Model);
        }


        [HttpPost]
        public ActionResult _DisplayTasks(DisplayTasksViewModel Model)
        {
            try
            {
                Model = TaskLogService.GetDisplayTasksViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Model.Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView("~/Views/Admin/Tasks/_DisplayTasks.cshtml", Model);
        }


        /// <summary>
        /// Reset the tasks from the databse data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetTasks()
        {
            bool _success = false;
            try
            {
                _success=ScheduledTaskService.SetScheduledTasks();
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Json(new { Result = _success });
        }
        #endregion
    }
}

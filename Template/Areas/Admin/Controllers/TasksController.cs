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
using Website.Controllers;
using Service.UserArea.Interface;

namespace Website.Areas.Admin.Controllers
{
    public class TasksController : BaseController
    {



        public TasksController(
            IUserService userService
            ) : base(userService)
        {

        }


        [HttpGet]
        public ActionResult Index()
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
            return PartialView(Model);
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
                _success = ScheduledTaskService.SetScheduledTasks();
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Json(new { Result = _success });
        }
    }
}
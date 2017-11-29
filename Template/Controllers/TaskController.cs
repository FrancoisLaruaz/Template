using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Commons;
using System.Net;
using Service;
using Models.BDDObject;
using Revalee.Client;
using System.Configuration;

namespace Website.Controllers
{
    public class TaskController : Controller
    {

        /// <summary>
        /// Send a mail to a user
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="UserId"></param>
        /// <param name="EMailTypeId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SendMailToUser(int Id, int UserId, int EMailTypeId)
        {
            HttpStatusCodeResult result = new HttpStatusCodeResult(HttpStatusCode.Found);

            try
            {
                if (!RevaleeRegistrar.ValidateCallback(Request))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                if (UserId <= 0 || EMailTypeId <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ScheduledTask Task = ScheduledTaskService.GetScheduledTaskById(Id);
                if (Task != null && Task.CancellationDate == null && Task.ExecutionDate == null)
                {
                    Task.ExecutionDate = DateTime.UtcNow;
                    ScheduledTaskService.SetTaskAsExecuted(Task.Id);


                    if (EMailService.SendEMailToUser(UserId, EMailTypeId))
                    {
                        result = new HttpStatusCodeResult(HttpStatusCode.OK);
                    }
                    else
                    {
                        result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
            }
            catch (Exception e)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id= " + Id + " and UserId = " + UserId + " and EMailTypeId = " + EMailTypeId);
            }
            return result;
        }

        /// <summary>
        /// Delete the old logs
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult DeleteLogs()
        {
            HttpStatusCodeResult result = new HttpStatusCodeResult(HttpStatusCode.Found);
            TaskLog Task = new TaskLog();
            try
            {
                Task.TypeId = Commons.TaskLogTypes.ErrorCleanUp;
                Task.Id=TaskLogService.InsertTaskLog(Task);
                if(LogService.DeleteLogs())
                {
                    Task.Result = true;
                }
                else
                {
                    Task.Result = false;
                }
                result = new HttpStatusCodeResult(HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                Task.Result = false;
                Task.Comment = e.ToString();
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            finally
            {
                Task.EndDate = DateTime.UtcNow;
                TaskLogService.UpdateTaskLog(Task);
            }
            return result;
        }
    

    }
}

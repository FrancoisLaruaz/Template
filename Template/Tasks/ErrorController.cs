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

namespace Tasks.Controllers
{
    public class ErrorController : Controller
    {

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
                Task.EndDate = DateTime.UtcNow;
                TaskLogService.UpdateTaskLog(Task);
            }
            catch (Exception e)
            {
                Task.Result = false;
                Task.Comment = e.ToString();
                Task.EndDate = DateTime.UtcNow;
                TaskLogService.UpdateTaskLog(Task);
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    

    }
}

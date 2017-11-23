using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Revalee;
using Revalee.Client;
using System.Configuration;
using Models;
using Models.BDDObject;

namespace Commons
{
    public static class TaskHelper
    {

        private static string WebsiteURL = ConfigurationManager.AppSettings["Website"];
        private static string BaseCallBackUrl = WebsiteURL + "/Tasks/Base/Execute/";

        /// <summary>
        ///  Schedule a task and return the guid
        /// </summary>
        /// <param name="callBackUrl"></param>
        /// <param name="callBackDelay"></param>
        /// <returns></returns>
        public static string ScheduleTask(string callBackUrl, TimeSpan callBackDelay)
        {
            Guid? TaskGuid = null;
            string result = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(callBackUrl))
                {
                    var callbackUri = new Uri(callBackUrl);
                    TaskGuid = RevaleeRegistrar.ScheduleCallback(callBackDelay, callbackUri);
                    result = TaskGuid.ToString();
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "callBackUrl = " + callBackUrl);
            }
            return result;
        }


        /// <summary>
        /// Cancellation of a task
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static bool CancelTask(ScheduledTask Task)
        {
            Guid? TaskGuid = null;
            bool result = true;
            try
            {
                if (Task != null)
                {
                    TaskGuid = new Guid(Task.CallbackId);
                    Uri TaskUri = new Uri(Task.CallbackUrl);
                    result=RevaleeRegistrar.CancelCallback(TaskGuid.Value, TaskUri);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TaskId = " + Task.Id);
            }
            return result;
        }

    }
}
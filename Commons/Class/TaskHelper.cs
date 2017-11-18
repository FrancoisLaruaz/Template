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
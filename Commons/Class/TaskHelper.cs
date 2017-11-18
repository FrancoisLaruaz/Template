using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Revalee;
using Revalee.Client;
using System.Configuration;


namespace Commons
{

    public static class TaskHelper
    {
        private static string WebsiteURL = ConfigurationManager.AppSettings["Website"];


        public static bool CancelTask(Task Task)
        {
            Guid? TaskGuid = null;
            bool result = true;
            try
            {
                if (Task != null)
                {
                    TaskGuid = new Guid(Task.CallbackId);
                    Uri TaskUri = new Uri(Task.CallbackUrl);
                    if (RevaleeRegistrar.CancelCallback(TaskGuid.Value, TaskUri))
                    {
                        Task.CancellationDate = DateTime.UtcNow;
                        result = _RevaleeTaskService.UpdateRevaleeTask(Task);
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email + " and FirstName = " + model.FirstName + " and LastName = " + model.LastName);
            }
            return result;
        }

    }
}
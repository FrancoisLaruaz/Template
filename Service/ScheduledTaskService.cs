using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Service
{
    public static class ScheduledTaskService
    {
        private static string WebsiteURL = ConfigurationManager.AppSettings["Website"];


        /// <summary>
        /// Schedule a mail for a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="EMailTypeId"></param>
        /// <returns></returns>
        public static bool ScheduleEMailUserTask(int UserId,int EMailTypeId,TimeSpan callbackDelay)
        {
            bool Result = false;
            try
            {
                ScheduledTask Task = new ScheduledTask();
                Task.UserId = null;
                Task.EmailTypeId = EMailTypeId;
                Task.CreationDate = DateTime.UtcNow;
                Task.ExpectedExecutionDate = Task.CreationDate.Add(callbackDelay);
                Task.CallbackUrl = WebsiteURL+ "/Tasks/EMail/SendMailToUser?Id=0&UserId=1&EMailTypeId=" + Task.EmailTypeId;
                Task.Id = InsertScheduledTask(Task);
                Task.CallbackId = Commons.TaskHelper.ScheduleTask(Task.CallbackUrl, callbackDelay);
                Task.CallbackUrl = WebsiteURL + "/Tasks/EMail/SendMailToUser?Id=" + Task.Id+"&UserId=1&EMailTypeId=" + Task.EmailTypeId;
                if (!String.IsNullOrWhiteSpace(Task.CallbackId))
                {
                    Result = true;
                    ScheduledTaskService.UpdateCallbackIdAndCallBackUrl(Task.Id, Task.CallbackUrl,Task.CallbackId);
                }
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString()+ " and EMailTypeId = "+ EMailTypeId);
            }
            return Result;
        }

        public static List<ScheduledTask> GetScheduledTasksListByUser(int UserId)
        {
            List<ScheduledTask> Result = new List<ScheduledTask>();
            try
            {
                Result = ScheduledTaskDAL.GetScheduledTasksList(UserId, null);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return Result;
        }

        public static ScheduledTask GetScheduledTaskById(int Id)
        {
            ScheduledTask Result = null;
            try
            {
                List<ScheduledTask> ListResult = ScheduledTaskDAL.GetScheduledTasksList(null, Id);
                if (ListResult != null && ListResult.Count > 0)
                {
                    Result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return Result;
        }

        /// <summary>
        /// Update The task
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CallbackUrl"></param>
        /// <param name="CallbackId"></param>
        /// <returns></returns>
        public static bool UpdateCallbackIdAndCallBackUrl(int Id,string CallbackUrl, string CallbackId)
        {
            bool Result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("CallbackId", CallbackId);
                Columns.Add("CallbackUrl", CallbackUrl);
                Result = GenericDAL.UpdateById("scheduledtask", Id, Columns);
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString()+ " and CallbackId = "+ CallbackId);
            }
            return Result;
        }

        public static bool SetTaskAsExecuted(int Id)
        {
            bool Result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("ExecutionDate", DateTime.UtcNow);
                Result = GenericDAL.UpdateById("scheduledtask", Id, Columns);
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return Result;
        }

        /// <summary>
        /// Cancel a Task by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CancelTaskById(int Id)
        {
            bool Result = false;
            try
            {
                ScheduledTask Task = GetScheduledTaskById(Id);
                Result = Commons.TaskHelper.CancelTask(Task);

                if (Result)
                {
                    Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                    Columns.Add("CancellationDate", DateTime.UtcNow);
                    Result = GenericDAL.UpdateById("scheduledtask", Id, Columns);
                }
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return Result;
        }


        /// <summary>
        /// Cancel wll the scheduled task of a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool CancelTaskByUserId(int UserId)
        {
            bool Result = false;
            try
            {
                List<ScheduledTask> TaskList = GetScheduledTasksListByUser(UserId);
                foreach(var Task in TaskList)
                {
                    Result = Result && CancelTaskById(Task.Id);
                }
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return Result;
        }


        /// <summary>
        /// Insertion of a row in scheduledtask table
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static int InsertScheduledTask(ScheduledTask Task)
        {
            int Result = -1;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("CallbackId", Task.CallbackId);
                Columns.Add("CallbackUrl", Task.CallbackUrl);
                Columns.Add("UserId", Task.UserId);
                Columns.Add("ExpectedExecutionDate", Task.ExpectedExecutionDate);
                Columns.Add("EmailTypeId", Task.EmailTypeId);
                Columns.Add("CreationDate", DateTime.UtcNow);
                Result = GenericDAL.InsertRow("scheduledtask", Columns);
            }
            catch (Exception e)
            {
                Result = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Result;
        }

    }
}

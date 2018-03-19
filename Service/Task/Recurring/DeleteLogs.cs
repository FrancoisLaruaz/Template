using Quartz;
using System;
using System.Net;
using System.Net.Mail;
using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Admin;

namespace Service.TaskClasses
{
    public class DeleteLogs : RecurringJobBase
    {
        LogService _LogService { get; set; }

        public DeleteLogs()
        {
             _LogService = new LogService();
        }

        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                base.Execute(context);
                bool Result= _LogService.DeleteLogs();
                TaskLog Log = new TaskLog();
                Log.Id = LogId;
                Log.Result = Result;
                Log.Comment="N/A";
                TaskLogService.UpdateTaskLog(Log);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}
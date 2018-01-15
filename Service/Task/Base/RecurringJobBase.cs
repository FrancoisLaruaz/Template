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

namespace Service.TaskClasses
{
    public class RecurringJobBase : IJob
    {
        public int LogId { get; set; }

        public RecurringJobBase()
        {

        }



        public virtual void Execute(IJobExecutionContext context)
        {
            try
            {
                TaskLog Log = new TaskLog();
                Log.CallbackId= context.JobDetail.Key.Name;
                Log.GroupName = context.JobDetail.Key.Group;
                LogId =TaskLogService.InsertTaskLog(Log);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + context.JobDetail.Key.Name);
            }
        }
    }
}
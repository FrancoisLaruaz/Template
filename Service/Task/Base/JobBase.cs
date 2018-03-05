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
    public class JobBase : IJob
    {

        public Boolean NeedToBeExectuted { get; set; }

        public JobBase()
        {
            NeedToBeExectuted = false;
        }



        public virtual void Execute(IJobExecutionContext context)
        {
            try
            {
                string CallBackId = context.JobDetail.Key.Name;
                NeedToBeExectuted = ScheduledTaskService.IsScheduledTaskActive(CallBackId);
                if (NeedToBeExectuted)
                {
                    ScheduledTaskService.SetTaskAsExecuted(CallBackId);
                }
                else
                {
                    Logger.GenerateInfo("Task not active triggered :  "+ CallBackId);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + context.JobDetail.Key.Name);
            }
        }
    }
}
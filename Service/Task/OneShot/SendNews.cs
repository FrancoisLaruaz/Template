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
    public class SendNews : JobBase
    {

        public int NewsId { get; set; }

        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                base.Execute(context);
                if (NeedToBeExectuted && context.MergedJobDataMap.Get("NewsId")!=null)
                {
                    NewsId = Convert.ToInt32(context.MergedJobDataMap.Get("NewsId"));

                    NewsService.SendNews(NewsId);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
        }
    }
}
using Commons;
using DataAccess;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Service.Admin.Interface;

namespace Service.Admin
{
    public  class LogService : ILogService
    {
        private readonly IGenericRepository<Log4Net> _logRepo;

        public LogService(IGenericRepository<Log4Net> logRepo)
        {
            _logRepo = logRepo;
        }

        public LogService()
        {
            var context = new TemplateEntities();
            _logRepo = new GenericRepository<Log4Net>(context);
        }

        public  LogsViewModel GetLogsViewModel()
        {
            LogsViewModel model = new LogsViewModel();
            try
            {
                using (DBConnect db = new DBConnect())
                {
                    model.InfoConnectionBBD = db.CheckConnection();
                }

            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return model;
        }


        /// <summary>
        /// Delete old logs
        /// </summary>
        /// <returns></returns>
        public bool DeleteLogs()
        {
            bool result = false;
            try
            {

                DateTime DateToCompare = DateTime.UtcNow.AddMonths(-3);
                List<Log4Net> List = _logRepo.FindAllBy(l => l.Date < DateToCompare).ToList();
                foreach (Log4Net Log in List)
                {
                    _logRepo.Delete(Log.Id);
                }
                result=_logRepo.Save();
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

       
        public  DisplayLogsViewModel GetDisplayLogsViewModel(string Pattern,int StartAt,int  PageSize)
        {
            DisplayLogsViewModel model = new DisplayLogsViewModel();
            try
            {
                model = LogDAL.GetLogsList(Pattern, StartAt, PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"Pattern = " + Pattern);
            }
            return model;
        }
    }
}

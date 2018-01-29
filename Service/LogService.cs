using Commons;
using DataAccess;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;

namespace Service
{
    public static class LogService
    {
        public static LogsViewModel GetLogsViewModel()
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
        public static bool DeleteLogs()
        {
            bool result = false;
            try
            {
                result = LogDAL.DeleteLogs();
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

       
        public static DisplayLogsViewModel GetDisplayLogsViewModel(string Pattern,int StartAt,int  PageSize)
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

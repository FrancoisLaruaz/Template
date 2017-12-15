using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Models.Class;
using Models.ViewModels;

namespace DataAccess
{
    public class LogDAL 
    {
        public LogDAL()
        {

        }


        /// <summary>
        /// Delete old logs
        /// </summary>
        /// <returns></returns>
        public static bool DeleteLogs()
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                DateTime DateToCompare = DateTime.UtcNow.AddMonths(-1);
                string Query = "delete from log4net";
                Query = Query + " where Date < "+MySQLHelper.GetDateTimeToInsert(DateToCompare);

                result = db.ExecuteQuery(Query);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }



        /// </summary>
        /// <returns>Fonction permettant de récupérer les logs applicatives</returns>
        /// </summary>
        /// <param name="Pattern"></param>
        /// <returns></returns>
        public static DisplayLogsViewModel GetLogsList(string Pattern, int StartAt=-1, int PageSize=-1)
        {
            DisplayLogsViewModel model = new DisplayLogsViewModel();
            List<Log4Net> LogList = new List<Log4Net>();
            DBConnect db = null;
            try
            {
                model.Pattern = Pattern;
                model.StartAt = StartAt;
                model.PageSize = PageSize;
                db = new DBConnect();
                string Query = "select Id, Level, Thread ,Date ";
                Query = Query + ",Logger,Message,Exception,UserName ";
                Query = Query + "from log4net where 1=1 ";

                Query = Query + "order by Id desc";
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    model.Count = db.GetData(Query).Rows.Count;
                    Query = Query + " LIMIT " + PageSize.ToString() + " OFFSET " + StartAt.ToString();
                }

                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    Log4Net log = new Log4Net();
                    log.Level = Convert.ToString(dr["Level"]);
                    log.Id = Convert.ToInt32(dr["Id"]);
                    log.Thread = Convert.ToString(dr["Thread"]);
                    log.Date = Convert.ToDateTime(dr["Date"]);
                    log.Logger = Convert.ToString(dr["Logger"]);
                    log.Message = Convert.ToString(dr["Message"]);
                    log.Exception = Convert.ToString(dr["Exception"]);
                    log.UserName = EncryptHelper.DecryptString(Convert.ToString(dr["UserName"]));
                    LogList.Add(log);
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<Log4Net> resultIEnumerable = LogList as IEnumerable<Log4Net>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.UserName != null && a.UserName.Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.Level.Contains(Pattern) || (a.Exception != null && a.Exception.Contains(Pattern)) || (a.Logger != null && a.Logger.Contains(Pattern)) || (a.Message != null && a.Message.Contains(Pattern) || (a.Thread != null && a.Message.Contains(Pattern))));
                    model.Count = resultIEnumerable.ToList().Count;
                    LogList = resultIEnumerable.Take(PageSize).Skip(StartAt).OrderByDescending(a => a.Id).ToList();
                }
                model.LogsList = LogList;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = "+ Pattern);
            }
            finally
            {
                db.Dispose();
            }
            return model;
        }

    }
}

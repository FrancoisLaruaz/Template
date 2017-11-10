using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace DataAccess
{
    public class Log4NetDAL 
    {
        public Log4NetDAL()
        {

        }


        /// <summary>
        /// Get the number of errors matching the pattern
        /// </summary>
        /// <param name="Pattern"></param>
        /// <returns></returns>
        public static int GetLogsCount(string Pattern)
        {
            int result = 0;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select count(*) ";
                Query = Query + "from log4net where 1=1 ";
                if (!string.IsNullOrEmpty(Pattern))
                {
                    Query = Query + " and (Level like '%" + Pattern + "%'";
                    Query = Query + " or Thread like '%" + Pattern + "%'";
                    Query = Query + " or Message like '%" + Pattern + "%'";
                    Query = Query + " or UserLogin like '%" + Pattern + "%'";
                    Query = Query + " or concat(id, '') like '%" + Pattern + "%'";
                    Query = Query + " or Exception like '%" + Pattern + "%' )";
                }
                Query = Query + "order by Id desc";
                result = GenericDAL.GetSingleNumericData(Query).Value;

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
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
        public static List<Log4Net> GetLogsList(string Pattern, int StartAt=-1, int PageSize=-1)
        {
            List<Log4Net> result = new List<Log4Net>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select Id, Level, Thread ,Date ";
                Query = Query + ",Logger,Message,Exception,UserLogin ";
                Query = Query + "from log4net where 1=1 ";
                if(!string.IsNullOrEmpty(Pattern))
                {
                    Query = Query + " and (Level like '%"+Pattern+"%'";
                    Query = Query + " or Thread like '%" + Pattern + "%'";
                    Query = Query + " or Message like '%" + Pattern + "%'";
                    Query = Query + " or UserLogin like '%" + Pattern + "%'";
                    Query = Query + " or concat(id, '') like '%" + Pattern + "%'";
                    Query = Query + " or Exception like '%" + Pattern + "%' )";
                }
                Query = Query + "order by Id desc";
                if (StartAt >= 0 && PageSize >= 0)
                    Query = Query + " LIMIT " + PageSize.ToString() + " OFFSET " + StartAt.ToString();

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
                    log.UserLogin = Convert.ToString(dr["UserLogin"]);
                    result.Add(log);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = "+ Pattern);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

    }
}

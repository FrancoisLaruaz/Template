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
    public class TaskLogDAL
    {
        public TaskLogDAL()
        {

        }

        /// <summary>
        /// Count the rows of the table
        /// </summary>
        /// <param name="Pattern"></param>
        /// <returns></returns>
        public static int GetTaskLogsCount(string Pattern)
        {
            int result = 0;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select count(*) ";
                Query = Query + "from tasklog l ";
                Query = Query + "inner join category c on c.Id=l.TypeId  where 1=1  ";
                if (!string.IsNullOrEmpty(Pattern))
                {
                    Query = Query + " and (Comment like '%" + Pattern + "%'";
                    Query = Query + " or C.Name like '%" + Pattern + "%'";
                    Query = Query + " or concat(l.Id, '') like '%" + Pattern + "%')";
                }
                result = GenericDAL.GetSingleNumericData(Query,null).Value;

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


        /// <summary>
        ///  Get the logs of the scheduled tasks    
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public static List<TaskLog> GetTaskLogsList(int? TypeId, string Pattern=null, int StartAt = -1, int PageSize = -1)
        {
            List<TaskLog> result = new List<TaskLog>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select l.Id, l.TypeId, l.StartDate,l.EndDate,l.Result,l.Comment ";
                Query = Query + ", C.Name as TypeName ";
                Query = Query + "from tasklog l ";
                Query = Query + "inner join category c on c.Id=l.TypeId ";
                Query = Query + " where 1=1 ";
                if (TypeId != null && TypeId.Value>0)
                {
                    Query = Query + " and l.TypeId="+ TypeId.Value.ToString();
                }
                if (!string.IsNullOrEmpty(Pattern))
                {
                    Query = Query + " and (Comment like '%" + Pattern + "%'";
                    Query = Query + " or C.Name like '%" + Pattern + "%'";
                    Query = Query + " or concat(l.Id, '') like '%" + Pattern + "%')";
                }
                Query = Query + " order by l.Id desc";
                if (StartAt >= 0 && PageSize >= 0)
                    Query = Query + " LIMIT " + PageSize.ToString() + " OFFSET " + StartAt.ToString();

                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    TaskLog element = new TaskLog();
                    element.Id = Convert.ToInt32(dr["Id"]);
                    element.TypeId = MySQLHelper.GetIntFromMySQL(dr["TypeId"]).Value;
                    element.TypeName = Convert.ToString(dr["TypeName"]);
                    element.Result = MySQLHelper.GetBoolFromMySQL(dr["Result"]);
                    element.StartDate = Commons.MySQLHelper.GetDateFromMySQL(dr["StartDate"]).Value.ToLocalTime();
                    element.EndDate = Commons.MySQLHelper.GetDateFromMySQL(dr["EndDate"]);
                    element.Comment = Commons.MySQLHelper.GetStringFromMySQL(dr["Comment"]);

                    if (element.EndDate != null)
                        element.EndDate = element.EndDate.Value.ToLocalTime();
                    result.Add(element);
                }

            }
            catch (Exception e)
            {
                string StrTypeId = "NULL";
                if (TypeId != null)
                    StrTypeId = TypeId.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TypeId = " + StrTypeId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }




        /// <summary>
        /// Update the task
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static bool UpdateTaskLog(TaskLog Task)
        {
            bool Result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("EndDate", Task.EndDate);
                Columns.Add("Result", Task.Result);
                Columns.Add("Comment",Task.Comment);
                Result = GenericDAL.UpdateById("tasklog", Task.Id, Columns);
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Task.Id.ToString());
            }
            return Result;
        }

    }
}

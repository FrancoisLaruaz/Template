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
        ///  Get the logs of the scheduled tasks    
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public static List<TaskLog> GetTaskLogsList(int? TypeId)
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

                Query = Query + " order by l.Id desc";
                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    TaskLog element = new TaskLog();
                    element.Id = Convert.ToInt32(dr["Id"]);
                    element.TypeId = MySQLHelper.GetIntFromMySQL(dr["TypeId"]).Value;
                    element.TypeName = Convert.ToString(dr["TypeName"]);
                    element.Result = MySQLHelper.GetBoolFromMySQL(dr["Email"]);
                    element.StartDate = Commons.MySQLHelper.GetDateFromMySQL(dr["StartDate"]).Value;
                    element.EndDate = Commons.MySQLHelper.GetDateFromMySQL(dr["EndDate"]).Value;
                    element.Comment = Commons.MySQLHelper.GetStringFromMySQL(dr["Comment"]);

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

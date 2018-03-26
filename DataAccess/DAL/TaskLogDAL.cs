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
                if (!string.IsNullOrEmpty(Pattern))
                {
                    Query = Query + " and (Comment like '%" + Pattern + "%'";
                    Query = Query + " or CallbackId like '%" + Pattern + "%'";
                    Query = Query + " or GroupName like '%" + Pattern + "%'";
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
                Columns.Add("EndDate", DateTime.UtcNow);
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

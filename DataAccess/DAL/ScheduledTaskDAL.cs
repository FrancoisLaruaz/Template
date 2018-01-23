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
    public class ScheduledTaskDAL
    {
        public ScheduledTaskDAL()
        {

        }

        /// <summary>
        /// Return the number of problems in the databse for the scheduled tasks
        /// </summary>
        /// <returns></returns>
        public static int GetNotExecutedTasksNumber()
        {
            int result = 0;
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "select count(*) ";
                Query = Query + "from scheduledtask where   CancellationDate is null and ExecutionDate is null and ExpectedExecutionDate < "+MySQLHelper.GetDateTimeToInsert(DateTime.UtcNow);
                result = GenericDAL.GetSingleNumericData(Query).Value;

            }
            catch (Exception e)
            {
                result = 0;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }


        /// <summary>
        /// Check if a job is active
        /// </summary>
        /// <param name="CallBackId"></param>
        /// <returns></returns>
        public static bool IsScheduledTaskActive(string CallBackId)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "select count(*) ";
                Query = Query + "from scheduledtask where LOWER(CallBackId)=LOWER(@CallBackId) and CancellationDate is null and ExecutionDate is null ";
                parameters.Add("@CallBackId", CallBackId);
                result = GenericDAL.GetSingleNumericData(Query, parameters).Value > 0 ? true : false;

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CallBackId = " + CallBackId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

    



        /// <summary>
        /// Set a scheduled  task as executed
        /// </summary>
        /// <param name="CallBackId"></param>
        /// <returns></returns>
        public static bool SetTaskAsExecuted(string CallBackId)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                db = new DBConnect();
                string Query = "update scheduledtask";
                Query = Query + " set ExecutionDate=@Date ";
                Query = Query + "where @CallBackId =  CallBackId";
                parameters.Add("@Date", DateTime.UtcNow);
                parameters.Add("@CallBackId", CallBackId);
                result = db.ExecuteQuery(Query, parameters);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CallBackId = " + CallBackId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Remove the cancelled tasks of the database 
        /// </summary>
        /// <returns></returns>
        public static bool DeleteCancelledScheduledTasks()
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "delete from scheduledtask ";
                Query += " where cancellationdate is not null ";
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

        /// <summary>
        /// Get a list of scheduled tasks
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Id"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static List<ScheduledTask> GetScheduledTasksList(int? UserId,int? Id,bool? Active=false)
        {
            List<ScheduledTask> result = new List<ScheduledTask>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select t.Id, t.CallbackId, t.UserId, t.GroupName, t.CancellationDate, t.ExecutionDate  ";
                Query = Query + ", t.ExpectedExecutionDate, t.EmailTypeId, t.CreationDate ";
                Query = Query + ", c.Name as 'EMailTypeName', u.FirstName, u.LastName, ui.EMail ";
                Query = Query + ", n.Id as NewsId, n.PublishDate as NewsPublishDate,n.Title as NewsTitle ";
                Query = Query + "from scheduledtask t ";
                Query = Query + "inner join category c on c.Id=t.EmailTypeId ";
                Query = Query + "left join news n on n.Id=t.NewsId ";
                Query = Query + "left join user u on u.Id=t.UserId ";
                Query = Query + "left join useridentity ui on ui.username=u.username ";
                Query = Query + " where 1=1 ";
                if (UserId != null && UserId.Value>0)
                {
                    Query = Query + " and t.UserId=" + UserId.Value.ToString();
                }
                if (Id != null && Id.Value > 0)
                {
                    Query = Query + " and t.Id=" + Id.Value.ToString();
                }
                if (Active != null && Active.Value)
                {
                    Query = Query + " and t.CancellationDate is null and t.ExecutionDate is null";
                }

                Query = Query + " order by u.id, t.Id desc";
                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    ScheduledTask element = new ScheduledTask();
                    element.Id = Convert.ToInt32(dr["Id"]);
                    element.CallbackId = MySQLHelper.GetStringFromMySQL(dr["CallbackId"]);
                    element.GroupName = MySQLHelper.GetStringFromMySQL(dr["GroupName"]);
                    element.UserId = MySQLHelper.GetIntFromMySQL(dr["UserId"]);
                    element.CancellationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["CancellationDate"]);
                    element.ExecutionDate = Commons.MySQLHelper.GetDateFromMySQL(dr["ExecutionDate"]);
                    element.ExpectedExecutionDate = Commons.MySQLHelper.GetDateFromMySQL(dr["ExpectedExecutionDate"]).Value;
                    element.CreationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["CreationDate"]).Value;
                    element.EmailTypeId = Commons.MySQLHelper.GetIntFromMySQL(dr["EmailTypeId"]);
                    element.NewsPublishDate = Commons.MySQLHelper.GetDateFromMySQL(dr["NewsPublishDate"]);
                    element.NewsId = Commons.MySQLHelper.GetIntFromMySQL(dr["NewsId"]);
                    element.NewsTitle = MySQLHelper.GetStringFromMySQL(dr["NewsTitle"]);
                    element.EMailTypeName = Commons.MySQLHelper.GetStringFromMySQL(dr["EMailTypeName"]);
                    element.UserFirstNameDecrypt = Commons.EncryptHelper.DecryptString(Commons.MySQLHelper.GetStringFromMySQL(dr["FirstName"]));
                    element.UserLastNameDecrypt = Commons.EncryptHelper.DecryptString(Commons.MySQLHelper.GetStringFromMySQL(dr["LastName"]));
                    element.UserEMailDecrypt = Commons.EncryptHelper.DecryptString(Commons.MySQLHelper.GetStringFromMySQL(dr["EMail"]));

                    result.Add(element);
                }

            }
            catch (Exception e)
            {
                string StrUserId = "NULL";
                if (UserId != null)
                    StrUserId = UserId.ToString();
                string StrId = "NULL";
                if (Id != null)
                    StrId = Id.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + StrId+ " and UserId = "+ StrUserId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }



    }
}

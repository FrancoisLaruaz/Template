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
        /// Get a list of scheduled tasks
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<ScheduledTask> GetScheduledTasksList(int? UserId,int? Id)
        {
            List<ScheduledTask> result = new List<ScheduledTask>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select t.Id, t.CallbackId, t.UserId, t.CallbackUrl, t.CancellationDate, t.ExecutionDate  ";
                Query = Query + ", t.ExpectedExecutionDate, t.EmailTypeId, t.CreationDate ";
                Query = Query + ", c.Name as 'EMailTypeName', u.FirstName, u.LastName, u.EMail ";
                Query = Query + "from scheduledtask t ";
                Query = Query + "inner join category c on c.Id=t.EmailTypeId ";
                Query = Query + "left join user u on u.Id=t.UserId ";
                Query = Query + " where 1=1 ";
                if (UserId != null && UserId.Value>0)
                {
                    Query = Query + " and t.UserId=" + UserId.Value.ToString();
                }
                if (Id != null && Id.Value > 0)
                {
                    Query = Query + " and t.Id=" + Id.Value.ToString();
                }

                Query = Query + " order by u.id, t.Id desc";
                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    ScheduledTask element = new ScheduledTask();
                    element.Id = Convert.ToInt32(dr["Id"]);
                    element.CallbackId = MySQLHelper.GetStringFromMySQL(dr["CallbackId"]);
                    element.CallbackUrl = MySQLHelper.GetStringFromMySQL(dr["CallbackUrl"]);
                    element.UserId = MySQLHelper.GetIntFromMySQL(dr["UserId"]);
                    element.CancellationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["CancellationDate"]);
                    element.ExecutionDate = Commons.MySQLHelper.GetDateFromMySQL(dr["ExecutionDate"]);
                    element.ExpectedExecutionDate = Commons.MySQLHelper.GetDateFromMySQL(dr["ExpectedExecutionDate"]).Value;
                    element.CreationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["CreationDate"]).Value;
                    element.EmailTypeId = Commons.MySQLHelper.GetIntFromMySQL(dr["EmailTypeId"]);

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

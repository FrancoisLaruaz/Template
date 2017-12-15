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
    public class NewsDAL
    {
        public NewsDAL()
        {

        }



        /// <summary>
        /// Get a list of news
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="TypeId"></param>
        /// <param name="Published"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static List<News> GetNewsList(int? Id=null,int? TypeId=null,bool? Published=null,bool? Active=null)
        {
            List<News> result = new List<News>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select U.Id as LastModificationUserId,  U.FirstName, U.LastName  ";
                Query = Query + ", n.*  ";
                Query = Query + ", t.Name as TypeName ";
                Query = Query + ", m.Name as TypeUserMailingName ";
                Query = Query + ", s.Id as ScheduledTaskId, s.Executiondate ";
                Query = Query + "from news n ";
                Query = Query + "inner join category t on t.Id=n.TypeId ";
                Query = Query + "left join  category m on m.Id=n.TypeUserMailingId ";
                Query = Query + "left join user U on U.Id=n.LastModificationUserId ";
                Query = Query + "left join scheduledtask s on s.NewsId=n.Id ";
                Query = Query + " where 1=1 ";
                if (Id!=null && Id.Value>0)
                {
                    Query = Query + " and n.Id="+Id.Value.ToString();
                }
                if (TypeId != null && TypeId.Value > 0)
                {
                    Query = Query + " and n.TypeId=" + TypeId.Value.ToString();
                }
                if (Published != null )
                {
                    if(Published.Value)
                    {
                        Query = Query + " and n.PublishDate<" +MySQLHelper.GetDateTimeToInsert(DateTime.UtcNow);
                    }
                    else
                    {
                        Query = Query + " and n.PublishDate>=" + MySQLHelper.GetDateTimeToInsert(DateTime.UtcNow);
                    }
                }
                if (Active != null)
                {
                    Query = Query + " and n.Active=" + MySQLHelper.GetBoolToInsert(Active.Value);
                }
                Query = Query + " order by n.PublishDate desc, n.id desc";
                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    News element = new News();
                    element.Id = Convert.ToInt32(dr["Id"]);
                    element.Title = Convert.ToString(dr["Title"]);
                    element.Description= Convert.ToString(dr["Description"]);
                    element.MailSubject = Convert.ToString(dr["MailSubject"]);
                    element.TypeId = Convert.ToInt32(dr["TypeId"]);
                    element.TypeUserMailingId = Commons.MySQLHelper.GetIntFromMySQL(dr["TypeUserMailingId"]);
                    element.TypeName = Convert.ToString(dr["TypeName"]);
                    element.TypeUserMailingName = Convert.ToString(dr["TypeUserMailingName"]);
                    element.PublishDate = Commons.MySQLHelper.GetDateFromMySQL(dr["PublishDate"]).Value;
                    element.ModificationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["ModificationDate"]).Value;
                    element.CreationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["CreationDate"]).Value;
                    element.LastModificationUserId = Commons.MySQLHelper.GetIntFromMySQL(dr["LastModificationUserId"]);
                    if(element.LastModificationUserId!=null)
                    {
                        element.LastModificationUserFullNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["FirstName"]))+" " + EncryptHelper.DecryptString(Convert.ToString(dr["LastName"])); ;
                    }

                    element.Active = Commons.MySQLHelper.GetBoolFromMySQL(dr["LockoutEnabled"]).Value;
                    element.ScheduledTaskId = Commons.MySQLHelper.GetIntFromMySQL(dr["ScheduledTaskId"]);
                    element.HasScheduledTaskBeenExecuted = element.ScheduledTaskId!=null?( Commons.MySQLHelper.GetDateFromMySQL(dr["Executiondate"])==null?false:true):false;

                    result.Add(element);
                }

            }
            catch (Exception e)
            {
                result = null;
                string strId = "NULL";
                if (Id != null)
                    strId = Id.ToString();
                string strTypeId = "NULL";
                if (TypeId != null)
                    strTypeId = TypeId.ToString();
                string strPublished = "NULL";
                if (Published != null)
                    strPublished = Published.ToString();
                string strActive = "NULL";
                if (Active != null)
                    strActive = Active.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + strId+ " and TypeId = " + strTypeId + " and published = " + strPublished + " and active = " + strActive);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }



    }
}

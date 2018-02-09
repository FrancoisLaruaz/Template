using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Models.ViewModels;

namespace DataAccess
{
    public class NewsDAL
    {
        public NewsDAL()
        {

        }

        /// <summary>
        /// Return a list of news
        /// </summary>
        /// <param name="Pattern"></param>
        /// <param name="StartAt"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DisplayPublishedNewsViewModel GetPublishedNewsList(string Pattern, int StartAt = -1, int PageSize = -1)
        {
            DisplayPublishedNewsViewModel model = new DisplayPublishedNewsViewModel();
            List<News> NewsList = new List<News>();
            DBConnect db = null;
            try
            {
                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
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

                Query = Query + " order by n.PublishDate desc, n.id desc";
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    model.Count = db.GetData(Query).Rows.Count;
                    Query = Query + " LIMIT " + PageSize.ToString() + " OFFSET " + StartAt.ToString();
                }
                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    News element = new News();
                    element.Id = Convert.ToInt32(dr["Id"]);
                    element.Title = Convert.ToString(dr["Title"]);
                    element.Description = Convert.ToString(dr["Description"]);
                    element.MailSubject = Convert.ToString(dr["MailSubject"]);
                    element.TypeId = Convert.ToInt32(dr["TypeId"]);
                    element.TypeUserMailingId = Commons.MySQLHelper.GetIntFromMySQL(dr["TypeUserMailingId"]);
                    element.TypeName = Convert.ToString(dr["TypeName"]);
                    element.TypeUserMailingName = Convert.ToString(dr["TypeUserMailingName"]);
                    element.PublishDate = Commons.MySQLHelper.GetDateFromMySQL(dr["PublishDate"]).Value.ToLocalTime();
                    element.ModificationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["ModificationDate"]).Value.ToLocalTime();
                    element.CreationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["CreationDate"]).Value.ToLocalTime();
                    element.LastModificationUserId = Commons.MySQLHelper.GetIntFromMySQL(dr["LastModificationUserId"]);
                    if (element.LastModificationUserId != null)
                    {
                        element.LastModificationUserFullNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["FirstName"])) + " " + EncryptHelper.DecryptString(Convert.ToString(dr["LastName"])); ;
                    }

                    element.Active = Commons.MySQLHelper.GetBoolFromMySQL(dr["Active"]).Value;
                    element.ScheduledTaskId = Commons.MySQLHelper.GetIntFromMySQL(dr["ScheduledTaskId"]);
                    element.HasScheduledTaskBeenExecuted = element.ScheduledTaskId != null ? (Commons.MySQLHelper.GetDateFromMySQL(dr["Executiondate"]) == null ? false : true) : false;

                    NewsList.Add(element);
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<News> resultIEnumerable = NewsList as IEnumerable<News>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.Title != null && a.Title.Contains(Pattern)) || (a.MailSubject != null && a.MailSubject.Contains(Pattern)) || (a.Description != null && a.Description.Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.TypeName.Contains(Pattern) || (a.TypeUserMailingName != null && a.TypeUserMailingName.Contains(Pattern)) || (a.PublishDate != null && a.PublishDate.ToString().Contains(Pattern) ));
                    model.Count = resultIEnumerable.ToList().Count;
                    NewsList = resultIEnumerable.Take(PageSize).Skip(StartAt).OrderByDescending(a => a.Id).ToList();
                }

                model.NewsList = NewsList;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            finally
            {
                db.Dispose();
            }
            return model;
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
                Dictionary<string, object> parameters = new Dictionary<string, object>();
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
                    Query = Query + " and n.Id=@Id";
                    parameters.Add("@Id", Id);
                }
                if (TypeId != null && TypeId.Value > 0)
                {
                    Query = Query + " and n.TypeId=@TypeId";
                    parameters.Add("@TypeId", TypeId);
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
                DataTable data = db.GetData(Query, parameters);
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
                    element.PublishDate = Commons.MySQLHelper.GetDateFromMySQL(dr["PublishDate"]).Value.ToLocalTime();
                    element.ModificationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["ModificationDate"]).Value.ToLocalTime();
                    element.CreationDate = Commons.MySQLHelper.GetDateFromMySQL(dr["CreationDate"]).Value.ToLocalTime();
                    element.LastModificationUserId = Commons.MySQLHelper.GetIntFromMySQL(dr["LastModificationUserId"]);
                    if(element.LastModificationUserId!=null)
                    {
                        element.LastModificationUserFullNameDecrypt = EncryptHelper.DecryptString(Convert.ToString(dr["FirstName"]))+" " + EncryptHelper.DecryptString(Convert.ToString(dr["LastName"])); ;
                    }

                    element.Active = Commons.MySQLHelper.GetBoolFromMySQL(dr["Active"]).Value;
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

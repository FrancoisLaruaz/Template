using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Models.Class;

namespace DataAccess
{
    public class EMailAuditDAL
    {
        public EMailAuditDAL()
        {

        }

        /// <summary>
        /// Return a list of EMail audit
        /// </summary>
        /// <param name="Pattern"></param>
        /// <param name="StartAt"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DisplayEmailAuditViewModel GetEMailsAuditList(string Pattern, int StartAt = -1, int PageSize = -1)
        {
            DisplayEmailAuditViewModel model = new DisplayEmailAuditViewModel();
            List<EMailAudit> ListAudits = new List<EMailAudit>();
            DBConnect db = null;
            try
            {
                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower();
                db = new DBConnect();
                string Query = "select a.Id, a.UserId, a.EMailFrom, a.EMailTo ,a.Date ";
                Query = Query + ",a.AttachmentNumber,a.EMailTypeId,u.FirstName, u.LastName,c.Name ";
                Query = Query + "from emailaudit a ";
                Query = Query + " left join user u on u.Id=a.UserId  ";
                Query = Query + " left join category c on c.Id=a.EMailTypeId ";
                Query = Query + "where 1=1 ";
                Query = Query + "order by a.Id desc";
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    model.Count = db.GetData(Query).Rows.Count;
                    Query = Query + " LIMIT " + PageSize.ToString() + " OFFSET " + StartAt.ToString();
                }

                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    EMailAudit Audit = new EMailAudit();
                    Audit.Id = MySQLHelper.GetIntFromMySQL(dr["Id"]).Value;
                    Audit.UserId = MySQLHelper.GetIntFromMySQL(dr["UserId"]);
                    Audit.EMailTypeId = MySQLHelper.GetIntFromMySQL(dr["EMailTypeId"]).Value;
                    Audit.EMailFrom = Convert.ToString(dr["EMailFrom"]);
                    Audit.UserFirstName = Convert.ToString(dr["FirstName"]);
                    Audit.EMailTo = Convert.ToString(dr["EMailTo"]);
                    Audit.UserLastName = Convert.ToString(dr["LastName"]);
                    Audit.Date = MySQLHelper.GetDateFromMySQL(dr["Date"]).Value.ToLocalTime();
                    Audit.EMailTypeName = Convert.ToString(dr["Name"]);
                    Audit.AttachmentNumber = Convert.ToInt32(dr["AttachmentNumber"]);

                    Audit.EMailFromDecrypt = EncryptHelper.DecryptString(Audit.EMailFrom)?.ToLower().Trim();
                    Audit.EMailToDecrypt = EncryptHelper.DecryptString(Audit.EMailTo)?.ToLower().Trim();
                    Audit.UserFirstNameDecrypt = EncryptHelper.DecryptString(Audit.UserFirstName)?.ToLower().Trim();
                    Audit.UserLastNameDecrypt = EncryptHelper.DecryptString(Audit.UserLastName)?.ToLower().Trim();
                    ListAudits.Add(Audit);
                }

                if(!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<EMailAudit> resultIEnumerable = ListAudits as IEnumerable<EMailAudit>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.UserFirstNameDecrypt!=null && a.UserFirstNameDecrypt.Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.EMailTypeName.Contains(Pattern) || (a.EMailToDecrypt != null && a.EMailToDecrypt.Contains(Pattern)) ||  (a.EMailFromDecrypt!=null && a.EMailFromDecrypt.Contains(Pattern)) || (a.UserLastNameDecrypt!=null && a.UserLastNameDecrypt.Contains(Pattern)));
                    model.Count = resultIEnumerable.ToList().Count;
                    ListAudits = resultIEnumerable.Take(PageSize).Skip(StartAt).OrderByDescending(a => a.Id).ToList();
                }

                model.AuditsList = ListAudits;
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
        /// Creation of a row in the database
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public static bool InsertAudit(EMailAudit EMail)
        {
            bool result = true;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "insert into emailaudit (";
                Query +="UserId, EMailFrom, EMailTo, AttachmentNumber, Date, EMailTypeId ) ";
                Query += " values (";
                Query += MySQLHelper.GetIntToInsert(EMail.UserId);
                Query += ","+ MySQLHelper.GetStringToInsert(EMail.EMailFrom);
                Query += "," + MySQLHelper.GetStringToInsert(EMail.EMailTo);
                Query += "," + MySQLHelper.GetIntToInsert(EMail.AttachmentNumber);
                Query += "," + MySQLHelper.GetDateTimeToInsert(EMail.Date);
                Query += "," + MySQLHelper.GetIntToInsert(EMail.EMailTypeId);
                Query += " )";

                result = db.ExecuteQuery(Query);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMail.EMailTypeId + " and emailto =" + EMail.EMailToDecrypt);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

    }
}

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



 

    }
}

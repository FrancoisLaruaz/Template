using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;


namespace DataAccess
{
    public static class GenericDAL 
    {

        /// <summary>
        /// Delete an object of a table by Id
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DeleteById(string Table, int Id)
        {
            bool result = false;
            DBConnect db=null;
            try
            {
                db = new DBConnect();
                string Query = "delete from " + Table + " where Id=" + Id;
                result = db.ExecuteQuery(Query);
                db.Dispose();

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Table = " + Table + " and Id = " + Id);
                db.Dispose();
            }
            return result;
        }
        /// <summary>
        /// Test if a value exist
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public static bool DataExist(string Query)
        {

            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                DataTable Data = db.GetData(Query);
                if (Data != null && Data.Rows != null && Data.Rows.Count > 0)
                {
                    result = true;
                }
                db.Dispose();
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Query = " + Query);
                db.Dispose();
            }

            return result;
        }

        public static int? GetSingleNumericData(string Query)
        {
            int? result = null;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                DataTable Data = db.GetData(Query);
                if (Data != null && Data.Rows != null && Data.Rows.Count > 0)
                {
                    result = Convert.ToInt32(Data.Rows[0][0]);
                }
                db.Dispose();
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Query = " + Query);
                db.Dispose();
            }

            return result;
        }

    }
}

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
        /// Update generic of a row
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Id"></param>
        /// <param name="Columns"></param>
        /// <returns></returns>
        public static bool UpdateById(string Table, Object Id,Dictionary<string,Object> Columns)
        {
            bool result = false;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                if (!String.IsNullOrWhiteSpace(MySQLHelper.GetValueToInsert(Id)) && !String.IsNullOrWhiteSpace(Table) && Columns != null && Columns.Count>0)
                {
                    string Query = "update " + Table+" set ";
                    foreach(var Column in Columns)
                    {
                        string strValue = MySQLHelper.GetValueToInsert(Column.Value);
                        Query = Query + Column.Key.ToString()+" = " + strValue+" , ";
                    }
                    Query=Query.Remove(Query.Length - 2);
                    Query = Query + " where Id=" + MySQLHelper.GetValueToInsert(Id);
                    result = db.ExecuteQuery(Query);

                    
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Table = " + Table + " and Id = " + Id);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }


        /// <summary>
        /// Insert ma row and return the id inserted
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Columns"></param>
        /// <returns></returns>
        public static int InsertRow(string Table,  Dictionary<string, Object> Columns)
        {
            int insertedId = -1;
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                if (!String.IsNullOrWhiteSpace(Table))
                {
                    string Query = "insert into  " + Table + " (  ";

                    foreach (var Column in Columns)
                    {
                        Query = Query + Column.Key.ToString() + " , ";
                    }
                    Query = Query.Remove(Query.Length - 2) + ") values (";
                    foreach (var Column in Columns)
                    {
                        string strValue = MySQLHelper.GetValueToInsert(Column.Value);
                        Query = Query +  strValue + " , ";
                    }
                    Query = Query.Remove(Query.Length - 2)+");";
                    insertedId = db.ExecuteInsertQuery(Query);


                }

            }
            catch (Exception e)
            {
                insertedId = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Table = " + Table );
            }
            finally
            {
                db.Dispose();
            }
            return insertedId;
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

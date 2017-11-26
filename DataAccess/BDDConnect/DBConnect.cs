using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Media;
using System.Resources;
using System.Threading;
using System.Globalization;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using Commons;

namespace DataAccess
{
    public class DBConnect : IDisposable
    {
        private MySqlDataAdapter MyAdapter;
        private string connectionString;
        private MySqlConnection mySqlConnection;
        private MySqlTransaction mySqlTransaction;

        public DBConnect()
        {
            InitDBConnect();
        }

        /// <summary>
        /// Start the SQL transaction
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {
            bool result = true;
            try
            {
                mySqlTransaction = mySqlConnection.BeginTransaction();
            }
            catch(Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Commit the transaction
        /// </summary>
        /// <returns></returns>
        public bool CommitTransaction()
        {
            bool result = true;
            try
            {
                if(mySqlTransaction!=null)
                {
                    mySqlTransaction.Commit();
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Rollback the SQL transaction
        /// </summary>
        /// <returns></returns>
        public bool RollbackTransaction()
        {
            bool result = true;
            try
            {
                if (mySqlTransaction != null)
                {
                    mySqlTransaction.Rollback();
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Fonction récupérant le dernier id inséré
        /// </summary>
        /// <returns>The last inserti d.</returns>
        public int GetLastInsertID()
        {
            int result = -1;
            try
            {
                string Query = "SELECT LAST_INSERT_ID()";
                DataTable data = GetData(Query);
                DataRow dr;
                if (data != null && data.Rows!=null && data.Rows.Count > 0)
                {
                    dr = data.Rows[0];
                    result = Convert.ToInt32(dr[0]);
                }



            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        public void Dispose()
        {
            try
            {
                if (mySqlConnection.State != ConnectionState.Closed)
                    mySqlConnection.Close();
                mySqlConnection.Dispose();
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        /// <summary>
        /// Recuperation de données à partir d'une requete
        /// </summary>
        /// <returns>The donnees.</returns>
        /// <param name="requete">Requete.</param>
        public DataTable GetData(string Query)
        {
            DataTable data = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                MyAdapter.SelectCommand = new MySql.Data.MySqlClient.MySqlCommand(Query, mySqlConnection);
                MyAdapter.Fill(ds);
                data = ds.Tables[0];
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Query = "+ Query);
            }
            return data;
        }

        /// <summary>
        /// get one data with one row
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public DataRow GetUniqueDataRow(string Query)
        {
            DataRow data = null;
            try
            {
                DataTable DT = GetData(Query);
                if (DT != null && DT.Rows != null && DT.Rows.Count == 1)
                {
                    data = DT.Rows[0];
                }
            }
            catch (Exception e)
            {
                data = null;
                string message = "Error while executing the query \n requete = " + Query + " \n " + e.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Query = " + Query);
            }
            return data;
        }

        /// <summary>
        /// Executers the requete.
        /// </summary>
        /// <returns><c>true</c>, if requete was executered, <c>false</c> otherwise.</returns>
        /// <param name="requete">Requete.</param>
        public bool ExecuteQuery(string Query)
        {
            bool result = true;
            try
            {
                MySqlCommand myCommand = new MySqlCommand(Query);
                myCommand.Connection = mySqlConnection;
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Query = " + Query);
            }
            return result;
        }

        /// <summary>
        /// Execute an insert query and return the inserted id
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public int ExecuteInsertQuery(string Query)
        {
            int InsertedId = -1;
            try
            {
                MySqlCommand myCommand = new MySqlCommand(Query);
                myCommand.Connection = mySqlConnection;
                myCommand.ExecuteNonQuery();
                InsertedId = Convert.ToInt32(myCommand.LastInsertedId);
            }
            catch (Exception e)
            {
                InsertedId = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Query = " + Query);
            }
            return InsertedId;
        }

        /// <summary>
        /// Checks the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        public string CheckConnection()
        {
            string result = "*** Connection OK ***";
            try
            {
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = mySqlConnection;
                if (mySqlConnection.State != ConnectionState.Open)
                    mySqlConnection.Open();
                if (mySqlConnection.State == ConnectionState.Open)
                {
                    result = "Connection OK and open.";
                }
                else
                {
                    result = "Connection OK but closed.";
                }
            }
            catch (Exception e)
            {
                result = "*** Connection KO ***</br></br>";
                result = result + "Connection string :</br>";
                if (mySqlConnection != null && mySqlConnection.ConnectionString != null)
                    result = result + mySqlConnection.ConnectionString.ToString() + "</br></br>";
                else
                    result = result + "ConnectionString Nulle!</br></br>";
                result = result + "Erreor :</br>";
                result = result + e.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Error while connecting to the database MySQL: " + e.ToString());
            }
            finally
            {
                Dispose();
            }
            result = result.Replace("/n", "</br>");
            return result;
        }
        /// <summary>
        /// Initiation de la connection
        /// </summary>
        public void InitDBConnect()
        {
            try
            {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
                MyAdapter = new MySqlDataAdapter();
                mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = connectionString;
                mySqlTransaction = null;
                if (mySqlConnection.State != ConnectionState.Open)
                    mySqlConnection.Open();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}

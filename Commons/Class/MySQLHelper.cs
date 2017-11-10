using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;

namespace Commons
{ 
    public static class MySQLHelper
    {
        // '2017-01-20 23:00:00'
        /// <summary>
        /// Renvoie un string avec le format my sql
        /// </summary>
        /// <returns>The format string pour date my sql.</returns>
        /// <param name="date">Date.</param>
        public static string GetFormatStringForDate(DateTime? date)
        {
            string result = "1990-01-01 23:00:00";
            try
            {
                if (date != null)
                {
                    result = date.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "date = " + date);
            }
            return result;
        }

        public static int? GetIntFromMySQL(Object Ob)
        {
            int? result = null;
            try
            {
                if (Ob != DBNull.Value && Ob != null)
                {
                    result = Convert.ToInt32(Ob);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Ob = " + Ob.ToString());
            }
            return result;
        }

        public static DateTime? GetDateFromMySQL(Object Ob)
        {
            DateTime? result = null;
            try
            {
                if (Ob!= DBNull.Value && Ob != null)
                {
                    result = Convert.ToDateTime(Ob);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Ob = " + Ob.ToString());
            }
            return result;
        }


        /// <summary>
        /// Function used to update/insert a string with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetStringToInsert(string Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    result = "'" + Value.Replace("'", "''") + "'";
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }

        /// <summary>
        /// Function used to update/insert am integer with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetIntToInsert(int? Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    result =Value.ToString() ;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }

        /// <summary>
        /// Function used to update/insert a date with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetDateTimeToInsert(DateTime? Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    result = "'"+ GetFormatStringForDate(Value) + "'";
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }
    }
}

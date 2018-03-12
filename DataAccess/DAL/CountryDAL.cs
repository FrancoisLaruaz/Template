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
    public class CountryDAL
    {
        public CountryDAL()
        {

        }


        public static List<Country> GeListCountries()
        {
            List<Country> result = new List<Country>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select c.Id, c.Code, c.Name ,c.Order ";
                Query = Query + "from country c  order by c.Order, c.Name ";


                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    Country country = new Country();
                    country.Id = Convert.ToInt32(dr["Id"]);
                    country.Order = Convert.ToInt32(dr["Order"]);
                    country.Code = Convert.ToString(dr["Code"]);
                    country.Name = Convert.ToString(dr["Name"]);
                    result.Add(country);
                }
            }
            catch (Exception e)
            {
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

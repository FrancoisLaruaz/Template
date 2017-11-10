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
    public class ProvinceDAL
    {
        public ProvinceDAL()
        {

        }


        public static List<Province> GeListProvinces(int? CountryId)
        {
            List<Province> result = new List<Province>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select Id, Code, Name ,Order, CountryId ";
                Query = Query + "from province";
                if(CountryId!=null && CountryId.Value>0)
                    Query = Query + " where CountryId="+CountryId.Value;
                Query = Query + " order by Order, Name ";


                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    Province province = new Province();
                    province.Id = Convert.ToInt32(dr["Id"]);
                    province.Order = Convert.ToInt32(dr["Order"]);
                    province.Code = Convert.ToString(dr["Code"]);
                    province.Name = Convert.ToString(dr["Name"]);
                    province.CountryId = MySQLHelper.GetIntFromMySQL(dr["CountryId"]).Value;
                    result.Add(province);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CountryId = "+ CountryId);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

    }
}

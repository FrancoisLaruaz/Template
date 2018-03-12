using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class ProvinceService
    {

        /// <summary>
        /// Get all the provinces of a country
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        public static List<SelectionListItem> GetProvinceList(int? CountryId)
        {
            List<SelectionListItem> newList = new List<SelectionListItem>();
            try
            {
                List<Province> provinces = GeListProvinces(CountryId);
                if (provinces != null && provinces.Count > 0)
                {
                    foreach (var item in provinces)
                    {
                        newList.Add(new SelectionListItem(item.Id, item.Name));
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CountryId = "+ CountryId);
            }
            return newList;
        }

        /// <summary>
        /// Get all the provinces of a country
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        public static List<Province> GeListProvinces(int? CountryId)
        {
            List<Province> result = null;
            try
            {
                result = ProvinceDAL.GeListProvinces(CountryId);
            }
            catch (Exception e)
            {
                result = new List<Province>() ;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

    }
}

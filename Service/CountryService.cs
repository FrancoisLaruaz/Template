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
    public static class CountryService
    {

        public static List<SelectionListItem> GetCountryList()
        {
            List<SelectionListItem> newList = new List<SelectionListItem>();
            try
            {
                List<Country> countries = GeListCountries();
                foreach (var item in countries)
                {
                    newList.Add(new SelectionListItem(item.Id, item.Name));
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return newList;
        }

        public static List<Country> GeListCountries()
        {
            List<Country> result = null;
            try
            {
                result = CountryDAL.GeListCountries();
            }
            catch (Exception e)
            {
                result = new List<Country>() ;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

    }
}

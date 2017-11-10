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
    public static class CategoryService
    {

        /// <summary>
        /// Get a list for a dropdownlist for the requested category
        /// </summary>
        /// <param name="categoryTypeId"></param>
        /// <returns></returns>
        public static List<SelectionListItem> GetSelectionList(int CategoryTypeId)
        {
            List<SelectionListItem> newList = new List<SelectionListItem>();
            try
            {
                List<Category> categories = GeListCategoriesByType(CategoryTypeId);
                foreach (var item in categories)
                {
                    newList.Add(new SelectionListItem(item.Id, item.Name, item.Code));
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CategoryTypeId = " + CategoryTypeId);
            }
            return newList;
        }

        public static List<Category> GeListCategories()
        {
            List<Category> result = null;
            try
            {
                result = CategoryDAL.GetCategoriesList();
            }
            catch (Exception e)
            {
                result = new List<Category>(); 
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        public static Category GetCategoryById(int Id)
        {
            Category result = null;
            try
            {
                List<Category> ListResult = CategoryDAL.GetCategoriesList(Id,null);
                if (ListResult != null && ListResult.Count > 0)
                {
                    result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return result;
        }

        public static List<Category> GeListCategoriesByType(int CategoryTypeId,bool ActiveOnly=true)
        {
            List<Category> result = null;
            try
            {
                result = CategoryDAL.GetCategoriesList(null, CategoryTypeId,ActiveOnly);
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Delete the specified category
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DeleteCategoryById(int Id)
        {
            bool result = false;
            try
            {
                result = GenericDAL.DeleteById("category", Id);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return result;
        }

    }
}

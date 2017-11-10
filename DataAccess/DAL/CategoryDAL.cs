using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace DataAccess
{
    public class CategoryDAL
    {
        public CategoryDAL()
        {

        }

        /// <summary>
        /// Return a list of categories
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CategoryTypeId"></param>
        /// <param name="ActiveOnly"></param>
        /// <returns></returns>
        public static List<Category> GetCategoriesList(int? Id=null, int? CategoryTypeId = null, bool ActiveOnly = false)
        {
            List<Category> result = new List<Category>();
            DBConnect db = null;
            try
            {
                db = new DBConnect();
                string Query = "select C.Id, C.Name, C.Code ,C.Description, C.DateModification ";
                Query = Query + ", C.Order, CT.Name as CategoryTypeName, C.Active, C.CategoryTypeId ";
                Query = Query + "from category C ";
                Query = Query + "inner  join categorytype CT on CT.Id=C.CategoryTypeId ";
                Query = Query + " where 1=1 " ;
                if(Id!=null && Id.Value>0)
                    Query = Query + " and C.Id = " + Id.ToString();
                if (CategoryTypeId != null && CategoryTypeId.Value > 0)
                    Query = Query + " and CategoryTypeId = " + CategoryTypeId.ToString();
                if (ActiveOnly)
                    Query = Query + " and Active=1 ";
                Query = Query + " order by CT.Name, C.Order ";

                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    Category Category = new Category();
                    Category.Id = Convert.ToInt32(dr["Id"]);
                    Category.CategoryTypeId = Convert.ToInt32(dr["CategoryTypeId"]);
                    Category.Active = Convert.ToBoolean(dr["Active"]);
                    Category.Code = Convert.ToString(dr["Code"]);
                    Category.Name = Convert.ToString(dr["Name"]);
                    Category.DateModification = Commons.MySQLHelper.GetDateFromMySQL(dr["DateModification"]).Value;
                    Category.Description = Convert.ToString(dr["Description"]);
                    Category.Order = Convert.ToInt32(dr["Order"]);
                    Category.CategoryTypeName = Convert.ToString(dr["CategoryTypeName"]);
                    Category.Active = Convert.ToBoolean(dr["Active"]);
                    result.Add(Category);
                }

            }
            catch (Exception e)
            {
                string strCategoryTypeId = "NULL";
                if (CategoryTypeId != null)
                    strCategoryTypeId = CategoryTypeId.ToString();
                string strId = "NULL";
                if (Id != null)
                    strId = Id.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CategoryTypeId = "+ strCategoryTypeId + " and Id = " + strId + " and ActiveOnly = " + ActiveOnly);
            }
            finally
            {
                db.Dispose();
            }
            return result;

        }




    }
}

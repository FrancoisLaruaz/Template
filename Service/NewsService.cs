using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;

namespace Service
{
    public static class NewsService
    {

   
        /// <summary>
        /// Return all the news 
        /// </summary>
        /// <returns></returns>
        public static List<News> GetNewsList()
        {
            List<News> result = null;
            try
            {
                result = NewsDAL.GetNewsList();
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Return all the news not published
        /// </summary>
        /// <returns></returns>
        public static List<News> GetNotPublishedNewsList()
        {
            List<News> result = null;
            try
            {
                result = NewsDAL.GetNewsList(null,null,false);
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        public static NewsEditViewModel GetNewsEditViewModel()
        {
            NewsEditViewModel model = new NewsEditViewModel();
            try
            {
                
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return model;
        }

        public static NewsViewModel GetNewsViewModel()
        {
            NewsViewModel model = new NewsViewModel();
            try
            {
                model.NotPublishedNews = GetNotPublishedNewsList();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return model;
        }


        /// <summary>
        /// Get a news By Id
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public static News GetNewsById(int NewsId)
        {
            News result = null;
            try
            {
                List<News> ListResult = NewsDAL.GetNewsList(NewsId);
                if (ListResult != null && ListResult.Count > 0)
                {
                    result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
            return result;
        }

    }
}

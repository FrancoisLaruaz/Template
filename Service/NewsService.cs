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

        /// <summary>
        /// Creationf of a news
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int CreateNews(NewsEditViewModel model)
        {
            int InsertedId = -1;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("PublishDate", model.PublishDate);
                Columns.Add("Title", model.NewsTitle);
                Columns.Add("Description", model.NewsDescription);
                Columns.Add("MailSubject", model.MailSubject);
                Columns.Add("TypeId", model.TypeId);
                Columns.Add("TypeUserMailingId", model.TypeUserMailingId);
              //  Columns.Add("TypeUserMailingId", model.mo);
                Columns.Add("ModificationDate", DateTime.UtcNow);
                Columns.Add("CreationDate", DateTime.UtcNow);
                InsertedId = GenericDAL.InsertRow("news", Columns);
            }
            catch (Exception e)
            {
                InsertedId = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return InsertedId;
        }

        /// <summary>
        /// Get the viewmodel for the News form
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public static NewsEditViewModel GetNewsEditViewModel(int? NewsId)
        {
            NewsEditViewModel model = new NewsEditViewModel();
            try
            {
                // Creation
                if(NewsId==null || NewsId <= 0)
                {
                    model.Id = -1;
                    model.Active = false;
                    model.PublishDate = DateTime.UtcNow.ToLocalTime();
                }
                // Modification
                else
                {
                    News RealNews = GetNewsById(NewsId.Value);
                    model.Id = NewsId.Value;
                    if (RealNews!=null)
                    {
                        model.NewsTitle = RealNews.Title;
                        model.NewsDescription = RealNews.Description;
                        model.MailSubject = RealNews.MailSubject;
                        model.TypeUserMailingId = RealNews.TypeUserMailingId;
                        model.TypeId = RealNews.TypeId;
                        model.Active = RealNews.Active;
                        model.PublishDate = RealNews.PublishDate;
                    }
                }


                model.NewsTypeList = CategoryService.GetSelectionList(Commons.CategoryTypes.NewsType);
                model.TypeUserMailingList = CategoryService.GetSelectionList(Commons.CategoryTypes.TypeUserMailing);
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

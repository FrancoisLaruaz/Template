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
                result = NewsDAL.GetNewsList(null, null, false);
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        public static List<News> GetPublishedNewsList()
        {
            List<News> result = null;
            try
            {
                result = NewsDAL.GetNewsList(null, null, true);
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
                Columns.Add("ModificationDate", DateTime.UtcNow);
                Columns.Add("Active", model.Active);
                Columns.Add("LastModificationUserId", model.LastModificationUserId);
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
        /// Edit an existing news
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool EditNews(NewsEditViewModel model)
        {
            bool Result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("PublishDate", model.PublishDate);
                Columns.Add("Title", model.NewsTitle);
                Columns.Add("Description", model.NewsDescription);
                Columns.Add("MailSubject", model.MailSubject);
                Columns.Add("TypeId", model.TypeId);
                Columns.Add("LastModificationUserId", model.LastModificationUserId);
                Columns.Add("Active", model.Active);
                Columns.Add("TypeUserMailingId", model.TypeUserMailingId);
                Columns.Add("ModificationDate", DateTime.UtcNow);
                Result = GenericDAL.UpdateById("news", model.Id, Columns);
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Result;
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
                if (NewsId == null || NewsId <= 0)
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
                    if (RealNews != null)
                    {
                        model.NewsTitle = RealNews.Title;
                        model.NewsDescription = RealNews.Description;
                        model.MailSubject = RealNews.MailSubject;
                        model.TypeUserMailingId = RealNews.TypeUserMailingId;
                        model.TypeId = RealNews.TypeId;
                        model.Active = RealNews.Active;
                        model.PublishDate = RealNews.PublishDate;
                        model.ScheduledTaskId = RealNews.ScheduledTaskId;
                        model.HasScheduledTaskBeenExecuted = RealNews.HasScheduledTaskBeenExecuted ?? false;
                    }
                }


                model.NewsTypeList = CategoryService.GetSelectionList(CommonsConst.CategoryTypes.NewsType);
                model.TypeUserMailingList = CategoryService.GetSelectionList(CommonsConst.CategoryTypes.TypeUserMailing);
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
        /// Delete a news
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public static bool DeleteNews(int NewsId)
        {
            bool result = false;
            try
            {
                News news = GetNewsById(NewsId);
                if (news != null)
                {
                    if(news.ScheduledTaskId!=null)
                         ScheduledTaskService.CancelTaskById(news.ScheduledTaskId.Value);
                    result = GenericDAL.DeleteById("news", NewsId);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
            return result;
        }

        /// <summary>
        /// Send a news to the users
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public static bool SendNews(int NewsId)
        {
            bool result = false;
            try
            {
                News news = GetNewsById(NewsId);
                if (news != null )
                {
                    List<int> MailingList = GetNewsMailList(news.TypeUserMailingId.Value);

                    foreach (int UserId in MailingList)
                    {

                        Email Email = new Email();
                        Email.UserId = UserId;
                        Email.EMailTypeId = CommonsConst.EmailTypes.News;
                        Email.RootPathDefault = FileHelper.GetRootPathDefault() + @"\";
                        List<Tuple<string, string>> EmailContent = new List<Tuple<string, string>>();
                        EmailContent.Add(new Tuple<string, string>("#Description#", news.Description));
                        EmailContent.Add(new Tuple<string, string>("#Title#", news.Title));

                        Email.Subject = news.MailSubject;
                        Email.EmailContent = EmailContent;


                        result = EMailService.SendMail(Email);

                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
            return result;
        }

        /// <summary>
        /// Get the list of the user to send the mail
        /// </summary>
        /// <param name="TypeUserMailingId"></param>
        /// <returns></returns>
        public static List<int> GetNewsMailList(int TypeUserMailingId)
        {
            List<int> result = new List<int>();
            try
            {
                List<User> UserList = UserService.GeListUsers();
                foreach (var User in UserList)
                {
                    switch (TypeUserMailingId)
                    {
                        case CommonsConst.TypeUserMailing.AllUsers:
                            result.Add(User.Id);
                            break;
                        case CommonsConst.TypeUserMailing.ConfirmedUsers:
                            if (User.EmailConfirmed)
                                {
                                result.Add(User.Id);
                                }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TypeUserMailingId = " + TypeUserMailingId);
            }
            return result;
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

using Commons;
using DataAccess;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using CommonsConst;
using Service.Admin.Interface;
using Service.UserArea.Interface;
using Service.UserArea;
using Models.ViewModels.Admin.News;
using Models.Class.Email;
using DataEntities.Model;

namespace Service.Admin
{
    public  class NewsService : INewsService
    {
        private readonly IEMailService _emailService;
        private readonly ICategoryService _categoryService;
        private readonly IGenericRepository<DataEntities.Model.News> _newsRepo;

        public NewsService(IEMailService emailService, ICategoryService categoryService,
             IGenericRepository<DataEntities.Model.News> newsRepo)
        {
            _emailService = emailService;
            _categoryService = categoryService;
            _newsRepo = newsRepo;
        }

        public NewsService()
        {
            _emailService = new EMailService();
        }



        /// <summary>
        /// Return all the news not published
        /// </summary>
        /// <returns></returns>
        public  List<News> GetNotPublishedNewsList()
        {
            List<News> result = null;
            try
            {
                result = _newsRepo.FindAllBy(n => n.PublishDate >= DateTime.UtcNow).ToList();
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        public  List<News> GetPublishedNewsList()
        {
            List<News> result = null;
            try
            {
                result = _newsRepo.FindAllBy(n => n.PublishDate < DateTime.UtcNow).ToList();
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
        public  int CreateNews(NewsEditViewModel model)
        {
            int InsertedId = -1;
            try
            {

                News news = new News();
                news.PublishDate = model.PublishDate;
                news.Title = model.NewsTitle;
                news.Description = model.NewsDescription;
                news.MailSubject = model.MailSubject;
                news.TypeId = model.TypeId;
                news.TypeUserMailingId = model.TypeUserMailingId;

                news.ModificationDate = DateTime.UtcNow;
                news.Active = model.Active;
                news.LastModificationUserId = model.LastModificationUserId;
                news.CreationDate = DateTime.UtcNow;

                _newsRepo.Add(news);
                if(_newsRepo.Save())
                {
                    InsertedId = news.Id;
                }
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
        public  bool EditNews(NewsEditViewModel model)
        {
            bool Result = false;
            try
            {
                News news = _newsRepo.Get(model.Id);
                if(news!=null)
                {
                    news.PublishDate = model.PublishDate;
                    news.Title = model.NewsTitle;
                    news.Description = model.NewsDescription;
                    news.MailSubject = model.MailSubject;
                    news.TypeId = model.TypeId;
                    news.TypeUserMailingId = model.TypeUserMailingId;
                    news.Active = model.Active;
                    news.LastModificationUserId = model.LastModificationUserId;
                    news.ModificationDate = DateTime.UtcNow;

                    _newsRepo.Edit(news);
                    Result = _newsRepo.Save();
                
                }

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
        public  NewsEditViewModel GetNewsEditViewModel(int? NewsId)
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
                        model.ScheduledTaskId = RealNews.ScheduledTasks?.FirstOrDefault()?.Id;
                        model.HasScheduledTaskBeenExecuted = RealNews.ScheduledTasks?.FirstOrDefault()?.ExecutionDate<DateTime.UtcNow?true: false;
                    }
                }


                model.NewsTypeList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.NewsType);
                model.TypeUserMailingList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.TypeUserMailing);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return model;
        }

        public  NewsViewModel GetNewsViewModel()
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
        public  bool DeleteNews(int NewsId)
        {
            bool result = false;
            try
            {
                News news = GetNewsById(NewsId);
                if (news != null)
                {
                    int? ScheduledTaskId = news.ScheduledTasks?.FirstOrDefault()?.Id;
                    if (ScheduledTaskId != null)
                         ScheduledTaskService.CancelTaskById(ScheduledTaskId.Value);
                    List<Tuple<string, object>> Parameters = new List<Tuple<string, object>>();
                    Parameters.Add(new Tuple<string, object>("@NewsId", NewsId));
                    result = _newsRepo.ExecuteStoredProcedure("DeleteNewsById", Parameters);
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
        public  bool SendNews(int NewsId)
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
                        Email.RelatedScheduledTaskId = news.ScheduledTasks?.FirstOrDefault()?.Id;

                        result = _emailService.SendMail(Email);

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

        public  PreviewNewsMailViewModel GetPreviewNewsMailViewModel(string Title, string Description, UserSession user)
        {
            PreviewNewsMailViewModel model = new PreviewNewsMailViewModel();
            try
            {
                if (Title == null)
                    Title = "";
                if (Description == null)
                    Description = "";

                List<Tuple<string, string>> GenericEmailContent = EMailHelper.GetGenericEmailContent();
                GenericEmailContent.Add(new Tuple<string, string>("#UserFirstName#", user.FirstNameDecrypt));
                GenericEmailContent.Add(new Tuple<string, string>("#UserFullName#", user.UserFullNameDecrypt));
                GenericEmailContent.Add(new Tuple<string, string>("#RealUserEMail#", ""));
                GenericEmailContent.Add(new Tuple<string, string>("#WebSiteURL#", Utils.Website));

                string BasePathFile = FileHelper.GetRootPathDefault() + @"\"+  Const.BasePathTemplateEMails.Replace("~/", "\\");
                string PathHeaderOnServer = BasePathFile + "\\_HeaderMail.html";
                string PathFooterOnServer = BasePathFile + "\\_FooterMail.html";
                string PathEndMailOnServer = BasePathFile + "\\_EndMail_en.html";
                string PathTemplateOnServer = BasePathFile + "\\news_en.html";
                string headerTemplate = System.IO.File.ReadAllText(PathHeaderOnServer);
                string bodyTemplate = System.IO.File.ReadAllText(PathTemplateOnServer);
                string footerTemplate = System.IO.File.ReadAllText(PathFooterOnServer);
                string endMailTemplate = System.IO.File.ReadAllText(PathEndMailOnServer);
                model.Body = headerTemplate + bodyTemplate + endMailTemplate + footerTemplate;

                foreach (var content in GenericEmailContent)
                {
                    model.Body = model.Body.Replace(content.Item1, content.Item2);
                }
                model.Body = model.Body.Replace("#Title#", Title).Replace("#Description#", Description);

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Title = " + Title);
            }

            return model;
        }

        public  DisplayPublishedNewsViewModel GetDisplayPublishedNewsViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayPublishedNewsViewModel model = new DisplayPublishedNewsViewModel();
            try
            {
                model = NewsDAL.GetPublishedNewsList(Pattern, StartAt, PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
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
                /*
                List<User> UserList = _user.GeListUsers();
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
                */
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
        public  News GetNewsById(int NewsId)
        {
            News result = null;
            try
            {
                result = _newsRepo.Get(NewsId);
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

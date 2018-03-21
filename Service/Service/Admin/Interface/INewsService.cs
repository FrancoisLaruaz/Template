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
using DataEntities.Model;
using Models.ViewModels.Admin.News;

namespace Service.Admin.Interface
{
    public interface INewsService
    {
         List<News> GetNewsList();

        List<News> GetPublishedNewsList();
        List<News> GetNotPublishedNewsList();

        int CreateNews(NewsEditViewModel model);

        bool EditNews(NewsEditViewModel model);

        NewsEditViewModel GetNewsEditViewModel(int? NewsId);

        NewsViewModel GetNewsViewModel();

        bool DeleteNews(int NewsId);

        bool SendNews(int NewsId);

        DisplayPublishedNewsViewModel GetDisplayPublishedNewsViewModel(string Pattern, int StartAt, int PageSize);

        PreviewNewsMailViewModel GetPreviewNewsMailViewModel(string Title, string Description, UserSession user);

    }
}

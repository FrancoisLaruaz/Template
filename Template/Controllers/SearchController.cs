using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models;
using Models.ViewModels;

using Service;
using Commons;
using i18n;
using System.Configuration;
using Models.Class;
using System.Web.Hosting;
using Service.UserArea.Interface;
using Models.ViewModels.Search;
using DataEntities.Model;

namespace Website.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(
            IUserService userService, ISearchService searchService
            ) : base(userService)
        {
            _searchService = searchService;
        }

        public ActionResult Index(int? id=null)
        {
            var model = new SearchViewModel();
            try
            {
                ViewBag.Title = "[[[Search]]]";
                model.SearchId = id;
                if (id != null)
                {
                    Search search = _searchService.GetSearchById(id.Value);
                    if (search != null)
                    {
                        model.Pattern = search.Pattern;
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "id = "+ id);
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult _DisplaySearch(DisplaySearchViewModel Model)
        {
            try
            {
                Model = new DisplaySearchViewModel();// _userRoleService.GetDisplayUsersViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView(Model);
        }


        [HttpPost]
        public ActionResult CreateSearch(string Pattern)
        {
            int searchId = -1;
            try
            {

                int? UserId=null;

                if (UserSession != null && UserSession.UserId > 0)
                {
                    UserId = UserSession.UserId;
                }

                searchId = _searchService.CreateSearch(Pattern, UserId);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return Json(new {SearchId= searchId });
        }
    }
}

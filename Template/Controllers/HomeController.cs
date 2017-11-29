using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models;
using Models.ViewModels;
using Models.BDDObject;
using Service;
using Commons;
using i18n;
using Revalee.Client;
using System.Configuration;

namespace Website.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult _Header()
        {
            HeaderViewModel model = new HeaderViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    model.UserFirstName = UserSession.FirstNameDecrypt;
                    model.UserNameDecrypt = UserSession.UserNameDecrypt;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return PartialView("~/Views/Shared/Layout/_Header.cshtml", model);
        }
        public ActionResult Index()
        {
            try
            {
              //  bool result = ScheduledTaskService.ScheduleEMailUserTask(5, Commons.EmailType.Forgotpassword, TimeSpan.FromMinutes(1.0));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
   
            return View();
        }

        /// <summary>
        /// Action executed to change the language of the website
        /// </summary>
        /// <param name="langtag"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SetLanguage(string langtag, string returnUrl)
        {
            try
            {
                // If valid 'langtag' passed.
                i18n.LanguageTag lt = i18n.LanguageTag.GetCachedInstance(langtag);
                if (lt.IsValid())
                {
                    // Set persistent cookie in the client to remember the language choice.
                    Response.Cookies.Add(new HttpCookie(Commons.Const.i18nlangtag)
                    {
                        Value = lt.ToString(),
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddYears(1)
                    });

                    if (User.Identity.IsAuthenticated)
                    {
                        UserService.UpdateLanguageUser(langtag, User.Identity.Name);
                    }
                }
                // Owise...delete any 'language' cookie in the client.
                else
                {
                    var cookie = Response.Cookies[Commons.Const.i18nlangtag];
                    if (cookie != null)
                    {
                        cookie.Value = null;
                        cookie.Expires = DateTime.UtcNow.AddMonths(-1);
                    }
                }
                // Update PAL setting so that new language is reflected in any URL patched in the 
                // response (Late URL Localization).
                System.Web.HttpContext.Current.SetPrincipalAppLanguageForRequest(lt);
                // Patch in the new langtag into any return URL.
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = LocalizedApplication.Current.UrlLocalizerForApp.SetLangTagInUrlPath(HttpContext, returnUrl, UriKind.RelativeOrAbsolute, lt == null ? null : lt.ToString()).ToString();
                }
                // Redirect user agent as approp.
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"Lanuguage = "+langtag+" and url = "+returnUrl);
            }
            if (returnUrl != "")
                return this.Redirect(returnUrl);
            else
                return null;
        }
    }
}

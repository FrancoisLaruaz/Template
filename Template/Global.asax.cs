﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Web.Optimization;
using i18n;
using System.Threading;
using Service;
using Commons;
using System.Transactions;
using Quartz;
using Quartz.Impl;
using Service.TaskClasses;
using System.Threading.Tasks;
using Service.UserArea.Interface;
using Service.UserArea;

namespace Template
{
    public class Global : HttpApplication
    {
        /*  Project -> Build events -> Post build event line
         *  "$(TargetDir)i18n.PostBuild.exe"  "$(ProjectDir)web.config"
         * 
         * 
         */


        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                AreaRegistration.RegisterAllAreas();
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
                SetGlobalization();


            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }




        /// <summary>
        /// Initialisation
        /// </summary>
        public void SetGlobalization()
        {
            try
            {
                // Change from the default of 'en'.
                i18n.LocalizedApplication.Current.DefaultLanguage = CommonsConst.Const.DefaultCulture;


                // Change from the of temporary redirects during URL localization
                i18n.LocalizedApplication.Current.PermanentRedirects = true;

                // This line can be used to disable URL Localization.
                //i18n.UrlLocalizer.UrlLocalizationScheme = i18n.UrlLocalizationScheme.Void;

                // Change the URL localization scheme from Scheme1.
                i18n.UrlLocalizer.UrlLocalizationScheme = i18n.UrlLocalizationScheme.Scheme2;

                // Change i18n's expectation for the ASP.NET application's virtual application root path on the server, 
                // used by Url Localization. Defaults to "/".
                //i18n.LocalizedApplication.Current.ApplicationPath = "/mysite";

                // Specifies whether the key for a message may be assumed to be the value for
                // the message in the default language. Defaults to true.
                //i18n.LocalizedApplication.Current.MessageKeyIsValueInDefaultLanguage = false;



                // Blacklist certain URLs from being 'localized' via a callback.
                i18n.UrlLocalizer.IncomingUrlFilters += delegate (Uri url)
                {
                    if (url.LocalPath.EndsWith("sitemap.xml", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    return true;
                };

                // Extend (+=) or override (=) the default handler for Set-PAL event.
                // The default handler applies the setting to both the CurrentCulture and CurrentUICulture
                // settings of the thread, as shown below.
                i18n.LocalizedApplication.Current.SetPrincipalAppLanguageForRequestHandlers = delegate (System.Web.HttpContextBase context, ILanguageTag langtag)
                {
                // Do own stuff with the language tag.
                // The default handler does the following:
                if (langtag != null)
                    {
                        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = langtag.GetCultureInfo();
                    }
                };
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        /// <summary>
        /// Session end
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Session_End(Object source, EventArgs e)
        {
            Session[CommonsConst.Const.UserSession] = null;
        }

        /// <summary>
        /// Sessions the start.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Session_Start(object sender, EventArgs e)
        {
            string languageBrowser = null;
            try
            {
                if (Request.IsAuthenticated)
                {
                    UserService _userService = new UserService();
                    var ConnectedUser = _userService.GetUserByUserName(User.Identity.Name);
                    if (ConnectedUser != null)
                        languageBrowser = ConnectedUser.Language?.Code;
                }
                else
                {
                    Session[CommonsConst.Const.UserSession] = null;
                }
                if (String.IsNullOrEmpty(languageBrowser))
                {

                    string[] languages = Request?.UserLanguages;
                    if (languages != null && languages.Length > 0)
                    {
                        string Favoritelanguage = languages[0];
                        languageBrowser = CommonsConst.Const.DefaultCulture;
                        CategoryService _categoryService = new CategoryService();
                        var ListLanguages = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.Language);
                        if (ListLanguages != null && ListLanguages.Count > 0)
                        {
                            foreach (var Language in ListLanguages)
                            {
                                if (Language.Code == Favoritelanguage || Favoritelanguage.IndexOf(Language.Code + "-") > -1)
                                {
                                    languageBrowser = Language.Code;
                                    break;
                                }
                            }
                        }
                    }
                }



                if (!String.IsNullOrEmpty(languageBrowser))
                {
                    i18n.LanguageTag lt = i18n.LanguageTag.GetCachedInstance(languageBrowser);
                    Response.Cookies.Add(new HttpCookie(CommonsConst.Const.i18nlangtag)
                    {
                        Value = lt.ToString(),
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddYears(1)
                    });


                    System.Web.HttpContext.Current.SetPrincipalAppLanguageForRequest(lt);
                }
            }
            catch (Exception ex)
            {
                Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        /// <summary>
        /// Applications the end.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Application_End(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Applications the error.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var ex = HttpContext.Current.Server.GetLastError();

                string messageErreur = "";
                string messagePageErreur = "";
                if (ex == null)
                {
                    messageErreur = "Unknown error";
                    ex = new Exception();
                    messagePageErreur = messageErreur;
                }
                else
                {
                    messageErreur = ex.ToString();
                    messagePageErreur = ex.Message;
                }

                Commons.Logger.GenerateError(ex, typeof(HttpApplication));

                string redirection = String.Format("~/Error/{0}/?Message={1}", "DisplayError", messagePageErreur.Replace("\r", "").Replace("\n", ""));
                Response.Redirect(redirection);
            }
            catch (Exception e2)
            {
                Commons.Logger.GenerateError(e2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}

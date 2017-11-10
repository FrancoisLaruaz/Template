using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Web.Optimization;
using i18n;
using System.Threading;

namespace Template
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
        //    SetGlobalization();
        }

        public void SetGlobalization()
        {
            // Change from the default of 'en'.
            i18n.LocalizedApplication.Current.DefaultLanguage = "fr";

            // Change from the default of 'i18n.langtag'.
            i18n.LocalizedApplication.Current.CookieName = "i18n_langtag";

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

            // Specifies a custom method called after a nugget has been translated
            // that allows the resulting message to be modified, for instance according to content type.
            // See [Issue #300](https://github.com/turquoiseowl/i18n/issues/300) for example usage case.
            i18n.LocalizedApplication.Current.TweakMessageTranslation = delegate (System.Web.HttpContextBase context, i18n.Helpers.Nugget nugget, i18n.LanguageTag langtag, string message)
            {
                switch (context.Response.ContentType)
                {
                    case "text/html":
                        return message.Replace("\'", "&apos;");
                }
                return message;
            };

            // Blacklist certain URLs from being 'localized' via a callback.
            i18n.UrlLocalizer.IncomingUrlFilters += delegate (Uri url) {
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

            // Blacklist certain URLs from being translated using a regex pattern. The default setting is:
            //i18n.LocalizedApplication.Current.UrlsToExcludeFromProcessing = new Regex(@"(?:\.(?:less|css)(?:\?|$))|(?i:i18nSkip|glimpse|trace|elmah)");

            // Whitelist content types to translate. The default setting is:
            //i18n.LocalizedApplication.Current.ContentTypesToLocalize = new Regex(@"^(?:(?:(?:text|application)/(?:plain|html|xml|javascript|x-javascript|json|x-json))(?:\s*;.*)?)$");

            // Change the types of async postback blocks that are localized
            //i18n.LocalizedApplication.Current.AsyncPostbackTypesToTranslate = "updatePanel,scriptStartupBlock,pageTitle";

            // Change which languages are parsed from the request, like skipping  the "Accept-Language"-header value. The default setting is:
            //i18n.HttpContextExtensions.GetRequestUserLanguagesImplementation = (context) => LanguageItem.ParseHttpLanguageHeader(context.Request.Headers["Accept-Language"]);

            // Override the i18n service injection. See source code for more details!
            //i18n.LocalizedApplication.Current.RootServices = new Myi18nRootServices();
        }

        /// <summary>
        /// Sessions the start.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Session_Start(object sender, EventArgs e)
		{

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
            catch(Exception e2)
            {
                Commons.Logger.GenerateError(e2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}

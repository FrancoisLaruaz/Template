using System.Web.Mvc;
using System.Web.Routing;

namespace Template
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Account


            routes.MapRoute(
                "ChangePassword",
                "ChangePassword",
                new { controller = "Account", action = "ChangePassword" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "PasswordChanged",
                "PasswordChanged",
                new { controller = "Account", action = "PasswordChanged" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "ResetPassword",
                "ResetPassword/{UserId}/{Token}",
                new { controller = "Account", action = "ResetPassword", UserId = UrlParameter.Optional, Token = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" }
            );

            

            routes.MapRoute(
                "TermsAndConditions",
                "TermsAndConditions",
                new { controller = "Account", action = "TermsAndConditions" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "PrivacyPolicy",
                "PrivacyPolicy",
                new { controller = "Account", action = "PrivacyPolicy" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "Login",
                "Login",
                new { controller = "Account", action = "Login" },
                namespaces: new[] { "Website.Controllers" }
            );
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

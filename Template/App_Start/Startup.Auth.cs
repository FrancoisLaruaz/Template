using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;
using Identity;
using Identity.Models;
using System.Web.Script.Serialization;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using static Website.Controllers.AccountController;
using Microsoft.Owin.Security.Google;

namespace Template
{

    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            try
            {
                // Configure the db context, user manager and signin manager to use a single instance per request
                app.CreatePerOwinContext(ApplicationDbContext.Create);
                app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
                app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

                // Enable the application to use a cookie to store information for the signed in user
                // and to use a cookie to temporarily store information about a user logging in with a third party login provider
                // Configure the sign in cookie
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                    Provider = new CookieAuthenticationProvider
                    {
                        // Enables the application to validate the security stamp when the user logs in.
                        // This is a security feature which is used when you change a password or add an external login to your account.  
                        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                    }
                });
                app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

                // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
                app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

                // Enables the application to remember the second login verification factor such as phone or email.
                // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
                // This is similar to the RememberMe option when you log in.
                app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

                #region Facebook Config
                string FacebookAppId = ConfigurationManager.AppSettings["FacebookAppId"];
                string FacebookAppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
                if (!String.IsNullOrWhiteSpace(FacebookAppId) && !String.IsNullOrWhiteSpace(FacebookAppSecret))
                {
                    var facebookOptions = new FacebookAuthenticationOptions()
                    {
                        AppId = FacebookAppId,
                        AppSecret = FacebookAppSecret,
                    };

                    facebookOptions.BackchannelHttpHandler = new FacebookBackChannelHandler();
                    facebookOptions.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;

                    // Set requested scope
                    facebookOptions.Scope.Add("email");
                    facebookOptions.Scope.Add("user_location");
                    facebookOptions.Scope.Add("user_birthday");

                    app.UseFacebookAuthentication(facebookOptions);

                    #endregion

                #region Google Config
                    string GoogleAppId = ConfigurationManager.AppSettings["GoogleAppId"];
                    string GoogleAppSecret = ConfigurationManager.AppSettings["GoogleAppSecret"];

                    if (!String.IsNullOrWhiteSpace(GoogleAppId) && !String.IsNullOrWhiteSpace(GoogleAppSecret))
                    {
                        app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                        {
                            ClientId = GoogleAppId,
                            ClientSecret = GoogleAppSecret
                        });
                    }
                    #endregion
                }
            }

            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }
    }

}

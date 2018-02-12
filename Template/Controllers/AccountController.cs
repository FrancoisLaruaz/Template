using Commons;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models.BDDObject;
using Models.ViewModels;
using Models.Class;
using i18n;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Script.Serialization;
using Identity;
using System.Net;
using System.IO;
using Identity.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Models.Class.ExternalAuthentification;
using Facebook;
using CommonsConst;
using System.Security.Claims;

namespace Website.Controllers
{
    public class AccountController : BaseController
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AccountController()
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_userManager != null)
                    {
                        _userManager.Dispose();
                        _userManager = null;
                    }

                    if (_signInManager != null)
                    {
                        _signInManager.Dispose();
                        _signInManager = null;
                    }
                }

                base.Dispose(disposing);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        private const string XsrfKey = "XsrfId";





        public class FacebookBackChannelHandler : HttpClientHandler
        {
            protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                try
                {
                    var result = await base.SendAsync(request, cancellationToken);
                    if (!request.RequestUri.AbsolutePath.Contains("access_token"))
                        return result;

                    // For the access token we need to now deal with the fact that the response is now in JSON format, not form values. Owin looks for form values.
                    var content = await result.Content.ReadAsStringAsync();
                    var facebookOauthResponse = JsonConvert.DeserializeObject<FacebookOauthResponse>(content);

                    var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
                    outgoingQueryString.Add(nameof(facebookOauthResponse.access_token), facebookOauthResponse.access_token);
                    outgoingQueryString.Add(nameof(facebookOauthResponse.expires_in), facebookOauthResponse.expires_in + string.Empty);
                    outgoingQueryString.Add(nameof(facebookOauthResponse.token_type), facebookOauthResponse.token_type);
                    var postdata = outgoingQueryString.ToString();

                    var modifiedResult = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(postdata)
                    };

                    string accessToken = facebookOauthResponse.access_token;
                    FacebookAccessToken = accessToken;
                    return modifiedResult;
                }

                catch (Exception e)
                {
                    Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }
                return null;
            }
        }




        public static string FacebookAccessToken
        {
            get;
            set;
        }

        #region ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }



        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            bool _Result = false;
            string _Error = "";
            string _Media = "";
            string _ImageSrc = "";
            string _Language = CurrentLangTag;
            string _Redirection = "";
            string _FirstName = "";
            string _LastName = "";
            UserSession userSession = null;
            try
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (loginInfo == null)
                {
                    _Error = "[[[No login information have been provided.]]]";
                }
                else
                {
                    var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                    _Media = loginInfo.Login.LoginProvider;
                    // Sign in the user with this external login provider if the user already has a login
                    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: true);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            _Result = true;
                             userSession = UserService.GetUserSession(User.Identity.GetUserName());
                            if (userSession != null)
                            {
                                UserIdentityService.UpdateUserIdentityLoginSuccess(userSession.UserIdentityId);
                                _Language = userSession.LanguageTag;
                            }


                            break;
                        case SignInStatus.LockedOut:
                            _Error = "[[[The user is currently locked out.]]]";
                            break;
                        case SignInStatus.RequiresVerification:
                            _Error = "[[[A user verification is required.]]]";
                            break;
                        case SignInStatus.Failure:
                        default:
                            ExternalSignUpInformation ExternalSignUpInformation = null;
                            if (loginInfo.Login.LoginProvider == LoginProviders.Facebook && !String.IsNullOrWhiteSpace(FacebookAccessToken))
                            {
                                ExternalSignUpInformation = SocialMediaHelper.GetFacebookInformation(FacebookAccessToken);

                            }
                            else if (loginInfo.Login.LoginProvider == LoginProviders.Google)
                            {
                                var GoogleAccessToken = loginInfo.ExternalIdentity.Claims.Where(c => c.Type.Equals("urn:google:accesstoken")).Select(c => c.Value).FirstOrDefault();

                                if (!String.IsNullOrWhiteSpace(GoogleAccessToken))
                                {
                                    ExternalSignUpInformation = SocialMediaHelper.GetGoogleInformation(GoogleAccessToken);
                                }
                            }

                            if (ExternalSignUpInformation != null)
                            {

                                if (ExternalSignUpInformation.EmailPermission)
                                {
                                    if (UserService.IsUserRegistered(EncryptHelper.EncryptToString(ExternalSignUpInformation.Email)))
                                    {

                                        bool success = UserLoginsService.CreateExternalLogin(ExternalSignUpInformation);

                                        if (success)
                                        {
                                            result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
                                            switch (result)
                                            {
                                                case SignInStatus.Success:
                                                    _Result = true;
                                                    userSession = UserService.GetUserSession(User.Identity.GetUserName());
                                                    if (userSession != null)
                                                    {
                                                        UserIdentityService.UpdateUserIdentityLoginSuccess(userSession.UserIdentityId);
                                                        SocialMediaConnectionService.InsertSocialMediaConnections(ExternalSignUpInformation.FriendsList, ExternalSignUpInformation.ProviderKey, ExternalSignUpInformation.LoginProvider);
                                                        _Language = userSession.LanguageTag;

                                                    }

                                                    break;
                                                case SignInStatus.Failure:
                                                default:
                                                    _Error = ExternalSignUpInformation.Email + "[[[ registered using an email address. Please log in with this email using a password.]]]";
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            _Error = ExternalSignUpInformation.Email + "[[[ registered using an email address. Please log in with this email using a password.]]]";
                                        }
                                    }
                                    else
                                    {
                                        _Redirection = ExternalAuthentificationRedirection.RedirectToExternalSignUp;
                                        _Error = "[[[No account exists for that ]]]" + ExternalSignUpInformation.LoginProvider + "[[[ login. Try signing up instead.]]]";
                                    }
                                }
                                else
                                {
                                    _Error = "[[[You must authorize ]]]" + CommonsConst.Const.WebsiteTitle + "[[[ to access your email address in order to sign up.]]]";
                                    if (!string.IsNullOrWhiteSpace(FacebookAccessToken))
                                    {
                                        var fb = new FacebookClient(FacebookAccessToken);
                                        if (fb != null)
                                        {
                                            var res = fb.Delete("me/permissions");
                                        }
                                    }
                                }

                            }
                            else
                            {
                                _Redirection = ExternalAuthentificationRedirection.RedirectToExternalSignUp;
                                _Error = "[[[No account exists for that login. Try signing up instead.]]]";
                            }
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View("~/Views/Account/ExternalAuthentificationResult.cshtml", new ExternalAuthentificationResult(_Result, returnUrl, _Error.Trim(), _Media, _ImageSrc, false, _Language, _Redirection, _FirstName, _LastName));
        }

        #endregion

        #region ExternalSignUp

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalSignUp(string provider, string returnUrl)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalSignUpCallback", "Account", new { ReturnUrl = returnUrl }));
        }


        [AllowAnonymous]
        public async Task<ActionResult> ExternalSignUpCallback(string returnUrl)
        {
            bool _Result = false;
            string _Error = "";
            string _Media = "";
            string _ImageSrc = null;
            string _Language = CurrentLangTag;
            string _Redirection = "";
            string _FirstName = "";
            string _LastName = "";
            try
            {


                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (loginInfo == null)
                {
                    _Error = "[[[No login information have been provided.]]]";
                }
                else
                {

                    _Media = loginInfo.Login.LoginProvider;
                    ExternalSignUpInformation ExternalSignUpInformation = null;
                    // added the following lines
                    if (loginInfo.Login.LoginProvider == LoginProviders.Facebook)
                    {
                        var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                        if (!String.IsNullOrWhiteSpace(FacebookAccessToken))
                        {
                            ExternalSignUpInformation = SocialMediaHelper.GetFacebookInformation(FacebookAccessToken);
                        }

                    }
                    else if (loginInfo.Login.LoginProvider == LoginProviders.Google)
                    {
                        var GoogleAccessToken = loginInfo.ExternalIdentity.Claims.Where(c => c.Type.Equals("urn:google:accesstoken")).Select(c => c.Value).FirstOrDefault();

                        if (!String.IsNullOrWhiteSpace(GoogleAccessToken))
                        {
                            ExternalSignUpInformation = SocialMediaHelper.GetGoogleInformation(GoogleAccessToken);
                        }
                    }


                    if (ExternalSignUpInformation != null && ExternalSignUpInformation.EmailPermission)
                    {

                        if (!String.IsNullOrWhiteSpace(ExternalSignUpInformation.Email))
                        {
                            string EncryptedUserName = EncryptHelper.EncryptToString(ExternalSignUpInformation.Email);

                            int CurrentLanguageId = CommonsConst.Languages.ToInt(CurrentLangTag);
                            if (!String.IsNullOrWhiteSpace(ExternalSignUpInformation.ImageSrc))
                            {
                                ExternalSignUpInformation.ImageSrc = FileHelper.SaveAndEncryptFileFromWeb(ExternalSignUpInformation.ImageSrc, "user", ".jpg");
                                _ImageSrc = ExternalSignUpInformation.ImageSrc;
                            }
                            var user = new ApplicationUser { UserName = EncryptedUserName, Email = EncryptedUserName, FirstName = ExternalSignUpInformation.FirstName, LastName = ExternalSignUpInformation.LastName, LanguageId = CurrentLanguageId, IsMasculine = ExternalSignUpInformation.IsMasculine, ReceiveNews = false , PictureSrc= _ImageSrc };

                            var result = await UserManager.CreateAsync(user);
                            if (result != null && result.Succeeded)
                            {
                                result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                                if (result.Succeeded)
                                {
                                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                    if (!String.IsNullOrWhiteSpace(EncryptedUserName))
                                    {
                                        UserSession userSession = UserService.GetUserSession(EncryptedUserName);
                                        _Result = true;
                                        _Language = userSession.LanguageTag;
                                        SocialMediaConnectionService.InsertSocialMediaConnections(ExternalSignUpInformation.FriendsList, ExternalSignUpInformation.ProviderKey, ExternalSignUpInformation.LoginProvider);
                                        EMailService.SendEMailToUser(EncryptedUserName, CommonsConst.EmailTypes.UserWelcome);
                                    }
                                    _Result = true;

                                }

                            }
                            else if (result != null)
                            {
                                foreach (var error in result.Errors)
                                {
                                    string message = error;
                                    _Error = _Error + " " + message;
                                }
                                if (_Error.Contains("is already taken"))
                                {
                                    _Error = ExternalSignUpInformation.Email+"[[[ is already registered on ]]]" + CommonsConst.Const.WebsiteTitle + ". [[[Please log in. ]]]";
                                    _Redirection = ExternalAuthentificationRedirection.RedirectToLogin;
                                }
                            }
                            else
                            {
                                _Error = "[[[An error occured while creating the user.]]]";
                            }
                        }
                        else
                        {
                            _Redirection = ExternalAuthentificationRedirection.RedirectToEmailSignUp;
                            _FirstName = ExternalSignUpInformation.FirstName;
                            _LastName = ExternalSignUpInformation.LastName;
                            _Error = "[[[Please review and provide any missing information to finish signing up.]]]";
                        }

                    }
                    else
                    {
                        _Error = "[[[You must authorize ]]]" + CommonsConst.Const.WebsiteTitle + "[[[ to access your email address in order to sign up.]]]";

                        if (!string.IsNullOrWhiteSpace(FacebookAccessToken))
                        {
                            var fb = new FacebookClient(FacebookAccessToken);
                            if (fb != null)
                            {
                                var res = fb.Delete("me/permissions");
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }


            return View("~/Views/Account/ExternalAuthentificationResult.cshtml", new ExternalAuthentificationResult(_Result, returnUrl, _Error.Trim(), _Media, _ImageSrc, true, _Language, _Redirection, _FirstName, _LastName));
        }
        #endregion

        #region SignUp
        public ActionResult _SignUpForm()
        {
            SignUpViewModel model = new SignUpViewModel();
            try
            {
                model.ReceiveNews = true;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return PartialView(model);
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _SignUpForm(SignUpViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _UserFirstName = "";

            try
            {
                if (!User.Identity.IsAuthenticated)
                {

                    if (ModelState.IsValid)
                    {
                        model.Email = model.Email.Trim().ToLower();
                        if (Utils.IsValidMail(model.Email))
                        {

                            model.Email = Commons.EncryptHelper.EncryptToString(model.Email);
                            int CurrentLanguageId = CommonsConst.Languages.ToInt(CurrentLangTag);
                            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, LanguageId = CurrentLanguageId, ReceiveNews = model.ReceiveNews };

                            var result = await UserManager.CreateAsync(user, model.Password);
                            if (result != null && result.Succeeded)
                            {
                                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                if (!String.IsNullOrWhiteSpace(model.Email))
                                {
                                    UserSession = UserService.GetUserSession(model.Email);
                                    _Result = true;
                                    string UserName = model.Email;
                                    EMailService.SendEMailToUser(UserName, CommonsConst.EmailTypes.UserWelcome);
                                }
                            }
                            else
                            {
                                if (UserService.IsUserRegistered(model.Email))
                                {
                                    _Error = "[[[This email address is already used.]]]";
                                }
                                else
                                {
                                    _Error = "[[[Error while creating the user.]]]";
                                }
                            }
                        }
                        else
                        {
                            _Error = "[[[Please enter a valid email address.]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[Please complete the form.]]]";
                    }


                }
                else
                {
                    _Error = "[[[You are already logged in.]]]";
                    _Result = false;
                }
            }
            catch (Exception e)
            {
                _Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email + " and FirstName = " + model.FirstName + " and LastName = " + model.LastName);
            }

            return Json(new { Result = _Result, Error = _Error, UserFirstName = _UserFirstName });
        }


        [HttpGet]
        public ActionResult _SignUpPictureForm()
        {
            SignUpPictureViewModel model = new SignUpPictureViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    User userLogged = UserService.GetUserByUserName(User.Identity.Name);
                    if (userLogged != null)
                    {
                        model.PictureSrc = userLogged.PictureSrc;
                        model.PicturePreviewSrc = FileHelper.GetDecryptedFilePath(model.PictureSrc, true);
                        return PartialView(model);
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult _SignUpPictureForm(SignUpPictureViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int UserId = UserSession.UserId;
                    if (UserId > 0 && !String.IsNullOrWhiteSpace(model.PictureSrc))
                    {
                        _Result = UserService.UpdateProfilePicture(UserId, model.PictureSrc);
                        if(_Result)
                        {
                            UserSession = UserService.GetUserSession(UserSession.UserName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Json(new { Result = _Result, Error = _Error });
        }


        #endregion


        #region LoginModal
        public ActionResult _LoginForm(string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                model.URLRedirect = returnUrl;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "returnUrl = " + returnUrl);
            }

            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _LoginForm(LoginViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _UserFirstName = "";

            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                if (ModelState.IsValid)
                {
                    model.Email = model.Email.Trim().ToLower();
                    model.Email = Commons.EncryptHelper.EncryptToString(model.Email);
                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            _Result = true;
                            UserSession = UserService.GetUserSession(model.Email);
                            if (UserSession != null)
                            {
                                _UserFirstName = UserSession.FirstNameDecrypt;
                                UserIdentityService.UpdateUserIdentityLoginSuccess(UserSession.UserIdentityId);
                                model.LanguageTag = UserSession.LanguageTag;
                            }
                            break;
                        case SignInStatus.LockedOut:
                            _Error = "[[[[Your account is currently lockout.]]]";
                            break;
                        case SignInStatus.RequiresVerification:
                            _Error = "[[[Invalid login attempt.]]]";
                            break;
                        case SignInStatus.Failure:
                            if (!UserService.IsUserRegistered(model.Email))
                            {
                                _Error = "[[[This user is not registered. Please sign-up.]]]";
                            }
                            else
                            {
                                UserIdentityService.UpdateUserIdentityLoginFailure(model.Email);
                                _Error = "[[[Invalid login attempt.]]]";
                            }
                            break;
                        default:
                            _Error = "[[[Invalid login attempt.]]]";
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                _Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email);
            }

            return Json(new { Result = _Result, Error = _Error, UserFirstName = _UserFirstName, URLRedirect = model.URLRedirect });
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                Session[CommonsConst.Const.UserSession] = null;
                Session.Clear();
                Session.Abandon();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + User.Identity.Name);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login(string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel();
            try
            {

                if (User.Identity.IsAuthenticated)
                {
                    if (String.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToLocal(returnUrl);
                    }
                }

                model.URLRedirect = returnUrl;
                ViewBag.Title = "[[[Login]]]";




            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "returnUrl = " + returnUrl);
            }

            return View(model);
        }

        #region Login


        /// <summary>
        /// Terms and conditions of the website
        /// </summary>
        /// <returns></returns>
        public ActionResult TermsAndConditions()
        {

            try
            {
                ViewBag.Title = "[[[Terms And Conditions]]]";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View();
        }

        /// <summary>
        /// Privacy policy of the website
        /// </summary>
        /// <returns></returns>
        public ActionResult PrivacyPolicy()
        {

            try
            {
                ViewBag.Title = "[[[Privacy Policy]]]";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View();
        }


        #endregion
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
    }
}

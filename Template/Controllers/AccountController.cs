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
                if (_signInManager == null)
                {
                    var owincontext = HttpContext.GetOwinContext();
                    return owincontext.Get<ApplicationSignInManager>();
                }
                else
                {
                    return _signInManager;
                }

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



        #region SignUp
        public ActionResult _SignUpForm()
        {
            SignUpViewModel model = new SignUpViewModel();
            try
            {

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
                            int CurrentLanguageId = CategoryService.GetCategoryByCode(CurrentLangTag).Id;
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, LanguageId = CurrentLanguageId };
                            var result = await UserManager.CreateAsync(user, model.Password);
                            if (result.Succeeded)
                            {
                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                if (!String.IsNullOrWhiteSpace(model.Email))
                                {
                                    UserSession = UserService.GetUserSession(model.Email);
                                    _Result = true;
                                    string UserName = model.Email;
                                    EMailService.SendEMailToUser(UserName, CommonsConst.EmailType.UserWelcome);
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
                if(User.Identity.IsAuthenticated)
                {
                    User userLogged = UserService.GetUserByUserName(User.Identity.Name);
                    if(userLogged!=null)
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
                    if(UserId>0 && !String.IsNullOrWhiteSpace(model.PictureSrc))
                    {
                        _Result = UserService.UpdateProfilePicture(UserId, model.PictureSrc);
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
                                model.LanguageTag= UserSession.LanguageTag;
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
                //  Session.Clear();
                // Session.Abandon();
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

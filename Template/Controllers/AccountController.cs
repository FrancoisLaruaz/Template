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
                if(_signInManager==null)
                {
                   var owincontext= HttpContext.GetOwinContext();
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

           // base.Dispose(disposing);
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
        public ActionResult _SignUpForm(SignUpViewModel model)
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
                        string Email = Commons.EncryptHelper.EncryptToString(model.Password.Replace("'", "''"));

                        bool IsUserRegistered = UserService.IsUserRegistered(Email);

                        if (!IsUserRegistered)
                        {
                            model.LangTagPreference = CurrentLangTag;
                            _Result = true;
                        }
                        else
                        {
                            _Error = "[[[This email address is already registered.]]]";
                            _Result = false;
                        }
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
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email+" and FirstName = "+model.FirstName + " and LastName = " + model.LastName);
            }

            return Json(new { Result = _Result, Error = _Error, UserFirstName = _UserFirstName });
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


                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            string username = User.Identity.GetUserName();
                            return Redirect(model.URLRedirect);
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", new { ReturnUrl = model.URLRedirect, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);

                            /*
                            string Email = Commons.EncryptHelper.EncryptToString(model.Password.Replace("'", "''"));

                            User User = UserService.GetUserByEMail(Email);

                            if (User != null)
                            {
                                if (User.PasswordDecrypt == model.Password)
                                {
                                    _Result = true;
                                    _UserFirstName = User.FirstNameDecrypt;
                                    // https://insidemysql.com/how-to-use-mysql-for-your-asp-net-identity-provider-with-a-custom-primary-key/
                             //       var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                                //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
                                }
                                else
                                {
                                    _Error = "[[[Invalid password for this user]]]";
                                }
                            }
                            else
                            {
                                _Error = "[[[Invalid username]]]";
                            }
                            */
                    }
                }
            }
            catch (Exception e)
            {
                _Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email);
            }

            return Json(new { Result = _Result, Error = _Error, UserFirstName = _UserFirstName, URLRedirect= model.URLRedirect });
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            UserId = null;
            return RedirectToAction("Index", "Home");
        }

        #region Login
        public ActionResult Login(string returnUrl=null)
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                model.URLRedirect = returnUrl;
                ViewBag.Title = "[[[Login]]]";
                
  
               
                Email Email = new Email();
                Email.ToEmail = "francois.laruaz2@gmail.com";
                Email.EMailTypeId = Commons.EmailType.Forgotpassword;
                //   bool result =EMailService.SendMail(Email);

                // UserService.UpdateDateLastConnection(5);
              //  EMailService.SendEMailToUser(5, Commons.EmailType.Forgotpassword);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "returnUrl = "+ returnUrl);
            }

            return View(model);
        }

 
        

#endregion
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
    }
}

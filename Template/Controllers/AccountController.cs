﻿using Commons;
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

namespace Website.Controllers
{
    public class AccountController : BaseController
    {

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
        public ActionResult _LoginForm(LoginViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _UserFirstName = "";

            try
            {
                
                if (ModelState.IsValid)
                {
                    string Email = Commons.EncryptHelper.EncryptToString(model.Password.Replace("'", "''"));

                    User User = UserService.GetUserByEMail(Email);

                    if (User != null)
                    {
                        if (User.PasswordDecrypt == model.Password)
                        {
                            _Result = true;
                            _UserFirstName = User.FirstNameDecrypt;
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
        #region Login
        public ActionResult Login(string returnUrl=null)
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                model.URLRedirect = returnUrl;
                ViewBag.Title = "[[[Login]]]";
                
                 List<string> Attachments = new List<string>();
                Attachments.Add(Const.BasePathImages+ "/Logo.png");
               
                Email Email = new Email();
                Email.ToEmail = "francois.laruaz@gmail.com";
                Email.EMailTypeId = EmailTemplate.Forgotpassword;
                Email.Attachments = Attachments;
                bool result =EMailService.SendMail(Email);


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

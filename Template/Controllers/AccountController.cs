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

namespace Website.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login(string returnUrl=null)
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                model.URLRedirect = returnUrl;
                ViewBag.Title = "Login";
                
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Email = Commons.EncryptHelper.EncryptToString(model.Password.Replace("'", "''"));

                    User User = UserService.GetUserByEMail(Email);

                    if (User!=null)
                    {
                        if(User.PasswordDecrypt==model.Password)
                        {
                            if (!String.IsNullOrWhiteSpace(model.URLRedirect))
                            {
                                return Redirect(model.URLRedirect);
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email);
            }

            return View(model);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
    }
}

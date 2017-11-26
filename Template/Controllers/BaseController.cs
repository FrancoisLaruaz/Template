using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using i18n;
using Commons;
using Service;
using Microsoft.AspNet.Identity;
using Models.BDDObject;

namespace Website.Controllers
{
    public class BaseController : Controller
    {
        public  string CurrentLangTag
        {
            get
            {
                try
                {
                    return Request.RequestContext.HttpContext.GetPrincipalAppLanguageForRequest().GetLanguage()?? Commons.Const.DefaultCulture;
                }
                catch
                {
                    return Commons.Const.DefaultCulture;
                }
            }
        }

        /// <summary>
        /// Get the id of the authenticated user
        /// </summary>
        public int? UserId
        {
            get
            {
                try
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return null;
                    }
                    else
                    {
                        if (Session[Commons.Const.SessionUserId] == null)
                        {
                            User ConnectedUser = UserService.GetUserByUserName(User.Identity.GetUserName());
                            if(ConnectedUser==null)
                            {
                                return null;
                            }
                            else
                            {
                                Session[Commons.Const.SessionUserId] = ConnectedUser.Id;
                                return ConnectedUser.Id;
                            }
                        }
                        else
                        {
                            return Convert.ToInt32(Session[Commons.Const.SessionUserId]);
                        }
                    }

                }
                catch
                {
                    return null;
                }
            }
            set
            {
                Session[Commons.Const.SessionUserId] = value;
            }
        }


        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                String ControllerName = filterContext.Controller.ToString();
                if ((!User.Identity.IsAuthenticated || !User.IsInRole(Commons.UserRoles.Admin)) && ControllerName== "Website.Controllers.AdminController" && filterContext.ActionDescriptor != null && filterContext.ActionDescriptor.ActionName != "Logs" && filterContext.ActionDescriptor.ActionName != "_DisplayLogs")
                {
               //     filterContext.Result = RedirectToAction("Login", "Account", new { returnUrl = Request.Url.AbsoluteUri.ToString() });
                    return;
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }
    }
}

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
using Models.Class;
using System.Web.Script.Serialization;
using System.Reflection;
using CommonsConst;

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
                    return Request.RequestContext.HttpContext.GetPrincipalAppLanguageForRequest().GetLanguage()?? CommonsConst.Const.DefaultCulture;
                }
                catch
                {
                    return CommonsConst.Const.DefaultCulture;
                }
            }
        }

        protected string JsonConstants
        {
            get
            {
                if (Session[CommonsConst.Const.JsonConstantsSession] == null)
                {
                    var constants = Commons.Utils.GetJSonConstants();
                    Session[CommonsConst.Const.JsonConstantsSession] = new JavaScriptSerializer().Serialize(constants);
                }
                return Session[CommonsConst.Const.JsonConstantsSession].ToString();
            }

            set
            {
                Session[CommonsConst.Const.JsonConstantsSession] = value;
            }
        }

        /// <summary>
        /// Get the id of the authenticated user
        /// </summary>
        public UserSession UserSession
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
                        if (Session[CommonsConst.Const.UserSession] == null)
                        {
                            UserSession ConnectedUser = UserService.GetUserSession(User.Identity.GetUserName());
                            if(ConnectedUser==null)
                            {
                                return null;
                            }
                            else
                            {
                                Session[CommonsConst.Const.UserSession] = ConnectedUser;
                                return ConnectedUser;
                            }
                        }
                        else
                        {
                            return Session[CommonsConst.Const.UserSession] as UserSession;
                        }
                    }

                }
                catch(Exception e)
                {
                    Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "User.Identity.IsAuthenticated = "+ User.Identity.IsAuthenticated + " and User.Identity.GetUserName() = "+ User.Identity.GetUserName());
                    return null;
                }
            }
            set
            {
                Session[CommonsConst.Const.UserSession] = value;
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


        /// <summary>
        /// Send constants to javascript
        /// </summary>
        /// <returns></returns>
        public ActionResult Constants()
        {
            try {
                return JavaScript("var Constants = " + JsonConstants + ";");
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return null;
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                String ControllerName = filterContext.Controller.ToString();
                if (ControllerName== "Website.Controllers.AdminController" && filterContext.ActionDescriptor != null && filterContext.ActionDescriptor.ActionName != "Logs" && filterContext.ActionDescriptor.ActionName != "_DisplayLogs")
                {
                  
                    if (!User.Identity.IsAuthenticated)
                        filterContext.Result = RedirectToAction("Login", "Account", new { returnUrl = Request.Url.AbsoluteUri.ToString() });
                    else if(!User.IsInRole(CommonsConst.UserRoles.Admin))
                        filterContext.Result = RedirectToAction("Index", "Home");
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

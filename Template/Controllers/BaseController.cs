﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Website.Controllers
{
    public class BaseController : Controller
    {
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

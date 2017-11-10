using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;


namespace Website.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult _Header()
        {

            try
            {

                if (User.Identity.IsAuthenticated)
                {

                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return PartialView("~/Views/Shared/Layout/_Header.cshtml");
        }
        public ActionResult Index()
        {
            try
            {

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
   
            return View();
        }
    }
}

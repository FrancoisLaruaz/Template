using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models;
using Models.ViewModels;
using Models.BDDObject;
using Service;
using Commons;

namespace Website.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult _Header()
        {
            HeaderViewModel model = new HeaderViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    User UserProfile = UserService.GetUserById(User.Identity.Name);
                    if (UserProfile != null)
                        model.UserFirstName = EncryptHelper.DecryptString(UserProfile.FirstName);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return PartialView("~/Views/Shared/Layout/_Header.cshtml", model);
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

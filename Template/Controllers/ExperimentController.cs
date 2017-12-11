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
using i18n;
using Revalee.Client;
using System.Configuration;
using System.IO;

namespace Website.Controllers
{
    public class ExperimentController : BaseController
    {

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Commons;
using Models.Class;
using Service;
namespace Website.Controllers
{
    public class AdminController : BaseController
    {



        [HttpGet]
        public ActionResult Index()
        { 
            return RedirectToAction("Logs");
        }

        #region EmailAudits

        [HttpGet]
        public ActionResult EmailAudits()
        {
            try
            {
                ViewBag.Title = "Email Audits";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View();
        }

        [HttpPost]
        public ActionResult _DisplayLogs(DisplayLogsViewModel Model)
        {
            try
            {
                Model = LogService.GetDisplayLogsViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Model.Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView(Model);
        }

        #endregion

        #region Logs

        [HttpGet]
        public ActionResult Logs()
        {
            LogsViewModel Model = new LogsViewModel();
            try
            {
                ViewBag.Title = "Logs";
                Model = LogService.GetLogsViewModel();
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult _DisplayEmailAudits(DisplayEmailAuditViewModel Model)
        {
            try
            {
                Model = EMailService.GetDisplayEmailAuditViewModel(Model.Pattern,Model.StartAt,Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Model.Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView(Model);
        }

        #endregion

    }
}

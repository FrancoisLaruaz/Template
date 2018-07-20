﻿using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Web.Optimization;

namespace Template
{
    public class BundleConfig
    {
        public static void RegisterJSBundles(BundleCollection bundles)
        {
            try
            {
                bundles.Add(new ScriptBundle("~/Scripts/JQuery").Include(
          "~/Scripts/jquery-{version}.js",
           "~/Scripts/jquery-ui.min.js"
           ));

                bundles.Add(new ScriptBundle("~/Scripts/JQueryVal").Include(
                            "~/Scripts/jquery.validate*",
                            "~/Scripts/jquery.unobtrusive*"));

                bundles.Add(new ScriptBundle("~/Scripts/ScriptsErrors").Include(
                "~/Scripts/General/BrowserHelper.js",
                "~/Scripts/General/Toastr.js",
                "~/Scripts/General/ToastrHelper.js",
                "~/Scripts/General/Utils.js",
                "~/Scripts/General/Error.js"
                ));


                bundles.Add(new ScriptBundle("~/Scripts/Scripts").Include(
                "~/Scripts/jquery.webcam.js",
                "~/Scripts/General/CarouselHelper.js",
                "~/Scripts/General/CaptchaHelper.js",
                "~/Scripts/General/Responsive.js",
                 "~/Scripts/moment.min.js",
                 "~/Scripts/bootstrap-datetimepicker.min.js",
                 "~/Scripts/General/daterangepicker.js",
                 "~/Scripts/General/Cookie.js",
                 "~/Scripts/General/BackToTop.js",
                "~/Scripts/General/AudioHelper.js",
                "~/Scripts/General/FileFormHelper.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/General/bootstrap-datepicker.js",
                "~/Scripts/General/bootstrap-slider.js",
                "~/Scripts/General/jtsage-datebox.min.js",
                "~/Scripts/General/jtsage-doc.js",
                "~/Scripts/General/SweetAlert.js",
                "~/Scripts/General/respond.js",
                "~/Scripts/General/DateCompare.js",
                "~/Scripts/General/CustomValidation.js",
                "~/Scripts/General/modernizr.js",
                "~/Scripts/General/SweetAlertHelper.js",
                "~/Scripts/General/FileDrop.js",
                "~/Scripts/General/FileDropHelper.js",
                 "~/Scripts/General/FlashVersion.js",
                "~/Scripts/General/WebcamHelper.js",
                 "~/Scripts/General/PopUp.js",
                "~/Scripts/Views/Account/_PasswordPolicy.js",
                "~/Scripts/General/lightbox.min.js"
              // "~/Scripts/General/Highcharts.js",
              //  "~/Scripts/General/HighchartsExporting.js",
              //  "~/Scripts/General/HighchartsHelper.js"
              , "~/Scripts/General/summernote.js"
               , "~/Scripts/General/summernoteHelper.js"
                , "~/Scripts/General/SexyCSSHelper.js"
                , "~/Scripts/General/Main.js"

                ));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }

        public static void RegisterCSSBundles(BundleCollection bundles)
        {
            try
            {
                bundles.Add(new StyleBundle("~/bundles/Bootstrap").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/General/bootstrap-slider.css",
                "~/Content/General/bootstrap-datepicker.css",
                "~/Content/General/bootstrap-social.css"
                ));

                bundles.Add(new StyleBundle("~/bundles/Style").Include(
                    "~/Content/General/animate.css",
                    "~/Content/General/_Spinner.css",
                    "~/Content/General/Carousel.css",
                     "~/Content/General/lightbox.min.css",
                    "~/Content/General/DragAndDrop.css",
                    "~/Content/font-awesome.min.css",
                    "~/Content/General/sweetalert.css",
                    "~/Content/General/PopUp.css",
                    "~/Content/jquery-ui-min.css",
                     "~/Content/General/fakeSocialMediaStyle.css",
                    "~/Content/General/toastr.csss",
                   "~/Content/General/webcam.css",
                   "~/Content/General/googleMap.css",
                    "~/Content/General/jtsage-datebox.min.css",
                    "~/Content/General/jtsage-syntax.css",
                    "~/Content/Views/Account/_PasswordPolicy.css"
                ));

                bundles.Add(new LessBundle("~/bundles/less").Include("~/Content/General/main.less"));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            try
            {
                RegisterCSSBundles(bundles);
                RegisterJSBundles(bundles);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}

using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Web.Optimization;

namespace Template
{
    public class BundleConfig
    {
        public static void RegisterJSBundles(BundleCollection bundles)
        {

             bundles.Add(new ScriptBundle("~/Scripts/JQuery").Include(
            "~/Scripts/jquery-2.2.3.min.js",
            "~/Scripts/jquery-ui.min.js",
            "~/Scripts/jquery.unobtrusive-ajax.min.js",
            "~/Scripts/jquery.validate.min.js",
            "~/Scripts/jquery.validate.unobtrusive.min.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/ScriptsErrors").Include(
            "~/Scripts/General/Toastr.js",
            "~/Scripts/General/ToastrHelper.js",
            "~/Scripts/General/Error.js"
            ));

          
            bundles.Add(new ScriptBundle("~/Scripts/Scripts").Include(
            "~/Scripts/General/Responsive.js",
             "~/Scripts/moment.min.js",
             "~/Scripts/bootstrap-datetimepicker.min.js",
             "~/Scripts/General/daterangepicker.js",
             "~/Scripts/General/Cookie.js",
             "~/Scripts/General/BackToTop.js",
            "~/Scripts/General/Utils.js",
            "~/Scripts/bootstrap.min.js",
            "~/Scripts/General/bootstrap-datepicker.js",
            "~/Scripts/General/bootstrap-slider.js",
            "~/Scripts/General/jtsage-datebox.min.js",
            "~/Scripts/General/SweetAlert.js",
            "~/Scripts/General/respond.js",
            "~/Scripts/General/modernizr.js",
            "~/Scripts/General/SweetAlertHelper.js",
            "~/Scripts/General/FileDrop.js",
            "~/Scripts/General/FileDropHelper.js",
            "~/Scripts/General/Highcharts.js",
            "~/Scripts/General/HighchartsExporting.js",
            "~/Scripts/General/HighchartsHelper.js",
            "~/Scripts/General/Main.js"
            ));
           
        }

        public static void RegisterCSSBundles(BundleCollection bundles)
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
                "~/Content/General/DragAndDrop.css",
                "~/Content/font-awesome.min.css",
                "~/Content/General/Main.css",
                "~/Content/General/sweetalert.css",
                "~/Content/General/toastr.csss",
                "~/Content/General/jtsage-datebox.min.css"
            ));
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCSSBundles(bundles);
            RegisterJSBundles(bundles);
        }
    }
}

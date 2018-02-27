using System;
using System.Text;
using System.Security.Cryptography;
using System.Web.Mvc;
using Website.Controllers;
using Commons;


namespace Website.ContentFileHelper
{
    public static class ContentFileHelpers
    {

        public static MvcHtmlString ScriptImportContent(this HtmlHelper htmlHelper, string contentPath, string minimisedContentPath = null)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(contentPath))
                {
                    var url = ContentFileController.ContentHashUrl(contentPath, "text/javascript", htmlHelper.ViewContext.HttpContext, new UrlHelper(htmlHelper.ViewContext.RequestContext));
                    return new MvcHtmlString(string.Format(@"<script src=""{0}""></script>", url));
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "contentPath= "
                + contentPath
                );
            }
            return new MvcHtmlString(string.Format(@"<script src=""{0}""></script>", contentPath));
        }


        public static MvcHtmlString CssImportContent(this HtmlHelper htmlHelper, string contentPath)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(contentPath))
                {
                    var url = ContentFileController.ContentHashUrl(contentPath, "text/css", htmlHelper.ViewContext.HttpContext, new UrlHelper(htmlHelper.ViewContext.RequestContext));
                    return new MvcHtmlString(String.Format(@"<link rel=""stylesheet"" type=""text/css"" href=""{0}"" />", url));
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "contentPath = "
                + contentPath);
            }
            return new MvcHtmlString(String.Format(@"<link rel=""stylesheet"" type=""text/css"" href=""{0}"" />", contentPath));
        }


        public static string ImageContent(this UrlHelper urlHelper, string contentPath)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(contentPath))
                {
                    string mime;
                    if (contentPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        mime = "image/png";
                    else if (contentPath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || contentPath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                        mime = "image/jpeg";
                    else if (contentPath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                        mime = "image/gif";
                    else
                        throw new NotSupportedException("Unexpected image extension.  Please add code to support it: " + contentPath);
                    return ContentFileController.ContentHashUrl(contentPath, mime, urlHelper.RequestContext.HttpContext, urlHelper);
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"contentPath = "
                + contentPath);
            }
            return null;
        }
    }
}
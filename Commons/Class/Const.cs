using System;
using System.Linq;
using System.Collections.Generic;

namespace Commons
{
    public static class Const
    {
        public static string DefaultCulture = "en";
        public static string WebsiteTitle = "Website Title";
        public static string GoogleAnalyticsID = "";
        // Define if the application need  login/logout features
        public static bool UserManagement = true;


        public static string BasePathRessources = "~/Ressources";
        public static string BasePathUpload = BasePathRessources +"/Upload";
        public static string BasePathFiles = BasePathRessources + "/Files";
        public static string BasePathTemplateEMails = BasePathRessources + "/EMails";
        public static string BasePathImages = BasePathFiles + "/Images";

		public static List<string> ListeExtensionsVideos = new List<string> { "MP4", "M4V", "MOV", "MKV", "AVI" };
		public static List<string> ListeExtensionsImage = new List<string> { "JPG", "JPEG", "PNG", "GIF" };
    }

    public static class EmailTemplate
    {
        public static int Forgotpassword = 1001;
    }

    public static class UserRoles
    {
        public static string Admin = "Admin";
    }

    public static class StaticFileVersion
    {
        public static string StaticFileVersionString = "20171006";
    }

}

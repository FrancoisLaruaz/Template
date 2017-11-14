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

        public static string i18nlangtag = "i18n.langtag";


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

    public static class CategoryTypes
    {
        public static int EmailType = 1;
        public static int UserRole = 2;
        public static int Language = 3;
    }

    public static class Languages
    {
        public const  int English = 3001;
        public const  int French = 3002;

        public static string ToString(int value)
        {
            switch (value)
            {
                case English:
                    return "en";
                case French:
                    return "fr";
                default:
                    return "";
            }
        }
    }


    public static class StaticFileVersion
    {
        public static string StaticFileVersionString = "20171006";
    }

}

using System;
using System.Linq;
using System.Collections.Generic;

namespace Commons
{
    public static class Const
    {
        public const string DefaultCulture = "en";
        public const string WebsiteTitle = "Website Title";
        public const  string GoogleAnalyticsID = "";
        public const  string FacebookPixelID = "110600362985483";
        public const  string EMailDev = "francois.laruaz@gmail.com";
        public const string DefaultPassword = "r4NHU$sNG(f'+:e!";
        // Define if the application need  login/logout features
        public const bool UserManagement = true;

        public const int MaxImageLength = 1000000;

        public const string BasePathRessources = "~/Ressources";
        public const string BasePathUpload = BasePathRessources +"/Upload";
        public const string BasePathFiles = BasePathRessources + "/Files";
        public const string BasePathTemplateEMails = BasePathRessources + "/EMails";
        public const string BasePathImages = BasePathFiles + "/Images";

        public const string DefaultImage = BasePathImages + "/Images/DefaultImage.jpg";
        public const string DefaultImageUser = BasePathImages + "/Images/DefaultUser.jpg";

        public const string i18nlangtag = "i18n.langtag";

        public const string UserSession = "UserSession";

    }

    public static class EmailType
    {
        public const int Forgotpassword = 1001;
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
    }

    public static class CategoryTypes
    {
        public const int EmailType = 1;
        public const int UserRole = 2;
        public const int Language = 3;
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


    public static class TaskLogTypes
    {
        public const int ErrorCleanUp = 4001;
    }


    public static class StaticFileVersion
    {
        public const string StaticFileVersionString = "20171006";
    }

}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace CommonsConst
{
    public static class Const
    {


        public const string DefaultCulture = "en";
        public const string WebsiteTitle = "Website Title";
        public const  string GoogleAnalyticsID = "";
        public const  string FacebookPixelID = "110600362985483";
        public const  string EMailDev = "francois.laruaz@gmail.com";

        // Define if the application need  login/logout features
        public const bool UserManagement = true;

        public const int MaxImageLength = 10000000;

        public const string BasePathRessources = "~/Ressources";
        public const string BasePathUpload = BasePathRessources + "/Upload";
        public const string BasePathUploadDecrypted = BasePathUpload + "/Decrypted";
        public const string BasePathUploadEncrypted = BasePathUpload + "/Encrypted";
        public const string BasePathFiles = BasePathRessources + "/Files";
        public const string BasePathTemplateEMails = BasePathRessources + "\\EMails";
        public const string BasePathImages = BasePathFiles + "/Images";

        public const string DefaultImage = BasePathImages + "/DefaultImage.jpg";
        public const string DefaultImageUser = BasePathImages + "/DefaultUser.jpg";
        public const string DefaultThumbnailUser = BasePathImages + "/DefaultThumbnailUser.jpg";


        public const string i18nlangtag = "i18n.langtag";

        public const string UserSession = "UserSession";
        public const string WebcamCaptureSession = "WebcamCaptureSession";
        public const string JsonConstantsSession = "JsonConstantsSession";

        public const string ColorWebsite = "rgba(45,191,183, 1)";
    }

    public static class EmailTypes
    {
        public const int Forgotpassword = 1001;
        public const int UserWelcome = 1002;
        public const int News = 1003;
        public const int ResetPassword = 1004;
        public const int Contact = 1005;
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
    }

    public static class ExternalAuthentificationRedirection
    {
        public const string RedirectToEmailSignUp = "RedirectToEmailSignUp";
        public const string RedirectToLogin = "RedirectToLogin";
        public const string RedirectToExternalSignUp = "RedirectToExternalSignUp";
    }

    public static class CategoryTypes
    {
        public const int EmailType = 1;
        public const int UserRole = 2;
        public const int Language = 3;
        public const int TaskLogType = 4;
        public const int NewsType = 5;
        public const int TypeUserMailing = 6;
    }

    public static class LoginProviders
    {
        public static string Facebook = "Facebook";
        public static string Google = "Google";
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

        public static int ToInt(string value)
        {
            switch (value)
            {
                case "en":
                    return English;
                case "fr":
                    return French;
                default:
                    return ToInt(Const.DefaultCulture);
            }
        }
    }


    public static class Periodicities
    {
        public const string Daily = "[[[Daily]]]";
    }

    public static class ScheduledTaskTypes
    {
        public const string SendEMailToUser = "SendEMailToUser";
        public const string SendNews = "SendNews";
    }

    public static class TaskLogTypes
    {
        public const string DeleteLogs = "DeleteLogs";
        public const string DeleteUploadedFile = "DeleteUploadedFile";
    }

    public static class TypeUserMailing
    {
        public const int AllUsers = 6001;
        public const int ConfirmedUsers = 6002;
    }

    public static class NewsType
    {
        public const int PublishOnly = 5001;
        public const int PublishAndMail = 5002;
        public const int MailOnly = 5003;
    }


    public static class StaticFileVersion
    {
        public const string StaticFileVersionString = "20171206";
    }

}

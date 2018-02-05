using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Models.ViewModels;
using System.IO;
using CommonsConst;

namespace Service
{
    public static class EMailService
    {

        private static string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        /// <summary>
        /// Methode used to send an email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool SendMail(Email Email)
        {
            bool result = false;
            try
            {
                int LanguageId = CommonsConst.Languages.English;
                string LangTag = CommonsConst.Languages.ToString(LanguageId);




                if (Email.EmailContent == null)
                {
                    Email.EmailContent = new List<Tuple<string, string>>();
                }
                User User = null;
                if (Email.UserId != null && Email.UserId.Value > 0)
                {
                    User = UserService.GetUserById(Email.UserId.Value);
                }

                if (User != null && User.Id > 0)
                {
                    LangTag = User.LanguageCode;
                    LanguageId = User.LanguageId;
                    if (String.IsNullOrWhiteSpace(Email.ToEmail))
                    {
                        Email.ToEmail = User.EMailDecrypt;
                    }
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", User.FirstNameDecrypt));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", User.UserFullNameDecrypt));
                }
                else
                {
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", "user"));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", "user"));
                }
                Email.EmailContent.Add(new Tuple<string, string>("#WebSiteURL#", Utils.Website));
                Email.LanguageId = LanguageId;

                EMailTypeLanguage EMailTypeLanguage = GetEMailTypeLanguage(Email.EMailTypeId, Email.LanguageId);
                if (EMailTypeLanguage == null && Email.LanguageId != Languages.English)
                {
                    EMailTypeLanguage = GetEMailTypeLanguage(Email.EMailTypeId, Languages.English);
                }

                if (EMailTypeLanguage != null && !String.IsNullOrWhiteSpace(Email.ToEmail))
                {
                    if (!Utils.IsProductionWebsite())
                    {
                        Email.EmailContent.Add(new Tuple<string, string>("#RealUserEMail#", "Real mail : " + Email.ToEmail));
                        Email.ToEmail = CommonsConst.Const.EMailDev;
                    }
                    else
                    {
                        Email.EmailContent.Add(new Tuple<string, string>("#RealUserEMail#", "&nbsp;"));
                    }

                    Email.EMailTypeLanguageId = EMailTypeLanguage.Id;
                    Email.EMailTemplate = EMailTypeLanguage.TemplateName;
                    Email.BasePathFile = Email.RootPathDefault+Const.BasePathTemplateEMails.Replace("~/", "\\");
                    Email.EndMailTemplate = EMailTypeLanguage.EndEMailTemplateName;
                    if (String.IsNullOrWhiteSpace(Email.Subject))
                        Email.Subject = EMailTypeLanguage.Subject;
                    if (String.IsNullOrWhiteSpace(Email.FromEmail))
                        Email.FromEmail = EMailHelper.MailAdress;
                    Email.AttachmentsMails = new List<System.Net.Mail.Attachment>();
                    if (Email.Attachments != null)
                    {
                        foreach (string file in Email.Attachments)
                        {
                            try
                            {
                                string FileName = FileHelper.GetStorageRoot(file); 
                                byte[] BitesTab = FileHelper.GetFileToDownLoad(FileName);
                                if (BitesTab != null)
                                {
                                    Email.AttachmentsMails.Add(new System.Net.Mail.Attachment(new MemoryStream(BitesTab), Path.GetFileName(FileName)));
                                }
                            }
                            catch (Exception e)
                            {
                                result = false;
                                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "file = " + file);
                            }
                        }
                    }
                    Task.Factory.StartNew(() => SendMailAsync(Email));
                    result = true;
                }
                else
                {
                    result = false;
                    Commons.Logger.GenerateError(new Exception("No emailtypelanguage found"), System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserMail = " + Email.ToEmail + " and Language = " + Email.LanguageId + " and EMailTypeId = " + Email.EMailTypeId);
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserMail = " + Email.ToEmail + " and subject = " + Email.Subject + " and EMailTypeId = " + Email.EMailTypeId);
            }
            return result;
        }

        /// <summary>
        /// Function called in order to send an asynchronous mail
        /// </summary>
        /// <param name="EMail"></param>
        public static void SendMailAsync(Email EMail)
        {
            try
            {
                Tuple<bool, int, int> ResultMail = EMailHelper.SendMail(EMail);
                if (ResultMail != null)
                {
                    if (ResultMail.Item1)
                    {
                        EMailService.InsertEMailAudit(EMail, ResultMail.Item2, ResultMail.Item3);
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMail.EMailTypeId + " and emailto =" + EMail.ToEmail);
            }
        }

        /// <summary>
        /// Send a mail to a user
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="EMailTypeId"></param>
        /// <returns></returns>
        public static bool SendEMailToUser(string UserName, int EMailTypeId)
        {
            bool result = false;
            try
            {


                User UserMail = UserService.GetUserByUserName(UserName);
                if (UserMail != null)
                {
                    int UserId = UserMail.Id;
                    result = SendEMailToUser(UserId, EMailTypeId);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMailTypeId + " and UserName =" + UserName);
            }
            return result;
        }

       
        /// <summary>
        /// Send a mail to a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="EMailTypeId"></param>
        /// <returns></returns>
        public static bool SendEMailToUser(int UserId, int EMailTypeId)
        {
            bool result = false;
            try
            {
                User UserMail = UserService.GetUserById(UserId);
                if (UserMail != null)
                {
                    Email Email = new Email();
                    Email.UserId = UserId;
                    Email.EMailTypeId = EMailTypeId;
                    Email.RootPathDefault = FileHelper.GetRootPathDefault()+ @"\";
                    List<Tuple<string, string>> EmailContent = new List<Tuple<string, string>>();
                    switch (EMailTypeId)
                    {
                        case CommonsConst.EmailTypes.Forgotpassword:
                            string ResetPasswordUrl = WebsiteURL + "/ResetPassword?UserId=" + UserMail.Id + "&Token=" + Commons.HashHelpers.HashEncode(UserMail.ResetPasswordToken);
                            EmailContent.Add(new Tuple<string, string>("#ResetPasswordUrl#", ResetPasswordUrl));
                            break;
                        case CommonsConst.EmailTypes.UserWelcome:
                            string ConfirmEmailUrl = WebsiteURL + "/ConfirmEmail?UserId=" + UserMail.Id + "&Token=" + Commons.HashHelpers.HashEncode(UserMail.EmailConfirmationToken);
                            EmailContent.Add(new Tuple<string, string>("#ConfirmEmailUrl#", ConfirmEmailUrl));
                            break;
                    }
                    Email.EmailContent = EmailContent;


                    result = EMailService.SendMail(Email);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMailTypeId + " and UserId =" + UserId);
            }
            return result;
        }

        /// <summary>
        /// Get the email type with the good language
        /// </summary>
        /// <param name="EMailTypeId"></param>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        public static EMailTypeLanguage GetEMailTypeLanguage(int EMailTypeId, int LanguageId)
        {
            EMailTypeLanguage retour = new EMailTypeLanguage();
            try
            {
                retour = EMailDAL.GetEMailTypeLanguage(EMailTypeId, LanguageId);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMailTypeId.ToString() + " and LanguageId=" + LanguageId.ToString());
            }
            return retour;
        }

        /// <summary>
        /// Insert a lign in the database with the result of the mail
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="AttachmentNumber"></param>
        /// <param name="CCUsersNumber"></param>
        /// <returns></returns>
        public static bool InsertEMailAudit(Email EMail, int AttachmentNumber, int CCUsersNumber)
        {
            bool result = false;
            try
            {
                EMailAudit Audit = new EMailAudit();
                Audit.UserId = EMail.UserId;
                Audit.EMailTypeLanguageId = EMail.EMailTypeLanguageId;
                Audit.EMailFrom = EncryptHelper.EncryptToString(EMail.FromEmail);
                Audit.EMailTo = EncryptHelper.EncryptToString(EMail.ToEmail);
                Audit.Date = DateTime.UtcNow;
                Audit.AttachmentNumber = AttachmentNumber;
                Audit.CCUsersNumber = CCUsersNumber;
                result = EMailDAL.InsertAudit(Audit);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMail.EMailTypeId + " and emailto =" + EMail.ToEmail);
            }
            return result;
        }

        public static DisplayEmailAuditViewModel GetDisplayEmailAuditViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayEmailAuditViewModel model = new DisplayEmailAuditViewModel();
            try
            {
                model = EMailDAL.GetEMailsAuditList(Pattern, StartAt, PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }



    }
}

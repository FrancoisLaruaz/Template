using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class EMailService
    {

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
                    if (String.IsNullOrWhiteSpace(Email.ToEmail))
                    {
                        Email.ToEmail = User.EmailDecrypt;
                    }
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", User.FirstNameDecrypt));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", User.FirstNameDecrypt + " " + User.LastNameDecrypt));
                }
                else
                {
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", "user"));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", "user"));
                }
                Email.EmailContent.Add(new Tuple<string, string>("#WebSiteURL#", Utils.GetURLWebsite()));

                Category MailType = CategoryService.GetCategoryById(Email.EMailTypeId);
                if (MailType != null && !String.IsNullOrWhiteSpace(Email.ToEmail))
                {
                    Email.EMailTemplate = MailType.Name;
                    Email.BasePathFile = FileHelper.GetStorageRoot(Const.BasePathTemplateEMails);
                    if (String.IsNullOrWhiteSpace(Email.Subject))
                        Email.Subject = MailType.Description;
                    if (String.IsNullOrWhiteSpace(Email.FromEmail))
                        Email.FromEmail = EMailHelper.MailAdress;
                    Email.AttachmentsMails = new List<System.Net.Mail.Attachment>();
                    if (Email.Attachments != null)
                    {
                        foreach (string file in Email.Attachments)
                        {
                            Email.AttachmentsMails.Add(new System.Net.Mail.Attachment(FileHelper.GetStorageRoot(file)));
                        }
                    }
                    Task.Factory.StartNew(() => SendMailAsync(Email));
                    result = true;
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
                Tuple<bool, int> ResultMail = EMailHelper.SendMail(EMail);
                if (ResultMail != null)
                {
                    if (ResultMail.Item1)
                    {
                        EMailService.InsertEMailAudit(EMail, ResultMail.Item2);
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMail.EMailTypeId + " and emailto =" + EMail.ToEmail);
            }
        }

        /// <summary>
        /// Insert a lign in the database with the result of the mail
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="AttachmentNumber"></param>
        /// <returns></returns>
        public static bool InsertEMailAudit(Email EMail, int AttachmentNumber)
        {
            bool result = false;
            try
            {
                EMailAudit Audit = new EMailAudit();
                Audit.UserId = EMail.UserId;
                Audit.EMailTypeId = EMail.EMailTypeId;
                Audit.EMailFrom = EncryptHelper.EncryptToString(EMail.FromEmail);
                Audit.EMailTo = EncryptHelper.EncryptToString(EMail.ToEmail);
                Audit.Date = DateTime.UtcNow;
                Audit.AttachmentNumber = AttachmentNumber;
                result = EMailAuditDAL.InsertAudit(Audit);
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
                model = EMailAuditDAL.GetEMailsAuditList(Pattern, StartAt, PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }



    }
}

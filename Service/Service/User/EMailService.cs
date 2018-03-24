﻿using Commons;
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
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.Class.Email;


namespace Service.UserArea
{
    public  class EMailService : IEMailService
    {

        private  string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        private readonly IGenericRepository<ValidTopLevelDomain> _validTopLevelDomainRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;
        private readonly IGenericRepository<EmailTypeLanguage> _emailTypeLanguageRepo;
        private readonly IGenericRepository<EmailAudit> _emailAuditRepo;

        public EMailService(IGenericRepository<DataEntities.Model.User> userRepo,
            IGenericRepository<ValidTopLevelDomain> validTopLevelDomainRepo,
            IGenericRepository<EmailTypeLanguage> emailTypeLanguageRepo,
            IGenericRepository<EmailAudit> emailAuditRepo)
        {
            _userRepo = userRepo;
            _validTopLevelDomainRepo = validTopLevelDomainRepo;
            _emailTypeLanguageRepo = emailTypeLanguageRepo;
            _emailAuditRepo = emailAuditRepo;
        }

        public EMailService()
        {
            var context = new TemplateEntities();
            _userRepo = new GenericRepository<DataEntities.Model.User>(context);
            _validTopLevelDomainRepo = new GenericRepository<DataEntities.Model.ValidTopLevelDomain>(context);
            _emailTypeLanguageRepo = new GenericRepository<DataEntities.Model.EmailTypeLanguage>(context);
        }

        public bool IsEmailAddressValid(string emailAddress)
        {
            bool result = false;
            try
            {
                if (Utils.IsValidMail(emailAddress) && IsTopLevelDomainValid(emailAddress))
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " emailAddress = " + emailAddress);
            }
            return result;
        }


        public bool IsTopLevelDomainValid(string emailAddress)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(emailAddress))
                {
                    if (emailAddress.Split('.').Length > 1)
                    {
                        string domain = emailAddress.Split('.')[emailAddress.Split('.').Length - 1].ToLower();
                        result = _validTopLevelDomainRepo.FindAllBy(v => v.Name == domain).Any();
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " emailAddress = " + emailAddress);
            }
            return result;
        }

        /// <summary>
        /// Methode used to send an email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public  bool SendMail(Email Email)
        {
            bool result = false;
            try
            {
                int LanguageId = CommonsConst.Languages.English;
                string LangTag = CommonsConst.Languages.ToString(LanguageId);

                if(String.IsNullOrWhiteSpace(Email.RootPathDefault))
                    Email.RootPathDefault = FileHelper.GetRootPathDefault() + @"\";


                if (Email.EmailContent == null)
                {
                    Email.EmailContent = new List<Tuple<string, string>>();
                }
                DataEntities.Model.User User = null;
                if (Email.UserId != null && Email.UserId.Value > 0)
                {
                    User = _userRepo.Get(Email.UserId.Value);
                }

                if (User != null && User.Id > 0)
                {
                    LangTag = User.Language?.Code??CommonsConst.Const.DefaultCulture;
                    LanguageId = User.LanguageId;
                    if (String.IsNullOrWhiteSpace(Email.ToEmail))
                    {
                        Email.ToEmail = EncryptHelper.DecryptString(User.AspNetUser.Email); 
                    }
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", EncryptHelper.DecryptString(User.FirstName)));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", EncryptHelper.DecryptString(User.LastName)));
                }
                else
                {
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", "user"));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", "user"));
                }
                Email.EmailContent.Add(new Tuple<string, string>("#WebSiteURL#", Utils.Website));
                Email.LanguageId = LanguageId;

                EmailTypeLanguage EMailTypeLanguage = GetEMailTypeLanguage(Email.EMailTypeId, Email.LanguageId);
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
                    Email.EndMailTemplate = EMailTypeLanguage.TemplateName;
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
                    Commons.Logger.GenerateInfo("No emailtypelanguage found : UserMail = " + Email.ToEmail + " and Language = " + Email.LanguageId + " and EMailTypeId = " + Email.EMailTypeId);
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
        public  void SendMailAsync(Email EMail)
        {
            try
            {
                Tuple<bool, int, int> ResultMail = EMailHelper.SendMail(EMail);
                if (ResultMail != null)
                {
                    if (ResultMail.Item1)
                    {
                        InsertEMailAudit(EMail, ResultMail.Item2, ResultMail.Item3);
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
        public  bool SendEMailToUser(string UserName, int EMailTypeId)
        {
            bool result = false;
            try
            {

                DataEntities.Model.User UserMail = _userRepo.FindAllBy(u => u.AspNetUser.UserName.Trim().ToLower()== UserName.Trim().ToLower()).FirstOrDefault();
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
        public  bool SendEMailToUser(int UserId, int EMailTypeId)
        {
            bool result = false;
            try
            {
                DataEntities.Model.User UserMail = _userRepo.Get(UserId);
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
                            string ResetPasswordUrl = WebsiteURL + "/ResetPassword/" + UserMail.Id + "/" + Commons.HashHelpers.HashEncode(UserMail.ResetPasswordToken);
                            EmailContent.Add(new Tuple<string, string>("#ResetPasswordUrl#", ResetPasswordUrl));
                            break;
                        case CommonsConst.EmailTypes.UserWelcome:
                            string ConfirmEmailUrl = WebsiteURL + "/ConfirmEmail?UserId=" + UserMail.Id + "&Token=" + Commons.HashHelpers.HashEncode(UserMail.EmailConfirmationToken);
                            EmailContent.Add(new Tuple<string, string>("#ConfirmEmailUrl#", ConfirmEmailUrl));
                            break;
                        case CommonsConst.EmailTypes.ResetPassword:
                            string ChangePasswordUrl = WebsiteURL + "/ResetPassword";
                            EmailContent.Add(new Tuple<string, string>("#ChangePasswordUrl#", ChangePasswordUrl));
                            break;
                    }
                    Email.EmailContent = EmailContent;


                    result = SendMail(Email);
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
        public EmailTypeLanguage GetEMailTypeLanguage(int EMailTypeId, int LanguageId)
        {
            EmailTypeLanguage result = null;
            try
            {
                result = _emailTypeLanguageRepo.FindAllBy(e => e.EMailTypeId == EMailTypeId && LanguageId == e.LanguageId).FirstOrDefault();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMailTypeId.ToString() + " and LanguageId=" + LanguageId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Insert a lign in the database with the result of the mail
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="AttachmentNumber"></param>
        /// <param name="CCUsersNumber"></param>
        /// <returns></returns>
        public  bool InsertEMailAudit(Email EMail, int AttachmentNumber, int CCUsersNumber)
        {
            bool result = false;
            try
            {
                EmailAudit Audit = new EmailAudit();
                Audit.UserId = EMail.UserId;
                Audit.EMailTypeLanguageId = EMail.EMailTypeLanguageId;
                Audit.EMailFrom = EncryptHelper.EncryptToString(EMail.FromEmail);
                Audit.EMailTo = EncryptHelper.EncryptToString(EMail.ToEmail);
                Audit.Date = DateTime.UtcNow;
                Audit.AttachmentNumber = AttachmentNumber;
                Audit.CCUsersNumber = CCUsersNumber;
                Audit.ScheduledTaskId = EMail.RelatedScheduledTaskId;
                Audit.Comment = EMail.Comment;
                _emailAuditRepo.Add(Audit);
                result = _emailAuditRepo.Save();

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMail.EMailTypeId + " and emailto =" + EMail.ToEmail);
            }
            return result;
        }

        public  DisplayEmailAuditViewModel GetDisplayEmailAuditViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayEmailAuditViewModel model = new DisplayEmailAuditViewModel();
            try
            {
                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower().Trim();

                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {

                }
                else
                {

                }


                db = new DBConnect();
                string Query = "select a.Id, a.UserId, a.EMailFrom, a.EMailTo ,a.Date ";
                Query = Query + ",a.AttachmentNumber,a.EMailTypeLanguageId,u.FirstName, u.LastName,c.Name, a.Comment ";
                Query = Query + ",l.EMailTypeId, l.LanguageId,cl.Name as 'LanguageName'  ";
                Query = Query + ",n.Id as NewsId, n.Title as Newstitle, a.ScheduledTaskId ";
                Query = Query + "from emailaudit a ";
                Query = Query + " inner join emailtypelanguage l on l.id=a.EMailTypeLanguageId ";
                Query = Query + " left join user u on u.Id=a.UserId  ";
                Query = Query + " left join category c on c.Id=l.EMailTypeId ";
                Query = Query + " left join category cl on cl.Id=l.LanguageId ";
                Query = Query + " left join scheduledtask st on st.Id=a.ScheduledTaskId ";
                Query = Query + " left join news n on n.Id=st.newsid ";
                Query = Query + "where 1=1 ";
                Query = Query + "order by a.Id desc";
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    model.Count = db.GetData(Query).Rows.Count;
                    Query = Query + " LIMIT " + PageSize.ToString() + " OFFSET " + StartAt.ToString();
                }

                DataTable data = db.GetData(Query);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = data.Rows[i];
                    EMailAudit Audit = new EMailAudit();
                    Audit.Id = MySQLHelper.GetIntFromMySQL(dr["Id"]).Value;
                    Audit.UserId = MySQLHelper.GetIntFromMySQL(dr["UserId"]);
                    Audit.LanguageName = Convert.ToString(dr["LanguageName"]);
                    Audit.LanguageId = MySQLHelper.GetIntFromMySQL(dr["LanguageId"]).Value;
                    Audit.EMailTypeId = MySQLHelper.GetIntFromMySQL(dr["EMailTypeId"]).Value;
                    Audit.EMailTypeLanguageId = MySQLHelper.GetIntFromMySQL(dr["EMailTypeLanguageId"]).Value;
                    Audit.EMailFrom = Convert.ToString(dr["EMailFrom"]);
                    Audit.UserFirstName = Convert.ToString(dr["FirstName"]);
                    Audit.EMailTo = Convert.ToString(dr["EMailTo"]);
                    Audit.UserLastName = Convert.ToString(dr["LastName"]);
                    Audit.Comment = Convert.ToString(dr["Comment"]);
                    Audit.Date = MySQLHelper.GetDateFromMySQL(dr["Date"]).Value.ToLocalTime();
                    Audit.EMailTypeName = Convert.ToString(dr["Name"]);
                    Audit.AttachmentNumber = Convert.ToInt32(dr["AttachmentNumber"]);
                    Audit.ScheduledTaskId = MySQLHelper.GetIntFromMySQL(dr["ScheduledTaskId"]);
                    Audit.NewsId = MySQLHelper.GetIntFromMySQL(dr["NewsId"]);
                    Audit.NewsTitle = MySQLHelper.GetStringFromMySQL(dr["NewsTitle"]);
                    Audit.EMailFromDecrypt = EncryptHelper.DecryptString(Audit.EMailFrom)?.ToLower().Trim();
                    Audit.EMailToDecrypt = EncryptHelper.DecryptString(Audit.EMailTo)?.ToLower().Trim();
                    Audit.UserFirstNameDecrypt = EncryptHelper.DecryptString(Audit.UserFirstName)?.ToLower().Trim();
                    Audit.UserLastNameDecrypt = EncryptHelper.DecryptString(Audit.UserLastName)?.ToLower().Trim();
                    ListAudits.Add(Audit);
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<EMailAudit> resultIEnumerable = ListAudits as IEnumerable<EMailAudit>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.EMailTypeName != null && a.EMailTypeName.ToLower().Contains(Pattern)) || (a.NewsTitle != null && a.NewsTitle.ToLower().Contains(Pattern)) || (a.UserFirstNameDecrypt != null && a.UserFirstNameDecrypt.ToLower().Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.EMailTypeName.Contains(Pattern) || (a.EMailToDecrypt != null && a.EMailToDecrypt.ToLower().Contains(Pattern)) || (a.EMailFromDecrypt != null && a.EMailFromDecrypt.ToLower().Contains(Pattern)) || (a.UserLastNameDecrypt != null && a.UserLastNameDecrypt.ToLower().Contains(Pattern)));
                    model.Count = resultIEnumerable.ToList().Count;
                    ListAudits = resultIEnumerable.Take(PageSize).Skip(StartAt).OrderByDescending(a => a.Id).ToList();
                }

                model.AuditsList = ListAudits;


                // model = EMailDAL.GetEMailsAuditList(Pattern, StartAt, PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }



    }
}

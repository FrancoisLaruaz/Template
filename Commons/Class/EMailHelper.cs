﻿using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using Models;
using Models.Class;
using Commons;
using Models.BDDObject;

namespace Commons
{
    public class EMailHelper
    {

        static string MailName = "Zaural Website Ltd";
        public static string MailAdress = "zaural.website@gmail.com";
        static string PasswordMailAdress = "071185004154171035122139135090006102148063065162163134156047062127032176181228062101123048131167";
        static string Banner = "http://mjcloreillecassee.com/home/wp-content/uploads/2017/03/Banniere-Soiree-Latino-site-OKC.jpg";
        static string Signature = "Zaural Website Team";
        static string CompanyDescription = "EMails and Website made by Zaural Website Ltd.";
        static string AdressFooter1 = "304-2385 West 5th Avenue";
        static string AdressFooter2 = "Vancouver, BC, V6K 1S6, CANADA";
        static string CompanyFooter = "Zaural Website is a registered trademark";
        static string SizeText = "14px";
        static string SizeTextButton = "16px";
        static string SizeTextFooter = "13px";


        public static List<Tuple<string, string>> GetGenericEmailContent()
        {
            List<Tuple<string, string>> GenericEmailContent = new List<Tuple<string, string>>();
            try
            {
                GenericEmailContent.Add(new Tuple<string, string>("#Banner#", Banner));
                GenericEmailContent.Add(new Tuple<string, string>("#Signature#", Signature));
                GenericEmailContent.Add(new Tuple<string, string>("#CompanyDescription#", CompanyDescription));
                GenericEmailContent.Add(new Tuple<string, string>("#AdressFooter1#", AdressFooter1));
                GenericEmailContent.Add(new Tuple<string, string>("#AdressFooter2#", AdressFooter2));
                GenericEmailContent.Add(new Tuple<string, string>("#CompanyFooter#", CompanyFooter));
                GenericEmailContent.Add(new Tuple<string, string>("#ColorElement#", CommonsConst.Const.ColorWebsite));
                GenericEmailContent.Add(new Tuple<string, string>("#WebsiteTitle#", CommonsConst.Const.WebsiteTitle));
                GenericEmailContent.Add(new Tuple<string, string>("#SizeText#", SizeText));
                GenericEmailContent.Add(new Tuple<string, string>("#SizeTextButton#", SizeTextButton));
                GenericEmailContent.Add(new Tuple<string, string>("#SizeTextFooter#", SizeTextFooter));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return GenericEmailContent;
        }



        /// <summary>
        /// Send an email using gmail
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static Tuple<bool,int,int> SendMail(Email Email)
        {
            bool result = false;
            int NbSentAttachments = 0;
            int CCUsersNumber = 0;
            Tuple<bool, int,int> EMailResult = null;
            try
            {
                MailAddress fromAddress = new MailAddress(Email.FromEmail, MailName);


                MailAddress toAddress = new MailAddress(Email.ToEmail, "To " + Email.ToEmail);
                string fromPassword = EncryptHelper.DecryptString(PasswordMailAdress);

                if (String.IsNullOrWhiteSpace(Email.EndMailTemplate))
                    Email.EndMailTemplate = "_EndMail_en";

                if (!String.IsNullOrWhiteSpace(Email.EMailTemplate) && !String.IsNullOrWhiteSpace(Email.ToEmail))
                {
                    string TemplateName = Email.EMailTemplate;
                    Email.Subject = "#WebsiteTitle# - " + Email.Subject;

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    string PathHeaderOnServer = Email.BasePathFile + "\\_HeaderMail.html";
                    string PathFooterOnServer = Email.BasePathFile + "\\_FooterMail.html";
                    string PathEndMailOnServer = Email.BasePathFile + "\\"+Email.EndMailTemplate+".html";
                    string PathTemplateOnServer = Email.BasePathFile  + "\\" + TemplateName + ".html";
                    string headerTemplate = new StreamReader(PathHeaderOnServer).ReadToEnd();
                    string bodyTemplate = new StreamReader(PathTemplateOnServer).ReadToEnd();
                    string footerTemplate = new StreamReader(PathFooterOnServer).ReadToEnd();
                    string endMailTemplate = new StreamReader(PathEndMailOnServer).ReadToEnd();
                    bodyTemplate = headerTemplate +  bodyTemplate + endMailTemplate + footerTemplate;
                //    bodyTemplate=  new StreamReader(Email.BasePathFile + "/_Test.html").ReadToEnd();
                    foreach (var content in Email.EmailContent)
                    {
                        bodyTemplate = bodyTemplate.Replace(content.Item1, content.Item2);
                        Email.Subject = Email.Subject.Replace(content.Item1, content.Item2);
                    }
                    List<Tuple<string, string>> GenericEmailContent = GetGenericEmailContent();
                    foreach (var content in GenericEmailContent)
                    {
                        bodyTemplate = bodyTemplate.Replace(content.Item1, content.Item2);
                        Email.Subject = Email.Subject.Replace(content.Item1, content.Item2);
                    }



                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = Email.Subject,
                        Body = bodyTemplate,
                        IsBodyHtml = true
                    })
                    {
                        if (Email.AttachmentsMails != null)
                        {
                            foreach (System.Net.Mail.Attachment file in Email.AttachmentsMails)
                            {
                                if (file!=null)
                                {
                                    try
                                    {
                                        message.Attachments.Add(file);
                                        NbSentAttachments++;
                                    }
                                    catch (Exception e)
                                    {
                                        Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "file = " + file + " and UserMail = " + Email.ToEmail + " and subject = " + Email.Subject);
                                    }
                                }
                            }
                        }
                        if (Email.CCList != null)
                        {
                            foreach (string CC in Email.CCList)
                            {
                                message.CC.Add(CC);
                                CCUsersNumber++;
                            }
                        }
                        smtp.Send(message);
                    }
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserMail = " + Email.ToEmail + " and subject = " + Email.Subject + " and EMailTypeId = " + Email.EMailTypeId);
            }
            EMailResult = new Tuple<bool, int,int>(result, NbSentAttachments, CCUsersNumber);
            return EMailResult;
        }


    }
}

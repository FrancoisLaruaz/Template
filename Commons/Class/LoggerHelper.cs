using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Commons
{
    public static class Logger
    {


        public static List<string> NotLoggedErrorsUrl = new List<string> { "Error/LogJavascriptError" };
        public static List<string> NotLoggedErrorsMessage = new List<string> { "Le contrôleur pour le chemin" };


        public static void GenerateJavascriptError(string Message = "", string URL = "", string lineNumber = "", string col = "", string error = "", string browser = "")
        {
            try
            {

                log4net.ILog logger = log4net.LogManager.GetLogger("*** JAVASCRIPT ***");
                if (string.IsNullOrEmpty(URL))
                {
                    try
                    {
                        URL = HttpContext.Current?.Request?.Url?.AbsoluteUri;
                    }
                    catch
                    {
                        URL = "Unknown";
                    }
                }

                bool LoggeError = Logger.IsErrorLogged(URL, Message);

                if (LoggeError && (!String.IsNullOrEmpty(Message) || !String.IsNullOrEmpty(URL) || !String.IsNullOrEmpty(error)))
                {
                    Message = "-Message => " + Message + " </br></br>- URL => " + URL + " </br></br>- lineNumber => " + lineNumber + "</br></br>- col => " + col + "</br></br>- browser => " + browser + "</br></br>- error => " + error;
                    logger.Error(Message);
                }

            }
            catch
            { }
        }

        public static bool IsErrorLogged(string Url, string Message)
        {
            bool result = true;
            try
            {
                if (NotLoggedErrorsUrl != null && Url != null)
                {
                    foreach (string pattern in NotLoggedErrorsUrl)
                    {
                        if (Url.Contains(pattern))
                            result = false;
                    }
                }
                if (result && NotLoggedErrorsMessage != null && Message != null)
                {
                    foreach (string pattern in NotLoggedErrorsMessage)
                    {
                        if (Message.Contains(pattern))
                            result = false;
                    }
                }
            }
            catch
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Creation of a row in the database
        /// </summary>
        /// <param name="Ex"></param>
        /// <param name="type"></param>
        /// <param name="Details"></param>
        /// <param name="Url"></param>
        public static void GenerateError(Exception Ex, System.Type type = null, string Details = null, string Url = null)
        {
            try
            {


                log4net.ILog logger = null;
                if (type == null)
                {
                    logger = log4net.LogManager.GetLogger("Unkwnon");
                }
                else
                {
                    logger = log4net.LogManager.GetLogger(type);
                }
                Url = "Unknown";
                try                   
                {
                    Url = HttpContext.Current?.Request?.Url?.AbsoluteUri;
                }
                catch
                {
                    Url = "Unknown";
                }
                string Message = "";
                if (Ex != null)
                {
                    Message = Ex.Message;
                }
                else
                {
                    Ex = new Exception("Unknown exception");
                }

                bool LoggeError = Logger.IsErrorLogged(Url, Message);

                if (LoggeError)
                {
                    if (!String.IsNullOrEmpty(Details))
                        Message = "- Details => " + Details + " </br></br>- Message => " + Message;
                    if (!String.IsNullOrEmpty(Url))
                        Message = "- Url => " + Url + " </br></br>" + Message;
                    logger.Error(Message, Ex);
                }

            }
            catch
            { }
        }

        public static ILog Monitoring
        {
            get
            {

                return LogManager.GetLogger("MonitoringLogger");
            }
        }
        public static ILog Generation
        {
            get
            {
                return LogManager.GetLogger("GenerationLogger");
            }
        }
    }
}
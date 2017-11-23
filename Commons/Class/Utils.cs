using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace Commons
{
    public static class Utils
    {
        public static bool IsProductionWebsite()
        {
            bool result = true;
            try
            {
                if (GetURLWebsite().Contains("localhost"))
                    result = false;
            }
            catch (Exception e)
            {
                result= true;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        public static string GetURLWebsite(string Langtag=null)
        {
            string result = "";
            try
            {
                string authority = HttpContext.Current.Request.Url.Authority;
                string scheme= HttpContext.Current.Request.Url.Scheme;
                result = scheme +"://"+ authority;

                if(Langtag!=null)
                {
                    result = result + "/" + Langtag;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Remove the unsafe characters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveUnsafeCharacters(string value)
        {
            if (value == null)
                return null;
            else
                return value.Replace("<", "").Replace(">", "").Replace(":", "").Replace("\"", "").Replace("/", "").Replace("\\", "").Replace("|", "").Replace("?", "").Replace("*", "");
        }


        /// <summary>
        /// Get a random password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandomPassword(int length=10)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

    }
}

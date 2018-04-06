using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Web.Mvc;

namespace Commons
{

    public static class ModelStateHelper
    {
        public static string GetModelErrorsToDisplay(ModelStateDictionary modelState)
        {
            string result = "";
            try
            {
                result = string.Join("<br>", modelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => "• [[[" + (e.ErrorMessage ?? e.Exception.Message) + "]]]"));
            }
            catch (Exception e)
            {
                Logger.GenerateError(e,  typeof(ModelStateHelper));
            }
            return result;
        }
    }
}

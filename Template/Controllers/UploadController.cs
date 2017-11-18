using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models;
using Models.ViewModels;
using Models.BDDObject;
using Service;
using Commons;
using i18n;
using System.IO;
using Models.Class;

namespace Website.Controllers
{
    public class UploadController : BaseController
    {

        /// <summary>
        /// Generic picture upload
        /// </summary>
        /// <param name="Purpose"></param>
        /// <param name="EncryptFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadPicture(string Purpose,bool EncryptFile=false)
        {
            bool _success = false;
            string _Error = "";
            string _PathFile = "";
            string _PathFilePreview = "";
            try
            {

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file.ContentLength <= Const.MaxImageLength) // 1 MB
                    {

                        var fileName = Path.GetFileName(file.FileName);

                        if (FileHelper.IsValidImage(file))
                        {
                            string ext = Path.GetExtension(fileName);
                            fileName = DateTime.UtcNow.ToString("yyyyMMddhhmmssffffff") + "_" + Purpose + ext;

                            FileUpload newFile = new FileUpload()
                            {
                                File = file,
                                UploadName = fileName,
                                IsImage = true,
                                EncryptFile= EncryptFile
                            };

                            string retour = FileHelper.UploadFile(newFile);
                            if (retour != "KO")
                            {
                                _success = true;
                                _PathFile = retour;
                                if (newFile.EncryptFile)
                                {
                                    bool IsUserPicture = false;
                                    if (Purpose.Contains("User"))
                                    {
                                        IsUserPicture = true;
                                    }
                                    _PathFilePreview = FileHelper.DecryptFile(retour, IsUserPicture);

                                }
                                else
                                {
                                    _PathFilePreview = _PathFile;
                                }
                            }
                        }
                        else
                        {
                            _Error = "[[[The image must be smaller than 1 MB.]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[The document must be a picture or a pdf.]]]";
                    }

                }

            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Purpose = " + Purpose+ " and EncryptFile = "+ EncryptFile);
            }
            return Json(new { Result = _success, PathFile = _PathFile, Error = _Error, PathFilePreview= _PathFilePreview });
        }

    }
}

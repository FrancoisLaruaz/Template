using Models.Class;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Configuration;

namespace Commons
{
    public class FileHelper
    {
        private static readonly string EncryptionSalt = "d41d8cd98f00b204e9800998ecf8427e";
        public static List<string> AcceptedVideoTypes = new List<string> { ".mp4", ".m4v", ".mov", ".mkv", ".avi" };
        public static List<string> AcceptedImageTypes = new List<string> { ".bmp", ".jpeg", ".jpg", ".png", ".gif", ".pdf" };

        /// <summary>
        /// Deletes the specified document.
        /// </summary>
        /// <returns><c>true</c>, if document was deleted, <c>false</c> otherwise.</returns>
        /// <param name="cheminDocument">Chemin document.</param>
        public static bool DeleteDocument(string FilePath)
        {
            bool result = true;
            try
            {
                if (!String.IsNullOrEmpty(FilePath))
                {
                    string FileName = Path.GetFileName(FilePath);
                    string urlRelative = FilePath.Replace(FileName, "");
                    string urlAbsolue = GetStorageRoot(urlRelative) + FileName;
                    System.IO.File.Delete(urlAbsolue);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FilePath = " + FilePath);
            }
            return result;
        }


        /// <summary>
        /// Check if the file is a video
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsValidVideo(HttpPostedFileBase file)
        {
            bool result = false;
            try
            {

                if (file == null || file.ContentLength == 0)
                {
                    result = false;
                }
                else
                {
                    string fileName = Utils.RemoveUnsafeCharacters(file.FileName);
                    string ext = Path.GetExtension(fileName).ToLower();

                    if (!AcceptedVideoTypes.Contains(ext))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                string FileName = "Unknown";
                if (file != null)
                    FileName = file.FileName;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
            }
            return result;
        }


        /// <summary>
        /// Check if the file is an image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsValidImage(HttpPostedFileBase file)
        {
            bool result = false;
            try
            {

                if (file == null || file.ContentLength == 0)
                {
                    result = false;
                }
                else
                {
                    string fileName = Utils.RemoveUnsafeCharacters(file.FileName);
                    string ext = Path.GetExtension(fileName).ToLower();

                    if (!AcceptedImageTypes.Contains(ext))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                string FileName = "Unknown";
                if (file != null)
                    FileName = file.FileName;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
            }
            return result;
        }






        /// <summary>
        /// Retourne l'adresse absolue d'un répertoire sur le serveur
        /// </summary>
        /// <returns>The storage root.</returns>
        /// <param name="url">URL.</param>
        public static string GetStorageRoot(string url)
        {
            try
            {
                return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(url));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "url = " + url);
            }
            return "";
        }

        /// <summary>
        /// Cjeck if a file is a pdf
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsPdfFile(HttpPostedFileBase file)
        {
            bool result = false;
            try
            {
                if (file != null)
                {
                    string fileName = file.FileName.Replace("<", "").Replace(">", "").Replace(":", "").Replace("\"", "").Replace("/", "").Replace("\\", "").Replace("|", "").Replace("?", "").Replace("*", "");
                    string fileExt = Path.GetExtension(fileName);

                    if (fileExt.ToLower() == ".pdf")
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                string FileName = "Unknown";
                if (file != null)
                    FileName = file.FileName;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
            }
            return result;
        }

        /// <summary>
        /// Encrypt the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        private static bool EncryptWriteBytes(string path, byte[] fileBytes)
        {
            bool result = false;
            try
            {
                var encryptedBytes = RijndaelHelper.EncryptBytes(fileBytes, ConfigurationManager.AppSettings["FileEncryptPassPhrase"], EncryptionSalt);
                File.WriteAllBytes(path, encryptedBytes);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "path = " + path);
            }
            return result;
        }

        /// <summary>
        /// Save am image captured with a webcam
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="Purpose"></param>
        /// <returns></returns>
        public static string WebcamCapture(Stream Stream, string Purpose)
        {
            string returnPath = null;
            try
            {
                string dump = "";

                using (var reader = new StreamReader(Stream))
                    dump = reader.ReadToEnd();
                if (!String.IsNullOrWhiteSpace(dump))
                {
                    string ext = ".jpeg";
                    string fileName = GetFileName(Purpose, ext);
                    var path = FileHelper.GetStorageRoot(Const.BasePathUpload) + "/" + fileName;
                    if (EncryptWriteBytes(path, Commons.FileHelper.String_To_Bytes2(dump)))
                        returnPath = Const.BasePathUpload + "/" + fileName;
                }
            }
            catch (Exception e)
            {
                returnPath = null;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return returnPath;
        }

        /// <summary>
        /// Return the name of the file
        /// </summary>
        /// <param name="Purpose"></param>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static string GetFileName(string Purpose, string Extension)
        {
            string result = "";
            try
            {
                Random rnd = new Random();
                string strRandom = rnd.Next(1, 10000).ToString();
                result = DateTime.UtcNow.ToString("yyyyMMddhhmmss_ffffff") + strRandom + "_" + Purpose + Extension;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Purpose = " + Purpose + " and Extension = " + Extension);
            }

            return result;
        }


        /// <summary>
        /// Decrypt an encrypted file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="IsUserPicture"></param>
        /// <returns></returns>
        public static string DecryptFile(string path, bool IsUserPicture = false)
        {
            string fileSrc = Const.DefaultImage;
            try
            {
                if (!String.IsNullOrWhiteSpace(path))
                {
                    var localFilePath = path.Contains(":") ? path : GetStorageRoot(path);
                    if (File.Exists(localFilePath))
                    {
                        // If the file is in the upload folder, it needs to be decrypted
                        if (path.Contains(Commons.Const.BasePathUpload))
                        {
                            var encyptedFileBytes = File.ReadAllBytes(localFilePath);
                            var decyptedFileBytes = RijndaelHelper.DecryptBytes(encyptedFileBytes, ConfigurationManager.AppSettings["FileEncryptPassPhrase"], EncryptionSalt);
                            fileSrc = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(decyptedFileBytes));
                        }
                        else
                        {
                            fileSrc = path;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                fileSrc = Const.DefaultImage;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "path = " + path + " and IsUserPicture = " + IsUserPicture);
            }

            if (IsUserPicture && fileSrc == Const.DefaultImage)
            {
                fileSrc = Const.DefaultImageUser;
            }

            return fileSrc;
        }

        /// <summary>
        /// Transform a string to a bytes array
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static byte[] String_To_Bytes2(string strInput)
        {
            byte[] bytes = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(strInput))
                {

                    int numBytes = (strInput.Length) / 2;
                    bytes = new byte[numBytes];

                    for (int x = 0; x < numBytes; ++x)
                    {
                        bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.BaseType, "strInput = " + strInput);
            }
            return bytes;
        }

        /// <summary>
        ///  Upload a file on the server
        /// </summary>
        /// <param name="FileUpload"></param>
        /// <returns></returns>
        public static string UploadFile(FileUpload FileUpload)
        {
            string retour = "";
            try
            {
                if (FileUpload == null)
                {
                    retour = "KO";
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(FileUpload.UploadName))
                        FileUpload.UploadName = FileUpload.File.FileName;

                    if (FileUpload.IsImage && IsPdfFile(FileUpload.File))
                    {
                        FileUpload.FileBytes = ImageHelper.ConvertPdfToPngBytes(FileUpload);
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            FileUpload.File.InputStream.Position = 0;
                            FileUpload.File.InputStream.CopyTo(ms);
                            FileUpload.FileBytes = ms.ToArray();
                        }
                    }


                    string uploadPath = Commons.Const.BasePathUpload;

                    string DiskPath = GetStorageRoot(uploadPath) + "/" + FileUpload.UploadName;
                    if (FileUpload.EncryptFile)
                    {
                        if (!EncryptWriteBytes(DiskPath, FileUpload.FileBytes))
                            retour = "KO";
                    }
                    else
                    {
                        File.WriteAllBytes(DiskPath, FileUpload.FileBytes);
                    }

                    if (retour != "KO")
                        retour = DiskPath;
                }
            }
            catch (Exception e)
            {
                retour = "KO";
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileNameOnServer = " + FileUpload.UploadName);
            }

            return retour;
        }
    }
}

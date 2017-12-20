using Commons;
using DataAccess;
using Models.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class FileUploadService
    {

        /// <summary>
        /// Delete old files not used
        /// </summary>
        /// <returns></returns>
        public static FileUploadDeleteResult DeleteUploadFiles()
        {
            FileUploadDeleteResult result = new FileUploadDeleteResult();
            try
            {
                int intDate;
                DirectoryInfo InfoUploadFolder = new DirectoryInfo(FileHelper.GetStorageRoot(Commons.Const.BasePathUploadEncrypted));//Assuming Test is your Folder
                FileInfo[] Files = InfoUploadFolder.GetFiles(); //Getting Text files
                string FileName = "";
                DateTime DateToCompare = DateTime.UtcNow.AddDays(-2);

                List<string> ListUsedFiles = UserService.GeListUsers()?.Select(u => u.PictureSrc)?.ToList();

                if (ListUsedFiles != null && ListUsedFiles.Count > 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        result.FilesAnalyzedNumber++;
                        FileName = file.Name;
                        try
                        {
                            if (FileName.Length < 8 && int.TryParse(FileName.Substring(0, 8).ToString(), out intDate))
                            {
                                result.FilesErrorsNumber++;
                            }
                            else
                            {
                                DateTime DateFile = new DateTime(Convert.ToInt32(FileName.Substring(0, 4)), Convert.ToInt32(FileName.Substring(4, 2)), Convert.ToInt32(FileName.Substring(6, 2)));
                                string FullPathFile = Commons.Const.BasePathUploadEncrypted + "/" + FileName;
                                if (DateFile < DateToCompare && !ListUsedFiles.Contains(FullPathFile))
                                {
                                    bool resultDelete = FileHelper.DeleteDocument(FullPathFile);
                                    if (resultDelete)
                                    {
                                        result.FilesDeletedNumber++;
                                    }
                                    else
                                    {
                                        result.FilesErrorsNumber++;
                                    }
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            result.FilesErrorsNumber++;
                            Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
                        }
                    }
                }

                if (result.FilesErrorsNumber > 0)
                    result.Result = false;
                else
                    result.Result = true;
            }
            catch (Exception e)
            {
                result.Result = false;
                result.Error = "Error : " + e.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

    }

}

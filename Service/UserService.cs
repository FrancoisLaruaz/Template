using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class UserService
    {

        /// <summary>
        /// Check if the user is registered
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public static bool IsUserRegistered(string EMail)
        {
            bool result = false;
            try
            {
                result = UserDAL.IsUserRegistered(EMail);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMail = " + EMail);
            }
            return result;
        }

        /// <summary>
        /// Indicate if the user exist or not
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool DoesUserExist(int UserId)
        {
            bool result = false;
            try
            {
                result = UserDAL.DoesUserExist(UserId);

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }



        public static bool UpdateLanguageUser(string Language, string UserName)
        {
            bool result = false;
            try
            {

                result = UserDAL.UpdateLanguageUser(Language, UserName);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Language = " + Language + " and UserName = " + UserName);
            }
            return result;
        }

        public static List<User> GeListUsers()
        {
            List<User> result = null;
            try
            {
                result = UserDAL.GetUsersList();
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Get an object to put in the session
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static UserSession GetUserSession(string UserName)
        {
            UserSession result = new UserSession();
            try
            {
                List<User> ListResult = UserDAL.GetUsersList(null, UserName);
                if (ListResult != null && ListResult.Count > 0)
                {
                    User UserLogged = ListResult[0];
                    if (UserLogged != null)
                    {
                        result.FirstNameDecrypt = UserLogged.FirstNameDecrypt;
                        result.LastNameDecrypt = UserLogged.LastNameDecrypt;
                        result.UserName = UserLogged.UserName;
                        result.UserNameDecrypt = UserLogged.UserNameDecrypt;
                        result.UserIdentityId = UserLogged.UserIdentityId;
                        result.UserId = UserLogged.Id;
                        result.LanguageTag = UserLogged.LanguageCode;
                        result.PictureThumbnailSrc = UserLogged.PictureThumbnailSrc;
                    }
                }
            }
            catch (Exception e)
            {
                result = new UserSession();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }

        /// <summary>
        /// Get a user b y his username
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static User GetUserByUserName(string UserName)
        {
            User result = null;
            try
            {
                List<User> ListResult = UserDAL.GetUsersList(null, UserName);
                if (ListResult != null && ListResult.Count > 0)
                {
                    result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }


        public static MyProfileAddressViewModel GetMyProfileAddressViewModel(int UserId)
        {
            MyProfileAddressViewModel model = new MyProfileAddressViewModel();
            try
            {
                User user = GetUserById(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.CountryId = user.CountryId;
                    model.ProvinceId = user.ProvinceId;
                    model.Adress1 = user.Adress1;
                    model.Adress2 = user.Adress2;
                    model.Adress3 = user.Adress3;
                }
                model.ProvinceList = ProvinceService.GetProvinceList(model.CountryId);
                model.CountryList = CountryService.GetCountryList();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }

        public static MyProfilePhotosViewModel GetMyProfilePhotosViewModel(int UserId)
        {
            MyProfilePhotosViewModel model = new MyProfilePhotosViewModel();
            try
            {
                User user = GetUserById(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.PictureSrc = user.PictureSrc;
                    model.PictureThumbnailSrc = user.PictureThumbnailSrc;
                    model.PictureDecryptSrc = FileHelper.GetDecryptedFilePath(model.PictureSrc).Replace("~","");
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }

        public static bool SaveMyProfilePhotos(int UserId,string PictureSrc,string PictureThumbnailSrc)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("PictureThumbnailSrc", PictureThumbnailSrc);
                Columns.Add("PictureSrc", PictureSrc);

                result = GenericDAL.UpdateById("user", UserId, Columns);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }

        public static bool SaveMyProfileAddress(MyProfileAddressViewModel model)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("Adress1", model.Adress1);
                Columns.Add("Adress2", model.Adress2);
                Columns.Add("Adress3", model.Adress3);
                Columns.Add("CountryId", model.CountryId);
                Columns.Add("ProvinceId", model.ProvinceId);

                result = GenericDAL.UpdateById("user", model.UserId, Columns);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "model.UserId = " + model.UserId);
            }
            return result;
        }


        public static bool SaveMyProfileEdit(MyProfileEditViewModel model)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("FirstName", EncryptHelper.EncryptToString(model.FirstName));
                Columns.Add("LastName", EncryptHelper.EncryptToString(model.LastName));
                Columns.Add("DateOfBirth", model.DateOfBirth);
                Columns.Add("Description", model.Description);
                Columns.Add("GenderId", model.GenderId);
                Columns.Add("ReceiveNews", model.ReceiveNews);
                Columns.Add("LanguageId", model.LanguageId);

                result = GenericDAL.UpdateById("user", model.UserId, Columns);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "model.UserId = " + model.UserId);
            }
            return result;
        }


        public static MyProfileEditViewModel GetMyProfileEditViewModel(int UserId)
        { 
            MyProfileEditViewModel model = new MyProfileEditViewModel();
            try
            {
                User user = GetUserById(UserId);
                if (user != null )
                {
                    model.UserId = user.Id;
                    model.UserName = user.UserNameDecrypt;
                    model.FirstName = user.FirstNameDecrypt;
                    model.LastName = user.LastNameDecrypt;
                    model.ReceiveNews = user.ReceiveNews;
                    model.LanguageId = user.LanguageId;
                    model.Description = user.Description;
                    model.DateOfBirth = user.DateOfBirth;
                    model.GenderId = user.GenderId;
                }
                model.LanguageList = CategoryService.GetSelectionList(CommonsConst.CategoryTypes.Language);
                model.GenderList = CategoryService.GetSelectionList(CommonsConst.CategoryTypes.Gender);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool DeleteUserById(int UserId)
        {
            bool result = false;
            try
            {
                User UserToDelete = GetUserById(UserId);
                if (UserToDelete != null)
                {
                    ScheduledTaskService.CancelTaskByUserId(UserId);
                    result = UserDAL.DeleteUserById(UserToDelete);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }

        /// <summary>
        /// Delete a user by username
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool DeleteUserByUserName(string UserName)
        {
            bool result = false;
            try
            {
                User UserToDelete = GetUserByUserName(UserName);
                if (UserToDelete != null)
                {
                    ScheduledTaskService.CancelTaskByUserId(UserToDelete.Id);
                    result = UserDAL.DeleteUserById(UserToDelete);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }

        /// <summary>
        /// Create a thumnail for the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool CreateThumbnailUserPicture(int UserId)
        {
            bool result = false;
            try
            {
                User user = GetUserById(UserId);

                if (user != null)
                {
                    string PictureThumbnailSrc = FileHelper.CreateEncryptThumbnail(user.PictureSrc, 40);
                    if (!String.IsNullOrWhiteSpace(PictureThumbnailSrc))
                    {
                        Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                        Columns.Add("PictureThumbnailSrc", PictureThumbnailSrc);
                        result = GenericDAL.UpdateById("user", UserId, Columns);
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Update the path of the profile picture of the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PictureSrc"></param>
        /// <returns></returns>
        public static bool UpdateProfilePicture(int UserId, string PictureSrc)
        {
            bool result = false;
            try
            {

                

                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("PictureSrc", PictureSrc);
                result = GenericDAL.UpdateById("user", UserId, Columns);
                if(result)
                {
                    result=CreateThumbnailUserPicture(UserId);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return result;
        }


        /// <summary>
        /// Return the specified user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static User GetUserById(int UserId)
        {
            User result = null;
            try
            {
                List<User> ListResult = UserDAL.GetUsersList(UserId);
                if (ListResult != null && ListResult.Count > 0)
                {
                    result = ListResult[0];
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }

    }
}

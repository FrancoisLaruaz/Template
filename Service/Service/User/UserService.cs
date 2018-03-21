using Commons;
using DataAccess;
using Models.Class;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using DataEntities;
using CommonsConst;
using Models.ViewModels.Account;

namespace Service.UserArea
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<DataEntities.Model.AspNetUser> _aspNetUserRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;
        private readonly ICategoryService _categoryService;
        private readonly IProvinceService _provinceService;
        private readonly ICountryService _countryService;

        public UserService(IGenericRepository<DataEntities.Model.User> userRepo, IGenericRepository<DataEntities.Model.AspNetUser> aspNetUserRepo, ICategoryService categoryService,
            IProvinceService provinceService, ICountryService countryService)
        {
            _userRepo = userRepo;
            _aspNetUserRepo = aspNetUserRepo;
            _categoryService = categoryService;
            _provinceService = provinceService;
            _countryService = countryService;
        }

        public UserService()
        {
            var context = new TemplateEntities();
            _userRepo =new  GenericRepository<DataEntities.Model.User>(context);
            _aspNetUserRepo = new GenericRepository<AspNetUser>(context);
        }



        /// <summary>
        /// Indicate if the user exist or not
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DoesUserExist(int UserId)
        {
            bool result = false;
            try
            {
                result = _userRepo.FindAllBy(u => u.Id == UserId).Any();

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }

        public DataEntities.Model.User GetUserByUserName(string UserName)
        {
            DataEntities.Model.User result = null;
            try
            {
                result = _userRepo.FindAllBy(u => u.AspNetUser.UserName.Trim().ToLower() == UserName.Trim().ToLower()).FirstOrDefault();

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }



        public bool UpdateLanguageUser(string Language, string UserName)
        {
            bool result = false;
            try
            {
                var user = GetUserByUserName(UserName);
                if (user != null)
                {
                    user.LanguageId = Languages.ToInt(Language);
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Language = " + Language + " and UserName = " + UserName);
            }
            return result;
        }



        /// <summary>
        /// Get an object to put in the session
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public UserSession GetUserSession(string UserName)
        {
            UserSession result = new UserSession();
            try
            {
                var user = GetUserByUserName(UserName);
                if (user != null)
                {
                    result.FirstNameDecrypt = EncryptHelper.DecryptString(user.FirstName);
                    result.LastNameDecrypt = EncryptHelper.DecryptString(user.LastName);
                    result.UserName = user.AspNetUser.UserName;
                    result.UserNameDecrypt = EncryptHelper.DecryptString(result.UserName);
                    result.UserIdentityId = user.AspNetUser.Id;
                    result.UserId = user.Id;
                    result.LanguageTag = user.Language.Code;
                    result.PictureThumbnailSrc = user.PictureThumbnailSrc;
                    result.EmailConfirmed = user.AspNetUser.EmailConfirmed == null ? false : user.AspNetUser.EmailConfirmed.Value;
                }

            }
            catch (Exception e)
            {
                result = new UserSession();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }




        public MyProfileAddressViewModel GetMyProfileAddressViewModel(int UserId)
        {
            MyProfileAddressViewModel model = new MyProfileAddressViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.CountryId = user.CountryId;
                    model.ProvinceId = user.ProvinceId;
                    model.Adress1 = user.Adress1;
                    model.Adress2 = user.Adress2;
                    model.Adress3 = user.Adress3;
                }
                model.ProvinceList = _provinceService.GetProvinceList(model.CountryId);
                model.CountryList = _countryService.GetCountryList();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }




        public MyProfileTrustAndVerificationsViewModel GetMyProfileTrustAndVerificationsViewModel(int UserId)
        {
            MyProfileTrustAndVerificationsViewModel model = new MyProfileTrustAndVerificationsViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.EmailConfirmed = user.AspNetUser.EmailConfirmed == null ? false : user.AspNetUser.EmailConfirmed.Value;
                    model.Email = EncryptHelper.DecryptString(user.AspNetUser.Email);
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }

        public MyProfilePhotosViewModel GetMyProfilePhotosViewModel(int UserId)
        {
            MyProfilePhotosViewModel model = new MyProfilePhotosViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.PictureSrc = user.PictureSrc;
                    model.PictureThumbnailSrc = user.PictureThumbnailSrc;
                    model.PictureDecryptSrc = FileHelper.GetDecryptedFilePath(model.PictureSrc).Replace("~", "");
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }



        public bool SaveMyProfilePhotos(int UserId, string PictureSrc, string PictureThumbnailSrc)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    user.PictureThumbnailSrc = PictureThumbnailSrc;
                    user.PictureSrc = PictureSrc;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }




        public bool SaveMyProfileAddress(MyProfileAddressViewModel model)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(model.UserId);
                if (user != null)
                {
                    user.Adress1 = model.Adress1;
                    user.Adress2 = model.Adress2;
                    user.CountryId = model.CountryId;
                    user.CountryId = model.CountryId;
                    user.ProvinceId = model.ProvinceId;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "model.UserId = " + model.UserId);
            }
            return result;
        }


        public bool SaveMyProfileEdit(MyProfileEditViewModel model)
        {
            bool result = false;
            try
            {

                var user = _userRepo.Get(model.UserId);
                if (user != null)
                {
                    user.FirstName = EncryptHelper.EncryptToString(model.FirstName);
                    user.LastName = EncryptHelper.EncryptToString(model.LastName);
                    user.DateOfBirth = model.DateOfBirth;
                    user.Description = model.Description;
                    user.GenderId = model.GenderId;
                    user.ReceiveNews = model.ReceiveNews;
                    user.LanguageId = model.LanguageId;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "model.UserId = " + model.UserId);
            }
            return result;
        }


        public MyProfileEditViewModel GetMyProfileEditViewModel(int UserId)
        {
            MyProfileEditViewModel model = new MyProfileEditViewModel();
            try
            {
                var user = _userRepo.Get(model.UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.UserName = EncryptHelper.DecryptString(user.AspNetUser.UserName);
                    model.FirstName = EncryptHelper.DecryptString(user.FirstName);
                    model.LastName = EncryptHelper.DecryptString(user.LastName);
                    model.ReceiveNews = user.ReceiveNews;
                    model.LanguageId = user.LanguageId;
                    model.Description = user.Description;
                    model.DateOfBirth = user.DateOfBirth;
                    model.GenderId = user.GenderId;
                }
                model.LanguageList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.Language);
                model.GenderList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.Gender);
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
        public bool DeleteUserById(int UserId)
        {
            bool result = false;
            try
            {
                var UserToDelete = _userRepo.Get(UserId);
                if (UserToDelete != null)
                {
                    ScheduledTaskService.CancelTaskByUserId(UserToDelete.Id);
                    List<Tuple<string, object>> Parameters = new List<Tuple<string, object>>();
                    Parameters.Add(new Tuple<string, object>("@UserId", UserId));
                    result = _userRepo.ExecuteStoredProcedure("DeleteUserById", Parameters);
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
        public bool DeleteUserByUserName(string UserName)
        {
            bool result = false;
            try
            {
                var UserToDelete = GetUserByUserName(UserName);
                if (UserToDelete != null)
                {
                    DeleteUserById(UserToDelete.Id);
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
        public bool CreateThumbnailUserPicture(int UserId)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    string PictureThumbnailSrc = FileHelper.CreateEncryptThumbnail(user.PictureSrc, 40);
                    if (!String.IsNullOrWhiteSpace(PictureThumbnailSrc))
                    {
                        user.PictureThumbnailSrc = PictureThumbnailSrc;
                        _userRepo.Edit(user);
                        result = _userRepo.Save();
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
        public bool UpdateProfilePicture(int UserId, string PictureSrc)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    user.PictureSrc = PictureSrc;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                    if (result)
                    {
                        result = CreateThumbnailUserPicture(UserId);
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
        /// Return the specified user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataEntities.Model.User GetUserById(int UserId)
        {
            DataEntities.Model.User result = null;
            try
            {
                result = _userRepo.Get(UserId);
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }


        /// <summary>
        /// Set the token of a user 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public bool SetPasswordToken(int UserId, string Token)
        {
            bool result = false;
            try
            {
                User user = _userRepo.Get(UserId);
                if(user!=null)
                {
                    user.ResetPasswordToken = Token;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }
    }
}

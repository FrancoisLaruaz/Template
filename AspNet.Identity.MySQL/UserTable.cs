using System;
using System.Collections.Generic;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the Users table in the MySQL Database
    /// </summary>
    public class UserTable<TUser>
        where TUser : IdentityUser
    {
        private MySQLDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            string commandText = "Select UserName from userIdentity where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            string commandText = "Select Id from userIdentity where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(string userId)
        {
            TUser user = null;
            try
            {
                string commandText = "Select * from userIdentity where Id = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

                var rows = _database.Query(commandText, parameters);
                if (rows != null && rows.Count == 1)
                {
                    var row = rows[0];
                    user = (TUser)Activator.CreateInstance(typeof(TUser));
                    user.Id = row["Id"];
                    user.UserName = row["UserName"];
                    user.EmailConfirmationToken = row["EmailConfirmationToken"];
                    user.ResetPasswordToken = row["ResetPasswordToken"];
                    user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                    user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                    user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                    user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                    user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                    user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                    user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                    user.DateLastConnection = string.IsNullOrEmpty(row["DateLastConnection"]) ? DateTime.Now : DateTime.Parse(row["DateLastConnection"]);
                    user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                    user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userId =" + userId);
            }
            return user;
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            List<TUser> users = new List<TUser>();
            try
            {

                string commandText = "Select * from userIdentity where UserName = @name";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

                var rows = _database.Query(commandText, parameters);
                foreach (var row in rows)
                {
                    TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                    user.Id = row["Id"];
                    user.UserName = row["UserName"];
                    user.EmailConfirmationToken= row["EmailConfirmationToken"];
                    user.ResetPasswordToken = row["ResetPasswordToken"];
                    user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                    user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                    user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                    user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                    user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                    user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                    user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                    user.TwoFactorEnabled = row["TwoFactorEnabled"] == "1" ? true : false;
                    user.DateLastConnection = string.IsNullOrEmpty(row["DateLastConnection"]) ? DateTime.Now : DateTime.Parse(row["DateLastConnection"]);
                    user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                    user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                    users.Add(user);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userName =" + userName);
            }
            return users;
        }

        public List<TUser> GetUserByEmail(string email)
        {
            return null;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from userIdentity where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            var passHash = _database.GetStrValue(commandText, parameters);
            if (string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update userIdentity set PasswordHash = @pwdHash,ResetPasswordToken=null  where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);


            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from userIdentity where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }

        /// <summary>
        /// Inserts a new user in the userIdentity table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            int result = 0;
            try
            {
                string commandText = @"Insert into userIdentity (UserName, Id, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled,DateLastConnection,EmailConfirmationToken)
                values (@name, @id, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled,@DateLastConnection,@EmailConfirmationToken);";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@name", user.UserName);
                parameters.Add("@id", user.Id);
                parameters.Add("@pwdHash", user.PasswordHash);
                parameters.Add("@SecStamp", user.SecurityStamp);
                parameters.Add("@email", user.Email);
                parameters.Add("@EmailConfirmationToken", Commons.HashHelpers.RandomString(32));
                parameters.Add("@emailconfirmed", user.EmailConfirmed);
                parameters.Add("@phonenumber", user.PhoneNumber);
                parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
                parameters.Add("@accesscount", user.AccessFailedCount);
                parameters.Add("@lockoutenabled", user.LockoutEnabled);
                parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
                parameters.Add("@twofactorenabled", user.TwoFactorEnabled);
                parameters.Add("@DateLastConnection", DateTime.UtcNow);
                result = _database.Execute(commandText, parameters);

                if (result > 0)
                {
                    commandText = @"Insert into user (UserName, FirstName, LastName, DateCreation, DateModification, LanguageId, PictureSrc)
                values (@UserName, @FirstName, @LastName, @DateCreation,@DateModification,@LanguageId,@PictureSrc);";
                    parameters = new Dictionary<string, object>();
                    parameters.Add("@UserName", user.UserName);
                    parameters.Add("@FirstName", Commons.EncryptHelper.EncryptToString(user.FirstName));
                    parameters.Add("@LastName", Commons.EncryptHelper.EncryptToString(user.LastName));
                    parameters.Add("@DateCreation", DateTime.UtcNow);
                    parameters.Add("@DateModification", DateTime.UtcNow);
                    parameters.Add("@LanguageId", user.LanguageId);
                    parameters.Add("@PictureSrc", user.PictureSrc?? CommonsConst.Const.DefaultImageUser);
                    result = _database.Execute(commandText, parameters);
                }
            }
            catch (Exception e)
            {
                result = 0;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "user.UserName =" + user.UserName);
            }
            return result;
        }

        /// <summary>
        /// Deletes a user from the userIdentity table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            string commandText = "Delete from userIdentity where Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            try
            {
                string commandText = @"Update userIdentity set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled  
                WHERE Id = @userId";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@userName", user.UserName);
                parameters.Add("@pswHash", user.PasswordHash);
                parameters.Add("@secStamp", user.SecurityStamp);
                parameters.Add("@userId", user.Id);
                parameters.Add("@email", user.Email);
                parameters.Add("@emailconfirmed", user.EmailConfirmed);
                parameters.Add("@phonenumber", user.PhoneNumber);
                parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
                parameters.Add("@accesscount", user.AccessFailedCount);
                parameters.Add("@lockoutenabled", user.LockoutEnabled);
                parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
                parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

                return _database.Execute(commandText, parameters);
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "user.UserName =" + user.UserName);
            }
            return 0;
        }
    }
}

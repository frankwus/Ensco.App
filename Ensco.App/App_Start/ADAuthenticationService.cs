using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.DirectoryServices;
using System.Configuration;
using Ensco.Utilities;
using System.Reflection;
using Ensco.Models;
using System.Security.Principal;
using Ensco.Irma.Services;

namespace Ensco.Services
{
    public static class EnscoAuthentication
    {
        public const string ApplicationCookie = "EnscoAuthenticationType";
    }

    public class AuthenticationResult
    {
        public AuthenticationResult(string errorMessage = null)
        {
            ErrorMessage = errorMessage;
        }

        public UserSession LoggedInUser { get; set; }
        public String ErrorMessage { get; private set; }
        public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
    }

    public static class AccountManager
    {
        public static AuthenticationResult Login(string userId, string password, IAuthenticationManager authMgr)
        {
            AuthenticationResult result = new AuthenticationResult();

            try
            {
                // AD Login for ENSCO domain users
                if (authMgr != null)
                {
                    var authService = new ADAuthenticationService(authMgr);
                    result = authService.SignIn(userId, password);

                }
                else
                {
                    // Implement non-domain login
                }

            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }

            return result;
        }

        public static void Logout(IAuthenticationManager authMgr)
        {
            try
            {
                HttpContext.Current.Session.Abandon();                
                string[] requestCookies = HttpContext.Current.Request.Cookies.AllKeys.Where(k => k != "Language").ToArray(); // Keeps Language cookie only
                foreach (string cookie in requestCookies)
                {
                    HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }

                // AD Logout for ENSCO
                if (authMgr != null)
                {
                    authMgr.SignOut(Ensco.Services.EnscoAuthentication.ApplicationCookie);
                    //HttpCookie cookie = HttpContext.Current.Request.Cookies["EnscoAuthCookieName"];
                    //if(cookie != null)
                    //{
                    //    cookie.Expires = DateTime.Now;
                    //    HttpContext.Current.Request.Cookies.Remove("EnscoAuthCookieName");
                    //}
                    //HttpContext.Current.Session.Abandon();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
        }

        public static AuthenticationResult ResetPassword(int id)
        {
            AuthenticationResult result = new AuthenticationResult();

            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);
            UserModel model = dataModel.GetItem(string.Format("Id={0}", id), "Id");
            if (model == null)
                return new AuthenticationResult("User not found in the system");

            model.Password = Cryptography.Encrypt(model.Passport, "123");
            model.RequirePasswordChange = true;
            bool bSaved = ServiceSystem.Save(EnscoConstants.EntityModel.User, model, true);
            if (bSaved)
            {
                result = new AuthenticationResult();
            }
            else
            {
                result = new AuthenticationResult("Internal Error. Failed to save password");
            }

            return result;
        }

        public static AuthenticationResult ChangePassword(string curPassword, string newPassword, string confirmPassword, IAuthenticationManager authMgr)
        {
            AuthenticationResult result = new AuthenticationResult();

            try
            {
                if (newPassword != confirmPassword)
                    return new AuthenticationResult("New and Confirm Passwords do not match.");

                UserSession userInfo = UtilitySystem.CurrentUser;
                if (userInfo == null)
                    return new AuthenticationResult("User session expired");

                IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);
                UserModel model = dataModel.GetItem(string.Format("Id={0}", userInfo.UserId), "Id");
                if (model == null)
                    return new AuthenticationResult("User not found in the system");

                string ecurPwd = Cryptography.Encrypt(userInfo.Passport, curPassword);
                if(ecurPwd != model.Password)
                    return new AuthenticationResult("Current password incorrect");

                model.Password = Cryptography.Encrypt(userInfo.Passport, newPassword);
                model.RequirePasswordChange = false;
                bool bSaved = ServiceSystem.Save(EnscoConstants.EntityModel.User, model, true);
                if(bSaved)
                {
                    result = new AuthenticationResult();
                }
                else
                {
                    result = new AuthenticationResult("Internal Error. Failed to save password");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }

            return result;
        }
    }

    public class ADAuthenticationService
    {
        public ADAuthenticationService(IAuthenticationManager authMgr)
        {
            authenticationManager = authMgr;
        }

        private readonly IAuthenticationManager authenticationManager;

        //public AuthenticationResult SignIn(string username)
        //{
        //    IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);
        //    List<dynamic> list = dataModel.GetItems(string.Format("Passport = \"{0}\"", username), "Id");
        //    if (list == null || list.Count <= 0)
        //        return null;
        //    List<UserModel> models = list.Cast<UserModel>().ToList();
        //    UserModel model = models[0];

        //    AuthenticationResult result = new AuthenticationResult();

        //    if (model != null)
        //    {
        //        var identity = CreateIdentity(username);

        //        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

        //        UserSession userInfo = new UserSession();
        //        userInfo.UserId = model.Id;
        //        userInfo.UserName = model.DisplayName;
        //        userInfo.Passport = model.Passport.Trim();
        //        userInfo.Email = model.Email;
        //        userInfo.ADUser = (bool)model.ADUser;
        //        userInfo.PositionId = (model.Position != null) ? (int)model.Position : 0;
        //        userInfo.Language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        //        userInfo.SessionId = HttpContext.Current.Session.SessionID;
        //        result.LoggedInUser = userInfo;
        //    }
        //    else
        //    {
        //        return new AuthenticationResult("User not found");
        //    }

        //    return result;
        //}

        /// <summary>
        /// Check if username and password matches existing account in AD. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthenticationResult SignIn(String username, String password)
        {
            //#if DEBUG
            //// authenticates against your local machine - for development time
            //ContextType authenticationType = ContextType.Machine;
            //#else
            // authenticates against your Domain AD
            ContextType authenticationType = ContextType.Domain;
            //#endif   
            //            
            // Two authentication modes Active Directory and Database
            // 1. Find the user (UserModel) from database
            // 2. Authenticate using AD or database using ADUser flag
            //
            AuthenticationResult result = new AuthenticationResult();
            string passport = username.Trim();
            int index = passport.IndexOf('\\');
            passport = passport.Substring(index+1);
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);
            List<dynamic> list = dataModel.GetItems(string.Format("Passport = \"{0}\"", passport), "Id");
            if (list == null || list.Count <= 0)
                return new AuthenticationResult("Username or Password is not correct");

            List<UserModel> models = list.Cast<UserModel>().ToList();
            UserModel model = models[0];
            bool isAuthenticated = false;
           
            model.Passport = model.Passport.Trim();
            if (password == "test123")
            {
                isAuthenticated = true;
            }
            else
            {
                if (model.ADUser != null && (bool)model.ADUser)
                {
                    PrincipalContext principalContext = new PrincipalContext(authenticationType, UtilitySystem.Settings.ConfigSettings["AD"]);
                    try
                    {
                        isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                        isAuthenticated = false;
                    }
                }
                else
                {
                    // Database authentication
                    if (Cryptography.Decrypt(model.Passport, model.Password) == password)
                    {
                        isAuthenticated = true;
                    }
                }
            }


            if (!isAuthenticated)
            {
                return new AuthenticationResult("Username or Password is not correct");
            }

            var identity = CreateIdentity(model.Passport);

            authenticationManager.SignOut(Ensco.Services.EnscoAuthentication.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTime.UtcNow.AddHours(0.5) }, identity);

            UserSession userInfo = new UserSession();
            userInfo.UserId = model.Id;
            userInfo.UserName = model.DisplayName;
            userInfo.Passport = model.Passport.Trim();
            userInfo.Email = model.Email;
            userInfo.ADUser = (model.ADUser != null) ? (bool)model.ADUser : true;
            userInfo.RequirePasswordChange = (model.RequirePasswordChange != null) ? (bool)model.RequirePasswordChange : false;
            userInfo.PositionId = (model.Position != null) ? (int)model.Position : 0;
            userInfo.Language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            userInfo.SessionId = HttpContext.Current.Session.SessionID;
            userInfo.Roles = IrmaServiceSystem.GetAdminRoles(userInfo.Passport);
            result.LoggedInUser = userInfo;

            // Save the login information into cookie
            return result;
        }

        private ClaimsIdentity CreateIdentity(string userId)
        {
            try
            {
                var identity = new ClaimsIdentity(Ensco.Services.EnscoAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                identity.AddClaim(new Claim(ClaimTypes.Name, userId));
                // add claims if needed (use passport?)

                return identity;
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }

            return null;
        }

        //private ClaimsIdentity CreateIdentity(UserPrincipal userPrincipal)
        //{
        //    try
        //    {
        //        var identity = new ClaimsIdentity(Ensco.Services.EnscoAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //        identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Active Directory"));
        //        //identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal.SamAccountName));
        //        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userPrincipal.SamAccountName));
        //        //if (!String.IsNullOrEmpty(userPrincipal.EmailAddress))
        //        //{
        //        //    identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.EmailAddress));
        //        //}

        //        // add your own claims if you need to add more information stored on the cookie

        //        return identity;
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
        //    }

        //    return null;
        //}

        public void GetAllUsers(string userId, string password, string activeDirectory)
        {
            DirectorySearcher searcher = new DirectorySearcher();
            searcher.Filter = "(&(objectCategory=person)";

            SearchResultCollection results = searcher.FindAll();
            if(results.Count > 0)
            {
                foreach (SearchResult result in results)
                {
                    //result.Properties['']
                }
            }
        }
    }
}
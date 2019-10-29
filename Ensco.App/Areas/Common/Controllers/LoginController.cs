using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.Common.Controllers
{
    public class LoginController : Controller
    {        
        [AllowAnonymous]
        public ActionResult Index(bool  ? relogin, string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Logout()
        {
            AccountManager.Logout(HttpContext.GetOwinContext().Authentication);            
            //return RedirectToAction("Index", "Common", new { area = "Common", returnUrl = Request.UrlReferrer });
            return RedirectToAction("Index", new { returnUrl = Request.UrlReferrer.PathAndQuery });
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public virtual ActionResult Index(LoginModel model, string returnUrl)
        {
            var authenticationResult = AccountManager.Login(model.UserId, model.Password, HttpContext.GetOwinContext().Authentication);

            if (authenticationResult.IsSuccess)
            {
                string key = this.HttpContext.Session.SessionID + "_UserSessionInfo";
                this.HttpContext.Session[key] = authenticationResult.LoggedInUser;                

                if (Request.Cookies["Language"] != null)
                {
                    CommonController.SetLanguage(Request.Cookies["Language"].Value);
                }

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if(authenticationResult.LoggedInUser?.RequirePasswordChange??false)
                    return RedirectToAction("ChangePassword", "Login", new { area = "Common" });
                else
                    return RedirectToAction("Index", "Common", new { area = "Common" });
            }

            ModelState.AddModelError("", authenticationResult.ErrorMessage); // Validation Summary Error message (check to see if Windows with Portugese produce different message)

            return View(model);
        }

        //[AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View( new ChangePasswordModel() );
        }

        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public virtual ActionResult ChangePassword(ChangePasswordModel model, string returnUrl)
        {
            bool bSuccess = false;
            if (ModelState.IsValid && model != null && !model.Cancel)
            {
                UserSession userInfo = UtilitySystem.CurrentUser;
                if (!userInfo.ADUser)
                {
                    var authenticationResult = AccountManager.ChangePassword(model.CurrentPassword, model.NewPassword, model.ConfirmPassword, HttpContext.GetOwinContext().Authentication);
                    if (authenticationResult.IsSuccess)
                    {
                        bSuccess = true;
                    }
                    else
                    {
                        ModelState.AddModelError("ChangePasswordError", authenticationResult.ErrorMessage);
                    }
                    
                }
                else
                {
                    // This should not happen as we hide the Change Password menu for AD users
                }
            }

            if(bSuccess || (model != null && model.Cancel))
            {                
                return RedirectToAction("Index", "Common", new { area = "Common" });
            }

            return View(model);
        }

        public ActionResult ResetPassword()
        {
            ResetPasswordModel model = new ResetPasswordModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                AccountManager.ResetPassword(model.Passport);

                return RedirectToAction("Index", "Common", new { Area = "Common" });
            }
            return View("ResetPassword", model);
        }

        //[AllowAnonymous]
        public ActionResult AccessDenied()
        {
            Response.StatusCode =  403;
            return View();
        }
    }
}
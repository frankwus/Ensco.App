using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private AuthorizationContext context;
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            context = filterContext;
            
            if (AuthenticationInitialized() == false)
            {
                RedirectToLoginPage();
            }
        }

        private void RedirectToLoginPage()
        {
            var urlHelper = new UrlHelper(context.RequestContext);
            var response = context.RequestContext.HttpContext.Response;
            var request = context.RequestContext.HttpContext.Request;
            if (request.IsAjaxRequest())
                response.Redirect(urlHelper.Action("Index", new { Area = "Common", Controller = "Login", returnUrl = context.RequestContext.HttpContext.Request.UrlReferrer.AbsolutePath }));
            else
                response.Redirect(urlHelper.Action("Index", new { Area = "Common", Controller = "Login", returnUrl = context.RequestContext.HttpContext.Request.Url.AbsolutePath }));
        }

        private bool AuthenticationInitialized()
        {
            string key = HttpContext.Current.Session.SessionID + "_UserSessionInfo";
            var userSession = (UserSession)HttpContext.Current.Session[key];
            return !(context.HttpContext.User.Identity.IsAuthenticated && userSession == null);
        }
    }
}
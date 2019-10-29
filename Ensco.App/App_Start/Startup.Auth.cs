using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Ensco.Services;
using System.Web.Mvc;

namespace Ensco.App.App_Start
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            UrlHelper _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string actionUri = _url.Action("Index", "Login", new { area = "Common" });

            CookieAuthenticationProvider provider = new CookieAuthenticationProvider();

            // need to add UserManager into owin, because this is used in cookie invalidation
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = EnscoAuthentication.ApplicationCookie,
                LoginPath = new PathString(actionUri),
                Provider = provider,
                CookieName = "EnscoAuthCookieName",
                CookieHttpOnly = true,
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromHours(0.5), // adjust to your needs
            });
        }
    }
}
using System;
using System.Web;

namespace Ensco.Irma.Oap.Common.Extensions
{
    public static class CookieExtensions
    {

        public static HttpCookie ToCookie(this String cookieValue, string cookieName)
        {
            var cookie = new HttpCookie(cookieName);
            var cn = cookieName.ToUpper();

            foreach (var si in cookieValue.Split(';'))
            {
                var s = si.Trim();
                int i1 = s.IndexOf('=');
                if (i1 != -1)
                {
                    var k = s.Split('=');  

                    if (k[0].Trim().Equals("DOMAIN", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Domain = k[1].Trim();
                    }
                    else if (k[0].Trim().Equals("PATH", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Path = k[1].Trim();
                    }
                    else if (k[0].Trim().Equals("EXPIRES", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Expires = DateTime.Parse(k[1].Trim());
                    }
                    else if (k[0].Trim().Equals(cookieName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Value = k[1].Trim();
                    }
                    else
                    {
                        cookie.Values.Add(k[0].Trim(), k[1].Trim());
                    }
                }
                else
                {
                    if (s.Equals("SECURE",StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Secure = true;
                    }
                    else if (s.Equals("HTTPONLY", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.HttpOnly = true; 
                    }
                }
            }
             
            return cookie;
        }

        public static System.Net.Cookie ToNetCookie(this HttpCookie cookie)
        {
            var netCookie = new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain)
            {
                Expires = cookie.Expires,
                HttpOnly = cookie.HttpOnly,
                Secure = cookie.Secure 
            };

            return netCookie;
        }
    }
}

using System;
using System.Configuration;
using System.Web.Mvc; 
using System.Net.Http;
using System.Net;
using Ensco.Irma.Oap.Common.WebClient;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Westwind.Globalization;

namespace Ensco.Irma.Oap.Common.Controllers
{
    [Authorize]
    public abstract class UIBaseController : Controller
    {
        protected const string HtmlHelperKey = "htmlHelper";

        private Lazy<ConcurrentDictionary<Type, object>> RegisteredClients { get; } = new Lazy<ConcurrentDictionary<Type, object>>(() => new ConcurrentDictionary<Type, object>());

        protected virtual string ApiBaseUrlName { get; set; } = "WebApiUrl";

        protected HttpClient Client { get; set; }

        protected CookieContainer CookieContainer { get; set; } = new CookieContainer();

        private HttpClientHandler Handler { get; set; }


        protected UIBaseController()
        {
            var Handler = new HttpClientHandler() { CookieContainer = CookieContainer };
            Client = new HttpClient(Handler, false);
        }

        protected string GetApiBaseUrl() => ConfigurationManager.AppSettings[ApiBaseUrlName];

        ~UIBaseController()
        {
            Handler?.Dispose();
            Client?.Dispose();
        }

        public virtual void RegisterClient(Type type)
        {
            var value = Activator.CreateInstance(type, new object[] { GetApiBaseUrl(), Client });

            RegisteredClients.Value.AddOrUpdate(type, value, (t, v) => v);
        }

        public virtual void RegisterClient(IEnumerable<Type> types)
        {
            types.ToList().ForEach(t => RegisterClient(t));
        }

        public virtual T GetClient<T>()
            where T : OapBaseWebClient
        {
            //if the client is not registered. Register and retrun the client.
            if (!RegisteredClients.Value.TryGetValue(typeof(T), out object value))
            {
                
                RegisterClient(typeof(T));
                RegisteredClients.Value.TryGetValue(typeof(T), out value);
            }

            return value as T;
        }

        protected HtmlHelper Html
        {
            get
            {

                return Session[HtmlHelperKey] as HtmlHelper;

            }
            set
            {
                if (Session[HtmlHelperKey] != null)
                {
                    Session.Remove(HtmlHelperKey);
                }

                Session[HtmlHelperKey] = value;
            }
        }

        protected ActionResult TryCatchCollectionDisplayView<T>(string viewName, string errorKey, Func<IList<T>> execute, [CallerMemberName] string methodName = null)
        {
            IList<T> data = new ObservableCollection<T>();
            try
            {
                data = execute();
            }
            catch (Exception ex)
            {
                var eMsg = ex.Message;

                if (!string.IsNullOrEmpty(errorKey))
                {
                    ViewData[errorKey] = $"Errors occurred while displaying view: {viewName}, {methodName} - {eMsg}";
                }
            }

            return View(viewName, data);
        }

        protected ActionResult TryCatchCollectionDisplayPartialView<T>(string viewName, string errorKey, Func<IEnumerable<T>> execute, [CallerMemberName] string methodName = null)
        {
            IEnumerable<T> data = new ObservableCollection<T>();
            try
            {
                data = execute();
            }
            catch (Exception ex)
            {
                var eMsg = ex.Message;

                ViewData[errorKey] = $"Errors occurred while displaying view: {viewName}, {methodName} - {eMsg}";

            }

            return PartialView(viewName, data);
        }

        protected ActionResult TryCatchDisplayView<T>(string viewName, string errorKey, Func<T> execute, [CallerMemberName] string methodName = null)
            where T: new()
        {
            T data = new T(); ;
            try
            {
                data = execute();
            }
            catch (Exception ex)
            {
                var eMsg = ex.Message;

                if (!string.IsNullOrEmpty(errorKey))
                {
                    ViewData[errorKey] = $"Errors occurred while displaying view: {viewName}, {methodName} - {eMsg}";
                }
            }

            return View(viewName, data);
        }

        protected ActionResult TryCatchDisplayPartialView<T>(string viewName, string errorKey, Func<T> execute, [CallerMemberName] string methodName = null)
            where T : new()
        {
            T data = new T(); ;
            try
            {
                data = execute();
            }
            catch (Exception ex)
            {
                var eMsg = ex.Message;

                ViewData[errorKey] = $"Errors occurred while displaying view: {viewName}, {methodName} - {eMsg}";

            }

            return PartialView(viewName, data);
        }

        protected virtual string Translate(string value, string resourceSet = "OapResources")
        {
            return DbRes.T(value, resourceSet);
        }

    }
}
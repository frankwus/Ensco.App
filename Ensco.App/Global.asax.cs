using DevExpress.Web;
using System.Data; 
using Ensco.App.TLC;
using Ensco.Irma.Services;
using Ensco.Resources;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;

namespace Ensco.App
{
    public class MvcApplication : System.Web.HttpApplication, ISessionUserInfo
    {
        protected void Application_Start()
        {
            // Load Web.Config AppSettings
            LoadConfigSettings();

            // Initialize Utilities
            UtilitySystem.Initialize();

            // Initialize Services
            ServiceSystem.Initialize();
            ServiceSystem.InitializeApplication();

            // Initialize Irma Services
            IrmaServiceSystem.Initialize();

            // Initialize Tlc LookupLists
            TlcManageLookupLists.InitLookupLists();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // DevExpress Model Binder
            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            //DevExpress.Web.BinaryStorageConfigurator.Mode = DevExpress.Web.BinaryStorageMode.Session;

            //Localization
            ModelMetadataProviders.Current = new EnscoLocalizationProvider();
            ASPxWebControl.CallbackError += Application_Error;
        }        

        protected void Session_Start(object sender, EventArgs args)
        {
            UtilitySystem.SessionInfo = this;
        }

        protected void Session_End(object sender, EventArgs args)
        {
            //HttpContext.Current.RewritePath(string.Format("{0}", Url.Action("Common", "Common", new {));
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = HttpContext.Current.Server.GetLastError();
            if (exception is HttpUnhandledException)
                exception = exception.InnerException;
            Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), exception.Message+exception.StackTrace));
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            UserSession userSession = (UtilitySystem.SessionInfo != null) ? UtilitySystem.SessionInfo.GetSessionUser() : null;
            if (userSession != null)
            {
                string culture = userSession.Language;
                CultureInfo ci = new CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }

        private void LoadConfigSettings()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            for (int i = 0; i < ConfigurationManager.AppSettings.Count; i++)
            {
                string key = ConfigurationManager.AppSettings.GetKey(i);
                string value = ConfigurationManager.AppSettings[key];
                config.Add(key, value);
            }
            DataTable dt  = Utilities.UtilitySystem.GetDataSet("select * from AdminCustom").Tables[0] ; 
            string[] arr = new string[] { "DateTimeFormat", "DateFormat", "RigId" , "SiteId"};
            foreach (string key in arr) {
                DataRow[] rows = dt.Select("Name='" + key + "'"); 
                config[key]= rows[0]["Value"].ToString() ;
            }
            UtilitySystem.Settings.ConfigSettings = config;                                  

            //To get RigId and SiteId from the Irma_Configuration table from database  
            //UtilitySystem.Settings.RigId = Convert.ToInt32(UtilitySystem.Settings.ConfigSettings["RigId"]);
            //UtilitySystem.Settings.SiteId = UtilitySystem.Settings.ConfigSettings["SiteId"]; 

        }

        public UserSession GetSessionUser()
        {
            if (HttpContext.Current.Session == null)
                return null;

            string key = HttpContext.Current.Session.SessionID + "_UserSessionInfo";

            return (UserSession)HttpContext.Current.Session[key];
        }

        public void SaveConfigSettings()
        {
            ExeConfigurationFileMap FileMap = new ExeConfigurationFileMap();
            FileMap.ExeConfigFilename = HttpContext.Current.Server.MapPath(@"~\Web.config");

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(FileMap, ConfigurationUserLevel.None);

            foreach (string key in UtilitySystem.Settings.ConfigSettings.Keys)
            {
                if (config.AppSettings.Settings[key] != null)
                    config.AppSettings.Settings[key].Value = UtilitySystem.Settings.ConfigSettings[key];
                else
                    config.AppSettings.Settings.Add(key, UtilitySystem.Settings.ConfigSettings[key]);
            }

            config.Save();
        }
    }

}


using System.Reflection;
using Ensco.Irma.Models;
using Ensco.Irma.Models.Admin;
using Ensco.Irma.Models.Irma;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Linq;
using System.Linq.DataExtensions;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Ensco.App.Infrastructure;
using Ensco.App.ProjectUtilities;
using Ensco.TLC.Models;

namespace Ensco.App.Areas.IRMA.Controllers
{
    public class IrmaController : Ensco.App.Areas.Common.Controllers.BaseController
    {
        // GET: IRMA/Irma     
        [CustomAuthorize]
        public ActionResult Index() {
            return RedirectToAction("Home");            
        }

        [CustomAuthorize]
        public ActionResult Home()
        {
            IrmaDashboardModel irmaDashboard = new IrmaDashboardModel();
            Session["IrmaDashboardModel"] = irmaDashboard;

            if (UtilitySystem.Settings.IsCorporateIrma)            
                return View("IrmaCorpHome",irmaDashboard);            
            else            
                return View("IrmaRigHome",irmaDashboard);
            
        }

        public ActionResult IrmaRigHomePartial()
        {
            return PartialView("IrmaHomePartial");
        }

        
        public ActionResult IrmaMenu()
        {
            return PartialView("IrmaMenu");
        }

        public ActionResult IrmaHomePartial() {
            IrmaDashboardModel irmaDashboard = (IrmaDashboardModel)Session["IrmaHomeModel"];

            return PartialView("IrmaHomePartial", irmaDashboard);
        }

        public ActionResult SystemInfo() {
            SystemInfoModel model = new SystemInfoModel(ServiceSystem.GetServerInfo());

            return View(model);
        }

        public ActionResult About() {
            return View();
        }

        public ActionResult MyPassport(Nullable<int> UserId) {
            int userId = (UserId == null) ? UtilitySystem.CurrentUserId : (int)UserId;

            IrmaHomeModel irmaHome = IrmaServiceSystem.GetLandingPageInfo(userId);
            irmaHome.Id = userId;
            Session["IrmaHomeModel"] = irmaHome;
            DataSet ds = this.GetDataSet("select * from vw_TLCPersonnelSummary  where EnscoPassportNo='" + this.UserId + "'");
            this.ViewBag.TimeAt = ds;

            return View(irmaHome);
        }
        public ActionResult IrmaHomeEnscoPassportPartial(IrmaHomeModel model) {
            IrmaHomeModel irmaHome = (IrmaHomeModel)Session["IrmaHomeModel"];

            return PartialView("IrmaHomeEnscoPassportPartial", irmaHome);
        }

        public ActionResult IrmaHomeEnscoPassportInfoPartial(IrmaHomePassportModel model) {
            IrmaHomeModel irmaHome = (IrmaHomeModel)Session["IrmaHomeModel"];

            return PartialView("IrmaHomeEnscoPassportInfoPartial", irmaHome.UserInfo); 
        }
        public ActionResult IrmaHomeEnscoPassportTasksPartial(IrmaHomeModel model) {
            IrmaHomeModel irmaHome = (IrmaHomeModel)Session["IrmaHomeModel"];

            return PartialView("IrmaHomeEnscoPassportTasksPartial", irmaHome);
        }

        public ActionResult IrmaHomeEnscoPassportTasksCapaPartial(IrmaHomeModel model) {
            DataTableModel dt = this.GetDataTable("Ensco.Models.IrmaCapaModel", "Ensco.Irma.Data.IrmaCapa");
            return PartialView("IrmaHomeEnscoPassportTasksCapaPartial", dt);
        }
        public ActionResult IrmaHomeEnscoPassportTasksTaskPartial( string sourceFormUrl ) {
            if(sourceFormUrl == null)
                sourceFormUrl = ""; 
            return PartialView("IrmaHomeEnscoPassportTasksTaskPartial", sourceFormUrl);
        }
        DataTableModel GetDataTable(string mType, string dType) {
            dynamic handler = IrmaServiceSystem.GetServiceListModel(mType, dType);

            MethodInfo[] AllMethods = handler.GetType().GetMethods();
            MethodInfo fun = AllMethods.FirstOrDefault(mi => mi.Name == "GetQueryable" && mi.GetParameters().Count() == 1);

            DataTableModel dt = new DataTableModel(1, fun.Invoke(handler, new string[] { "Id" }));
            return dt;
        }
        public ActionResult IrmaHomeEnscoPassportTasksJobPartial(IrmaHomeModel model) {
            DataTableModel dt = this.GetDataTable("Ensco.Models.OpenJobModel", "Ensco.Irma.Data.vw_OpenJob");
            dt.Dataset = dt.Dataset.Where(string.Format(@" PassportId={0} ", UtilitySystem.CurrentMyUserId))  ; 
            return PartialView("IrmaHomeEnscoPassportTasksJobPartial", dt);
        }
        public ActionResult IrmaHomeEnscoPassportTasksIsolationPartial(IrmaHomeModel model) {
            DataTableModel dt = this.GetDataTable("Ensco.Models.OpenEiModel", "Ensco.Irma.Data.vw_OpenEi");
            dt.Dataset = dt.Dataset.Where(string.Format(@" PassportId={0} ", UtilitySystem.CurrentMyUserId));
            return PartialView("IrmaHomeEnscoPassportTasksIsolationPartial", dt);
        }
        public ActionResult IrmaHomeEnscoCapPartial(IrmaHomeModel model) {
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];

            string culture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
            //if (info != null)
            //    culture = info.Language;


            if (userSession == null) {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }
            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null) {
                tlcCapURL = oConfig.ConfigValue;
            }

            //string url = "http://localhost:88/RigCap?userId="+ HttpUtility.UrlEncode(ensPassportId);

            ViewBag.CapURL = tlcCapURL + "/MyTLCRig/CAP?language=" + culture + "&userId=" + HttpUtility.UrlEncode(ensPassportId);

            //DataTableModel dt = this.GetDataTable("Ensco.Models.OpenCapModel", "Ensco.Irma.Data.vw_OpenCap");
            //dt.Dataset = dt.Dataset.Where(string.Format(@" EnscoPassportNo= ""{0}"" ", this.UserId));
            return PartialView("IrmaHomeEnscoCapPartial");
        }
        public ActionResult IrmaHomeEnscoPassportTrainingPartial(IrmaHomeModel model) {
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];

            string culture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
            //if (info != null)
            //    culture = info.Language;


            if (userSession == null) {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }
            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null) {
                tlcCapURL = oConfig.ConfigValue;
            }

            //string url = "http://localhost:88/RigCap?userId="+ HttpUtility.UrlEncode(ensPassportId);

            ViewBag.CapURL = tlcCapURL + "/MyTLCRig/Training?language=" + culture + "&userId=" + HttpUtility.UrlEncode(ensPassportId);

            //DataTableModel dt = this.GetDataTable("Ensco.Models.OpenCapModel", "Ensco.Irma.Data.vw_OpenCap");
            //dt.Dataset = dt.Dataset.Where(string.Format(@" EnscoPassportNo= ""{0}"" ", this.UserId));
            return PartialView("IrmaHomeEnscoCapPartial");
            //DataTableModel dt = this.GetDataTable("Ensco.Models.OpenTrainingModel", "Ensco.Irma.Data.vw_OpenTraining");

            //dt.Dataset = dt.Dataset.Where (string.Format(@" EnscoPassportNo= ""{0}"" ", this.UserId));

            //return PartialView("IrmaHomeEnscoPassportTasksTlcPartial", dt);
        }
        public ActionResult IrmaHomeEnscoPassportCareerDevPartial(IrmaHomeModel model) {
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];

            string culture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
            //if (info != null)
            //    culture = info.Language;


            if (userSession == null) {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }
            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null) {
                tlcCapURL = oConfig.ConfigValue;
            }

            //string url = "http://localhost:88/RigCap?userId="+ HttpUtility.UrlEncode(ensPassportId);

            ViewBag.CapURL = tlcCapURL + "/MyTLCRig/CareerDevelopment?language=" + culture + "&userId=" + HttpUtility.UrlEncode(ensPassportId);

            //DataTableModel dt = this.GetDataTable("Ensco.Models.OpenCapModel", "Ensco.Irma.Data.vw_OpenCap");
            //dt.Dataset = dt.Dataset.Where(string.Format(@" EnscoPassportNo= ""{0}"" ", this.UserId));
            return PartialView("IrmaHomeEnscoCapPartial");
            //DataTableModel dt = this.GetDataTable("Ensco.Models.OpenTrainingModel", "Ensco.Irma.Data.vw_OpenTraining");

            //dt.Dataset = dt.Dataset.Where (string.Format(@" EnscoPassportNo= ""{0}"" ", this.UserId));

            //return PartialView("IrmaHomeEnscoPassportTasksTlcPartial", dt);
        }
        public ActionResult IrmaHomeEnscoPobPartial(IrmaHomeModel model) {
            IrmaHomeModel irmaHome = (IrmaHomeModel)Session["IrmaHomeModel"];

            return PartialView("IrmaHomeEnscoPobPartial", irmaHome);
        }
        public ActionResult IrmaCorpPartial() {
            IrmaDashboardModel irmaDashboard = (IrmaDashboardModel)Session["IrmaHomeModel"];

            return PartialView("IrmaCorpPartial", irmaDashboard);
        }
        public ActionResult DisablePassport(int id)
        {
            this.GetDataSet("exec usp_DisablePassport " + id.ToString());

            return null;
        }
        public ActionResult ReconcilePassport() {
            ReconcilePassportModel model = new ReconcilePassportModel();
            Session["ReconcilePassportModel"] = model;

            model.Dataset = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.ReconcilePassport, string.Format("MatchGroup != 0"), "MatchGroup"));

            return View(model);
        }

        public ActionResult ReconcilePassportPartial() {
            ReconcilePassportModel model = (ReconcilePassportModel)Session["ReconcilePassportModel"];

            return PartialView("ReconcilePassportPartial", model);
        }
    }
}
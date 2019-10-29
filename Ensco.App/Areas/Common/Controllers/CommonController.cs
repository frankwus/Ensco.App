using Ensco.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic; 
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Linq.DataExtensions;
using Ensco.App.App_Start;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using Ensco.App.Infrastructure;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data; 
namespace Ensco.App.Areas.Common.Controllers
{
    public class CommonController : BaseController
    {
        // GET: Common/Common
        public ActionResult Index(string returnUrl)
        {
            // Initialize application, this is the first place that we have the application id from query string
            ServiceSystem.InitializeApplication();

            // TEST CODE TO AUTOLOGIN
            LoginTest();
            // TEST CODE TO AUTOLOGIN

            ApplicationModel model = ServiceSystem.ApplicationModel;
            if (model != null)
            {
                if (returnUrl != null)
                    return Redirect(returnUrl);
                //if (currentCrumb != null)
                //    return RedirectToAction(currentCrumb.Action, currentCrumb.Controller, new { area=currentCrumb.Area });
                else
                    return RedirectToAction(model.Action, model.Controller, new { area = model.Area });
            }      

            return View();
        }

        public ActionResult LayoutHeader()
        {
            return PartialView();
        }

        public ActionResult LayoutFooter()
        {
            return PartialView();
        }        

        [CustomAuthorize]
        public ActionResult Language(string lang)
        {
            SetLanguage(lang);

            var langCookie = new HttpCookie("Language", lang)
            {
                Expires = DateTime.MaxValue
            };
            Response.Cookies.Add(langCookie);

            var urlReferrer = this.Request.UrlReferrer?.ToString();

            return Redirect(urlReferrer ?? "/");
            //BreadCrumbModel currentCrumb = null;
            //if (ServiceSystem.ApplicationMenu != null && ServiceSystem.ApplicationMenu.BreadCrumbs.Count > 1)
            //{
            //    currentCrumb = ServiceSystem.ApplicationMenu.BreadCrumbs[ServiceSystem.ApplicationMenu.BreadCrumbs.Count - 1];
            //}

            //if (currentCrumb != null)
            //    return RedirectToAction(currentCrumb.Action, currentCrumb.Controller, new { area = currentCrumb.Area });
            //else
            //    return RedirectToAction("Index", "Common", new { area = "Common", id=UtilitySystem.Settings.ApplicationId});
        }

        internal static void SetLanguage(string lang)
        {
            UserSession info = UtilitySystem.SessionInfo.GetSessionUser();
            if (info != null)
                info.Language = lang;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            ResourceDataManager.Load(UtilitySystem.Settings.ApplicationId, lang);
        }

        public ActionResult ReportProblem()
        {
            return View();
        }

        public ActionResult EnscoPopup()
        {
            PopupModel model = (PopupModel)Session["PopupModel"];

            return View(model);
        }
        public ActionResult GridMultiLookupPartial(string name=null, string lookup=null, bool multi=true, object selected=null, string focusRowChangedEvent=null, string selectionChangedEvent=null, string rowClick=null,  bool? IsRequired=null, bool? readOnly=false)
        {
           object list = Ensco.Irma.Services.IrmaServiceSystem.GetListLookup(lookup); 

            this.ViewBag.Name = name;
            this.ViewBag.lookup = lookup;
            this.ViewBag.Selected = selected;
            this.ViewBag.readOnly = readOnly; 
            return PartialView("GridMultiLookupPartial", list);
        }
        public ActionResult GridLookupPartial(string name, string lookup, bool multi, object selected, string focusRowChangedEvent=null, string selectionChangedEvent=null, string rowClick=null,  bool? IsRequired=null, bool? readOnly=false)
        {
            if(multi && (new string[] { "PassportEmail", "Passport" }).Contains(lookup)  )
                return this.GridMultiLookupPartial(name, lookup, multi, selected, focusRowChangedEvent, selectionChangedEvent, rowClick, IsRequired, readOnly); 
            LookupListModel <dynamic> lkpList = (LookupListModel<dynamic>)Session[name];
            if (lkpList == null && lookup != null){
                lkpList = UtilitySystem.GetLookupList(lookup);
                lkpList.BoundFieldName = name;
                lkpList.MultiSelect = multi;
                lkpList.FocusedRowChanged = focusRowChangedEvent;
                lkpList.RowClick = rowClick; 
                lkpList.SelectionChanged = selectionChangedEvent;
                Session[name] = lkpList;
            }
            if (IsRequired != null && IsRequired.Value)
                this.ViewBag.IsRequired = true; 
            if (lkpList != null)
                lkpList.Selected = selected;
            lkpList.ReadOnly = (readOnly == null ? false : readOnly.Value);

            return PartialView("GridLookupPartial", lkpList);
        }

        public ActionResult TreeLookupPartial(string name, string lookup, bool multi, object selected, string focusRowChangedEvent = null, string selectionChangedEvent = null)
        {
            LookupListModel<dynamic> lkpList = (LookupListModel<dynamic>)Session[name];
            if (lkpList == null && lookup != null)
            {
                lkpList = UtilitySystem.GetLookupList(lookup);
                if (lkpList == null)
                {
                    lkpList.Name = lookup;
                    lkpList.BoundFieldName = name;
                    lkpList.MultiSelect = multi;
                    lkpList.FocusedRowChanged = focusRowChangedEvent;
                    lkpList.SelectionChanged = selectionChangedEvent;
                    Session[name] = lkpList;
                    return new EmptyResult();
                }
            }

            if (lkpList != null)
            {
                lkpList.Selected = selected;
            }

            return PartialView("TreeLookupPartial", lkpList);
        }

        public ActionResult ComboBoxPartial(string name, string lookup, object selected, bool setreadonly, string cascadeField = null, string cascadeFilter = null)
        {
            LookupListModel<dynamic> lkpList = (LookupListModel<dynamic>)Session[name];
            if (lkpList == null && lookup != null)
            {
                lkpList = UtilitySystem.GetLookupList(lookup);
                lkpList.BoundFieldName = name;                
                lkpList.MultiSelect = false;
                Session[name] = lkpList;
            }

            if (lkpList != null)
            {
                lkpList.Selected = selected;
                lkpList.ReadOnly = setreadonly;
                //HttpContext.Session["cbo" + name] = selected;
                //if(cascadeField != null && cascadeField.Length > 0)
                //{
                //    object selitem = HttpContext.Session["cbo" + cascadeField];
                //    if (selitem != null)
                //    {
                //        IQueryable<dynamic> list = lkpList.Items.AsQueryable();
                //        list = list.Where(string.Format("{0}={1}", cascadeFilter, (int)selitem));
                //        lkpList.Items = list.Cast<dynamic>().ToList();
                //    }
                //}
            }

            return PartialView("ComboBoxPartial", lkpList);
        }

        //public JsonResult ProcessScheduledJob(int jobid)
        //{

        //    Logger.Info(new LogInfo(MethodBase.GetCurrentMethod(), string.Format("Schedule job arrived for procesing with jobId={0}", jobid)));

        //    App_Start.SchedulerService.ProcessJob(jobid);

        //    return Json(new { Result = jobid });
        //}

        public void LoginTest()
        {
            WindowsIdentity windowsUser = WindowsIdentity.GetCurrent();
            LoginModel model = new LoginModel();
            string user = windowsUser.Name;
            int index = user.IndexOf('\\');
            if (index == -1)
                index = user.IndexOf('/');
            if (index >= 0)
                user = user.Substring(6);
            if (user.Contains("EnscoApp"))
                user = "";

            if (user.Length != 6 && user.Length != 9)
                return;

            var authenticationResult = AccountManager.Login(user, "test123", HttpContext.GetOwinContext().Authentication);

            if (authenticationResult.IsSuccess)
            {
                string key = this.HttpContext.Session.SessionID + "_UserSessionInfo";
                this.HttpContext.Session[key] = authenticationResult.LoggedInUser;
            }
        }
        public FileStreamResult Pdf( ) {
            try {
               string page = this.Request.QueryString["page"].ToLower(); ;
                string id = this.Request.QueryString["id"];
                string print = this.Request.QueryString["print"];
                string lang = this.Request.QueryString["lang"];
                if (lang == null)
                    lang = "EN";
                PdfAndWord common = new PdfAndWord();
                switch (page) {
                    case "capa":
                        common = new PdfCapa();
                        break;
                    case "ba":
                        common = new PdfBa();
                        break;
                    case "permit":
                        common = new PdfPermit();
                        break;
                    case "gas":
                        common = new PdfGas();
                        break;
                    case "confined":
                        common = new PdfCofined();
                        break;
                    case "lift":
                        common = new PdfLift();
                        break;
                    case "jsa":
                        common = new PdfJsa();
                        break;
                    case "ei":
                        common = new PdfEi();
                        break;
                    case "hotwork":
                        common = new PdfHotwork();
                        break;
                    case "crane":
                        common = new PdfCrane();
                        break;
                    case "riding":
                        common = new PdfRiding();
                        break;
                    case "wi":
                        common = new PdfWi();
                        break;
                    case "fullroster":
                        common = new PdfFullRoster();
                        break;
                    case "swa":
                        common = new PdfSwa();
                        break;
                    case "capaplan":
                        common = new PdfCapaPlan();
                        break;
                    case "poblifeboat":
                        common = new PdfPobLifeboat();
                        break;
                    case "oap":
                        common = new PdfOap();
                        break;
                }
                SortedList sl = new SortedList();
                sl.Add("id", id);
                sl.Add("lang", lang);
                DataSet ds = this.GetDataSet("usp_Pdf" + page + " '" + id.ToString() + "', '" + lang + "'");
                common.ds = ds;
                if (lang != "EN")
                    common.dsLang = this.da.GetDataSet(" select Name, " + lang + " as Name1 from tbl_Label where page like '%" + page + "%'");
                // common.BaseUrl = HttpContextb .Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;

                if (this.Request.Url.Authority.ToLower().Contains("localhost"))
                    common.BaseUrl = "http://localhost:82"; 
                else 
                    common.BaseUrl = UtilitySystem.Settings.ConfigSettings["BaseUrlPdf"];
                common.Lang = lang;
                if (page == "permit")
                    common.Title = ds.Tables[2].Rows[0]["Type"].ToString();
                string userName = Ensco.Utilities.UtilitySystem.CurrentUser.UserName;
                //common.Start(System.Web.HttpContext.Current.Response, userName);
                MemoryStream workStream = new MemoryStream();
                common.Start(workStream, userName);
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                 return new FileStreamResult(workStream, "application/pdf");
            } catch (Exception e) {
                this.GetDataSet("insert tbl_error select  getdate(), '" + e.Message.Replace("'", "''") + "'"  ) ; 
            }
            return null; 
            //return;
            //if(print == "1") {
            //    this.FileName = common.Start(null, this.UserName);
            //    // ClientScript.RegisterStartupScript(typeof(Page), "MessagePopUp","<script language=JavaScript>document.Pdf2.printAll()</script>");
            //} else
            //    common.Start(this.Response, this.UserName);
        }
    }
}
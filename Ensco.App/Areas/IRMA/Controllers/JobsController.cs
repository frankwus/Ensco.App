using Ensco.App.App_Start;
using Ensco.Irma.Models.Jobs;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Utilities;
using mUtilities.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Ensco.App.Areas.IRMA.Controllers
{

    public class JobsController : Controller
    {
        // GET: IRMA/Jobs
        public ActionResult Index()
        {
            //JobsHomeModel model = JobServiceSystem.GetJobsHomeModel();
            //Session["JobsHomeModel"] = model;

            //return View(model);

            return RedirectToAction("ShowJobWindow", new { name="popupJobsHome", title="Home", url = "~/JOB/Home.htm" });

            //return Redirect("~/JOB/Home.htm");
        }

        //public ActionResult JobHomeSummaryPermits()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomeSummaryPermits", model);
        //}
        //public ActionResult JobHomeSummaryPartial()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomeSummaryPartial", model);
        //}
        //public ActionResult JobHomePermitsPartial()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomePermitsPartial", model);
        //}

        //public ActionResult JobHomeIsolations()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomeIsolations", model);
        //}
        //public ActionResult JobHomeShortTermIsolations()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomeShortTermIsolations", model);
        //}
        //public ActionResult JobHomeLongTermIsolations()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomeLongTermIsolations", model);
        //}

        //public ActionResult JobHomeAuthorities()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomeAuthorities", model);
        //}
        //public ActionResult JobHomePermitAuthorities()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomePermitAuthorities", model);
        //}
        //public ActionResult JobHomeIsolationAuthorities()
        //{
        //    JobsHomeModel model = (JobsHomeModel)Session["JobsHomeModel"];

        //    return PartialView("JobHomeIsolationAuthorities", model);
        //}
        //public ActionResult PermitToWork()
        //{
        //    return View();
        //}

        public ActionResult MyPage()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupMyTasks", title = "Home", url = "~/Common/MyPage.htm" });

            //return View();
        }

        public ActionResult ColdWorkPermit()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsColdWorkPermit", title = "Home", url = "~/JOB/LandingPermit.htm?id=0" });

            //return View();
        }

        public ActionResult HotWorkPermit()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsHotWorkPermit", title = "Home", url = "~/JOB/LandingPermit.htm?id=1" });

            //return View();
        }

        public ActionResult ConfinedSpaceEntryPermit()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsConfinedSpaceEntryPermit", title = "Home", url = "~/JOB/LandingPermit.htm?id=2" });
            //return View();
        }

         
        public ActionResult ConfinedSpaceEntryLog()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsConfinedSpaceEntryLog", title = "Home", url = "~/JOB/LandingConfined.htm" });

            //return View();
        }

         
        public ActionResult CranePersonnelLiftChecklist()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsCranePersonnelLiftChecklist", title = "Home", url = "~/JOB/LandingLift.htm" });

            //return View();
        }

         
        public ActionResult EnergyIsolationCertificate()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsEnergyIsolationCertificate", title = "Home", url = "~/JOB/LandingEi.htm" });

            //return View();
        }

         
        public ActionResult GasCertificate()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsGasCertificate", title = "Home", url = "~/JOB/LandingGas.htm" });

            //return View();
        }

         
        public ActionResult HotWorkChecklist()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsHotWorkChecklist", title = "Home", url = "~/JOB/LandingHotworkChecklist.htm" });

            //return View();
        }

         
        public ActionResult LiftPlanChecklist()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsLiftPlanChecklist", title = "Home", url = "~/JOB/LandingLift.htm" });

            //return View();
        }

         
        public ActionResult ManRidingChecklist()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsManRidingChecklist", title = "Home", url = "~/JOB/LandingRiding.htm" });

            //return View();
        }
         
        public ActionResult WorkInstructionsAll()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsWorkInstructionsAll", title = "Home", url = "~/JOB/LandingWims.htm" });

            //return View();
        }


         
        public ActionResult JobSafetyAnalysis()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsJobSafetyAnalysis", title = "Home", url = "~/JOB/LandingJsa.htm" });

            //return View();
        }
         
        public ActionResult JsaWithoutWorkInstruction()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsJsaWithoutWorkInstruction", title = "Home", url = "~/JOB/LandingJsa.htm" });

            //return View();
        }
         
        public ActionResult JsaWithWorkInstruction()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsJsaWithoutWorkInstruction", title = "Home", url = "~/JOB/LandingJsa.htm" });
            //return View();
        }
         
        public ActionResult JsaForWorkInstructionChecklist()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsJsaWithoutWorkInstruction", title = "Home", url = "~/JOB/LandingJsa.htm" });

            //return View();
        }
         
        public ActionResult HazardIdPromptCard()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsHazardIdPromptCard", title = "Home", url = "~/JOB/LandingJsa.htm" });

            //return View();
        }

         
        public ActionResult JobDashboard()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobDashboard", title = "Home", url = "~/JOB/Dashboard.htm" });

            //return View();
        }

         
        public ActionResult SummaryOfOperationalBoundaries()
        {
            return View();
        }

         
        public ActionResult SafetyCriticalAssetList()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupSafetyCriticalAssetList", title = "Home", url = "~/JOB/SCAL.htm" });

            //return View();
        }

         
        public ActionResult DailySummaryPage()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsDailySummaryPage", title = "Home", url = "~/JOB/summary.htm" });

            //            return View();
        }

         
        public ActionResult JobClone()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsDailySummaryPage", title = "Home", url = "~/JOB/summary.htm" });

            //return View();
        }

         
        public ActionResult JobFromWims()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsJobFromWims", title = "Home", url = "~/JOB/LandingWims.htm" });

            //return View();
        }

         
        public ActionResult JobWithPermit()
        {
            return View();
        }

         
        public ActionResult JobWithBuilder()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsJobWithBuilder", title = "Home", url = "~/JOB/builder.htm" });

            //return View();
        }

         
        public ActionResult JobBasic()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsJobWithBuilder", title = "Home", url = "~/JOB/Job.htm?nopermit=1" });

           // return View();
        }
         
        public ActionResult StopWorkAuthority()
        {
            return RedirectToAction("ShowJobWindow", new { name = "popupJobsStopWorkAuthority", title = "Home", url = "~/JOB/LandingSwa.htm" });

            // return View();
        }
         
        public ActionResult CapaCenter()
        {
            return RedirectToAction("ShowJobWindow", "Jobs", new { name = "popupJobsCapaCenter", title = "Home", url = "~/Capa/LandingCapa.htm" });

            //return View();
        }

         
        public ActionResult Capa(int id)
        {
            return RedirectToAction("ShowJobWindow", "Jobs", new { name = "popupCapa", title = "Home", url = string.Format("~/Capa/Capa.htm?id={0}", id) });

            //return View();
        }

         
        public ActionResult CapaPlan(int id)
        {
            return RedirectToAction("ShowJobWindow", "Jobs", new { name = "popupCapaPlan", title = "Home", url = string.Format("~/Capa/CapaPlan.htm?id={0}", id) });

            //return View();
        }

         
        public ActionResult RigAdminPage()
        {
            return RedirectToAction("ShowJobWindow", "Jobs", new { name = "popupJobsRigAdminPage", title = "Home", url = "~/Admin/Admin.htm" });

            //return View();
        }

        public ActionResult ShowJobWindow(string name, string title, string url)
        {
            PopupModel popupModel = SetupJobWindow(name, title, url, Session);

            return View(popupModel);
        }

        internal PopupModel SetupJobWindow(string name, string title, string url, HttpSessionStateBase httpSession)
        {
            PopupModel popupModel = new PopupModel();
            popupModel.Name = name;
            popupModel.Title = title;
            popupModel.ContentUrl = url;
            popupModel.Data = null;
            httpSession["PopupModel"] = popupModel;
            return popupModel;
        }

        [HttpPost]
        public JsonResult GetUserInfo()
        {
            string info = "";
            UserSession userInfo = UtilitySystem.CurrentUser;
            if(userInfo != null)
            {
                LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("Position");
                string position = (lkpList != null) ? (string)lkpList.GetDisplayValue(userInfo.PositionId) : "";
                info = string.Format("{0}:{1}:{2}:{3}", userInfo.Passport, userInfo.UserName, userInfo.Email, position);
            }
            return Json(new { Info = info });
        }

        [HttpPost]
        public JsonResult GetLanguage()
        {
            string info = "";
            UserSession userInfo = UtilitySystem.CurrentUser;
            if (userInfo != null)
            {
                info = userInfo.Language;
            }
            return Json(new { Info = info });
        }

        [HttpPost]
        public JsonResult RunArray(IList<object> array)
        {
            DataAccessor da = new DataAccessor(UtilitySystem.Settings.ConfigSettings["ConnectionString"]);
            DataSet ds;
            string userId = UtilitySystem.CurrentUserPassport;
            List<object> list = new JavaScriptSerializer().ConvertToType<List<object>>(array);
            string sp = list[0].ToString();
            list.RemoveAt(0);
            string sql = "exec " + sp;
            if (list.Count > 0)
                sql += " '" + string.Join("', '", list.ToArray()) + "'";
            try
            {
                da.GetDataSet("insert tbl_log select getdate(), '" + sql.Replace("'", "''") + "', '" + userId + "'");

                ds = da.GetDataSet(sql);
                if (ds.Tables.Count == 0)
                    return null;
                return Json(new { xml = this.GetNewDs(ds).GetXml() });
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
            return null;
        }

        [HttpPost]
        public JsonResult RunArrayRaw(IList<object> array)
        {
            DataAccessor da = new DataAccessor(UtilitySystem.Settings.ConfigSettings["ConnectionString"]);
            DataSet ds;

            string userId = UtilitySystem.CurrentUserPassport;
            List<object> list = new JavaScriptSerializer().ConvertToType<List<object>>(array);
            string sp = list[0].ToString();
            list.RemoveAt(0);
            string sql = "exec " + sp;
            if (list.Count > 0)
                sql += " '" + string.Join("', '", list.ToArray()) + "'";
            try
            {
                ds = da.GetDataSet(sql);
                if (ds.Tables.Count == 0)
                    return null;
                return Json(new { xml = ds.GetXml() });
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
            return null;
        }
        [HttpPost]
        public JsonResult RunSearch(IList<object> array)
        {
            DataAccessor da = new DataAccessor(UtilitySystem.Settings.ConfigSettings["ConnectionString"]);
            DataSet ds;

            string userId = UtilitySystem.CurrentUserPassport;
            List<string> list = new JavaScriptSerializer().ConvertToType<List<string>>(array);
            string sp = list[0].ToString();
            list.RemoveAt(0);
            for (int i = 0; i < list.Count; i++)
                if (list[i] == "")
                    list[i] = "null";
            string sql = "exec " + sp;
            if (list.Count > 0)
                sql += " '" + string.Join("', '", list.ToArray()) + "'";
            sql = sql.Replace("'null'", "null");
            try
            {

                ds = da.GetDataSet(sql);
                if (ds.Tables.Count == 0)
                    return null;
                return Json(new { xml = this.GetNewDs(ds).GetXml() });
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
            return null;
        }
        private DataSet GetNewDs(DataSet ds)
        {
            DataSet ds1 = new DataSet();
            foreach (DataTable dt in ds.Tables)
                ds1.Tables.Add(this.GetNullFilledDataTableForXML(dt));

            return ds1;
        }

        private DataTable GetNullFilledDataTableForXML(DataTable dtSource)
        {
            DataTable dtTarget = dtSource.Clone();
            foreach (DataColumn col in dtTarget.Columns)
                col.DataType = typeof(string);

            int colCountInTarget = dtTarget.Columns.Count;
            foreach (DataRow sourceRow in dtSource.Rows)
            {
                DataRow targetRow = dtTarget.NewRow();
                targetRow.ItemArray = sourceRow.ItemArray;

                for (int ctr = 0; ctr < colCountInTarget; ctr++)
                    if (targetRow[ctr] == DBNull.Value)
                        targetRow[ctr] = String.Empty;

                dtTarget.Rows.Add(targetRow);
            }
            return dtTarget;
        }
    }
}

using Ensco.App.ProjectUtilities;
using System.Data; 
using Ensco.App.TLC;
using Ensco.Irma.Models;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.TLC.Models;
using Ensco.TLC.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.DataExtensions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.IRMA.Controllers
{
    public class CapController : Ensco.App.Areas.Common.Controllers.BaseController
    {
        private RigCapService service = new RigCapService();

        // GET: IRMA/Cap
        public ActionResult CapBook()
        {
            
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];

            string culture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
            //if (info != null)
            //    culture = info.Language;


            if (userSession == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }
            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null)
            {
                tlcCapURL = oConfig.ConfigValue;
            }

            //string url = "http://localhost:88/RigCap?userId="+ HttpUtility.UrlEncode(ensPassportId);
            string url = this.GetTlcUrl() + "/RigCap?language="+ culture + "&userId=" + HttpUtility.UrlEncode(ensPassportId);

            var viewModel = new JobsController().SetupJobWindow("popupJobsHome", "Home", url, Session);

            return View("ShowJobWindow", viewModel);            
        }
        string  GetTlcUrl() {
            if(this.Request.Url.ToString().ToLower().Contains("localhost"))
                return "http://localhost:81";
            DataSet ds = this.GetDataSet("select ConfigValue from IRMA_Configuration where ConfigKey='TLCCapUrl'");
            return ds.Tables[0].Rows[0][0].ToString(); 
        }
        public ActionResult CapBookApproval(string FromMyTLC)
        {
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];
            string culture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();

            if (userSession == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }

            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null)
            {
                tlcCapURL = oConfig.ConfigValue;
            }

            //string url = "http://localhost:88/CapApproval?UserId=" + HttpUtility.UrlEncode(ensPassportId);
            string url = string.Empty;
            if (FromMyTLC == "true")
            {
                url = this.GetTlcUrl() + "/CapApproval?language=" + culture + "&UserId=" + HttpUtility.UrlEncode(ensPassportId)+ "&FromMyTLC=true";
            }
            else
            {
                url = this.GetTlcUrl() + "/CapApproval?language=" + culture + "&UserId=" + HttpUtility.UrlEncode(ensPassportId);
            }


            var viewModel = new JobsController().SetupJobWindow("popupJobsHome", "Home", url, Session);

            return View("ShowJobWindow", viewModel);
        }
        public ActionResult CAPFleetSummary()
        {
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];
            string culture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();

            if (userSession == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }

            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null)
            {
                tlcCapURL = oConfig.ConfigValue;
            }

            int RigId = UtilitySystem.Settings.RigId;
            string strRigId = Encryption.Encrypt(RigId.ToString(), configSalt);
            string url = this.GetTlcUrl() + "/CapApproval/summaryrig?language=" + culture + "&UserId=" + HttpUtility.UrlEncode(ensPassportId) + "&strRigId=" + HttpUtility.UrlEncode(strRigId);

            var viewModel = new JobsController().SetupJobWindow("popupJobsHome", "Home", url, Session);

            return View("ShowJobWindow", viewModel);
        }
        public ActionResult TRNFleetSummary()
        {
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];

            if (userSession == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }

            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null)
            {
                tlcCapURL = oConfig.ConfigValue;
            }

            int RigId = UtilitySystem.Settings.RigId;
            string strRigId = Encryption.Encrypt(RigId.ToString(), configSalt);
            string url = this.GetTlcUrl() + "/TrnFleetSummary/summaryrig?UserId=" + HttpUtility.UrlEncode(ensPassportId) + "&strRigId=" + HttpUtility.UrlEncode(strRigId);

            var viewModel = new JobsController().SetupJobWindow("popupJobsHome", "Home", url, Session);

            return View("ShowJobWindow", viewModel);
        }

        public ActionResult LearningEnrollment()
        {
            string[] configSalt = CommonMethods.GetIrmaConfig().Where(c => (c.ConfigKey == "SaltString" || c.ConfigKey == "bytePermutation1" || c.ConfigKey == "bytePermutation2" || c.ConfigKey == "bytePermutation3" || c.ConfigKey == "bytePermutation4") && c.IsActive == true).Select(c => c.ConfigValue).ToArray();
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];

            if (userSession == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Common" });
            }

            string enscoPassport = userSession.Passport;
            string ensPassportId = Encryption.Encrypt(enscoPassport, configSalt);

            string tlcCapURL = string.Empty;
            IRMA_ConfigurationModel oConfig = CommonMethods.GetIrmaConfig().Where(c => c.ConfigKey == "TLCCapUrl" && c.IsActive == true).FirstOrDefault();
            if (oConfig != null)
            {
                tlcCapURL = oConfig.ConfigValue;
            }

            int RigId = UtilitySystem.Settings.RigId;
            string strRigId = Encryption.Encrypt(RigId.ToString(), configSalt);
            string url = this.GetTlcUrl() + "/RigLearning/Index?UserId=" + HttpUtility.UrlEncode(ensPassportId);

            var viewModel = new JobsController().SetupJobWindow("popupJobsHome", "Home", url, Session);

            return View("ShowJobWindow", viewModel);
        }

        //public ActionResult CapBook()
        //{
        //    ManageCapBookModel model = new ManageCapBookModel();
        //    model.PersonalSummary = new DataTableModel(1, (IQueryable)service.GetVwTLCSummaryQueryable());

        //    Session["ManageCapBookModel"] = model;
        //    ViewData["SelectedCapBookPersonalId"] = (long)0;

        //    return View(model);
        //}
        public ActionResult CapBookPartial(Nullable<long> PersonalId)
        {
            ManageCapBookModel model = (ManageCapBookModel) Session["ManageCapBookModel"];

            if (PersonalId != null)
            {
                ViewData["SelectedCapBookPersonalId"] = (long)PersonalId;
                model.SelectedPersonnelId = (long)PersonalId;
                TLC_PersonalSummaryModel ps = model.PersonalSummary.GetItem(PersonalId);
                if (ps != null)
                    model.Passport = ps.EnscoPassportNo;
            }

            IQueryable<CAP_CompetencyAssessModel> compItems = service.GetCompAssessQueryable(model.Passport, null);
            compItems = compItems.OrderBy(x => x.Title);

            model.Competency = new DataTableModel(2, compItems);
            if (model.Passport != null && model.SelectedPersonnelId != 0)
            {
                model.CapBooks = service.GetCAPBookQueryable().Where(x => x.EnscoPassportNo == model.Passport);
                model.KSAs = new DataTableModel(3, model.CapBooks.Where(x => x.CompId == (long)0));
                model.Additional = new DataTableModel(3, model.CapBooks.Where(x => x.CompId == (long)0));
                model.Expired = new DataTableModel(3, model.CapBooks.Where(x => x.CompId == (long)0));
                model.AssessorGuide = new DataTableModel(3, model.CapBooks.Where(x => x.CompId == (long)0));

                int count = model.KSAs.Dataset.Count();
            }

            return PartialView("CapBookPartial", model);
        }

        public ActionResult CapBookDetailPartial()
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            return PartialView("CapBookDetailPartial", model);
        }

        public ActionResult CapBookKSAsPartial(Nullable<int> passport)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            return PartialView("CapBookKSAsPartial", model);
        }

        public ActionResult CapBookAdditionalPartial(Nullable<int> passport)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            return PartialView("CapBookAdditionalPartial", model);
        }

        public ActionResult CapBookExpirationPartial(Nullable<int> passport)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            return PartialView("CapBookExpirationPartial", model);
        }

        public ActionResult CapBookAssessorGuidePartial(Nullable<int> passport)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            return PartialView("CapBookAssessorGuidePartial", model);
        }


        public ActionResult CapBookKSAsDetailPartial(Nullable<long> compId)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            if(model.CapBooks != null && compId != null)
            {
                model.KSAs.Dataset = model.CapBooks.Where(x => x.CompId == (long)compId);
                model.CompId = (long)compId;
            }

            return PartialView("CapBookKSAsDetailPartial", model);
        }

        [HttpPost]
        public async Task<ActionResult> CapBookKSAsDetailUpdate(CAP_BookModel capBook)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            if (ModelState.ContainsKey("AssessedDate"))
            {
                ModelState["AssessedDate"].Errors.Clear();
            }

            if (string.IsNullOrEmpty(capBook.AssessmentMethod))
            {
                ModelState.AddModelError("AssessmentMethod", "Please select Assessment Method");
            }
            if (string.IsNullOrEmpty(capBook.Assessment))
            {
                ModelState.AddModelError("Assessment", "Please select Assessment");
            }
            if (ModelState.IsValid)
            {
                List<CAP_BookModel> entity = model.CapBooks.Where(x => x.Id == capBook.Id).ToList();

                await service.UpdateCAPPersonnelSummary(capBook, UtilitySystem.CurrentUserName);
            }

            return PartialView("CapBookKSAsDetailPartial", model);
        }
        public ActionResult CapBookAdditionalDetailPartial(Nullable<int> compId)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];
            if (model != null && model.CapBooks != null && compId != null)
            {
                IQueryable<CAP_BookModel> items = model.CapBooks.Where(x => x.CompId == (long)compId);
                model.Additional.Dataset = items.Where(x => x.IsAddl == true);
            }
            return PartialView("CapBookAdditionalDetailPartial", model);
        }
       
        [HttpPost]
        public async Task<ActionResult> CapBookAdditonalDetailAdd(CAP_BookModel capBook)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            if (ModelState.ContainsKey("AssessedDate"))
            {
                ModelState["AssessedDate"].Errors.Clear();
            }

            if (ModelState.ContainsKey("AssessmentMethod"))
            {
                ModelState["AssessmentMethod"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                List<int> KSAIds = new List<int>();
                for (int i = 0; i < capBook.SelectedKSAs.Length; i++)
                {
                    int KSAId = -1;
                    if (Int32.TryParse(capBook.SelectedKSAs[i], out KSAId))
                    {
                        KSAIds.Add(KSAId);
                    }
                }

                CAP_AdditionalModel addlModel = new CAP_AdditionalModel();
                addlModel.PositionId = capBook.PositionId.ToString();
                addlModel.SelCompId = capBook.CompId.ToString();
                addlModel.Department = capBook.Department.ToString();

                await service.AddAdditionalKSAs(addlModel, KSAIds, model.Passport, UtilitySystem.CurrentUserName);

            }

            return PartialView("CapBookAdditionalDetailPartial", model);
        }

        [HttpPost]
        public ActionResult CapBookAdditonalDetailUpdate(CAP_BookModel capBook)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            if (ModelState.ContainsKey("AssessedDate"))
            {
                ModelState["AssessedDate"].Errors.Clear();
            }

            if (string.IsNullOrEmpty(capBook.AssessmentMethod))
            {
                ModelState.AddModelError("AssessmentMethod", "Please select Assessment Method");
            }
            if (string.IsNullOrEmpty(capBook.Assessment))
            {
                ModelState.AddModelError("Assessment", "Please select Assessment");
            }
            if (ModelState.IsValid)
            {
                List<CAP_BookModel> entity = model.CapBooks.Where(x => x.Id == capBook.Id).ToList();

                Task task = service.UpdateCAPPersonnelSummary(capBook, UtilitySystem.CurrentUserName);
                task.Wait();
            }

            return PartialView("CapBookAdditionalDetailPartial", model);
        }

        public ActionResult CapBookExpirationDetailPartial(Nullable<int> compId)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];
            if (model.CapBooks != null && compId != null)
            {
                IQueryable<CAP_BookModel> items = model.CapBooks.Where(x => x.CompId == (long)compId);
                model.Expired.Dataset = items.Where(x => x.IsAddl == false);
            }
            return PartialView("CapBookExpirationDetailPartial", model);
        }

        [HttpPost]
        public ActionResult CapBookExpirationDetailUpdate(CAP_BookModel capBook)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            if (ModelState.ContainsKey("AssessedDate"))
            {
                ModelState["AssessedDate"].Errors.Clear();
            }

            if (string.IsNullOrEmpty(capBook.AssessmentMethod))
            {
                ModelState.AddModelError("AssessmentMethod", "Please select Assessment Method");
            }
            if (string.IsNullOrEmpty(capBook.Assessment))
            {
                ModelState.AddModelError("Assessment", "Please select Assessment");
            }
            if (ModelState.IsValid)
            {
                List<CAP_BookModel> entity = model.CapBooks.Where(x => x.Id == capBook.Id).ToList();

                Task task = service.UpdateCAPPersonnelSummary(capBook, UtilitySystem.CurrentUserName);
                task.Wait();
            }

            return PartialView("CapBookExpirationDetailPartial", model);
        }

        public ActionResult CapBookAssessorGuideDetailPartial(Nullable<int> compId)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];
            if (model.CapBooks != null && compId != null)
            {
                IQueryable<CAP_BookModel> items = model.CapBooks.Where(x => x.CompId == (long)compId);
                model.AssessorGuide.Dataset = items.Where(x => x.IsAddl == false);
            }

            return PartialView("CapBookAssessorGuideDetailPartial", model);
        }

        //public ActionResult CapBookApproval()
        //{
        //    ManageCapBookModel model = new ManageCapBookModel();
        //    model.CapBookApprovals = new DataTableModel(1, service.GetCapBookApprovals());

        //    Session["ManageCapBookModel"] = model;
        //    ViewData["SelectedCapApprovalPassport"] = "";

        //    return View(model);
        //}

        public ActionResult CapBookApprovalPartial(string passport)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            if (passport != null && passport != "")
            {
                ViewData["SelectedCapApprovalPassport"] = passport;
                model.Passport = passport;
            }


            //if (model.Passport != null)
            //{
            //    CAP_BookApprovalViewModel capBookApproval = model.CapBookApprovals.GetItem(model.Passport);

            //    UserModel user = ServiceSystem.GetUserFromPassport(model.Passport);
            //    if (capBookApproval != null && user != null)
            //    {
            //        LookupListModel<dynamic> lkpPos = UtilitySystem.GetLookupList("Position");

            //        // Admin users
            //        IIrmaServiceDataModel adminCustom = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewChangeApprovalView);
            //        List<dynamic> items = adminCustom.GetAllItems();

            //        IIrmaServiceDataModel approvalModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            //        List<dynamic> approvals = approvalModel.GetItems(string.Format("RequestItemId={0} and Type={1}", user.Id, (int)ApprovalModel.ApprovalType.CapBook), "Id");
            //        if (approvals.Count <= 0)
            //        {
            //            ApprovalModel approval = new ApprovalModel();
            //            approval.Requester = user.Id;
            //            approval.RequestedDate = DateTime.Now;
            //            approval.RequestItemId = user.Id;
            //            approval.RequestInfo = Url.Action("CapBookApprovalStatusUpdate", "Cap", new { Area = "IRMA" });
            //            approval.Name = "Cap Book Approval";
            //            approval.Type = (int)ApprovalModel.ApprovalType.CapBook;
            //            approval.Approver = user.Id;
            //            approval.Position = (int)user.Position;
            //            approval.Sequence = 1;
            //            approval = approvalModel.Add(approval, true);

            //            // Don't have supervisors in Master_Users table (add self for now)
            //            approval = new ApprovalModel();
            //            approval.Requester = user.Id;
            //            approval.RequestedDate = DateTime.Now;
            //            approval.RequestItemId = user.Id;
            //            approval.RequestInfo = Url.Action("CapBookApprovalStatusUpdate", "Cap", new { Area = "IRMA" });
            //            approval.Name = "Cap Book Approval";
            //            approval.Type = (int)ApprovalModel.ApprovalType.CapBook;
            //            approval.Approver = user.Id;
            //            approval.Position = (int)user.Position;
            //            approval.Sequence = 2;
            //            approval = approvalModel.Add(approval, true);

            //            // OIM
            //            CrewChangeApproverModel approver = items.FirstOrDefault(x => x.Name == "OIM");
            //            approval = new ApprovalModel();
            //            approval.Requester = UtilitySystem.CurrentUserId;
            //            approval.RequestedDate = DateTime.Now;
            //            approval.RequestItemId = user.Id;
            //            approval.RequestInfo = Url.Action("CapBookApprovalStatusUpdate", "Cap", new { Area = "IRMA" });
            //            approval.Name = "Cap Book Approval";
            //            approval.Type = (int)ApprovalModel.ApprovalType.CapBook;
            //            approval.Approver = approver.Id;
            //            approval.Position = approver.Position;
            //            approval.Sequence = 3;
            //            approval = approvalModel.Add(approval, true);

            //            // Rig Manager
            //            approver = items.FirstOrDefault(x => x.Name == "Master");
            //            approval = new ApprovalModel();
            //            approval.Requester = UtilitySystem.CurrentUserId;
            //            approval.RequestedDate = DateTime.Now;
            //            approval.RequestItemId = user.Id;
            //            approval.RequestInfo = Url.Action("CapBookApprovalStatusUpdate", "Cap", new { Area = "IRMA" });
            //            approval.Name = "Cap Book Approval";
            //            approval.Type = (int)ApprovalModel.ApprovalType.CapBook;
            //            approval.Approver = approver.Id;
            //            approval.Position = approver.Position;
            //            approval.Sequence = 4;
            //            approval = approvalModel.Add(approval, true);
            //        }

            //        model.Approvals = approvals.Cast<ApprovalModel>().ToList();
            //        foreach(ApprovalModel am in model.Approvals)
            //        {
            //            am.Initialize();
            //        }
            //    }
            //}

            return PartialView("CapBookApprovalPartial", model);
        }

        public ActionResult CapBookApprovalDetailPartial()
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            return PartialView("CapBookApprovalDetailPartial", model);
        }

        [HttpPost]
        public ActionResult CapBookApprovalDetailUpdate(ApprovalModel approval)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            if(ModelState.IsValid)
            {
                IIrmaServiceDataModel approvalModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
                ApprovalModel entity = approvalModel.GetItem(string.Format("Id={0}", approval.Id), "Id");
                if(entity != null)
                {
                    entity.ApproverComments = approval.ApproverComments;
                    approvalModel.Update(entity);
                    int index = model.Approvals.FindIndex(x => x.Id == approval.Id);
                    if (index >= 0 && index < model.Approvals.Count)
                    {
                        model.Approvals[index].ApproverComments = approval.ApproverComments;
                    }
                }
            }

            return PartialView("CapBookApprovalDetailPartial", model);
        }

        public ActionResult CapBookApprovalStatusUpdate(int Id, int ApprovalId, int Approver, int Status)
        {
            ManageCapBookModel model = (ManageCapBookModel)Session["ManageCapBookModel"];

            IIrmaServiceDataModel statusDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            ApprovalModel approval = statusDataModel.GetItem(string.Format("Id={0}", ApprovalId), "Id");
            if (approval != null)
            {
                approval.Status = Status;
                if (Status != 2)
                    approval.ApprovedDate = DateTime.Now;
                statusDataModel.Update(approval);
            }

            return View("CapBookApproval", model);
        }

        //public ActionResult FleetSummary(Nullable<int> passportId)
        //{
        //    FleetSummaryModel model = new FleetSummaryModel();

        //    Session["FleetSummaryModel"] = model;

        //    model.FleetSummaryRig = new DataTableModel(1, service.GetCapBookFleetSummaryRig().Where(x => x.RigId == UtilitySystem.Settings.RigId));
        //    model.Totals = service.GetCapBookFleetSummaryTotals().Where(x=>x.RigId == UtilitySystem.Settings.RigId);

        //    model.RigName = UtilitySystem.Settings.RigName;
        //    model.CapBooksNotStarted = model.Totals.Where(x => x.NotStarted > 0).Count();
        //    model.CapBooksInProgress = model.Totals.Where(x => x.InProgress > 0).Count();
        //    model.CapBooksPendingApproval = model.Totals.Where(x => x.PendingApproval > 0).Count();
        //    model.CapBooksOverdue = model.Totals.Where(x => x.Overdue > 0).Count();
        //    model.CapBooksExpiringNext3Months = model.Totals.Where(x => x.ExpireNext3Month > 0).Count();
        //    model.TotalCapBooks = model.Totals.Where(x => x.Total > 0).Count();
        //    model.CapBooksApproved = model.Totals.Where(x => x.Approved > 0).Count();

        //    model.PassportId = (passportId != null) ? (int)passportId : 0;
        //    if (model.PassportId != null && model.PassportId != 0)
        //    {
        //        UserModel user = ServiceSystem.GetUser((int)model.PassportId);
        //        if (user != null)
        //        {
        //            LookupListModel<dynamic> lkpDept = UtilitySystem.GetLookupList("Department");
        //            LookupListModel<dynamic> lkpPos = UtilitySystem.GetLookupList("Position");
        //            model.EnscoPassportNo = user.Passport.Trim();
        //            model.Department = (string)lkpDept.GetDisplayValue(user.Department);
        //            model.Position = (string)lkpPos.GetDisplayValue(user.Position);

        //            model.FleetSummaryEmployee = new DataTableModel(2, service.GetCapBookFleetSummaryEmployee().Where(x=>x.EnscoPassportNo == model.EnscoPassportNo));
        //            model.EmployeeTotals = service.GetCapBookFleetSummaryEmployeeTotals().Where(x => x.EnscoPassportNo == model.EnscoPassportNo);
        //            foreach(CAP_FleetSummaryEmployeeTotalsModel item in model.EmployeeTotals)
        //            {
        //                model.EmpTotals = item;
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        model.FleetSummaryEmployee = new DataTableModel(2, service.GetCapBookFleetSummaryEmployee().Where(x => x.EnscoPassportNo == ""));
        //        model.EmployeeTotals = service.GetCapBookFleetSummaryEmployeeTotals().Where(x => x.EnscoPassportNo == "");
        //        model.EmpTotals = new CAP_FleetSummaryEmployeeTotalsModel();
        //    }

        //    return View(model);
        //}

        public ActionResult FleetSummaryRigPartial()
        {
            FleetSummaryModel model = (FleetSummaryModel)Session["FleetSummaryModel"];

            return PartialView("FleetSummaryRigPartial", model);
        }

        public ActionResult FleetSummaryRigDetailPartial()
        {
            FleetSummaryModel model = (FleetSummaryModel)Session["FleetSummaryModel"];

            return PartialView("FleetSummaryRigDetailPartial", model);
        }

        public ActionResult FleetSummaryEmployeePartial()
        {
            FleetSummaryModel model = (FleetSummaryModel)Session["FleetSummaryModel"];

            return PartialView("FleetSummaryEmployeePartial", model);
        }

        public ActionResult FleetSummaryEmployeeDetailPartial()
        {
            FleetSummaryModel model = (FleetSummaryModel)Session["FleetSummaryModel"];

            return PartialView("FleetSummaryEmployeeDetailPartial", model);
        }
    }
}
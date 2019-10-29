using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Rig;
    using Ensco.App.Areas.Oap.Extensions;    
    using System.Web.Routing;
    using System.Web;
    using Ensco.Models;
    using System.Linq;
    using Ensco.App.Areas.Common.Controllers;
    using System.Web.Mvc.Html;
    using Ensco.App.Areas.Oap.Models;
    using System.Collections.ObjectModel;
    using System.Web.UI.WebControls;
    using Ensco.Services;    
    using System;        
    using Ensco.Irma.Data.Services.WILocal;
    using Ensco.Irma.Data.Services.SWA;
    using Ensco.Irma.Data.Services.CAPA;
    using Ensco.Irma.Data.Services.Job;

    public class OIMOversightDashboardController : GenericDashboardController
    {
        private readonly CAPAService capaService;
        private readonly SWAService swaService;
        private readonly WILocalService wiLocalService;
        private readonly JobService jobService;

        protected string VerifierRole { get; set; }
        private UserModel currentOIM { get; set; }

        public OIMOversightDashboardController(CAPAService capaService, SWAService swaService, WILocalService wiLocalService, JobService jobService)
        {
            GridDataChecklist.Title = "Open OIM Oversight Checklists";
            ChecklistType = "RO";
            ChecklistSubType = "OIM";
            GridDataChecklist = new GridData(oapChecklistGridName, "OIMOversightDashboard", "RigChecklists", "Open OIM Oversight Checklists", "AddNewRigChecklist", "Add Checklist", "search checklists", initializeCallBack: true)
            {
                EditController = "OIMOversight"
            };

            this.capaService = capaService;
            this.swaService = swaService;
            this.wiLocalService = wiLocalService;
            this.jobService = jobService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["CurrentOIM"] != null)
                currentOIM = (UserModel)Session["CurrentOIM"];
            else
            {
                currentOIM = ServiceSystem.GetCurrentOIM();
                Session["CurrentOIM"] = currentOIM;
            }

            base.OnActionExecuting(filterContext);
        }

        public override ActionResult Index()
        {
            var data = GetRigChecklistData(true);

            return View("OIMOversightDashboard", data);
        }

        public override ActionResult RigChecklists()
        {
            var data = GetRigChecklistData(true);
            return PartialView("OIMOversightDashboardPartial", data);

        }

        protected override IEnumerable<OapChecklist> GetChecklistData(bool useTypeSubTypeCode = false)
        {
            return base.GetChecklistData(true);
        }

        public IEnumerable<OIMBehavioralBasedSafetyFlatModel> GetBBSFSOList()
        {
            ChecklistType = "FSO";
            ChecklistSubType = "All";
            ChecklistStatus = "Pending";
            VerifierRole = "OIM";
            return OapExtensions.ToBBSFlattenedModel(GetClient<RigOapChecklistClient>(), ChecklistType, ChecklistSubType, ChecklistStatus, VerifierRole);
        }

        public IEnumerable<OIMBehavioralBasedSafetyFlatModel> GetBBSSWAList()
        {
            ChecklistType = "SWA";
            ChecklistSubType = "All";
            ChecklistStatus = "Pending";
            VerifierRole = "OIM";
            return OapExtensions.ToBBSFlattenedModel(GetClient<RigOapChecklistClient>(), ChecklistType, ChecklistSubType, ChecklistStatus, VerifierRole);
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            base.Configure(settings, html, viewContext);
        }

        protected override void InitializeRigChecklistGridData(HtmlHelper html, ViewContext viewContext, GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            base.InitializeRigChecklistGridData(html, viewContext, gridData, createAction, updateAction, deleteAction);

            var leadAssessor = gridData.DisplayColumns.FirstOrDefault(gc => gc.Name.Equals("OwnerId"));
            if (leadAssessor != null)
            {
                leadAssessor.DisplayName = "OIM";
            }

            var leadAssessorPosition = gridData.DisplayColumns.FirstOrDefault(gc => gc.Name.Equals("PositionId"));
            if (leadAssessorPosition != null)
            {
                leadAssessorPosition.DisplayName = "OIM Position";
                leadAssessorPosition.Order = 2000;
                leadAssessorPosition.Width = 0;
                leadAssessorPosition.IsVisible = false;
            }
        }

        public GridData BBSFSOGridData { get; } = new GridData("rigChecklistBBSFSOGrid", "OIMOversightDashboard", "DisplayBBSFSOGrid", initializeCallBack: true, showPager: false); // addToolBarItemStyle: new GridToolBarItemStyle(System.Drawing.Color.DarkGray,System.Drawing.Color.Black, System.Drawing.Color.DarkGray,BorderStyle.Solid));
        public GridData BBSSWAGridData { get; } = new GridData("rigChecklistBBSSWAGrid", "OIMOversightDashboard", "DisplayBBSSWAGrid", initializeCallBack: true, showPager: false);


        public ActionResult CAVPendingReviewPartial()
        {
            ViewBag.GridName = "CAVPendingGrid";
            ViewBag.GridTitle = "Critical Area Verifications Pending Review";
            ViewBag.GridLinkController = "CriticalAreaVerification";
            ViewBag.GridControllerCallBackAction = "CAVPendingReviewPartial";

            var cavList = RigOapChecklistClient.GetAllByTypeStatusAsync("Critical Area Verification", "Pending").Result?.Result.Data;

            var viewModelList = GenerateChecklistViewModelList(cavList);

            return PartialView("ChecklistPendingReviewPartial", viewModelList);
        }

        public ActionResult FSOPendingReviewPartial()
        {
            ViewBag.GridName = "FSOPendingGrid";
            ViewBag.GridTitle = "Formal Safety Observations Pending Review";
            ViewBag.GridLinkController = "FSO";
            ViewBag.GridControllerCallBackAction = "FSOPendingReviewPartial";

            var fsoList = RigOapChecklistClient.GetAllByTypeStatusAsync("Formal Safety Observation", "Pending").Result?.Result.Data;
            var viewModelList = GenerateChecklistViewModelList(fsoList);

            return PartialView("ChecklistPendingReviewPartial", viewModelList);
        }

        public ActionResult LifeSaversPendingReviewPartial()
        {
            ViewBag.GridName = "LSPendingGrid";
            ViewBag.GridTitle = "Ensco Life Savers Pending Review";
            ViewBag.GridLinkController = "LifeSaver";
            ViewBag.GridControllerCallBackAction = "LifeSaversPendingReviewPartial";

            var lifeSaversList = RigOapChecklistClient.GetAllByTypeStatusAsync("Life Savers", "Pending").Result?.Result.Data;
            var viewModelList = GenerateChecklistViewModelList(lifeSaversList);

            return PartialView("ChecklistPendingReviewPartial", viewModelList);
        }

        public ActionResult BACPendingReviewPartial()
        {
            ViewBag.GridName = "BACPendingGrid";
            ViewBag.GridTitle = "Barrier Authority Checklists Pending Review";
            ViewBag.GridLinkController = "BarrierAuthority";

            var bacList = RigOapChecklistClient.GetAllByTypeStatusAsync("Barrier Authority Checklist", "Pending").Result?.Result.Data;

            IList<OIMDashboardBACGridViewModel> viewModelList = new List<OIMDashboardBACGridViewModel>();

            foreach (var bacChecklist in bacList)
            {
                var leadAssessor = bacChecklist.Assessors.FirstOrDefault(a => a.IsLead);
                UserModel assessor = null;
                PositionModel leadAssessorPosition = null;

                if (leadAssessor != null && leadAssessor.UserId != 0)
                {
                    assessor = ServiceSystem.GetUser(leadAssessor.UserId);
                    leadAssessorPosition = ServiceSystem.GetUserPosition(leadAssessor.UserId);
                }

                OIMDashboardBACGridViewModel viewModel = new OIMDashboardBACGridViewModel()
                {
                    ChecklistGuid = bacChecklist.Id,
                    Id = bacChecklist.RigChecklistUniqueId,
                    Location = bacChecklist.LocationName,
                    Title = bacChecklist.Title,
                    Assessor = assessor?.DisplayName,
                    Position = leadAssessorPosition?.Name
                };

                foreach (var question in bacChecklist.Questions)
                {
                    foreach (var answer in question.Answers)
                    {
                        if (answer.Value == "N")
                        {
                            switch (answer.Ordinal)
                            {
                                case 0:
                                    viewModel.Day1.ConformityCount++;
                                    break;
                                case 1:
                                    viewModel.Day2.ConformityCount++;
                                    break;
                                case 2:
                                    viewModel.Day3.ConformityCount++;
                                    break;
                                case 3:
                                    viewModel.Day4.ConformityCount++;
                                    break;
                                case 4:
                                    viewModel.Day5.ConformityCount++;
                                    break;
                                case 5:
                                    viewModel.Day6.ConformityCount++;
                                    break;
                                case 6:
                                    viewModel.Day7.ConformityCount++;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                viewModelList.Add(viewModel);
            }

            return PartialView("BarrierAuthorityPendingReviewPartial", viewModelList);
        }

        public ActionResult GetOpenSWAs()
        {
            var openSwaList = swaService.GetByStatus("Pending Verification");

            return PartialView("SWAGridPartial", openSwaList);
        }

        public ActionResult GetOpenJobs()
        {
            IEnumerable<Irma.Data.vw_Job> jobs = jobService.GetByStatus("Pending");
            return PartialView("JobGridPartial", jobs);
        }

        public ActionResult GetOpenWIs()
        {
            var wiList = wiLocalService.GetNewOrPendingWiLocals();
            return PartialView("WIGridPartial", wiList);
        }

        public ActionResult GetOpenCAPAs()
        {
            var CAPAs = capaService.GetUserCAPAs(currentOIM.Id);

            Action<Ensco.Irma.Data.IrmaCapa, CapaOIMGridViewModel> perItemAction = (capa, viewModel) => viewModel.AssignedBy = ServiceSystem.GetUser(capa.AssignedBy ?? -1)?.DisplayName;
            var viewModelList = MapToViewModel<Ensco.Irma.Data.IrmaCapa, CapaOIMGridViewModel>(CAPAs, perItemAction);

            return PartialView("CAPAGridPartial", viewModelList);
        }

        private IList<OIMDashboardOAPGridViewModel> GenerateChecklistViewModelList(ObservableCollection<RigOapChecklist> checklistList)
        {
            IList<OIMDashboardOAPGridViewModel> viewModelList = new List<OIMDashboardOAPGridViewModel>();


            foreach (var checklist in checklistList)
            {
                var leadAssessor = checklist.Assessors?.FirstOrDefault(a => a.IsLead);
                UserModel assessor = null;
                PositionModel leadAssessorPosition = null;

                if (leadAssessor != null && leadAssessor.UserId != 0)
                {
                    assessor = ServiceSystem.GetUser(leadAssessor.UserId);
                    leadAssessorPosition = ServiceSystem.GetUserPosition(leadAssessor.UserId);
                }
                    

                int nonConformityCount = 0;
                foreach (var question in checklist.Questions)
                {
                    nonConformityCount += question.Answers.Count(a => a.Value == "N");
                }

                var viewModel = new OIMDashboardOAPGridViewModel()
                {
                    Id = checklist.RigChecklistUniqueId,
                    ChecklistGuid = checklist.Id,
                    Assessor = assessor?.DisplayName,
                    Position = leadAssessorPosition?.Name,
                    Title = checklist.Title,
                    Date = checklist.ChecklistDateTime,
                    Location = checklist.LocationName,
                    NonConformity = nonConformityCount
                };
                viewModelList.Add(viewModel);
            }

            return viewModelList;
        }

    }

    public class CapaOIMGridViewModel
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public string Criticality { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public string ActionRequired { get; set; }
    }

    public class OIMDashboardOAPGridViewModel
    {
        public Guid ChecklistGuid { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Assessor { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public int NonConformity { get; set; }
        public string ChecklistType { get; set; }        
    }

    public class OIMDashboardBACGridViewModel : OIMDashboardOAPGridViewModel
    {
        public DailyReport Day1 { get; set; } = new DailyReport();
        public DailyReport Day2 { get; set; } = new DailyReport();
        public DailyReport Day3 { get; set; } = new DailyReport();
        public DailyReport Day4 { get; set; } = new DailyReport();
        public DailyReport Day5 { get; set; } = new DailyReport();
        public DailyReport Day6 { get; set; } = new DailyReport();
        public DailyReport Day7 { get; set; } = new DailyReport();
    }

    public class DailyReport
    {
        public bool Conformity { get; set; }
        public int ConformityCount { get; set; }
    }
}
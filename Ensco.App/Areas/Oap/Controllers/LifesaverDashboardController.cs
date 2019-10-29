using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Rig;
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
    using System.Web.Routing;
    using System.Web;
    using Ensco.Models;
    using System.Linq;
    using Ensco.App.App_Start;
    using System.Web.Mvc.Html;
    using Ensco.Utilities;
    using System.Web.UI.WebControls;
    using Ensco.OAP.Models.ViewModels;
    using System.Web.UI;
    using Ensco.App.Areas.Oap.Models;

    public class LifesaverDashboardController : GenericDashboardController
    {
        public LifesaverDashboardController() : base()
        {
            GridDataChecklist.Title = "Open Lifesaver Checklists";
            ChecklistType = "LS";            
        }        

        [Route("LS")]
        public override ActionResult Index()
        {   
            ViewBag.Title = "Life Saver Dashboard";
            return View("LifesaverDashboard");
        }

        public override ActionResult RigChecklists()
        {
            var lsOapChecklist = GetClient<OapChecklistClient>().GetByNameAsync("Life Savers").Result?.Result?.Data;
            if (lsOapChecklist == null) throw new Exception("Could not find any Life Savers OAP Checklist");
            ViewBag.OapChecklistId = lsOapChecklist.Id;

            var data = GetRigChecklistData(true).OrderByDescending(c => c.RigChecklistUniqueId);
            var modelList = MapToViewModel(data, perItemAction: mappingAction);

            return PartialView("LifesaverDashboardPartial", modelList);
            
        }        

        protected override IEnumerable<OapChecklist> GetChecklistData(bool useTypeSubTypeCode = false)
        {
            return base.GetChecklistData(true);
        }                

        public IEnumerable<LifesaverComplianceViewModel> GetLifesaverCompliance()
        {
            var singleRowList = new List<LifesaverComplianceViewModel>();           

            var lifesaverViewModel = PrepareComplianceViewModel(RigOapChecklistClient.GetLifesaverComplianceAsync().Result?.Result?.Data);
            singleRowList.Add(lifesaverViewModel);

            return singleRowList;
        }     

        public ActionResult DisplayLifeSaverScheduleAbdJobs()
        {            
            return PartialView("LifesaverScheduleAndJobsPartial", GetLifesaverCompliance());
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> AddScheduledChecklist(int numberOfDays, DateTime StartingAt, string title, string description)
        {
            if (StartingAt != null && numberOfDays > 0)
            {
                var lifeSaverChecklist = OapChecklistClient.GetByNameAsync("Life Savers").Result?.Result?.Data;
                if (lifeSaverChecklist == null) throw new Exception("The life savers checklist was not found. Please contact the development support team");

                for (int i = 0; i < numberOfDays; i++)
                {
                    var checklistDate = StartingAt.AddDays(i);
                    RigOapChecklist checklist = new RigOapChecklist() {
                        Status = "Open",
                        ChecklistDateTime = checklistDate,
                        OapchecklistId = lifeSaverChecklist.Id,
                        IsAutoScheduled = true,
                        //RigId = UtilitySystem.Settings.RigId.ToString(),
                        Title = title,
                        Description = description,
                        CreatedDateTime = DateTime.UtcNow
                    };                    

                    await RigOapChecklistClient.AddRigChecklistAsync(checklist);
                }
            }
            return DisplayLifeSaverScheduleAbdJobs();
        }

        private Action<RigOapChecklist, LifeSaverDashboardGridModel> mappingAction = (checklist, viewModel) => {
            var leadAssessor = checklist.Assessors.FirstOrDefault(a => a.IsLead);
            var position = string.Empty;
            if (leadAssessor != null)
                position = Services.ServiceSystem.GetUserPosition(leadAssessor.UserId)?.Name;

            viewModel.LeadAssessor = leadAssessor?.Name;
            viewModel.Position = position;
            viewModel.Location = Services.ServiceSystem.GetLocation(checklist.LocationId).Name;
        };

        private LifesaverComplianceViewModel PrepareComplianceViewModel(LifesaverCompliance data)
        {
            if (data == null)
                return new LifesaverComplianceViewModel();

            var viewModel = new LifesaverComplianceViewModel();

            viewModel.CM = data.CM == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastCMDate.HasValue)
                viewModel.CM += string.Format(" (Last on: {0})", data.LastCMDate.Value.ToString("dd-MMM-yyyy"));

            viewModel.CSE = data.CSE == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastCSEDate.HasValue)
                viewModel.CSE += string.Format(" (Last on: {0})", data.LastCSEDate.Value.ToString("dd-MMM-yyyy"));

            viewModel.DO = data.DO == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastDODate.HasValue)
                viewModel.DO += string.Format(" (Last on: {0})", data.LastDODate.Value.ToString("dd-MMM-yyyy"));

            viewModel.EI = data.EI == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastEIDate.HasValue)
                viewModel.EI += string.Format(" (Last on: {0})", data.LastEIDate.Value.ToString("dd-MMM-yyyy"));

            viewModel.JSA = data.JSA == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastJSADate.HasValue)
                viewModel.JSA += string.Format(" (Last on: {0})", data.LastJSADate.Value.ToString("dd-MMM-yyyy"));

            viewModel.LO = data.LO == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastLODate.HasValue)
                viewModel.LO += string.Format(" (Last on: {0})", data.LastLODate.Value.ToString("dd-MMM-yyyy"));

            viewModel.PTW = data.PTW == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastPTWDate.HasValue)
                viewModel.PTW += string.Format(" (Last on: {0})", data.LastPTWDate.Value.ToString("dd-MMM-yyyy"));

            viewModel.PS = data.PS == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastPSDate.HasValue)
                viewModel.PS += string.Format(" (Last on: {0})", data.LastPSDate.Value.ToString("dd-MMM-yyyy"));

            viewModel.SWA = data.SWA == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastSWADate.HasValue)
                viewModel.SWA += string.Format(" (Last on: {0})", data.LastSWADate.Value.ToString("dd-MMM-yyyy"));

            viewModel.WH = data.WH == true ? "<img src=\"/images/BlueTick.png\" /><br />" : "<img src=\"/images/No.png\" /><br />";
            if (data.LastWHDate.HasValue)
                viewModel.WH += string.Format(" (Last on: {0})", data.LastWHDate.Value.ToString("dd-MMM-yyyy"));


            return viewModel;

        }
    }
}
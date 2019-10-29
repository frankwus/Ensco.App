using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    using DevExpress.Web;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
    using Ensco.Irma.Oap.WebClient.Rig;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BarrierAuthorityDashboardController : GenericDashboardController
    {
        public BarrierAuthorityDashboardController() : base()
        {
            ChecklistType = "BAC";
            GridDataChecklist = new GridData(oapChecklistGridName,  "BarrierAuthorityDashboard", "RigChecklists", "Open Barrier Authority Checklists", "AddNewRigChecklist", "Add Checklist", "search checklists", initializeCallBack: true)
            {
                EditController = "BarrierAuthority"
            }; 
        }

        [Route("BA")]
        public override ActionResult Index()
        {            
            return View("BarrierAuthorityDashboard");
        }

        public override ActionResult RigChecklists()
        {
            var data = GetRigChecklistData(true);

            var bacOapChecklists = GetClient<OapChecklistClient>().GetByTypeNameAsync("Barrier Authority Checklist").Result?.Result?.Data;
            ViewBag.ChecklistTypes = bacOapChecklists.Select(c => new { Id = c.Id, Name = c.Name });
            return PartialView("BarrierAuthorityDashboardPartial", data);
            
        }

        [HttpPost]
        public async Task<ActionResult> AddChecklist(RigOapChecklist model)
        {
            //model.RigId = OapUtilities.GetRigId();
            model.CreatedDateTime = DateTime.UtcNow;

            var response = await RigOapChecklistClient.AddRigChecklistAsync(model);

            return RigChecklists();
        }


        protected override IEnumerable<OapChecklist> GetChecklistData(bool useTypeSubTypeCode = false)
        {
            return base.GetChecklistData(true);
        }


        protected override void InitializeRigChecklistGridData(HtmlHelper html, ViewContext viewContext, GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            base.InitializeRigChecklistGridData(html, viewContext, gridData, createAction, updateAction, deleteAction);

            //var locationLookup = CommonUtilities.GetLookup<LocationModel>("Location");
            //var locationCombo = new GridCombo("Location", locationLookup);

          
            //gridData.DisplayColumns.Add(new GridDisplayColumn("LocationId", displayName: "Location", order: 25, columnType:MVCxGridViewColumnType.ComboBox, lookup: locationCombo, width: 15, editLayoutWidth: 30));


            //gridData.DisplayColumns.Add(new GridDisplayColumn("", displayName: "Assessor Position", order: 26, width: 25, editLayoutWidth: 30));
            gridData.DisplayColumns.Add(new GridDisplayColumn("IsAutoScheduled", displayName: "Auto Schedule", order: 55, columnType: MVCxGridViewColumnType.CheckBox, width: 25, editLayoutWidth: 30));


            var columnAssessorName = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("OwnerId"));

            if (columnAssessorName != null)
            {
                columnAssessorName.DisplayName = "Assessor Name";               
            }

            var columnChecklistDateTime = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("ChecklistDateTime"));

            if (columnChecklistDateTime != null)
            {                
                columnChecklistDateTime.DisplayName = "Date";
                columnChecklistDateTime.Width = 20;
                columnChecklistDateTime.Order = 24;
            }

            var columnDescription = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("Description"));

            if (columnDescription != null)
            {
                columnDescription.IsVisible = false;
                columnDescription.Width = 0;            
                columnDescription.Order = 1099;
            }

            var columnOapchecklist = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("OapchecklistId"));

            if (columnOapchecklist != null)
            {
                columnOapchecklist.IsVisible = false;
                columnOapchecklist.Width = 0;
                columnOapchecklist.Order = 1100;
            }
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            base.Configure(settings, html, viewContext);

            settings.CommandButtonInitialize = (sender, e) =>
            {

                MVCxGridView gridView = (MVCxGridView) sender;

                bool.TryParse(gridView.GetRowValues(e.VisibleIndex, "IsAutoScheduled")?.ToString(), out bool isAutoScheduled); 

                if (e.ButtonType == ColumnCommandButtonType.Delete && isAutoScheduled)
                {
                    e.Visible = false;
                }
            };
        }
    }
}
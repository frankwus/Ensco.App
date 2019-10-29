using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using Ensco.Irma.Oap.Web.Oap.Controllers;

    public class OapAuditPlanController : OapCorpGridController
    {

        private OapChecklistClient OapChecklistClient { get; }

        private OapHierarchyClient OapHierarchyClient { get; }
        public OapAuditPlanController() : base()
        {
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);

            OapHierarchyClient = new OapHierarchyClient(GetApiBaseUrl(), Client);           
        }























        


        public ActionResult Index()
        {
            return View();
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            //Master Grid Specific configurations         
            settings.Name = "OapAuditPlanGrid";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;                  

            settings.CommandColumn.Visible = false;
        }

        #region Summary Details
        public void ConfigureSummaryDetails(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeSummaryGridData(html);
            settings.Configure(gridData, html, viewContext);
            settings.CommandColumn.Visible = false;
        }
        private GridData InitializeSummaryGridData(HtmlHelper htmlHelper)
        {
            var action = "DisplaySummaryDetails";          

            var gridData = new GridData("oapAuditPlanSummaryGrid", "OapAuditPlan", action, "Summary Details", "AddSummary", "Add", "search results", initializeCallBack: true, historicalRow: false);

            gridData.ToolBarOptions.DisplayCustomAddNew = false;
            gridData.AddRemoveCustomAddNew();
            gridData.ToolBarOptions.DisplayXlsExport = true;
           
            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("BUId", displayName:"BU", width:10),
                new GridDisplayColumn("RigId", displayName:"Rig",width:10),
                new GridDisplayColumn("Protocol", displayName:"Protocol",width:10),
                new GridDisplayColumn("ProtocolCompleteCount", displayName:"Complete",width:20),
                new GridDisplayColumn("ProtocolCompletePercentage", displayName:"%",width:10),
                new GridDisplayColumn("Findings", displayName:"Findings",width:10),
                new GridDisplayColumn("FindingsCompleteCount", displayName:"Complete",width:10),
                new GridDisplayColumn("FindingsCompletePercentage", displayName:"%",width:10),
                new GridDisplayColumn("Capa", displayName:"CAPA",width:10),
                new GridDisplayColumn("CapaCompleteCount", displayName:"Complete",width:10),
                new GridDisplayColumn("CapaCompletePercentage", displayName:"%",width:10),
            };

            gridData.DisplayColumns = displayColumns;

            return gridData;
        }
        public ActionResult DisplaySummaryDetails()
        {
            return PartialView("OapSummaryDetailsPartial",null);
        }
        #endregion

        #region Protocols
        public void ConfigureProtocols(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeProtocolsGridData(html);
            settings.Configure(gridData, html, viewContext);
            settings.CommandColumn.Visible = false;
        }
        private GridData InitializeProtocolsGridData(HtmlHelper htmlHelper)
        {
            var action = "DisplayProtocols";

            var gridData = new GridData("oapAuditPlanProtocolsGrid", "OapAuditPlan", action, "Protocols", "AddProtocols", "Add", "search results", initializeCallBack: true, historicalRow: false);

            gridData.ToolBarOptions.DisplayCustomAddNew = false;
            gridData.AddRemoveCustomAddNew();
            gridData.ToolBarOptions.DisplayXlsExport = true;

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("BUId", displayName:"BU", width:10),
                new GridDisplayColumn("RigId", displayName:"Rig",width:10),
                new GridDisplayColumn("Id", displayName:"ID",width:10),
                new GridDisplayColumn("Protocol", displayName:"Protocol",width:10),
                new GridDisplayColumn("OapCategory", displayName:"OAP Category",width:20),
                new GridDisplayColumn("OapLevel", displayName:"OAP Level",width:10),
                new GridDisplayColumn("OwnerId", displayName:"Assessor",width:10),
                new GridDisplayColumn("DueDate", displayName:"Due Date",width:10),
                new GridDisplayColumn("DateCompleted", displayName:"Date Completed",width:10),
                new GridDisplayColumn("Findings", displayName:"Findings",width:10),
                new GridDisplayColumn("Status", displayName:"Status",width:10)                
            };

            gridData.DisplayColumns = displayColumns;

            return gridData;
        }
        public ActionResult DisplayProtocols()
        {
            return PartialView("OapProtocolPartial",null);
        }

        #endregion

        #region Regulatory Audit Compliance Protocols
        public void ConfigureRegulatoryAuditComplianceProtocols(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeRegulatoryAuditComplianceProtocolsGridData(html);         
            settings.Configure(gridData, html, viewContext);
            settings.CommandColumn.Visible = false;
        }
        private GridData InitializeRegulatoryAuditComplianceProtocolsGridData(HtmlHelper htmlHelper)
        {
            var action = "DisplayRACProtocols";

            var gridData = new GridData("oapAuditPlanRACProtocolsGrid", "OapAuditPlan", action, "Protocols", "AddProtocols", "Add", "search results", initializeCallBack: true, historicalRow: false);

            gridData.ToolBarOptions.DisplayCustomAddNew = false;
            gridData.AddRemoveCustomAddNew();
            gridData.ToolBarOptions.DisplayXlsExport = true;

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("BUId", displayName:"BU", width:10),
                new GridDisplayColumn("RigId", displayName:"Rig",width:10),
                new GridDisplayColumn("AuditType", displayName:"Regulatory Audit Type",width:10),
                new GridDisplayColumn("Element", displayName:"Element / Criteria",width:10),
                new GridDisplayColumn("ProtocolId", displayName:"Protocol ID",width:20),
                new GridDisplayColumn("Protocol", displayName:"Protocol",width:10),
                new GridDisplayColumn("OapLevel", displayName:"OAP Level",width:10),                
                new GridDisplayColumn("DateCompleted", displayName:"Date Completed",width:10),
                new GridDisplayColumn("TotalFindings", displayName:"Total Findings",width:10),
                new GridDisplayColumn("OpenFindings", displayName:"Open Findings",width:10),
                new GridDisplayColumn("TotalCapa", displayName:"Total CAPA",width:10),
                new GridDisplayColumn("OpenCapa", displayName:"Open CAPA",width:10)              
            };

            gridData.DisplayColumns = displayColumns;

            return gridData;
        }
        public ActionResult DisplayRACProtocols()
        {
            return PartialView("OapRegulatoryAuditProtocolsPartial", null);
        }

        #endregion

        #region Regulatory Audit Compliance Missing Protocols
        public void ConfigureRegulatoryAuditComplianceMissingProtocols(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeRegulatoryAuditComplianceMissingProtocolsGridData(html);
            settings.Configure(gridData, html, viewContext);
            settings.CommandColumn.Visible = false;
        }
        private GridData InitializeRegulatoryAuditComplianceMissingProtocolsGridData(HtmlHelper htmlHelper)
        {
            var action = "DisplayRACMissingProtocols";

            var gridData = new GridData("oapAuditPlanRACMissingProtocolsGrid", "OapAuditPlan", action, "Missing Protocols", "AddProtocols", "Add", "search results", initializeCallBack: true, historicalRow: false);

            gridData.ToolBarOptions.DisplayCustomAddNew = false;
            gridData.AddRemoveCustomAddNew();
            gridData.ToolBarOptions.DisplayXlsExport = true;

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("BUId", displayName:"BU", width:10),
                new GridDisplayColumn("RigId", displayName:"Rig",width:10),
                new GridDisplayColumn("AuditType", displayName:"Regulatory Audit Type",width:10),
                new GridDisplayColumn("Element", displayName:"Element / Criteria",width:10),
                new GridDisplayColumn("ProtocolId", displayName:"Protocol ID",width:20),
                new GridDisplayColumn("Protocol", displayName:"Protocol",width:10),
                new GridDisplayColumn("OapLevel", displayName:"OAP Level",width:10),
                new GridDisplayColumn("DateCompleted", displayName:"Date Completed",width:10),
                new GridDisplayColumn("TotalFindings", displayName:"Total Findings",width:10),
                new GridDisplayColumn("OpenFindings", displayName:"Open Findings",width:10),
                new GridDisplayColumn("TotalCapa", displayName:"Total CAPA",width:10),
                new GridDisplayColumn("OpenCapa", displayName:"Open CAPA",width:10)
            };

            gridData.DisplayColumns = displayColumns;

            return gridData;
        }
        public ActionResult DisplayRACMissingProtocols()
        {
            return PartialView("OapRegulatoryAuditMissingProtocolsPartial", null);
        }

        #endregion

        #region CVT
        public void ConfigureCVTCompliance(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeCVTComplianceGridData(html);
            settings.Configure(gridData, html, viewContext);
            settings.CommandColumn.Visible = false;
        }
        private GridData InitializeCVTComplianceGridData(HtmlHelper htmlHelper)
        {
            var action = "DisplayCVTCompliance";

            var gridData = new GridData("oapAuditPlanCVTGrid", "OapAuditPlan", action, "Compliance", "AddProtocols", "Add", "search results", initializeCallBack: true, historicalRow: false);

            gridData.ToolBarOptions.DisplayCustomAddNew = false;
            gridData.AddRemoveCustomAddNew();
            gridData.ToolBarOptions.DisplayXlsExport = true;

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("BUId", displayName:"BU", width:10),
                new GridDisplayColumn("RigId", displayName:"Rig",width:10),
                new GridDisplayColumn("LastCompleted", displayName:"Last Completed",width:10),
                new GridDisplayColumn("DueDate", displayName:"Next Due Date",width:10),               
                new GridDisplayColumn("NextSchedule", displayName:"Next Schedule",width:10),
                new GridDisplayColumn("Compliant", displayName:"Compliant",width:10)
            };

            gridData.DisplayColumns = displayColumns;

            return gridData;
        }
        public ActionResult DisplayCVTCompliance()
        {
            return PartialView("OapCVTCompliancePartial", null);
        }

        #endregion
                                    
    }
}

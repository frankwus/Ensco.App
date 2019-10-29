using Ensco.Irma.Oap.Common.Models;
using Ensco.App.Areas.Oap.Controllers;
using Ensco.Irma.Oap.WebClient.Corp;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web.Mvc;
using Ensco.Irma.Oap.Common.Extensions;
using DevExpress.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Routing;
using System;
using DevExpress.Web.Mvc.UI;
using System.Drawing;
using Ensco.Utilities;
using System.Collections.ObjectModel;

namespace Ensco.App.Areas.cOap.Controllers
{
    public class OapProtocolsToAuditController : GenericDashboardController
    {
        protected override string ApiBaseUrlName { get; set; } = "CorpWebApiUrl";
        protected new OapChecklistClient OapChecklistClient { get; }
        protected OapAuditClient OapAuditClient { get; }
        private OapAuditClient OapAuditsClient { get; }

        public OapProtocolsToAuditController() : base()
        {
            OapAuditClient = new OapAuditClient(GetApiBaseUrl(), Client);

            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);

            OapAuditsClient = new OapAuditClient(GetApiBaseUrl(), Client);

            GridDataChecklist.Title = "Open Protocol Checklists";
            GridDataChecklist = new GridData("OapProtocolsGrid", "OapProtocolsToAudit", "RigChecklists", "Protocols to Audit", "AddNewProtocol", "Add Protocol", "search", initializeCallBack: true);
        }

        protected virtual IEnumerable<OapChecklist> GetProtocols()
        {
            var protocols = OapChecklistClient.GetAllAuditProtocolsAsync().Result?.Result?.Data;
            return protocols;
        }

        protected virtual IEnumerable<RigOapChecklist> GetCorpProtocolData()
        {
            var audit = (OapAudit)Session["OapAudit"];
            int auditId = audit.Id;
            int rigId = audit.RigId;                    
            var rigChecklistData = audit.OapAuditProtocols?.Select(p => p?.RigOapChecklist).ToList().Where(p => p.OapChecklist.OapType.Code != "AR");                
            return rigChecklistData;
        }

        public override ActionResult RigChecklists()
        {
            return TryCatchCollectionDisplayPartialView("ProtocolsToAuditPartial", "GenericDashBoardErrorsKey", () => GetCorpProtocolData());

            //return PartialView("ProtocolsToAuditPartial", GetCorpProtocolData());
        }

        public ActionResult DisplayProtocols(int aId)
        {
            ViewData["SelectedAuditId"] = aId;
            var rlist = new List<RigOapChecklist>();
            if (!(bool)Session["isAuditIdClick"])
            {
                var protocols = OapAuditClient.GetAuditProtocolsAsync(aId).Result?.Result?.Data;
                var ap = protocols?.Where(r => r.OapChecklist?.OapType?.Code == "AR").FirstOrDefault();
                protocols.Remove(ap);

                Session["isAuditIdClick"] = false;
                return PartialView("ProtocolsToAuditPartial", protocols); 
            }
            return PartialView("ProtocolsToAuditPartial", rlist);
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            InitializeRigChecklistGridData(html, viewContext, GridDataChecklist, "CreateProtocol", "UpdateProtocol", "DeleteProtocol");

            //Master Grid Specific configurations
            settings.KeyFieldName = GridDataChecklist.Key;
            settings.Name = $"{GridDataChecklist.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            if (!(bool)Session["isAuditIdClick"])
                GridDataChecklist.CallBackRoute = new { Area = "cOap", Controller = "OapProtocolsToAudit", Action = "DisplayProtocols", aId = ViewData["SelectedAuditId"] };

            if ((bool)Session["isAuditIdClick"])
                settings.ClientSideEvents.EndCallback = "function (s,e){ refreshProtocolAssessorGrid(s,e);}";

            //Generic configurations
            GridDataChecklist.Title = Translate(GridDataChecklist.Title);
            settings.Configure(GridDataChecklist, html, viewContext);
            settings.CommandColumn.Visible = false;

            // Popup form customization
            settings.SettingsPopup.EditForm.Modal = true;
            settings.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.Center;
            settings.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            settings.SettingsPopup.EditForm.CloseOnEscape = AutoBoolean.True;
            settings.SettingsText.PopupEditFormCaption = "Add Protocols To Audit";

            settings.CellEditorInitialize = (s, e) =>
            {
                e.Editor.ReadOnly = false;
            };

            settings.SettingsExport.BeforeExport = (sender, e) =>
            {
                MVCxGridView gridView = sender as MVCxGridView;
                if (sender == null)
                    return;
                gridView.Columns["OwnerId"].Visible = false;
            };
        }

        protected override void InitializeRigChecklistGridData(HtmlHelper html, ViewContext viewContext, GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.EditController = "OapProtocol";
            base.InitializeRigChecklistGridData(html, viewContext, gridData, createAction, updateAction, deleteAction);

            if (ViewData["Protocols"] == null)
            {
                ViewData["Protocols"] = GetProtocols();                 
            }

            var protocols = ViewData["Protocols"] as IEnumerable<OapChecklist>;
            
            var oapProtocolsCombo = new GridCombo("OapChecklist", protocols);
                  
            //Remove Display Columns
            var columnId = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("Id"));
            gridData.DisplayColumns.Remove(columnId);

            var columnTitle = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("Title"));
            gridData.DisplayColumns.Remove(columnTitle);

            var columnProtocols = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("OapchecklistId"));
            gridData.DisplayColumns.Remove(columnProtocols);

            var columnDescription = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("Description"));
            gridData.DisplayColumns.Remove(columnDescription);

            var columnPosition = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("PositionId"));
            gridData.DisplayColumns.Remove(columnPosition);

            var columnChecklistDateTime = gridData.DisplayColumns.ToList().FirstOrDefault(c => c.Name.Equals("ChecklistDateTime"));
            gridData.DisplayColumns.Remove(columnChecklistDateTime);

            //Add Display Columns
            gridData.DisplayColumns.Insert(1,
                                new GridDisplayColumn("Id", displayName: "Checklist Id", order: 10, columnAction: c =>
                                {
                                    c.FieldName = "Id";
                                    c.Caption = Translate("Checklist Id");
                                    c.Width = Unit.Percentage(5);
                                    c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    c.EditorProperties().HyperLink(hl =>
                                        {
                                            Session["isChecklistIdClick"] = true;
                                            var url = Url.Action("List", gridData.EditController, new RouteValueDictionary(new { Id = "{0}" }));
                                            hl.NavigateUrlFormatString = HttpUtility.UrlDecode(url);
                                            hl.TextField = "RigChecklistUniqueId";
                                        }
                                    );
                                }));

            gridData.DisplayColumns.Insert(2, new GridDisplayColumn("OapchecklistId", displayName: "Protocol", order: 20, columnType: MVCxGridViewColumnType.ComboBox, lookup: oapProtocolsCombo, width: 20, editLayoutWidth: 100));

            gridData.DisplayColumns.Insert(2, new GridDisplayColumn("OapchecklistId", editLayoutWidth: 100, editLayoutHeight: 100, isVisible: false, columnAction: c =>
              {
                  c.FieldName = "OapchecklistId";
                  c.Caption = Translate("Protocol");
                  //c.Width = Unit.Percentage(20);                  
                  c.SetEditItemTemplateContent(con =>
                  {
                      html.DevExpress().DropDownEdit(dd =>
                      {
                          dd.Name = "OapChecklistId";
                          dd.Width = Unit.Percentage(100);
                          dd.Height = Unit.Percentage(100);

                          dd.SetDropDownWindowTemplateContent(cc =>
                          {
                              html.DevExpress().ListBox(listBoxSettings =>
                              {
                                  listBoxSettings.Name = "checkProtocolListBox";
                                  listBoxSettings.ControlStyle.Border.BorderWidth = 0;
                                  listBoxSettings.ControlStyle.BorderBottom.BorderWidth = 1;
                                  listBoxSettings.ControlStyle.BorderBottom.BorderColor = Color.FromArgb(0xDCDCDC);
                                  listBoxSettings.Height = Unit.Pixel(200);
                                  listBoxSettings.Width = Unit.Percentage(100);

                                  listBoxSettings.Properties.TextField = oapProtocolsCombo.DisplayColumnName;
                                  listBoxSettings.Properties.ValueField = oapProtocolsCombo.KeyColumnName;

                                  //listBoxSettings.Properties.EnableSelectAll = true;
                                  listBoxSettings.Properties.FilteringSettings.ShowSearchUI = true;
                                  listBoxSettings.Properties.SelectionMode = ListEditSelectionMode.CheckColumn;

                                  listBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = "updateText";
                                  listBoxSettings.Properties.ClientSideEvents.Init = "updateText";

                              }).BindList(oapProtocolsCombo.DataSource).GetHtml();
                          });
                          dd.Properties.AnimationType = AnimationType.None;
                          dd.Properties.ClientSideEvents.TextChanged = "synchronizeListBoxValues";
                          dd.Properties.ClientSideEvents.DropDown = "synchronizeListBoxValues";
                      }).GetHtml();                      
                  });
              })); //added for multi select of protocols
            
            gridData.DisplayColumns.Insert(3, new GridDisplayColumn("OapType", displayName: "OAP Type", order: 30, width: 20));
                  

            //Add EditLayout Columns
            var editColumnTitle = gridData.LayoutColumns.ToList().FirstOrDefault(c => c.Name.Equals("Title"));
            gridData.LayoutColumns.Remove(editColumnTitle);

            var editColumnDescription = gridData.LayoutColumns.ToList().FirstOrDefault(c => c.Name.Equals("Description"));
            gridData.LayoutColumns.Remove(editColumnDescription);

            var editColumnChecklistDateTime = gridData.LayoutColumns.ToList().FirstOrDefault(c => c.Name.Equals("ChecklistDateTime"));
            gridData.LayoutColumns.Remove(editColumnChecklistDateTime);

            var editColumnOapchecklistId = gridData.LayoutColumns.ToList().FirstOrDefault(c => c.Name.Equals("OapchecklistId"));

            if (editColumnOapchecklistId != null)
            {
                editColumnOapchecklistId.DisplayName = "Protocol Type";
            }

            gridData.FormLayout = new GridEditFormLayout(
                   gridData.LayoutColumns
                   , editMode: GridViewEditingMode.PopupEditForm
                   , processLayout: i =>
                   {
                       i.ShowUpdateButton = true;
                       i.ShowCancelButton = true;
                       i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                   }
                   , emptyLayputItemCount: 2
                );
        }    

        //Not Used
        [HttpPost]
        [ValidateInput(false)]
        public virtual async Task<ActionResult> CreateProtocolChecklist(int OapChecklistId, int OwnerId)
        {
            RigOapChecklist model = new RigOapChecklist() { OapchecklistId = OapChecklistId, OwnerId = OwnerId };

            var audit = (OapAudit)Session["OapAudit"];      

            model.RigId = audit.RigId.ToString();
            model.Title = "Audit Protocol";
            model.ChecklistDateTime = DateTime.UtcNow;

            var count = audit.OapAuditProtocols.Where(p => p.RigOapChecklist.OapchecklistId == model.OapchecklistId).Count();

            var response = (count == 0) ? await OapAuditClient.AddProtocolToAuditAsync(audit.Id, model) : throw new Exception("Protocol Already Exists");

            var protocols = OapAuditClient.GetAuditProtocolsAsync(audit.Id).Result?.Result?.Data;

            return TryCatchCollectionDisplayPartialView("ProtocolsToAuditPartial", "GenericDashBoardErrorsKey", () => protocols);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual async Task<ActionResult> CreateProtocol(string OapChecklistId, int OwnerId)
        {
            var audit = (OapAudit)Session["OapAudit"];
         
            List<RigOapChecklist> protocolList = new List<RigOapChecklist>();

            IEnumerable<OapChecklist> OapChecklist = GetProtocols();

            string[] values = OapChecklistId.Split(';');
            
            foreach (var value in values)
            {
                int checklistId = OapChecklist.Where(p => p.Name == value).FirstOrDefault().Id;
                RigOapChecklist protocol = new RigOapChecklist();
                protocol.OapchecklistId = checklistId;   
                protocol.RigId = audit.RigId.ToString();
                protocol.OwnerId = OwnerId;
                protocol.Title = "Audit Protocol by " + UtilitySystem.CurrentUserName;
                protocol.ChecklistDateTime = DateTime.UtcNow;

                var count = audit.OapAuditProtocols.Where(p => p.RigOapChecklist.OapchecklistId == protocol.OapchecklistId).Count();
                if(count == 0)
                {
                    protocolList.Add(protocol);
                }               
            }

            IEnumerable<RigOapChecklist> model = protocolList;

            var protocols = (model.Count() != 0) ? await OapAuditClient.AddProtocolsAsync(audit.Id, model) : throw new Exception("Protocol Already Exists");          

            return TryCatchCollectionDisplayPartialView("ProtocolsToAuditPartial", "GenericDashBoardErrorsKey", () => protocols?.Result?.Data);
        }
    }
}
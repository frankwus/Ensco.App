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
    using System.Collections.ObjectModel;
    using System.Linq;
    using Ensco.Utilities;
    using System.Web.UI.WebControls;
    using System.Web.UI;
    using System.Web.Mvc.Html;
    using Ensco.Models;
    using Ensco.App.App_Start;
    using Ensco.App.Infrastructure;    

    [CustomAuthorize]
    public class GenericDashboardController : OapRigGridController
    {
        protected const string oapChecklistGridName = "RigOapChecklistGrid";

        protected string RigChecklistId { get; set; } = "RigChecklistId";

        protected string RigChecklistName { get; set; } = "RigChecklist";


        public string GenericDashBoardErrorsKey { get; } = "GenericDashboardErrors";

        protected RigOapChecklistClient RigOapChecklistClient { get; }

        protected OapChecklistClient OapChecklistClient { get; }

        protected PeopleClient PeopleClient { get; }

        public IEnumerable<Person> Owner { get; set; }      
        
        protected GridData GridDataChecklist { get; set; } = new GridData(oapChecklistGridName, "GenericDashboard", "RigChecklists", "Rig Open Checklists", "AddNewRigChecklist", "Add Checklist", "search checklists", initializeCallBack: true);

        protected string ChecklistStatus { get; set; }

        public string ChecklistType { get; set; }

        protected string ChecklistSubType { get; set; }

        protected string FormType { get; set; }

        public GenericDashboardController() : base()
        {
            RigOapChecklistClient = new RigOapChecklistClient(GetApiBaseUrl(), Client);

            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);

            PeopleClient = new PeopleClient(GetApiBaseUrl(), Client);

            Owner = CommonUtilities.GetUsers(PeopleClient);

            ChecklistType = "BRC";

            ChecklistSubType = "All";

            ChecklistStatus = "Open";

            FormType = "All";
        }

        protected virtual void InitializeRigChecklistGridData(HtmlHelper html, ViewContext viewContext, GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            if (ViewData["Users"] == null)
            {
                ViewData["Users"] = CommonUtilities.GetUsers(PeopleClient);
            }

            var users = ViewData["Users"] as IEnumerable<Person>;

            var usersCombo = new GridCombo("Users", users);

            if (ViewData["Checklists"] == null)
            {
                ViewData["Checklists"] = GetChecklistData(true);
            }

            var locationLookup = OapUtilities.GetLookup<LocationModel>("Location");
            var locations = new GridCombo("Location", locationLookup);            

            if (ViewData["Checklists"] == null)
            {
                ViewData["Checklists"] = GetChecklistData(true);
            }

            var checklists = ViewData["Checklists"] as IEnumerable<OapChecklist>;

            var oapChecklistCombo = new GridCombo("OapChecklist", checklists);
            

            var lkpList = Ensco.Utilities.UtilitySystem.GetLookupList("Position");
            if (lkpList == null)
            {
                lkpList = new LookupListModel<dynamic>();
                lkpList.Items = new List<object>();
            }
            var items = (lkpList.Items as List<object>)?.Cast<PositionModel>();
            if (ViewData["Position"] == null)
            {
                ViewData["Position"] = items;
            }
            var positions = ViewData["Position"];
            var positionsCombo = new GridCombo("PositionModel", positions);

            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Id", displayName:"Checklist Id", order:10, columnAction: c => {
                                    c.FieldName = "Id";
                                    c.Caption = Translate("Checklist Id");
                                    c.Width = Unit.Percentage(5);
                                    c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    c.EditorProperties().HyperLink(hl =>
                                        {
                                            Session["isChecklistIdClick"] = true;                                             
                                            var url = Url.Action("List",  gridData.EditController, new RouteValueDictionary(new  { Id = "{0}"}));
                                            hl.NavigateUrlFormatString = HttpUtility.UrlDecode(url);
                                            hl.TextField = "RigChecklistUniqueId";                                          
                                        }
                                    );
                                }, editLayoutWidth:100, isWidthAndHeightInPercentage:false),
                                new GridDisplayColumn("Title", displayName: "Title", order:20, width:20, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName: "Description", order:30, width:20, editLayoutWidth:100, columnType: MVCxGridViewColumnType.Memo),                           
                                new GridDisplayColumn("OapchecklistId", displayName: "Checklist Type", order:40, columnType:MVCxGridViewColumnType.ComboBox, lookup:oapChecklistCombo, width:20, editLayoutWidth:100),
                                new GridDisplayColumn("OwnerName", displayName: "Lead Assessor", order:42,columnType:MVCxGridViewColumnType.TextBox, width:12),
                                new GridDisplayColumn("OwnerId", displayName: "Lead Assessor1", order:45, columnType:MVCxGridViewColumnType.TextBox, width:0, editLayoutWidth:100,
                                 columnAction: CommonUtilities.GetPassportColumnAction(html, viewContext,"OwnerId","Assessor",Translate("Lead Assessor"),users), isVisible:false),
                                new GridDisplayColumn("PositionId", displayName: "Position", columnType:MVCxGridViewColumnType.ComboBox, lookup:positionsCombo, order:50, width:12, editLayoutWidth:100),
                                new GridDisplayColumn("ChecklistDateTime", displayName: "Checklist Date", order:60, columnType:MVCxGridViewColumnType.DateEdit, width:25, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("Status", displayName: "Status", order:65, width:15, isReadOnly:true, editLayoutWidth:100),
                                new GridDisplayColumn("CreatedBy", displayName: "Created By", order:997, isReadOnly:true, width:0,isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", order:998, columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat:"g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", order:999, width:0, isReadOnly:true,isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", order:1000, columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat:"g", isVisible:false),
                                new GridDisplayColumn("RigChecklistUniqueId", displayName:"Checklist Id1", order:1001, width:0, isVisible:false),
                                //new GridDisplayColumn("SiteId", displayName:"Site Id", order:1010, width:0, isVisible:false)                               
            }; 

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Title", displayName:"Title", layoutAction: col => {
                                    col.Width = Unit.Percentage(100);
                                    col.ColumnSpan = 2;
                                }),
                                new GridEditLayoutColumn("Description", displayName:"Description", columnType: MVCxGridViewColumnType.Memo, layoutAction: col => {
                                    col.ColumnSpan = 2;
                                    col.Width = Unit.Percentage(100);                                    
                                }),
                                new GridEditLayoutColumn("OapchecklistId", displayName:"Checklist Type"),
                                new GridEditLayoutColumn("OwnerId", displayName:"Lead Assessor"),
                                new GridEditLayoutColumn("ChecklistDateTime", displayName:"Date & Time", columnType: MVCxGridViewColumnType.DateEdit),
                                new GridEditLayoutColumn("Status", displayName:"Status", isVisible: false),
                                new GridEditLayoutColumn("RigChecklistUniqueId", isVisible:false),
                                new GridEditLayoutColumn("SiteId", isVisible:false)                                 
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.PopupEditForm
                    , processLayout: i => {
                        i.ShowUpdateButton = true;
                        i.ShowCancelButton = true;
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                    }
                    , columnCount: 2
                    , emptyLayputItemCount: 3
                    );            

            gridData.RowInitializeEvent = (s, e) =>
            { 
                e.NewValues["OwnerId"] = UtilitySystem.CurrentUserId;
                e.NewValues["Status"] = "Open";
                e.NewValues["ChecklistDateTime"] = DateTime.UtcNow;
                //e.NewValues["SiteId"] = UtilitySystem.Settings.ConfigSettings["SiteId"];            
                gridData.DefaultNewRowInitializeFields(e);
            };
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            InitializeRigChecklistGridData(html, viewContext, GridDataChecklist, "CreateRigChecklist", "UpdateRigChecklist", "DeleteRigChecklist");
           
            // Popup form customization
            settings.SettingsPopup.EditForm.Modal = true;
            settings.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.Center;
            settings.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            settings.SettingsPopup.EditForm.CloseOnEscape = AutoBoolean.True;
            settings.SettingsText.PopupEditFormCaption = "Add " + RigChecklistName;

            //Master Grid Specific configurations
            settings.KeyFieldName = GridDataChecklist.Key;
            settings.Name = $"{GridDataChecklist.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            //Generic configurations
            GridDataChecklist.Title = Translate(GridDataChecklist.Title);
            settings.Configure(GridDataChecklist, html, viewContext);

            var column = settings.Columns["Description"] as MVCxGridViewColumn;
            if (column != null)
            {
                column.EditorProperties().Memo(m =>
                {
                    m.Width = Unit.Percentage(100);
                    m.Rows = 5;
                });                
            }

            settings.SettingsExport.BeforeExport = (sender, e) => {
                MVCxGridView gridView = sender as MVCxGridView;
                if (sender == null)
                    return;
                gridView.Columns["OwnerId"].Visible = false;                 
            };

            settings.CellEditorInitialize = (s, e) =>
            {
                e.Editor.ReadOnly = false;
            };

            settings.CommandColumn.Visible = false;

            SelectRow((int)(ViewData[RigChecklistId] ?? 0), (string)(ViewData[RigChecklistName] ?? string.Empty), ref settings);
        }

        protected virtual ObservableCollection<RigOapChecklist> GetRigChecklistData(bool useTypeSubTypeCode = false)
        {
            var rigChecklistData = (useTypeSubTypeCode) ? 
                                    RigOapChecklistClient.GetAllByTypeSubTypeCodeStatusAsync(ChecklistType, ChecklistSubType, ChecklistStatus).Result?.Result?.Data : 
                                    RigOapChecklistClient.GetAllByTypeSubTypeStatusAsync(ChecklistType, ChecklistSubType, ChecklistStatus).Result?.Result?.Data;
     
            return rigChecklistData;
        }


        protected virtual IEnumerable<OapChecklist> GetChecklistData(bool useTypeSubTypeFormTypeCode = false)
        {
            return CommonUtilities.GetChecklists(OapChecklistClient, ChecklistType, ChecklistSubType, FormType, useTypeSubTypeFormTypeCode);
        }

        public virtual ActionResult Index()
        {
            return TryCatchCollectionDisplayView("GenericDashboard", "GenericDashBoardErrorsKey", () => GetRigChecklistData(true));            
        }

        public virtual ActionResult RigChecklists()
        {
            return TryCatchCollectionDisplayPartialView("GenericDashboardPartial", "GenericDashBoardErrorsKey", () => GetRigChecklistData(true));           
        }

        [HttpPost]
        public virtual async Task<ActionResult> CreateRigChecklist(RigOapChecklist model)
        {
            // TODO: Add insert logic here 
            model.RigId = "0";
            var response = await RigOapChecklistClient.AddRigChecklistAsync(model);

            if (ModelState.ContainsKey("RigChecklistUniqueId")) // Handled in the API
                ModelState["RigChecklistUniqueId"].Errors.Clear();

            return RigChecklists();
        }

        [HttpPost]
        public ActionResult UpdateRigChecklist(RigOapChecklist model)
        {
            // TODO: Add update logic here
            var response = RigOapChecklistClient.UpdateRigChecklistAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRigChecklist(RigOapChecklist model)
        {
            if (model.Id != Guid.Empty)
            {
                var response = await RigOapChecklistClient.DeleteRigOapChecklistAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

    }
     
}
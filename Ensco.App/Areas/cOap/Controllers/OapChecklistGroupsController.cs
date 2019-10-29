using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.WebClient.Corp;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using System.Collections.ObjectModel;
    using System.Web.Mvc.Html;
    using System.Web.UI;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapChecklistGroupsController : OapCorpGridController
    {
        public const string CurrentGroupId = "CurrentGroupId";
        public const string CurrentGroupName = "CurrentGoupName";

        private OapChecklistGroupClient OapChecklistGroupClient { get; }
        private OapChecklistSubGroupClient OapChecklistSubGroupClient { get; }

        private OapChecklistClient OapChecklistClient { get; }

        private OapGraphicClient OapGraphicClient { get; }

        private GridData GridData { get; } = new GridData("OapChecklistGroupGrid", "OapChecklistGroups", "Groups", "Checklist Groups", "AddNewChecklistGroup", "Add Checklist Group", "search checklist groups",initializeCallBack:true);

        private GridData ChildGridData { get; } = new GridData("OapChecklistSubGroupGrid", "OapChecklistGroups", "SubGroups","Checklist Sub Groups", "AddNewChecklistSubGroup", "Add Checklist Sub Group", "search checklist sub groups");


        
        private ObservableCollection<OapChecklist> GetOapChecklists()
        {
            return OapChecklistClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private ObservableCollection<OapGraphic> GetGraphics()
        {
            return OapGraphicClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {

            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (ViewData["Checklists"] == null)
            {
                ViewData["Checklists"] = GetOapChecklists();
            }

            if (ViewData["Graphics"] == null)
            {
                ViewData["Graphics"] = GetGraphics();
            }

            var oapChecklists = ViewData["Checklists"] as ObservableCollection<OapChecklist>;
            var oapGraphics = ViewData["Graphics"] as ObservableCollection<OapGraphic>;

            var oapChecklistCombo = new GridCombo("OapChecklist", oapChecklists);

            var oapGraphicsCombo = new GridCombo("OapGraphic", oapGraphics);

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("OapChecklistId", displayName:"Checklist", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapChecklistCombo ,width:15, editLayoutWidth:100),
                                new GridDisplayColumn("OapGraphicId", displayName:"Graphic", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapGraphicsCombo ,width:15, editLayoutWidth:100),
                                new GridDisplayColumn("Name", displayName:"Group Name",width:20, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Group Description",width:30, editLayoutWidth:100),
                                new GridDisplayColumn("Order", displayName:"Order", columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100, columnAction: c=> {
                                    c.FieldName = "Order";
                                    c.Caption =  Translate("Order");
                                    c.Width = Unit.Percentage(5);
                                    c.CellStyle.HorizontalAlign = HorizontalAlign.Left;}),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),

                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false)
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("OapChecklistId", displayName:"Checklist"),
                                new GridEditLayoutColumn("OapGraphicId", displayName:"Graphic"),
                                new GridEditLayoutColumn("Name", displayName:"Group Name"),
                                new GridEditLayoutColumn("Description", displayName:"Group Description"),
                                new GridEditLayoutColumn("Order", displayName:"Order"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i => {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    }
                    , columnCount: 3
                    , emptyLayputItemCount: 1
                    );
        }

        private void InitializeChildGridData(GridData gridData, string createAction, string updateAction, string deleteAction, int id, string name)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Name", displayName:"Sub Group Name",width:15, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Sub Group Description",width:20, editLayoutWidth:100),
                                new GridDisplayColumn("Order", displayName:"Order",columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100,
                                columnAction: c=> {
                                    c.FieldName = "Order";
                                    c.Caption =  Translate("Order");
                                    c.Width = Unit.Percentage(5);
                                    c.CellStyle.HorizontalAlign = HorizontalAlign.Left;}),
                                new GridDisplayColumn("IsVisible", displayName:"Visible", columnType:MVCxGridViewColumnType.CheckBox, width:5, editLayoutWidth: 100),
                                new GridDisplayColumn("IsPlantMonitoring", displayName:"Plant Monitoring", columnType:MVCxGridViewColumnType.CheckBox, width:10, editLayoutWidth: 100),
                                new GridDisplayColumn("IsPlantMaintaining", displayName:"Plant Maintaining", columnType:MVCxGridViewColumnType.CheckBox, width:10, editLayoutWidth: 100),
                                new GridDisplayColumn("IsWorkInstructions", displayName:"Work Instructions", columnType:MVCxGridViewColumnType.CheckBox, width:10, editLayoutWidth: 100),
                                new GridDisplayColumn("IsThirdPartyActivities", displayName:"Third Party Activities", columnType:MVCxGridViewColumnType.CheckBox, width:10, editLayoutWidth: 100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),

                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:10, isReadOnly:true, isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("OapChecklistGroupId", displayName:"Checklist Group Id", width:0,isVisible:false)
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = GridData.Controller, Action = "CreateChecklistSubGroup"}),
                                new GridRoute(GridRouteTypes.Update, new { Controller = GridData.Controller, Action = "UpdateChecklistSubGroup" }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = GridData.Controller, Action = "DeleteChecklistSubGroup" }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Name", displayName:"Sub Group Name"),
                                new GridEditLayoutColumn("Description", displayName:"Sub Group Description"),
                                new GridEditLayoutColumn("Order", displayName:"Order"),
                                new GridEditLayoutColumn("IsVisible", displayName:"Visible"),
                                new GridEditLayoutColumn("IsPlantMonitoring", displayName:"Plant Monitoring"),
                                new GridEditLayoutColumn("IsPlantMaintaining", displayName:"Plant Maintaining"),
                                new GridEditLayoutColumn("IsWorkInstructions", displayName:"Work Instructions"),
                                new GridEditLayoutColumn("IsThirdPartyActivities", displayName:"Third Party Activities"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                                new GridEditLayoutColumn("OapChecklistGroupId", displayName:"Checklist Group Id",isVisible:false)
                            };

            gridData.FormLayout = new GridEditFormLayout(
                gridData.LayoutColumns
                , i => {
                    i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                    i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                }
                 , columnCount: 2
                 );

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["OapChecklistGroupId"] = id;
                gridData.DefaultNewRowInitializeFields(e);
            };


            gridData.CallBackRoute = new { Controller = gridData.Controller, Action = gridData.Action, oapChecklistGroupId = id, oapChecklistGroupName = name };
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridData, "CreateChecklistGroup", "UpdateChecklistGroup", "DeleteChecklistGroup");

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.SetDetailRowTemplateContent(c =>
            {
                var loapChecklistGroupName = (string)DataBinder.Eval(c.DataItem, GridData.DisplayColumnName);

                var loapChecklistGroupId = (int)DataBinder.Eval(c.DataItem, GridData.Key);

                ChildGridData.ToolBarOptions.SearchName = $"{ChildGridData.ToolBarOptions.SearchName}_{loapChecklistGroupId}";

                if (!c.Grid.IsEditing && !c.Grid.IsNewRowEditing)
                {
                    //Important to use RenderAction and Not Action todisplay the child grid rows
                    html.RenderAction(ChildGridData.Action, ChildGridData.Controller, new { oapChecklistGroupId = loapChecklistGroupId, oapChecklistGroupName = loapChecklistGroupName });
                }
            });

            int oapChecklistGroupId = (int)(ViewData[CurrentGroupId] ?? 0);
            string oapChecklistGroupName = (string)(ViewData[CurrentGroupName] ?? string.Empty);

            //Select the Current Row when it defined.
            if (oapChecklistGroupId != 0)
            {
                settings.Columns.Grid.Selection.SelectRowByKey(oapChecklistGroupId);
            }
        }

        public void ConfigureChild(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            int oapChecklistGroupId = (int)(ViewData[CurrentGroupId] ?? 0);
            string oapChecklistGroupName = (string)(ViewData[CurrentGroupName] ?? string.Empty);

            ChildGridData.ToolBarOptions.SearchName = $"{ChildGridData.ToolBarOptions.SearchName}_{oapChecklistGroupId}";

            InitializeChildGridData(ChildGridData, "CreateChecklistSubGroup", "UpdateChecklistSubGroup", "DeleteChecklistSubGroup", oapChecklistGroupId, oapChecklistGroupName);

            ChildGridData.Name = $"{ChildGridData.Name}_{oapChecklistGroupName}_{oapChecklistGroupId}";
            //very sensitive to characters only use Unserscore.  using anyother charaters other than alphabets could cause grid display issues.
            settings.SettingsDetail.MasterGridName = $"{GridData.Name}"; // GridInformation.MasterGridName;

            settings.Configure(ChildGridData, html, viewContext);
        }

        public OapChecklistGroupsController():base()
        {
            OapChecklistGroupClient = new OapChecklistGroupClient(GetApiBaseUrl(), Client);
            OapChecklistSubGroupClient = new OapChecklistSubGroupClient(GetApiBaseUrl(), Client);
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);
            OapGraphicClient = new OapGraphicClient(GetApiBaseUrl(), Client);
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapChecklistGroupClient.GetAllAsync(GetAllModelsCorp());
            return View("OapChecklistGroup", response.Result.Data);
        }

        public async Task<ActionResult> Groups()
        {
            var response = await OapChecklistGroupClient.GetAllAsync(GetAllModelsCorp());

            return PartialView("OapChecklistGroupPartial", response.Result.Data);
        }

        public ActionResult SubGroups(int? oapChecklistGroupId, string oapChecklistGroupName)
        {
            if (!oapChecklistGroupId.HasValue)
            {
                return new EmptyResult();
            }

            //Set View Data
            ViewData[CurrentGroupId] = oapChecklistGroupId.Value;
            ViewData[CurrentGroupName] = oapChecklistGroupName;

            var response = OapChecklistGroupClient.GetAsync(oapChecklistGroupId ?? 0).Result;

            return PartialView("OapChecklistSubGroupPartial", response.Result.Data.SubGroups);
        }


        [HttpPost]
        public async Task<ActionResult> CreateChecklistGroup(OapChecklistGroup model)
        {            
            // TODO: Add insert logic here
            var response = await OapChecklistGroupClient.AddChecklistGroupAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklistGroup(OapChecklistGroup model)
        {           
            // TODO: Add update logic here
            var response = OapChecklistGroupClient.UpdateChecklistGroupAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistGroup(OapChecklistGroup model)
        {
            if (model.Id > 0)
            {
                var response = await OapChecklistGroupClient.DeleteChecklistGroupAsync(model.Id);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> CreateChecklistSubGroup(OapChecklistSubGroup model)
        {
            if (model.OapChecklistGroupId > 0)
            {
                ViewData[CurrentGroupId] = model.OapChecklistGroupId;               
                var response = await OapChecklistSubGroupClient.AddChecklistSubGroupAsync(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklistSubGroup(OapChecklistSubGroup model)
        {           
            // TODO: Add update logic here
            var response = OapChecklistSubGroupClient.UpdateChecklistSubGroupAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistSubGroup(OapChecklistSubGroup model)
        {
            if (model.Id > 0)
            {
                var response = await OapChecklistSubGroupClient.DeleteChecklistSubGroupAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        ~OapChecklistGroupsController()
        {
            Client.Dispose();
        }
    }
}

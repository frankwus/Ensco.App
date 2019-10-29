using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.WebClient.Corp;
    using DevExpress.Web.Mvc;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.Common.Extensions;
    using DevExpress.Web;
    using System.Threading;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.Mvc.Html;
    using System.Collections.ObjectModel;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapTypesController : OapCorpGridController
    {
        public const string CurrentTypeId = "CurrentTypeId";
        public const string CurrentTypeName = "CurrentTypeName";

        private OapHierarchyClient OapHierarchyClient { get; }

        private OapChecklistLayoutClient OapChecklistLayoutClient { get; }

        public GridData GridData { get; } = new GridData("OapTypesMasterGrid", "OapTypes", "OapTypes", "OAP Type", "AddNewOapType", "Add OAP Type", "search oap types", initializeCallBack: true);

        public GridData ChildGridData { get; } = new GridData("OapTypesDetailsGrid", "OapTypes", "OapSubType", "OAP Sub Type", "AddNewOapSubType", "Add OAP Sub Type", "search oap sub types");


        private ObservableCollection<OapChecklistLayout> GetOapChecklistLayouts()
        {
            return OapChecklistLayoutClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (ViewData["ChecklistLayouts"] == null)
            {
                ViewData["ChecklistLayouts"] = GetOapChecklistLayouts();
            }

            var oapChecklistLayouts = ViewData["ChecklistLayouts"] as ObservableCollection<OapChecklistLayout>;

            var oapChecklistLayoutCombo = new GridCombo("OapChecklistLayout", oapChecklistLayouts);

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Name", displayName:"Name", width:20, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Description", width:40, editLayoutWidth:100),
                                new GridDisplayColumn("IsProtocol", displayName:"Protocol", columnType:MVCxGridViewColumnType.CheckBox, width:5, editLayoutWidth:100),
                                new GridDisplayColumn("ChecklistLayoutId", displayName:"Checklist Layout", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapChecklistLayoutCombo, width:15, editLayoutWidth:100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, displayFormat: "g", editLayoutWidth:100),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, displayFormat: "g", editLayoutWidth:100),

                                new GridDisplayColumn("Code", displayName:"Code", width:0, isVisible:false),
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g",  isVisible:false)
                            };

            gridData.Buttons = new List<GridButton>()
                            {
                                new GridButton(GridButtonTypes.Add),
                                new GridButton(GridButtonTypes.Update),
                                new GridButton(GridButtonTypes.Delete),
                                new GridButton(GridButtonTypes.Cancel),
                                new GridButton(GridButtonTypes.Edit)
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Name", displayName:"Name"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("IsProtocol", displayName:"Protocol"),
                                new GridEditLayoutColumn("ChecklistLayoutId", displayName:"Checklist Layout"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                                new GridEditLayoutColumn("Code", displayName:"Code"),
                            };

            gridData.FormLayout = new GridEditFormLayout(GridData.LayoutColumns
                    , i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    },
                    columnCount: 3);  
        }

        private void InitializeChildGridData(GridData gridData, string createAction, string updateAction, string deleteAction, int id, string name)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Name", displayName:"Name", width:30, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Description", width:55, editLayoutWidth:100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7,  editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),

                                new GridDisplayColumn("Code", displayName:"Code", width:0, isVisible:false),
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("ParentHierarchyId", displayName:"Parent Hierarchy Id", width:0, isVisible:false)
                            }; 

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Name", displayName:"Name"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                                new GridEditLayoutColumn("Code", displayName:"Code"),
                                new GridEditLayoutColumn("ParentHierarchyId", displayName:"Parent Hierarchy Id",isVisible:false)
                            };

            gridData.FormLayout = new GridEditFormLayout(gridData.LayoutColumns
                    , i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    },
                    columnCount: 2);

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["ParentHierarchyId"] = id;
                gridData.DefaultNewRowInitializeFields(e);
            };

            gridData.CallBackRoute = new { Controller = gridData.Controller, Action = gridData.Action, oapTypeId = id, oapTypeName = name };
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridData, "CreateMaster", "UpdateMaster", "DeleteMaster");

            //Master Grid Specific configurations
            settings.KeyFieldName = GridData.Key;
            settings.Name = $"{GridData.Name}";
            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.SetDetailRowTemplateContent(c =>
            {
                var loapTypeName = (string)DataBinder.Eval(c.DataItem, GridData.DisplayColumnName);

                var loapTypeId = (int)DataBinder.Eval(c.DataItem, GridData.Key);

                if (!c.Grid.IsEditing && !c.Grid.IsNewRowEditing)
                {
                    ChildGridData.ToolBarOptions.SearchName = $"{ChildGridData.ToolBarOptions.SearchName}_{loapTypeId}";

                    //Important to use RenderAction and Not Action todisplay the child grid rows
                    html.RenderAction(ChildGridData.Action, ChildGridData.Controller, new { oapTypeId = loapTypeId, oapTypeName = loapTypeName });
                }
            });

            int oapTypeInputId = (int)(ViewData[CurrentTypeId] ?? 0);
            string oapTypeInputName = (string)(ViewData[CurrentTypeName] ?? string.Empty);

            //Select the Current Row when it defined.
            if (oapTypeInputId != 0)
            {
                settings.Columns.Grid.Selection.SelectRowByKey(oapTypeInputId);
            }
        }

        public void ConfigureChild(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            int oapTypeInputId = (int)(ViewData[CurrentTypeId] ?? 0);
            string oapTypeInputName = (string)(ViewData[CurrentTypeName] ?? string.Empty);

            ChildGridData.ToolBarOptions.SearchName = $"{ChildGridData.ToolBarOptions.SearchName}_{oapTypeInputId}";

            InitializeChildGridData(ChildGridData, "CreateChild", "UpdateChild", "DeleteChild", oapTypeInputId, oapTypeInputName);

            //very sensitive to characters only use Unserscore.  using anyother charaters other than alphabets could cause grid display issues.
            ChildGridData.Name = $"{ChildGridData.Name}_{oapTypeInputName}_{oapTypeInputId}";
            settings.SettingsDetail.MasterGridName = $"{GridData.Name}"; // GridInformation.MasterGridName;

            settings.Configure(ChildGridData, html, viewContext);
        }

        public OapTypesController()
        {
            OapHierarchyClient = new OapHierarchyClient(GetApiBaseUrl(), Client);
            OapChecklistLayoutClient = new OapChecklistLayoutClient(GetApiBaseUrl(), Client);
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapHierarchyClient.GetAllParentsAsync(GetAllModelsCorp());
            return View("OapTypesMaster", response.Result.Data);
        }

        public async Task<ActionResult> OapTypes()
        {
            var response = await OapHierarchyClient.GetAllParentsAsync(GetAllModelsCorp());
            return PartialView("OapTypesMasterPartial", response.Result.Data);
        }

        public ActionResult OapSubType(int? oapTypeId, string oapTypeName)
        {
            if (!oapTypeId.HasValue)
            {
                return new EmptyResult();
            }
            //Set View Data
            ViewData[CurrentTypeId] = oapTypeId.Value;
            ViewData[CurrentTypeName] = oapTypeName;

            var response = OapHierarchyClient.GetAsync(oapTypeId ?? 0).Result;
            return PartialView("OapTypesDetailsPartial", response.Result.Data.ChildrenHierarchies);
        }


        [HttpPost]
        public async Task<ActionResult> CreateMaster(OapHierarchy model)
        {
            // TODO: Add insert logic here
            var response = await OapHierarchyClient.AddHierarchyAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateMaster(OapHierarchy model)
        {
            // TODO: Add update logic here
            var response = OapHierarchyClient.UpdateHierarchyAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteMaster(OapHierarchy model)
        {
            if (model.Id > 0)
            {
                var response = await OapHierarchyClient.DeleteHierarchyAsync(model.Id);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> CreateChild(OapHierarchy model)
        {
            if (model.ParentHierarchyId > 0)
            {
                ViewData[CurrentTypeId] = model.ParentHierarchyId;               
                var response = await OapHierarchyClient.AddHierarchyAsync(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChild(OapHierarchy model)
        {             
            // TODO: Add update logic here
            var response = OapHierarchyClient.UpdateHierarchyAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChild(OapHierarchy model)
        {
            if (model.Id > 0)
            {
                var response = await OapHierarchyClient.DeleteHierarchyAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        ~OapTypesController()
        {
            Client.Dispose();
        }
    }
}



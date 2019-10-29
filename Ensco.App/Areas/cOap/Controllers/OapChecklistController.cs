using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.App.App_Start; 

    public class OapChecklistsController : OapCorpGridController
    {
        public const string CurrentChecklistId = "CurrentChecklistId";
        public const string CurrentChecklistName = "CurrentChecklistName";

        private OapChecklistClient OapChecklistClient { get; }

        private OapHierarchyClient OapHierarchyClient { get; }

        private OapProtocolFormTypeClient OapProtocolFormTypeClient { get; }

        private OapFrequencyTypeClient OapFrequencyTypeClient { get; }

        private GridData GridDataChecklist { get; } = new GridData("OapChecklistGrid", "OapChecklists", "Checklists", "Checklists", "AddNewChecklist", "Add Checklist", "search checklists", initializeCallBack: true);

        private ObservableCollection<OapHierarchy> GetOapTypes()
        {
            return OapHierarchyClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private ObservableCollection<OapProtocolFormType> GetOapProtocolFormTypes()
        {
            return OapProtocolFormTypeClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private ObservableCollection<OapFrequencyType> GetOapFrequencyTypes()
        {
            return OapFrequencyTypeClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            if (ViewData["OapTypes"] == null)
            {
                var allOapTypes = GetOapTypes();
                ViewData["OapTypes"] = allOapTypes.Where(h => h.ParentHierarchyId == null);
                ViewData["OapSubTypes"] = allOapTypes.Where(h => h.ParentHierarchyId != null);
            }

            if (ViewData["ProtocolFormTypes"] == null)
            {
                ViewData["ProtocolFormTypes"] = GetOapProtocolFormTypes();
            }

            if (ViewData["FrequencyTypes"] == null)
            {
                ViewData["FrequencyTypes"] = GetOapFrequencyTypes();
            }

            var oapTypes = ViewData["OapTypes"] as IEnumerable<OapHierarchy>;
            var oapSubTypes = ViewData["OapSubTypes"] as IEnumerable<OapHierarchy>;
            var oapProtocolFormTypes = ViewData["ProtocolFormTypes"] as ObservableCollection<OapProtocolFormType>;
            var oapFrequencyTypes = ViewData["FrequencyTypes"] as ObservableCollection<OapFrequencyType>;

            var oapTypeCombo = new GridCombo()
            {
                Name = "OapType",
                DataSource = oapTypes,
                KeyColumnName = "Id",
                DisplayColumnName = "Name",
                ValueColumnName = "Id",
                SelectedIndexChangedEvent = "OapTypeOnSelectedeChanged"
            };

            var oapSubTypeCombo = new GridCombo()
            {
                Name = "OapSubType",
                DataSource = new ObservableCollection<OapHierarchy>(),
                KeyColumnName = "Id",
                DisplayColumnName = "Name",
                ValueColumnName = "Id",
                CallbackRouteValues = new { Controller = "OapChecklists", Action = "GetOapSubTypes", Id = "OapSubTypeId" }
            };

            var oapProtocolFormTypeCombo = new GridCombo()
            {
                Name = "OapProtocolFormType",
                DataSource = oapProtocolFormTypes,
                KeyColumnName = "Id",
                DisplayColumnName = "Name",
                ValueColumnName = "Id"
            };

            var oapFrequencyTypeCombo = new GridCombo()
            {
                Name = "OapFrequencyType",
                DataSource = oapFrequencyTypes,
                KeyColumnName = "Id",
                DisplayColumnName = "Name",
                ValueColumnName = "Id"
            };

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Name", displayName:"Name", width:15, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Description", width:15, editLayoutWidth:100),
                                new GridDisplayColumn("OapTypeId", displayName:"Oap Type", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapTypeCombo, width:10, editLayoutWidth:100),
                                new GridDisplayColumn("OapSubTypeId", displayName:"Oap Sub Type", columnType:MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, columnAction:c => {
                                    c.Name = "OapSubTypeId";
                                    c.Caption = "Oap Sub Type";
                                    c.FieldName = "OapSubTypeId";
                                    c.Width = Unit.Percentage(10);
                                    c.EditorProperties().ComboBox(cb =>
                                    {
                                        cb.CallbackRouteValues = new { Controller = "OapChecklists", Action = "GetOapSubTypes", TextField="Name", ValueField = "Id" };
                                        cb.TextField = "Name";
                                        cb.ValueField = "Id";
                                        cb.DataSource = oapSubTypes;
                                        cb.ClientSideEvents.BeginCallback = "OapSubTypeBeginCallback";
                                        cb.ClientSideEvents.EndCallback = "OapSubTypeEndCallback";
                                        cb.Width = Unit.Percentage(20);
                                    });
                                }),
                                new GridDisplayColumn("OapProtocolFormTypeId", displayName:"Protocol Form Type", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapProtocolFormTypeCombo ,width:10, editLayoutWidth:100),
                                new GridDisplayColumn("OapFrequencyTypeId", displayName:"Frequency Type", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapFrequencyTypeCombo ,width:10, editLayoutWidth:100),
                                new GridDisplayColumn("Randomize", displayName:"Randomize", columnType:MVCxGridViewColumnType.CheckBox, width:7),
                                new GridDisplayColumn("ControlNumber", displayName:"Control Number", width:10, editLayoutWidth:100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),

                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible: false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible: false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible: false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible: false)
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
                                new GridEditLayoutColumn("OapTypeId", displayName:"Oap Type"),
                                new GridEditLayoutColumn("OapSubTypeId", displayName:"Oap Sub Type"),
                                new GridEditLayoutColumn("OapProtocolFormTypeId", displayName:"Oap Protocol Form Type"),
                                new GridEditLayoutColumn("OapFrequencyTypeId", displayName:"Oap Frequency Type"),
                                new GridEditLayoutColumn("Randomize", displayName:"Randomize",width:50),
                                new GridEditLayoutColumn("ControlNumber", displayName:"ControlNumber"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date", columnType: MVCxGridViewColumnType.DateEdit),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date", columnType: MVCxGridViewColumnType.DateEdit),
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i => {
                        i.ShowUpdateButton = true;
                        i.ShowCancelButton = true;
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                    }
                    , columnCount: 2
                    , emptyLayputItemCount: 1
                    );
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridDataChecklist, "CreateChecklist", "UpdateChecklist", "DeleteChecklist");

            //Master Grid Specific configurations
            settings.KeyFieldName = GridDataChecklist.Key;
            settings.Name = $"{GridDataChecklist.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.ClientSideEvents.BeginCallback = "OnBeginCallback";
            //settings.ClientSideEvents.EndCallback = "OnEndCallback";           

            //Generic configurations
            settings.Configure(GridDataChecklist, html, viewContext);

            SelectRow((int)(ViewData[CurrentChecklistId] ?? 0), (string)(ViewData[CurrentChecklistName] ?? string.Empty), ref settings);
        }

        public OapChecklistsController() : base()
        {
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);

            OapHierarchyClient = new OapHierarchyClient(GetApiBaseUrl(), Client);

            OapProtocolFormTypeClient = new OapProtocolFormTypeClient(GetApiBaseUrl(), Client);

            OapFrequencyTypeClient = new OapFrequencyTypeClient(GetApiBaseUrl(), Client);
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapChecklistClient.GetAllAsync(GetAllModelsCorp());
            return View("OapChecklist", response.Result.Data);
        }
              
        public async Task<ActionResult> Checklists()
        {
            var response = await OapChecklistClient.GetAllAsync(GetAllModelsCorp());
            return PartialView("OapChecklistPartial", response.Result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChecklist(OapChecklist model)
        {             
            // TODO: Add insert logic here
            var response = await OapChecklistClient.AddOapChecklistAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklist(OapChecklist model)
        {           
            // TODO: Add update logic here
            var response = OapChecklistClient.UpdateOapChecklistAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklist(OapChecklist model)
        {
            if (model.Id > 0)
            {
                var response = await OapChecklistClient.DeleteOapChecklistAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetOapSubTypes(int oapTypeId, string textField, string valueField)
        {
            return GridViewExtension.GetComboBoxCallbackResult(p => {
                p.TextField = textField;
                p.ValueField = valueField;
                var oapSubTypes = OapHierarchyClient.GetAsync(oapTypeId).Result.Result.Data?.ChildrenHierarchies??new ObservableCollection<OapHierarchy>(); 
                p.BindList(oapSubTypes);
            });
        }
    }
}

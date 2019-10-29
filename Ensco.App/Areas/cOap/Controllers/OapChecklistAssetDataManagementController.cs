using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using System.Collections.ObjectModel;
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapChecklistAssetDataManagementController : OapCorpGridController
    {
        public OapChecklistAssetDataManagementController() : base()
        {
            RegisterClient(new List<Type> {
                typeof(OapChecklistAssetDataManagementClient),
                typeof(OapSystemClient),
                typeof(OapSubSystemClient),
                typeof(OapSystemGroupClient),
                });
        }       

        private GridData GridData { get; } = new GridData("OapChecklistAssetDataManagementGrid", "OapChecklistAssetDataManagement", "AssetDataManagement", "OAP ADM List", "AddNewADMChecklist", "Add", "search", initializeCallBack: true);

        private ObservableCollection<OapSystemGroup> GetSystemGroups()
        {
            return GetClient<OapSystemGroupClient>().GetAllAsync().Result?.Result?.Data;
        }

        private ObservableCollection<OapSystem> GetSystems()
        {
            return GetClient<OapSystemClient>().GetAllAsync().Result?.Result?.Data;
        }

        private ObservableCollection<OapSubSystem> GetSubSystems()
        {
            return GetClient<OapSubSystemClient>().GetAllAsync().Result?.Result?.Data;
        }

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (ViewData["SystemGroup"] == null)
            {
                ViewData["SystemGroup"] = GetSystemGroups();
            }

            var oapSystemGroups = ViewData["SystemGroup"] as ObservableCollection<OapSystemGroup>;
            var oapSystemGroupsCombo = new GridCombo("OapSystemGroup", oapSystemGroups);


            if (ViewData["System"] == null)
            {
                ViewData["System"] = GetSystems();
            }
            var oapSystems = ViewData["System"] as ObservableCollection<OapSystem>;
            var oapSystemsCombo = new GridCombo("OapSystem", oapSystems);

            if (ViewData["SubSystem"] == null)
            {
                ViewData["SubSystem"] = GetSubSystems();
            }
            var oapSubSystems = ViewData["SubSystem"] as ObservableCollection<OapSubSystem>;
            var oapSubSystemsCombo = new GridCombo("OapSubSystem", oapSubSystems);

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Id", displayName:"ID", width:10, editLayoutWidth:50),
                                new GridDisplayColumn("OapChecklistGroupId", displayName:"Group Id", width:20,  editLayoutWidth:50),
                                new GridDisplayColumn("OapSystemGroupId", displayName:"System Group", width:15, columnType:MVCxGridViewColumnType.ComboBox, lookup:oapSystemGroupsCombo,  editLayoutWidth:50),
                                new GridDisplayColumn("OapSystemId", displayName:"System", width:15, columnType:MVCxGridViewColumnType.ComboBox, lookup:oapSystemsCombo,  editLayoutWidth:50),
                                new GridDisplayColumn("OapSubSystemId", displayName:"Sub System", width:15, columnType:MVCxGridViewColumnType.ComboBox, lookup:oapSubSystemsCombo,  editLayoutWidth:50),
                                new GridDisplayColumn("Order", displayName:"Order", width:5,  editLayoutWidth:50),
                                new GridDisplayColumn("Description", displayName:"Description", width:25,  editLayoutWidth:50),

                                new GridDisplayColumn("SafetyCritical", displayName:"SC",isReadOnly:true, columnType:MVCxGridViewColumnType.CheckBox, width:10),
                                new GridDisplayColumn("OperationalCritical", displayName:"OC", isReadOnly:true,columnType:MVCxGridViewColumnType.CheckBox, width:10),
                                new GridDisplayColumn("ClientCritical", displayName:"CC",isReadOnly:true, columnType:MVCxGridViewColumnType.CheckBox, width:10),
                                new GridDisplayColumn("EnvironmentalCritical", displayName:"EC", isReadOnly:true,columnType:MVCxGridViewColumnType.CheckBox, width:10),
                                new GridDisplayColumn("NonCritical", displayName:"NC",isReadOnly:true, columnType:MVCxGridViewColumnType.CheckBox, width:10),
                                new GridDisplayColumn("ACritical", displayName:"AC", isReadOnly:true,columnType:MVCxGridViewColumnType.CheckBox, width:10),
                                new GridDisplayColumn("ReferenceId", displayName:"Reference Id", isVisible:false,columnType:MVCxGridViewColumnType.TextBox)
            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {                                
                                new GridEditLayoutColumn("OapChecklistGroupId", displayName:"Group"),
                                new GridEditLayoutColumn("OapSystemGroupId", displayName:"System Group"),
                                new GridEditLayoutColumn("OapSystemId", displayName:"System"),
                                new GridEditLayoutColumn("OapSubSystemId", displayName:"Sub System"),
                                new GridEditLayoutColumn("Order", displayName:"Order"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("ReferenceId", displayName:"Reference Id")
                            };

            gridData.FormLayout = new GridEditFormLayout(GridData.LayoutColumns
                 , i => {
                     i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                     i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                 }, 2
                 , editMode: GridViewEditingMode.EditFormAndDisplayRow);


            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["ReferenceId"] = 0;
                gridData.DefaultNewRowInitializeFields(e);
            };
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridData, "CreateADMChecklist", "UpdateADMChecklist", "DeleteADMChecklist");

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.SettingsDetail.ShowDetailRow = false;

        }

        public ActionResult Index()
        {
            var assets = GetClient<OapChecklistAssetDataManagementClient>().GetAllAsync().Result.Result.Data ?? new ObservableCollection<OapAssetDataManagement>();
            return View("OapChecklistAssetDataManagement", assets);
        }


        public ActionResult AssetDataManagement()
        {            
            var assets = GetClient<OapChecklistAssetDataManagementClient>().GetAllAsync().Result.Result.Data ?? new ObservableCollection<OapAssetDataManagement>();
            return PartialView("OapChecklistAssetDataManagementPartial", assets);
        }


        [HttpPost]
        public async Task<ActionResult> CreateADMChecklist(OapAssetDataManagement model)
        {           
            var response = await GetClient<OapChecklistAssetDataManagementClient>().AddAssetDataManagementAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateADMChecklist(OapAssetDataManagement model)
        {            
            var response = GetClient<OapChecklistAssetDataManagementClient>().UpdateAssetDataManagementAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteADMChecklist(OapAssetDataManagement model)
        {
            if (model.Id > 0)
            {
                var response = await GetClient<OapChecklistAssetDataManagementClient>().DeleteAssetDataManagementAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        ~OapChecklistAssetDataManagementController()
        {
            Client.Dispose();
        }
    }
}

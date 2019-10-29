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
    using Ensco.Models;
    using System.Linq;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapChecklistEquipmentController : OapCorpGridController
    {
        public OapChecklistEquipmentController() : base()
        {
            RegisterClient(new List<Type> {
                typeof(OapChecklistEquipmentClient),
                typeof(OapEquipmentClient),
                typeof(OapChecklistClient)               
               });
        }

        private ObservableCollection<OapChecklist> GetOapChecklists()
        {
            return GetClient<OapChecklistClient>().GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private ObservableCollection<OapEquipment> GetEquipments()
        {
            return GetClient<OapEquipmentClient>().GetAllAsync().Result?.Result?.Data;
        }
        
        private OapChecklistEquipmentClient OapChecklistEquipmentClient { get; }      

        private GridData GridData { get; } = new GridData("OapChecklistEquipmentGrid", "OapChecklistEquipment", "ChecklistEquipment", "Checklist and Protocol Plant List", "AddNewChecklistEquipment", "Add", "search", initializeCallBack: true);
        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            var lkpList = Ensco.Utilities.UtilitySystem.GetLookupList("Rig");
            if (lkpList == null)
            {
                lkpList = new LookupListModel<dynamic>();
                lkpList.Items = new List<object>();
            }
            var items = (lkpList.Items as List<object>)?.Cast<RigModel>();
            if (ViewData["Rig"] == null)
            {
                ViewData["Rig"] = items;
            }
            var oapRigs = ViewData["Rig"];
            var oapRigsCombo = new GridCombo("RigModel", oapRigs);

            if (ViewData["Checklists"] == null)
            {
                ViewData["Checklists"] = GetOapChecklists();
            }
            var oapChecklists = ViewData["Checklists"] as ObservableCollection<OapChecklist>;
            var oapChecklistsCombo = new GridCombo("OapChecklist", oapChecklists);

            if (ViewData["Equipment"] == null)
            {
                ViewData["Equipment"] = GetEquipments();
            }
            var oapEquipments = ViewData["Equipment"] as ObservableCollection<OapEquipment>;
            var oapEquipmentsCombo = new GridCombo("OapEquipment", oapEquipments);


            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Id", displayName:"ID",width:10, editLayoutWidth:50),
                                new GridDisplayColumn("RigId", displayName:"Rig",width:10, columnType:MVCxGridViewColumnType.ComboBox,  lookup:oapRigsCombo, editLayoutWidth:50),
                                new GridDisplayColumn("OapChecklistId", displayName:"Checklist / Protocol", columnType:MVCxGridViewColumnType.ComboBox,  lookup:oapChecklistsCombo, width:15, editLayoutWidth:50),
                                new GridDisplayColumn("OapEquipmentId", displayName:"Equipment", columnType:MVCxGridViewColumnType.ComboBox,  lookup:oapEquipmentsCombo, width:15, editLayoutWidth:50),
                                new GridDisplayColumn("Order", displayName:"Order",width:5, editLayoutWidth:50),
                                new GridDisplayColumn("Description", displayName:"Description", width:30, editLayoutWidth:50),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:50, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:50, displayFormat: "g"),

                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", isReadOnly:true, displayFormat: "g", isVisible: false),
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:10, isVisible: false),                               
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {                              
                                new GridEditLayoutColumn("RigId", displayName:"Rig"),
                                new GridEditLayoutColumn("OapChecklistId", displayName:"Checklist / Protocol"),
                                new GridEditLayoutColumn("OapEquipmentId", displayName:"Equipment"),
                                new GridEditLayoutColumn("Order", displayName:"Order"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),                         
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                            };
                  

            gridData.FormLayout = new GridEditFormLayout(GridData.LayoutColumns
                 , i => {
                     i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                     i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                 }, 2
                 , editMode: GridViewEditingMode.EditFormAndDisplayRow);

        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridData, "CreateChecklistEquipment", "UpdateChecklistEquipment", "DeleteChecklistEquipment");

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.SettingsDetail.ShowDetailRow = false;          
        }

        public async Task<ActionResult> Index()
        {           
            var response = await GetClient<OapChecklistEquipmentClient>().GetAllAsync();
            return View("OapChecklistEquipment", response.Result.Data ?? new ObservableCollection<OapChecklistEquipment>());             
        }
        
        public async Task<ActionResult> ChecklistEquipment()
        {            
            var response = await GetClient<OapChecklistEquipmentClient>().GetAllAsync();
            return PartialView("OapChecklistEquipmentPartial", response.Result.Data ?? new ObservableCollection<OapChecklistEquipment>());
        }

        [HttpPost]
        public async Task<ActionResult> CreateChecklistEquipment(OapChecklistEquipment model)
        {             
            var response = await GetClient<OapChecklistEquipmentClient>().AddChecklistEquipmentAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklistEquipment(OapChecklistEquipment model)
        {           
            var response = GetClient<OapChecklistEquipmentClient>().UpdateChecklistEquipmentAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistEquipment(OapChecklistEquipment model)
        {
            if (model.Id > 0)
            {
                var response = await GetClient<OapChecklistEquipmentClient>().DeleteChecklistEquipmentAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        ~OapChecklistEquipmentController()
        {
            Client.Dispose();
        }
    }
}

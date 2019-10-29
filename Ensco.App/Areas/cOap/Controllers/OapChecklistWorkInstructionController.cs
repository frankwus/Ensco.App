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
    using System.Linq;
    using Ensco.Models;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapChecklistWorkInstructionController : OapCorpGridController
    {
        public OapChecklistWorkInstructionController() : base()
        {
            RegisterClient(new List<Type> {
                typeof(OapChecklistWorkInstructionClient),
                typeof(OapWorkInstructionClient),
                typeof(OapChecklistClient)
              });
        }

        private ObservableCollection<OapChecklist> GetOapChecklists()
        {
            return GetClient<OapChecklistClient>().GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }
        private ObservableCollection<OapWorkInstruction> GetWorkInstructions()
        {
            return GetClient<OapWorkInstructionClient>().GetAllAsync().Result?.Result?.Data;
        }

        private GridData GridData { get; } = new GridData("OapChecklistWorkInstructionGrid", "OapChecklistWorkInstruction", "ChecklistWorkInstruction", "Checklist and Protocol Work Instruction List", "AddNewChecklistWorkInstruction", "Add", "search", initializeCallBack: true);
        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {

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

            if (ViewData["WorkInstruction"] == null)
            {
                ViewData["WorkInstruction"] = GetWorkInstructions();
            }
            var oapWorkInstructions = ViewData["WorkInstruction"] as ObservableCollection<OapWorkInstruction>;
            var oapWorkInstructionsCombo = new GridCombo("OapWorkInstruction", oapWorkInstructions,"Id","Title");

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                                {
                                      new GridDisplayColumn("RigId", displayName:"Rig",columnType:MVCxGridViewColumnType.ComboBox, lookup:oapRigsCombo, width:12, editLayoutWidth:100),
                                      new GridDisplayColumn("OapChecklistId", displayName:"Checklist Protocol",columnType:MVCxGridViewColumnType.ComboBox, lookup:oapChecklistsCombo, width:20, editLayoutWidth:100),
                                      new GridDisplayColumn("OapWorkInstructionId", displayName:"Work Instruction", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapWorkInstructionsCombo, width:20, editLayoutWidth:100),
                                      new GridDisplayColumn("Order", displayName:"Order",width:5, editLayoutWidth:100),
                                      new GridDisplayColumn("Description", displayName:"Description", width:25, editLayoutWidth:100),
                                      new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                      new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),

                                      new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", isReadOnly:true, width:0, displayFormat: "g", isVisible: false),
                                      new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible: false),
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
                                new GridEditLayoutColumn("OapWorkInstructionId", displayName:"Work Instruction"),
                                new GridEditLayoutColumn("Order", displayName:"Order"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date")
                            };


            gridData.FormLayout = new GridEditFormLayout(GridData.LayoutColumns
                    , i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    }, columnCount: 2
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow); 
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridData, "CreateChecklistWorkInstruction", "UpdateChecklistWorkInstruction", "DeleteChecklistWorkInstruction");

            //Master Grid Specific configurations
            settings.KeyFieldName = GridData.Key;
            settings.Name = $"{GridData.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            //Generic configurations
            settings.Configure(GridData, html, viewContext);
        }

        public ActionResult Index()
        {
            var data = GetClient<OapChecklistWorkInstructionClient>().GetAllAsync().Result.Result.Data;
            return View("OapChecklistWorkInstruction", data ?? new ObservableCollection<OapChecklistWorkInstruction>());
        }

       
        public ActionResult ChecklistWorkInstruction()
        {
            var data = GetClient<OapChecklistWorkInstructionClient>().GetAllAsync().Result.Result.Data;
            return PartialView("OapChecklistWorkInstructionPartial", data ?? new ObservableCollection<OapChecklistWorkInstruction>());
        }


        [HttpPost]
        public async Task<ActionResult> CreateChecklistWorkInstruction(OapChecklistWorkInstruction model)
        {           
            var response = await GetClient<OapChecklistWorkInstructionClient>().AddChecklistWorkInstructionAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklistWorkInstruction(OapChecklistWorkInstruction model)
        {            
            var response = GetClient<OapChecklistWorkInstructionClient>().UpdateChecklistWorkInstructionAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistWorkInstruction(OapChecklistWorkInstruction model)
        {
            if (model.Id > 0)
            {
                var response = await GetClient<OapChecklistWorkInstructionClient>().DeleteChecklistWorkInstructionAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        ~OapChecklistWorkInstructionController()
        {
            Client.Dispose();
        }
    }
}

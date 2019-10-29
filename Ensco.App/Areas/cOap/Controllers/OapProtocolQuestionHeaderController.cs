using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Ensco.Irma.Oap.Common;
    using Ensco.App.App_Start;
    using Ensco.Utilities;

    public class OapProtocolQuestionHeadersController : OapCorpGridController
    {
        public const string CurrentProtocolQuestionHeaderId = "CurrentProtocolQuestionHeaderId";
        public const string CurrentProtocolQuestionHeaderName = "CurrentProtocolQuestionHeaderName";

        private OapProtocolQuestionHeaderClient OapProtocolQuestionHeaderClient { get; }

        private OapChecklistClient OapChecklistClient { get; }

        private OapChecklistQuestionClient OapChecklistQuestionClient { get; }

        private OapFrequencyTypeClient OapFrequencyTypeClient { get; }

        private GridData GridDataProtocolQuestionHeader { get; } = new GridData("OapProtocolQuestionHeaderGrid", "OapProtocolQuestionHeaders", "ProtocolQuestionHeaders", "Open ProtocolQuestionHeaders", "AddNewProtocolQuestionHeader", "Add Protocol Question Sub Text", "search Protocol Question Sub Text", initializeCallBack: true);

        private List<OapChecklist> GetProtocols()
        {
            var protocols = new List<OapChecklist>();
            var checklists =  OapChecklistClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
            if(checklists != null)
            {
                protocols.AddRange(checklists.Where(c => c.OapType.IsProtocol));
            }

            return protocols;
        }

        private ObservableCollection<OapChecklistQuestion> GetOapProtocolQuestions()
        {
            return OapChecklistQuestionClient.GetAllProtocolQuestionsAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }
         

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            if (ViewData["OapProtocols"] == null)
            {
                var allOapProtocols = GetProtocols();
                ViewData["OapProtocols"] = allOapProtocols;  
            }

            if (ViewData["ProtocolQuestions"] == null)
            {
                ViewData["ProtocolQuestions"] = GetOapProtocolQuestions();
            }
              
            var oapProtocols = ViewData["OapProtocols"] as IEnumerable<OapChecklist>;
            var oapProtocolQuestions = ViewData["ProtocolQuestions"] as IEnumerable<OapChecklistQuestion>; 

            var oapProtocolCombo = new GridCombo()
            {
                Name = "OapProtocol",
                DataSource = oapProtocols,
                KeyColumnName = "Id",
                DisplayColumnName = "Name",
                ValueColumnName = "Id",
                SelectedIndexChangedEvent = "OapProtocolOnSelectedeChanged"
            };

            var oapProtocolQuestionCombo = new GridCombo()
            {
                Name = "OapProtocolQuestion",
                DataSource = oapProtocolQuestions,
                KeyColumnName = "Id",
                DisplayColumnName = "Name",
                ValueColumnName = "Id", 
                CallbackRouteValues = new { Controller = "OapProtocolQuestionHeaders", Action = "GetOapProtocolQuestions", Id = "OapSubTypeId" }
            };

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Section", displayName:"Section", width:15, editLayoutWidth:25),
                                new GridDisplayColumn("Description", displayName:"Description", width:25, editLayoutWidth:30),
                                
                                new GridDisplayColumn("OapProtocolId", displayName:"Protocol", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapProtocolCombo, width:15, editLayoutWidth:20),
                                new GridDisplayColumn("OapChecklistQuestionId", displayName:"Question", columnType:MVCxGridViewColumnType.ComboBox, columnAction:c => {
                                    c.Name = "OapChecklistQuestionId"; 
                                    c.Caption = "Question";
                                    c.FieldName = "OapChecklistQuestionId";
                                    c.Width = Unit.Percentage(15);
                                    c.EditorProperties().ComboBox(cb =>
                                    {
                                        cb.CallbackRouteValues = new { Controller = "OapProtocolQuestionHeaders", Action = "GetProtocolQuestions", TextField="Name", ValueField = "Id" }; 
                                        cb.TextField = "Name";
                                        cb.ValueField = "Id";
                                        cb.DataSource = oapProtocolQuestions;
                                        cb.ClientSideEvents.BeginCallback = "OapProtocolQuestionBeginCallback";
                                        cb.ClientSideEvents.EndCallback = "OapProtocolQuestionEndCallback";
                                        cb.Width = Unit.Percentage(20);
                                    });
                                }) ,
                                new GridDisplayColumn("Criticality", displayName:"Criticality", columnType:MVCxGridViewColumnType.ComboBox, lookup: CommonDataLists.GetCriticalityCombo(), width:15, editLayoutWidth:20),
                                new GridDisplayColumn("BasicCause", displayName:"BasicCause", width:15, editLayoutWidth:20),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:10, editLayoutWidth:30, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:10, editLayoutWidth:30, displayFormat: "g"),
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:10),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:10, isReadOnly:true, displayFormat: "g"),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:10, isReadOnly:true ),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:10, isReadOnly:true, displayFormat: "g")
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Section", displayName:"Section",width:50),
                                new GridEditLayoutColumn("Description", displayName:"Description",width:50),
                                new GridEditLayoutColumn("OapProtocolId", displayName:"Protocol",width:50),
                                new GridEditLayoutColumn("OapChecklistQuestionId", displayName:"Question",width:50),
                                new GridEditLayoutColumn("Criticality", displayName:"Criticality",width:50),   
                                new GridEditLayoutColumn("BasicCause", displayName:"BasicCause",width:50),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date", columnType: MVCxGridViewColumnType.DateEdit , width:50),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date", columnType: MVCxGridViewColumnType.DateEdit , width:50),
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i => {
                        i.ShowUpdateButton = true;
                        i.ShowCancelButton = true;
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                    }
                    , columnCount:2
                    , emptyLayputItemCount: 1
                    );
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            InitializeGridData(GridDataProtocolQuestionHeader, "CreateProtocolQuestionHeader", "UpdateProtocolQuestionHeader", "DeleteProtocolQuestionHeader");
             
            //Master Grid Specific configurations
            settings.KeyFieldName = GridDataProtocolQuestionHeader.Key;
            settings.Name = $"{GridDataProtocolQuestionHeader.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.ClientSideEvents.BeginCallback = "OnBeginCallback";
            settings.ClientSideEvents.EndCallback = "OnEndCallback";

            //Generic configurations
            settings.Configure(GridDataProtocolQuestionHeader, html, viewContext);

            SelectRow((int)(ViewData[CurrentProtocolQuestionHeaderId] ?? 0), (string)(ViewData[CurrentProtocolQuestionHeaderName] ?? string.Empty), ref settings);
        }

        public OapProtocolQuestionHeadersController() : base()
        {
            OapProtocolQuestionHeaderClient = new OapProtocolQuestionHeaderClient(GetApiBaseUrl(), Client);
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);
            OapChecklistQuestionClient = new OapChecklistQuestionClient(GetApiBaseUrl(), Client);
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapProtocolQuestionHeaderClient.GetAllAsync(GetAllModelsCorp());
            return View("OapProtocolQuestionHeader", response.Result.Data);
        }
                 
        public async Task<ActionResult> ProtocolQuestionHeaders()
        {
            var response = await OapProtocolQuestionHeaderClient.GetAllAsync(GetAllModelsCorp());
            return PartialView("OapProtocolQuestionHeaderPartial", response.Result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProtocolQuestionHeader(OapProtocolQuestionHeader model)
        {          
            // TODO: Add insert logic here
            var response = await OapProtocolQuestionHeaderClient.AddProtocolQuestionHeaderAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateProtocolQuestionHeader(OapProtocolQuestionHeader model)
        {           
            // TODO: Add update logic here
            var response = OapProtocolQuestionHeaderClient.UpdateProtocolQuestionHeaderAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProtocolQuestionHeader(OapProtocolQuestionHeader model)
        {
            if (model.Id > 0)
            {
                var response = await OapProtocolQuestionHeaderClient.DeleteProtocolQuestionHeaderAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetProtocolQuestions(int oapProtocolId, string textField, string valueField)
        {
            return GridViewExtension.GetComboBoxCallbackResult(p => {
                p.TextField = textField;
                p.ValueField = valueField;
                var oapProtocolQuestions = OapChecklistQuestionClient.GetAllChecklistQuestionsAsync(oapProtocolId).Result.Result.Data ??new ObservableCollection<OapChecklistQuestion>(); 
                p.BindList(oapProtocolQuestions);
            });
        }
    }
}

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

    public class OapChecklistCommentsController : OapCorpGridController
    {
        public const string CurrentChecklistCommentId = "CurrentChecklistCommentId";
        public const string CurrentChecklistCommentName = "CurrentChecklistCommentName";

        private OapChecklistCommentClient OapChecklistCommentClient { get; } 

        private OapChecklistClient OapChecklistClient { get; }

        private GridData GridData { get; } = new GridData("OapChecklistCommentGrid", "OapChecklistComments", "Comments", "Checklist Comments", "AddNewChecklistComment", "Add Checklist Comment", "search checklist comments",initializeCallBack:true);
        
        private ObservableCollection<OapChecklist> GetOapChecklists()
        {
            return OapChecklistClient.GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }

        private GetAllModel GetAllModel()
        {
            return new GetAllModel() { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow };
        }

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {

            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (ViewData["Checklists"] == null)
            {
                ViewData["Checklists"] = GetOapChecklists();
            }

            var oapChecklists = ViewData["Checklists"] as ObservableCollection<OapChecklist>; 

            var oapChecklistCombo = new GridCombo("OapChecklist", oapChecklists); 

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("OapChecklistId", displayName:"Checklist", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapChecklistCombo ,width:15, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Description",width:15, editLayoutWidth:100),
                                new GridDisplayColumn("Question", displayName:"Question",width:25, editLayoutWidth:100), 
                                new GridDisplayColumn("SubQuestion", displayName:"Sub Question",width:25, editLayoutWidth:100),
                                new GridDisplayColumn("Order", displayName:"Order", columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100),
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
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("Question", displayName:"Question"),
                                new GridEditLayoutColumn("SubQuestion", displayName:"Sub Question"),         
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                                new GridEditLayoutColumn("Order", displayName:"Order")
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i => {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    }
                    , columnCount: 2
                    , emptyLayputItemCount: 1
                    );

        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridData, "CreateChecklistComment", "UpdateChecklistComment", "DeleteChecklistComment");

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.SettingsDetail.ShowDetailRow = false; 
             
            int oapChecklistCommentId = (int)(ViewData[CurrentChecklistCommentId] ?? 0);
            string oapChecklistCommentName = (string)(ViewData[CurrentChecklistCommentName] ?? string.Empty);

            //Select the Current Row when it defined.
            if (oapChecklistCommentId != 0)
            {
                settings.Columns.Grid.Selection.SelectRowByKey(oapChecklistCommentId);
            }
        }
         
        public OapChecklistCommentsController():base()
        {
            OapChecklistCommentClient = new OapChecklistCommentClient(GetApiBaseUrl(), Client); 
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client); 
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapChecklistCommentClient.GetAllAsync(GetAllModelsCorp());
            return View("OapChecklistComment", response.Result.Data);
        }

        public async Task<ActionResult> Comments()
        {
            var response = await OapChecklistCommentClient.GetAllAsync(GetAllModelsCorp());
            return PartialView("OapChecklistCommentPartial", response.Result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChecklistComment(OapChecklistComment model)
        {            
            // TODO: Add insert logic here
            var response = await OapChecklistCommentClient.AddChecklistCommentAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklistComment(OapChecklistComment model)
        {             
            // TODO: Add update logic here
            var response = OapChecklistCommentClient.UpdateChecklistCommentAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistComment(OapChecklistComment model)
        {
            if (model.Id > 0)
            {
                var response = await OapChecklistCommentClient.DeleteChecklistCommentAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        ~OapChecklistCommentsController()
        {
            Client.Dispose();
        }
    }
}

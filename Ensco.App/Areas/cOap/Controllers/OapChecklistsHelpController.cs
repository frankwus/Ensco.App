using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using System.Collections.ObjectModel;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapChecklistsHelpController : OapCorpGridController
    {
        public const string CurrentChecklistId = "CurrentChecklistId";
        public const string CurrentChecklistName = "CurrentChecklistName";

        private OapChecklistClient OapChecklistClient { get; } 

        private GridData GridData { get; } = new GridData("OapChecklistsHelpGrid", "OapChecklistsHelp", "ChecklistsHelp", "Checklist Help", "SearchChecklistHelp", "search checklists help",initializeCallBack:true);
       
        private void InitializeGridData(GridData gridData, string updateAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Name", displayName:"Name", width:15),
                                new GridDisplayColumn("Description", displayName:"Description", width:15),
                                new GridDisplayColumn("Help", displayName:"Help Text", columnType:MVCxGridViewColumnType.Memo, width:50, editLayoutWidth:100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("OapTypeId", width:0, isVisible:false),
                                new GridDisplayColumn("OapSubTypeId", width:0, isVisible:false),
                                new GridDisplayColumn("OapProtocolFormTypeId", width:0, isVisible:false),
                                new GridDisplayColumn("OapFrequencyTypeId", width:0, isVisible:false),
                                new GridDisplayColumn("Randomize", width:0, isVisible:false),
                                new GridDisplayColumn("ControlNumber", width:0, isVisible:false)
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction })                              
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            { 
                                new GridEditLayoutColumn("Help", displayName:"Help Text",  columnType: MVCxGridViewColumnType.Memo, layoutAction: col => {
                                    col.ColumnSpan = 2;
                                    col.Width = Unit.Percentage(100);                                                                   
                                }),
                                new GridEditLayoutColumn("StartDateTime",displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                                new GridEditLayoutColumn("Description", isVisible:false),
                                new GridEditLayoutColumn("Name", isVisible:false), 
                                new GridEditLayoutColumn("OapTypeId", isVisible:false),
                                new GridEditLayoutColumn("OapSubTypeId", isVisible:false),
                                new GridEditLayoutColumn("OapProtocolFormTypeId", isVisible:false),
                                new GridEditLayoutColumn("OapFrequencyTypeId", isVisible:false),
                                new GridEditLayoutColumn("Randomize", isVisible:false),
                                new GridEditLayoutColumn("ControlNumber", isVisible:false)                                
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i => {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                    , columnCount: 2
                    , emptyLayputItemCount: 1
                    );
        }


        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            InitializeGridData(GridData, "UpdateChecklistHelp");

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.SettingsDetail.ShowDetailRow = false;
            settings.CommandColumn.Width = Unit.Percentage(2);

            int oapChecklistCommentId = (int)(ViewData[CurrentChecklistId] ?? 0);
            string oapChecklistCommentName = (string)(ViewData[CurrentChecklistName] ?? string.Empty);

            //Select the Current Row when it defined.
            if (oapChecklistCommentId != 0)
            {
                settings.Columns.Grid.Selection.SelectRowByKey(oapChecklistCommentId);
            }

            var columnHelp = settings.Columns["Help"] as MVCxGridViewColumn;
            if (columnHelp != null)
            {
                columnHelp.EditorProperties().Memo(m =>
                {
                    m.Width = Unit.Percentage(100);
                    m.Rows = 10;
                });
            }
        }
         
        public OapChecklistsHelpController():base()
        {
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);  
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapChecklistClient.GetAllAsync(GetAllModelsCorp());
            return View("OapChecklistsHelp", response.Result.Data);
        }
         
        public async Task<ActionResult> ChecklistsHelp()
        {
            var response = await OapChecklistClient.GetAllAsync(GetAllModelsCorp());
            return PartialView("OapChecklistsHelpPartial", response.Result.Data);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateChecklistHelp(OapChecklist model)
        {             
            // TODO: Add update logic here
            var response = OapChecklistClient.UpdateOapChecklistAsync(model).Result;
            return RedirectToAction("Index");
        } 
    }
}

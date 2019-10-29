using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Ensco.App.App_Start;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using System.Collections.ObjectModel;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapChecklistQuestionsHelpController : OapCorpGridController
    {
        public const string CurrentChecklists = "CurrentChecklists";
        public const string CurrentChecklistId = "CurrentChecklistId";
        public const string CurrentChecklistQuestionId = "CurrentChecklistQuestionId";
        public const string CurrentChecklistQuestionName = "CurrentChecklistQuestionName";

        private OapChecklistClient OapChecklistClient { get; }

        private OapChecklistQuestionClient OapChecklistQuestionClient { get; } 

        private GridData GridData { get; } = new GridData("OapChecklistQuestionsHelpGrid", "OapChecklistQuestionsHelp", "ChecklistQuestions", "Checklist Question Help", "SearchChecklistQuestion", "search checklist questions", initializeCallBack:true);
       
        private void InitializeGridData(GridData gridData, string updateAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Question", displayName:"Question", width:20),
                                new GridDisplayColumn("Description", displayName:"Description", width:15),
                                new GridDisplayColumn("Help", displayName:"Help Text", columnType:MVCxGridViewColumnType.Memo,width:50, editLayoutWidth:100, editLayoutHeight:100),                               
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),

                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("OapChecklistTopicId", width:0, isVisible:false),
                                new GridDisplayColumn("Order", width:0, isVisible:false),
                                new GridDisplayColumn("Weight", width:0, isVisible:false),
                                new GridDisplayColumn("Maximum", width:0, isVisible:false),
                                new GridDisplayColumn("YesValue", width:0, isVisible:false),
                                new GridDisplayColumn("EditNoValue", width:0, isVisible:false),
                                new GridDisplayColumn("Randomize", width:0, isVisible:false)
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction })                              
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Help", displayName:"Help Text", columnType: MVCxGridViewColumnType.Memo, layoutAction: col => {
                                    col.ColumnSpan = 2;
                                    col.Width = Unit.Percentage(100);
                                }),
                                new GridEditLayoutColumn("StartDateTime",displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                                new GridEditLayoutColumn("Question", isVisible:false),
                                new GridEditLayoutColumn("Description", isVisible:false),
                                new GridEditLayoutColumn("Order", isVisible:false),
                                new GridEditLayoutColumn("Weight", isVisible:false),
                                new GridEditLayoutColumn("Maximum", isVisible:false),
                                new GridEditLayoutColumn("YesValue", isVisible:false),
                                new GridEditLayoutColumn("EditNoValue", isVisible:false),
                                new GridEditLayoutColumn("Randomize", isVisible:false),                              
                                new GridEditLayoutColumn("OapChecklistTopicId", isVisible:false)                                
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
            InitializeGridData(GridData, "UpdateChecklistQuestionHelp");

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.ClientSideEvents.BeginCallback = "OnBeginCallback"; 

            settings.SettingsDetail.ShowDetailRow = false;
            settings.CommandColumn.Width = Unit.Percentage(2);


            int oapChecklistCommentId = (int)(ViewData[CurrentChecklistQuestionId] ?? 0);
            string oapChecklistCommentName = (string)(ViewData[CurrentChecklistQuestionName] ?? string.Empty);

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
                    m.Rows = 5;
                });
            }
        }

        public  void Configure(ComboBoxSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            viewContext.Writer.Write("</br>");
            settings.Name = "CheckListCombo";
            settings.Width = Unit.Percentage(30);
            settings.Properties.TextField = "Name";
            settings.Properties.ValueField = "Id";
            settings.Properties.Caption = "Select Checklist / Protocol:";
            settings.Properties.ClientSideEvents.SelectedIndexChanged = "CheckListComboOnSelectedIndexChanged";   
        }

        public OapChecklistQuestionsHelpController():base()
        {
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);
            OapChecklistQuestionClient = new OapChecklistQuestionClient(GetApiBaseUrl(), Client);  
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapChecklistClient.GetAllAsync(GetAllModelsCorp());
            Session[CurrentChecklists] = response.Result.Data;
            return View("OapChecklistQuestionsHelp", response.Result.Data);
        }

        public async Task<ActionResult> ChecklistQuestions(int? checklistId)
        {
            var questions = new List<OapChecklistQuestion>();

            if (checklistId.HasValue)
            {
                var response = await OapChecklistQuestionClient.GetAllChecklistQuestionsAsync(checklistId.Value);
                if (response.Result?.Data != null)
                {
                    questions.AddRange(response.Result.Data);
                }
            }
            return PartialView("OapChecklistQuestionsHelpPartial", questions);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateChecklistQuestionHelp(OapChecklistQuestion model)
        {            
            // TODO: Add update logic here
            var response = OapChecklistQuestionClient.UpdateQuestionAsync(model).Result;
            var questions = new List<OapChecklistQuestion>() { model };            
            return PartialView("OapChecklistQuestionsHelpPartial", questions);
        } 
    }
}

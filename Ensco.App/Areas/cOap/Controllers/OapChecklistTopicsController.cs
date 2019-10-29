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
    using System.Web.Mvc.Html;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Linq;
    using Ensco.App.App_Start;
    using Ensco.Utilities;

    public class OapChecklistTopicsController : OapCorpGridController
    {
        public const string CurrentTopicId = "CurrentTopicId";
        public const string CurrentTopicName = "CurrentTopicName";

        public const string CurrentQuestionId = "CurrentQuestionId";
        public const string CurrentQuestionName = "CurrentQuestionName";

        private GridData GridData { get; } = new GridData("OapChecklistTopicGrid", "OapChecklistTopics", "Topics", "Checklist Topics", "AddNewChecklistTopic", "Add Checklist Topic", "search checklist topics", initializeCallBack: true, displayColumnName: "Topic");

        private GridData ChildGridData { get; } = new GridData("OapChecklistQuestionGrid", "OapChecklistTopics", "Questions", "Checklist Questions", "AddNewChecklistQuestion", "Add Checklist Question", "search checklist questions", displayColumnName: "Question", initializeCallBack: true );

        private GridData SubChildRelatedQuestionGridData { get; } = new GridData("OapChecklistRelatedQuestionGrid", "OapChecklistTopics", "RelatedQuestions", "Related Questions", "AddNewChecklistRelatedQuestion", "Add Related Question", "search questions", initializeCallBack: true);

        private GridData SubChildTagGridData { get; } = new GridData("OapChecklistQuestionTagGrid", "OapChecklistTopics", "QuestionTags", "Barrier Health Tags", "AddNewChecklistQuestionTag", "Add Tag", "search tags", initializeCallBack: true);

        private ObservableCollection<OapChecklistGroup> GetOapChecklistGroups()
        {
            return GetClient<OapChecklistGroupClient>().GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }
        private ObservableCollection<OapChecklistSubGroup> GetChecklistSubGroups()
        {
            return GetClient<OapChecklistSubGroupClient>().GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }
        private ObservableCollection<OapChecklist> GetOapChecklists()
        {
            return GetClient<OapChecklistClient>().GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }
        private List<OapChecklistQuestion> GetQuestions()
        {
            var oapChecklists = GetOapChecklists();
            var allChecklistQuestions = (from c in oapChecklists
                                         from g in c.Groups
                                         from s in g.SubGroups
                                         from t in s.Topics
                                         from q in t.Questions ?? new ObservableCollection<OapChecklistQuestion>()
                                         where t.Questions != null
                                         select q).ToList();

            return allChecklistQuestions;
        }
        private ObservableCollection<OapChecklistQuestionTag> GetQuestionTags()
        {
            return GetClient<OapChecklistQuestionTagClient>().GetAllAsync().Result?.Result?.Data;
        }
        private ObservableCollection<OapChecklistQuestionDataType> GetQuestionDataTypes()
        {
            return GetClient<OapChecklistQuestionDataTypeClient>().GetAllAsync().Result?.Result?.Data;
        }
        private ObservableCollection<OapFrequency> GetFrequencies()
        {
            return GetClient<OapFrequencyClient>().GetAllAsync(GetAllModelsCorp()).Result?.Result?.Data;
        }
        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (ViewData["ChecklistGroups"] == null)
            {
                ViewData["ChecklistGroups"] = GetOapChecklistGroups();
            }

            if (ViewData["ChecklistSubGroups"] == null)
            {
                ViewData["ChecklistSubGroups"] = GetChecklistSubGroups();
            }

            var oapChecklistGroups = ViewData["ChecklistGroups"] as ObservableCollection<OapChecklistGroup>;
            var oapChecklistSubGroups = ViewData["ChecklistSubGroups"] as ObservableCollection<OapChecklistSubGroup>;

            var oapChecklistGroupCombo = new GridCombo("OapChecklistGroups", oapChecklistGroups);
            oapChecklistGroupCombo.SelectedIndexChangedEvent = "OapChecklistGroupOnSelectedChanged";
            oapChecklistGroupCombo.DisplayColumnName = "Name";


            var oapChecklistSubGroupsCombo = new GridCombo("ChecklistSubGroups", oapChecklistSubGroups);
            oapChecklistSubGroupsCombo.CallbackRouteValues = new { Controller = "OapChecklistTopics", Action = "GetOapChecklistSubGroups", Id = "OapChecklistSubGroupId" };
            oapChecklistSubGroupsCombo.DisplayColumnName = "Name";

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("OapChecklistGroupId", displayName:"Group", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapChecklistGroupCombo ,width:15, editLayoutWidth:100),
                                new GridDisplayColumn("OapChecklistSubGroupId", displayName:"Sub Group", editLayoutWidth:100, columnAction:c => {
                                    c.Name = "OapChecklistSubGroupId";
                                    c.Caption = Translate("Sub Group");
                                    c.FieldName = "OapChecklistSubGroupId";
                                    c.Width = Unit.Percentage(15);
                                    c.EditorProperties().ComboBox(cb =>
                                    {
                                        cb.CallbackRouteValues = new { Controller = "OapChecklistTopics", Action = "GetOapChecklistSubGroups", TextField="Name", ValueField = "Id" };
                                        cb.TextField = "Name";
                                        cb.ValueField = "Id";
                                        cb.DataSource = oapChecklistSubGroups;
                                        cb.ClientSideEvents.BeginCallback = "OapChecklistSubGroupBeginCallback";
                                        cb.ClientSideEvents.EndCallback = "OapChecklistSubGroupEndCallback";
                                        cb.Width = Unit.Percentage(100);
                                    });
                                }),
                                new GridDisplayColumn("Topic", displayName:"Topic",width:20, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Description",width:30, editLayoutWidth:100),
                                new GridDisplayColumn("Order", displayName:"Order", columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g")
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
                                new GridEditLayoutColumn("OapChecklistSubGroupId", displayName:"Sub Group"),
                                new GridEditLayoutColumn("Topic", displayName:"Topic"),
                                new GridEditLayoutColumn("Order", displayName:"Order"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i =>
                    {
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

            if (ViewData["ChecklistQuestionDataType"] == null)
            {
                ViewData["ChecklistQuestionDataType"] = GetQuestionDataTypes();
            }

            var oapChecklistQuestionDataTypes = ViewData["ChecklistQuestionDataType"] as ObservableCollection<OapChecklistQuestionDataType>;
            var oapChecklistQuestionDataTypeCombo = new GridCombo("oapChecklistQuestionDataTypes", oapChecklistQuestionDataTypes);

            if (ViewData["OapFrequency"] == null)
            {
                ViewData["OapFrequency"] = GetFrequencies();
            }

            var oapFrequencies = ViewData["OapFrequency"] as ObservableCollection<OapFrequency>;
            var oapFrequencyCombo = new GridCombo("ChecklistOapFrequencies", oapFrequencies);

            var radioButtonDisplayTypeId = oapChecklistQuestionDataTypes?.FirstOrDefault(dt => dt.Code.Equals("RBL", StringComparison.InvariantCultureIgnoreCase));

            dynamic spinEditProperties = new System.Dynamic.ExpandoObject();
            spinEditProperties.NumberType = SpinEditNumberType.Float;
        

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Question", displayName:"Question",width:15, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Sub Group Description",width:15, editLayoutWidth:100),
                                new GridDisplayColumn("Help", displayName:"Help Text", width:10),
                                new GridDisplayColumn("Order", displayName:"Order", columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100),
                                new GridDisplayColumn("Weight", displayName:"Scoring Weight", columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100, editorProperties: spinEditProperties),
                                new GridDisplayColumn("Maximum", displayName:"Scoring Max", columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100, editorProperties: spinEditProperties),
                                new GridDisplayColumn("YesValue", displayName:"Yes Value", columnType:MVCxGridViewColumnType.SpinEdit, width:5, editLayoutWidth:100),
                                new GridDisplayColumn("EditNoValue", displayName:"Edit No Value", columnType:MVCxGridViewColumnType.CheckBox, width:5, editLayoutWidth:100),
                                new GridDisplayColumn("Randomize", displayName:"Randomize", columnType:MVCxGridViewColumnType.CheckBox, width:5, editLayoutWidth:100),
                                new GridDisplayColumn("OapFrequencyId", displayName:"Frequency", columnType:MVCxGridViewColumnType.ComboBox, width:5, editLayoutWidth:100, lookup: oapFrequencyCombo),
                                new GridDisplayColumn("OapChecklistQuestionDataTypeId", displayName:"Display Type", columnType:MVCxGridViewColumnType.ComboBox, width:5, editLayoutWidth:100, lookup:oapChecklistQuestionDataTypeCombo),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                               
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0,isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g",isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true,isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g",isVisible:false),
                                new GridDisplayColumn("OapChecklistTopicId", displayName:"Checklist Topic Id", width:0,isVisible:false)
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = GridData.Controller, Action = "CreateChecklistQuestion"}),
                                new GridRoute(GridRouteTypes.Update, new { Controller = GridData.Controller, Action = "UpdateChecklistQuestion" }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = GridData.Controller, Action = "DeleteChecklistQuestion" }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Question", displayName:"Question"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("Order", displayName:"Order"),
                                new GridEditLayoutColumn("Weight", displayName:"Scoring Weight"),
                                new GridEditLayoutColumn("Maximum", displayName:"Scoring Max"),
                                new GridEditLayoutColumn("YesValue", displayName:"Yes Value"),
                                new GridEditLayoutColumn("EditNoValue", displayName:"Edit No Value"),
                                new GridEditLayoutColumn("Randomize", displayName:"Randomize"),
                                new GridEditLayoutColumn("OapFrequencyId", displayName:"Frequency"),
                                new GridEditLayoutColumn("OapChecklistQuestionDataTypeId", displayName:"Display Type"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                                new GridEditLayoutColumn("OapChecklistTopicId", displayName:"Checklist Group Id",isVisible:false)
                            };

            gridData.FormLayout = new GridEditFormLayout(
                gridData.LayoutColumns
                , i =>
                {
                    i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                    i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                }
                 , columnCount: 2
                 );

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["OapChecklistTopicId"] = id;
                e.NewValues["OapChecklistQuestionDataTypeId"] = radioButtonDisplayTypeId;
                gridData.DefaultNewRowInitializeFields(e);
            };

            gridData.CallBackRoute = new { Controller = gridData.Controller, Action = gridData.Action, oapChecklistTopicId = id, oapChecklistTopicName = name };
        }
        private void InitializeSubChildRelatedQuestionGridData(GridData gridData, string createAction, string deleteAction, int id, string name)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (ViewData["Checklists"] == null)
            {
                ViewData["Checklists"] = GetOapChecklists();
            }
            var oapChecklists = ViewData["Checklists"] as ObservableCollection<OapChecklist>;
            var oapChecklistCombo = new GridCombo("OapChecklist", oapChecklists);
            oapChecklistCombo.SelectedIndexChangedEvent = "OapChecklistOnSelectedChanged";
            oapChecklistCombo.DisplayColumnName = "Name";

            gridData.ButtonOptions.DisplayEditButton = false;

            if (ViewData["ChecklistQuestions"] == null)
            {
                ViewData["ChecklistQuestions"] = GetQuestions();
            }
            var oapChecklistQuestions = ViewData["ChecklistQuestions"] as List<OapChecklistQuestion>;
            var oapChecklistQuestionsCombo = new GridCombo("oapChecklistQuestions", oapChecklistQuestions, "Id", "Question");
            oapChecklistQuestionsCombo.CallbackRouteValues = new { Controller = "OapChecklistTopics", Action = "GetChecklistQuestions"};
            oapChecklistQuestionsCombo.DisplayColumnName = "Question";

            gridData.DisplayColumns = new List<GridDisplayColumn>()
            {
                new GridDisplayColumn("ChecklistId", displayName:"Checklist", columnType:MVCxGridViewColumnType.ComboBox, width:25, lookup:oapChecklistCombo, editLayoutWidth:100),
                new GridDisplayColumn("OapChecklistRelatedQuestionId", displayName:"Related Question", columnType:MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, columnAction:c => {
                    c.Name = "OapChecklistRelatedQuestionId";
                    c.Caption = "Related Question";
                    c.FieldName = "OapChecklistRelatedQuestionId";
                    c.Width = Unit.Percentage(100);
                    c.EditorProperties().ComboBox(cb =>
                    {
                        cb.CallbackRouteValues = new { Controller = "OapChecklistTopics", Action="GetChecklistQuestions", TextField="Question", ValueField = "Id" };
                        cb.TextField = "Question";
                        cb.ValueField = "Id";
                        cb.DataSource = oapChecklistQuestions;
                        cb.ClientSideEvents.BeginCallback = "OapChecklistQuestionsBeginCallback";
                        cb.ClientSideEvents.EndCallback = "OapChecklistQuestionsEndCallback";
                        cb.Width = Unit.Percentage(100);
                    });
                }),
                new GridDisplayColumn("OapChecklistQuestionId", displayName:"Question", isVisible:false)
            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = GridData.Controller, Action = "CreateChecklistQuestionRelatedQuestionMap" }),
                                //new GridRoute(GridRouteTypes.Update, new { Controller = GridData.Controller, Action = "UpdateChecklistQuestionRelatedQuestionMap" }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = GridData.Controller, Action = "DeleteChecklistQuestionRelatedQuestionMap" }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("ChecklistId", displayName:"Checklist"),
                                new GridEditLayoutColumn("OapChecklistRelatedQuestionId", displayName:"Related Question"),
                                new GridEditLayoutColumn("OapChecklistQuestionId",isVisible:false)
                            };

            gridData.FormLayout = new GridEditFormLayout(
                   gridData.LayoutColumns
                   , processLayout: i => {
                       i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                       i.Width = Unit.Percentage(100);
                   }
                   , columnCount: 2);

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["OapChecklistQuestionId"] = id;
                gridData.DefaultNewRowInitializeFields(e);
            };

            gridData.CallBackRoute = new { Controller = gridData.Controller, Action = gridData.Action, OapChecklistQuestionId = id, oapChecklistQuestionName = name };
        }
        private void InitializeSubChildTagGridData(GridData gridData, string createAction, string deleteAction, int id, string name)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (ViewData["ChecklistQuestionTags"] == null)
            {
                ViewData["ChecklistQuestionTags"] = GetQuestionTags();
            }

            var oapChecklistQuestionTags = ViewData["ChecklistQuestionTags"] as ObservableCollection<OapChecklistQuestionTag>;

            var oapChecklistQuestionTagsCombo = new GridCombo("OapChecklistQuestionTags", oapChecklistQuestionTags);
            oapChecklistQuestionTagsCombo.DisplayColumnName = "Tag";

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("OapChecklistQuestionTagId", displayName:"Tag", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapChecklistQuestionTagsCombo, width:100, editLayoutWidth:45),
                                new GridDisplayColumn("OapChecklistQuestionId", displayName:"Question", width:20, isVisible:false),
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = GridData.Controller, Action = "CreateChecklistQuestionTagMap"}),
                                //new GridRoute(GridRouteTypes.Update, new { Controller = GridData.Controller, Action = "UpdateChecklistQuestionTagMap" }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = GridData.Controller, Action = "DeleteChecklistQuestionTagMap" }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("OapChecklistQuestionTagId", displayName:"Tag"),
                                new GridEditLayoutColumn("OapChecklistQuestionId",isVisible:false)
                            };

            gridData.FormLayout = new GridEditFormLayout(
                gridData.LayoutColumns
                , i =>
                {
                    i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                    i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                }
                 , columnCount: 1);

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["OapChecklistQuestionId"] = id;
                gridData.DefaultNewRowInitializeFields(e);
            };

            gridData.CallBackRoute = new { Controller = gridData.Controller, Action = gridData.Action, OapChecklistQuestionId = id, oapChecklistQuestionName = name };
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            InitializeGridData(GridData, "CreateChecklistTopic", "UpdateChecklistTopic", "DeleteChecklistTopic");

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.SetDetailRowTemplateContent(c =>
            {
                var loapChecklistTopicName = (string)DataBinder.Eval(c.DataItem, GridData.DisplayColumnName);

                var loapChecklistTopicId = (int)DataBinder.Eval(c.DataItem, GridData.Key);

                ChildGridData.ToolBarOptions.SearchName = $"{ChildGridData.ToolBarOptions.SearchName}_{loapChecklistTopicId}";

                if (!c.Grid.IsEditing && !c.Grid.IsNewRowEditing)
                {
                    //Important to use RenderAction and Not Action todisplay the child grid rows
                    html.RenderAction(ChildGridData.Action, ChildGridData.Controller, new { oapChecklistTopicId = loapChecklistTopicId, oapChecklistTopicName = loapChecklistTopicName });
                }
            });

            //settings.ClientSideEvents.FocusedRowChanged = "OnFindingsFocusedRowChanged";

            int oapChecklistTopicId = (int)(ViewData[CurrentTopicId] ?? 0);
            string oapChecklistTopicName = (string)(ViewData[CurrentTopicName] ?? string.Empty);

            //Select the Current Row when it defined.
            if (oapChecklistTopicId != 0)
            {
                settings.Columns.Grid.Selection.SelectRowByKey(oapChecklistTopicId);
            }
        }
        public void ConfigureChild(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            int oapChecklistTopicId = (int)(ViewData[CurrentTopicId] ?? 0);
            string oapChecklistTopicName = (string)(ViewData[CurrentTopicName] ?? string.Empty);

            ChildGridData.ToolBarOptions.SearchName = $"{ChildGridData.ToolBarOptions.SearchName}_{oapChecklistTopicId}";

            InitializeChildGridData(ChildGridData, "CreateChecklistQuestion", "UpdateChecklistQuestion", "DeleteChecklistQuestion", oapChecklistTopicId, oapChecklistTopicName);

            ChildGridData.Name = $"{ChildGridData.Name}_{oapChecklistTopicName}_{oapChecklistTopicId}";
            //very sensitive to characters only use Unserscore.  using anyother charaters other than alphabets could cause grid display issues.
            settings.SettingsDetail.MasterGridName = $"{GridData.Name}"; // GridInformation.MasterGridName;

            settings.Configure(ChildGridData, html, viewContext);

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.SetDetailRowTemplateContent(c =>
            {
                var loapChecklistQuestionName = (string)DataBinder.Eval(c.DataItem, ChildGridData.DisplayColumnName);

                var loapChecklistQuestionId = (int)DataBinder.Eval(c.DataItem, ChildGridData.Key);

                //Calling SubChild Grids in Child Grid
                SubChildRelatedQuestionGridData.ToolBarOptions.SearchName = $"{SubChildRelatedQuestionGridData.ToolBarOptions.SearchName}_{loapChecklistQuestionId}";
                SubChildTagGridData.ToolBarOptions.SearchName = $"{SubChildTagGridData.ToolBarOptions.SearchName}_{loapChecklistQuestionId}";

                if (!c.Grid.IsEditing && !c.Grid.IsNewRowEditing)
                {
                    //Important to use RenderAction and Not Action todisplay the child grid rows
                    html.RenderAction(SubChildRelatedQuestionGridData.Action, SubChildRelatedQuestionGridData.Controller, new { oapChecklistQuestionId = loapChecklistQuestionId, oapChecklistQuestionName = loapChecklistQuestionName });

                    html.RenderAction(SubChildTagGridData.Action, SubChildTagGridData.Controller, new { oapChecklistQuestionId = loapChecklistQuestionId, oapChecklistQuestionName = loapChecklistQuestionName });
                }
            });
        }
        public void ConfigureRelatedQuestionSubChild(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            int oapChecklistTopicId = (int)(ViewData[CurrentTopicId] ?? 0);
            string oapChecklistTopicName = (string)(ViewData[CurrentTopicName] ?? string.Empty);

            int oapChecklistQuestionId = (int)(ViewData[CurrentQuestionId] ?? 0);
            string oapChecklistQuestionName = (string)(ViewData[CurrentQuestionName] ?? string.Empty);

            SubChildRelatedQuestionGridData.ToolBarOptions.SearchName = $"{SubChildRelatedQuestionGridData.ToolBarOptions.SearchName}_{oapChecklistQuestionId}";
            InitializeSubChildRelatedQuestionGridData(SubChildRelatedQuestionGridData, "CreateChecklistRelatedQuestionMap", "DeleteChecklistRelatedQuestionMap", oapChecklistQuestionId, oapChecklistQuestionName);
            //SubChildRelatedQuestionGridData.Name = $"{SubChildRelatedQuestionGridData.Name}_{oapChecklistQuestionName}_{oapChecklistQuestionId}";
            //very sensitive to characters only use Unserscore.  using anyother charaters other than alphabets could cause grid display issues.
            settings.SettingsDetail.MasterGridName = $"{ChildGridData.Name}_{oapChecklistTopicName}_{oapChecklistTopicId}"; // GridInformation.MasterGridName;
            settings.Configure(SubChildRelatedQuestionGridData, html, viewContext);
            settings.SettingsDetail.ShowDetailRow = false;
        }
        public void ConfigureTagSubChild(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            int oapChecklistTopicId = (int)(ViewData[CurrentTopicId] ?? 0);
            string oapChecklistTopicName = (string)(ViewData[CurrentTopicName] ?? string.Empty);

            int oapChecklistQuestionId = (int)(ViewData[CurrentQuestionId] ?? 0);
            string oapChecklistQuestionName = (string)(ViewData[CurrentQuestionName] ?? string.Empty);

            SubChildTagGridData.ToolBarOptions.SearchName = $"{SubChildTagGridData.ToolBarOptions.SearchName}_{oapChecklistQuestionId}";
            InitializeSubChildTagGridData(SubChildTagGridData, "CreateChecklistQuestionTagMap", "DeleteChecklistQuestionTagMap", oapChecklistQuestionId, oapChecklistQuestionName);
            SubChildTagGridData.Name = $"{SubChildTagGridData.Name}_{oapChecklistQuestionName}_{oapChecklistQuestionId}";
            //very sensitive to characters only use Unserscore.  using anyother charaters other than alphabets could cause grid display issues.
            settings.SettingsDetail.MasterGridName = $"{ChildGridData.Name}_{oapChecklistTopicName}_{oapChecklistTopicId}"; // GridInformation.MasterGridName;
            settings.Configure(SubChildTagGridData, html, viewContext);
            settings.SettingsDetail.ShowDetailRow = false;
            settings.CommandColumn.ShowEditButton = false;
        }
        public OapChecklistTopicsController() : base()
        {
            RegisterClient(new List<Type>()
                            { typeof(OapChecklistTopicClient)
                                ,typeof(OapChecklistQuestionClient)
                                ,typeof(OapChecklistGroupClient)
                                ,typeof(OapChecklistSubGroupClient)
                                ,typeof(OapChecklistQuestionTagClient)
                                ,typeof(OapChecklistQuestionTagMapClient)
                                ,typeof(OapChecklistQuestionDataTypeClient)
                                ,typeof(OapChecklistQuestionRelatedQuestionMapClient)
                                ,typeof(OapFrequencyClient)
                                ,typeof(OapChecklistClient)
                            });
        }
        public async Task<ActionResult> Index()
        {
            var response = await GetClient<OapChecklistTopicClient>().GetAllAsync(GetAllModelsCorp());
            return View("OapChecklistTopic", response.Result.Data);
        }
        public async Task<ActionResult> Topics()
        {
            var response = await GetClient<OapChecklistTopicClient>().GetAllAsync(GetAllModelsCorp());

            return PartialView("OapChecklistTopicPartial", response.Result.Data);
        }
        public ActionResult GetOapChecklistSubGroups(int OapChecklistGroupId, string textField, string valueField)
        {
            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = textField;
                p.ValueField = valueField;
                var oapChecklistSubGroups = GetClient<OapChecklistGroupClient>().GetAsync(OapChecklistGroupId).Result.Result.Data?.SubGroups ?? new ObservableCollection<OapChecklistSubGroup>();
                p.BindList(oapChecklistSubGroups);
            });
        }
        public ActionResult GetChecklistQuestions(int ChecklistId, string textField, string valueField)
        {
            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = textField;
                p.ValueField = valueField;
                var oapChecklistQuestions = GetClient<OapChecklistQuestionClient>().GetAllChecklistQuestionsAsync(ChecklistId).Result.Result.Data ?? new ObservableCollection<OapChecklistQuestion>();
                p.BindList(oapChecklistQuestions);
            });
        }
        public ActionResult Questions(int? oapChecklistTopicId, string oapChecklistTopicName)
        {
            if (!oapChecklistTopicId.HasValue)
            {
                return new EmptyResult();
            }

            //Set View Data
            ViewData[CurrentTopicId] = oapChecklistTopicId.Value;
            ViewData[CurrentTopicName] = oapChecklistTopicName;

            var response = GetClient<OapChecklistTopicClient>().GetAsync(oapChecklistTopicId ?? 0).Result;

            return PartialView("OapChecklistQuestionsPartial", response.Result.Data.Questions);
        }
        public ActionResult QuestionTags(int? oapChecklistQuestionId, string oapChecklistQuestionName)
        {
            if (!oapChecklistQuestionId.HasValue)
            {
                return new EmptyResult();
            }
            // Set View Data
            ViewData[CurrentQuestionId] = oapChecklistQuestionId.Value;
            ViewData[CurrentQuestionName] = oapChecklistQuestionName;

            var response = GetClient<OapChecklistQuestionClient>().GetAsync(oapChecklistQuestionId ?? 0).Result;
            return PartialView("OapChecklistQuestionTagMapPartial", response.Result.Data.OapChecklistQuestionTagMaps);
        }
        public ActionResult RelatedQuestions(int? oapChecklistQuestionId, string oapChecklistQuestionName)
        {
            if (!oapChecklistQuestionId.HasValue)
            {
                return new EmptyResult();
            }

            ViewData[CurrentQuestionId] = oapChecklistQuestionId.Value;
            ViewData[CurrentQuestionName] = oapChecklistQuestionName;

            if (ViewData["Checklists"] == null)
            {
                ViewData["Checklists"] = GetOapChecklists();
            }

            var response = GetClient<OapChecklistQuestionClient>().GetAsync(oapChecklistQuestionId ?? 0).Result;
            var relatedQuestions = response.Result.Data.OapChecklistRelatedQuestionMaps.ToList();

            var oapChecklists = ViewData["Checklists"] as ObservableCollection<OapChecklist>;

            var allChecklistQuestions = (from c in oapChecklists
                                         from g in c.Groups
                                         from s in g.SubGroups
                                         from t in s.Topics                                                                             
                                         from q in t.Questions ?? new ObservableCollection<OapChecklistQuestion>()
                                         where t.Questions != null                                         
                                         select new { Checklist = c, Question = q }).ToList();

            relatedQuestions.ForEach(rq =>
            {
                rq.ChecklistId = allChecklistQuestions.FirstOrDefault(cq => cq.Question.Id == rq.OapChecklistRelatedQuestionId)?.Checklist.Id;
            });
            return PartialView("OapChecklistRelatedQuestionMapPartial", relatedQuestions);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChecklistTopic(OapChecklistTopic model)
        {            
            // TODO: Add insert logic here
            var response = await GetClient<OapChecklistTopicClient>().AddTopicAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklistTopic(OapChecklistTopic model)
        {          
            // TODO: Add update logic here
            var response = GetClient<OapChecklistTopicClient>().UpdateTopicAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistTopic(OapChecklistTopic model)
        {
            if (model.Id > 0)
            {
                var response = await GetClient<OapChecklistTopicClient>().DeleteTopicAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CreateChecklistQuestion(OapChecklistQuestion model)
        {
            if (model.OapChecklistTopicId > 0)
            {
                ViewData[CurrentTopicId] = model.OapChecklistTopicId;               
                var response = await GetClient<OapChecklistQuestionClient>().AddQuestionAsync(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateChecklistQuestion(OapChecklistQuestion model)
        {            
            // TODO: Add update logic here
            var response = GetClient<OapChecklistQuestionClient>().UpdateQuestionAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistQuestion(OapChecklistQuestion model)
        {
            if (model.Id > 0)
            {
                var response = await GetClient<OapChecklistQuestionClient>().DeleteQuestionAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateChecklistQuestionTagMap(OapChecklistQuestionTagMap model)
        {           
            var response = await GetClient<OapChecklistQuestionTagMapClient>().AddQuestionTagMapAsync(model);
            var res = GetClient<OapChecklistQuestionClient>().GetAsync(model.OapChecklistQuestionId).Result;
            //return PartialView("OapChecklistQuestionTagMapPartial", res.Result.Data.OapChecklistQuestionTagMaps);  
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult UpdateChecklistQuestionTagMap(OapChecklistQuestionTagMap model)
        //{
        //    var response = GetClient<OapChecklistQuestionTagMapClient>().UpdateQuestionTagMapAsync(model).Result;

        //    var res = GetClient<OapChecklistQuestionClient>().GetAsync(model.OapChecklistQuestionId).Result;

        //    return PartialView("OapChecklistQuestionTagMapPartial", res.Result.Data.OapChecklistQuestionTagMaps);
           
        //    //return RedirectToAction("Index");

        //}

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> DeleteChecklistQuestionTagMap(OapChecklistQuestionTagMap model)
        {
            if (model.Id > 0)
            {
                var response = await GetClient<OapChecklistQuestionTagMapClient>().DeleteQuestionTagMapAsync(model.Id);
            }
            //var res = GetClient<OapChecklistQuestionClient>().GetAsync(model.OapChecklistQuestionId).Result;
            //return PartialView("OapChecklistQuestionTagMapPartial", res.Result.Data.OapChecklistQuestionTagMaps); 
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateChecklistQuestionRelatedQuestionMap(OapCheckListQuestionRelatedQuestionMap model)
        {           
            var response = await GetClient<OapChecklistQuestionRelatedQuestionMapClient>().AddQuestionRelatedQuestionMapAsync(model);
            //var res = GetClient<OapChecklistQuestionRelatedQuestionMapClient>().GetAllAsync(model.OapChecklistQuestionId).Result.Result.Data;
            //return PartialView("OapChecklistRelatedQuestionMapPartial", res);
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult UpdateChecklistQuestionRelatedQuestionMap(OapCheckListQuestionRelatedQuestionMap model)
        //{
        //    var response = GetClient<OapChecklistQuestionRelatedQuestionMapClient>().UpdateQuestionRelatedQuestionMapAsync(model).Result;

        //    //var res = GetClient<OapChecklistQuestionRelatedQuestionMapClient>().GetAllAsync(model.OapChecklistQuestionId).Result.Result.Data;
        //    //return PartialView("OapChecklistRelatedQuestionMapPartial", res);

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> DeleteChecklistQuestionRelatedQuestionMap(OapCheckListQuestionRelatedQuestionMap model)
        {
            if (model.Id > 0)
            {
                var response = await GetClient<OapChecklistQuestionRelatedQuestionMapClient>().DeleteQuestionRelatedQuestionMapAsync(model.Id);
            }
            //var res = GetClient<OapChecklistQuestionRelatedQuestionMapClient>().GetAllAsync(model.Id).Result.Result.Data;
            //return PartialView("OapChecklistRelatedQuestionMapPartial",res);
            return RedirectToAction("Index");
        }

        ~OapChecklistTopicsController()
        {
            Client.Dispose();
        }
    }
}

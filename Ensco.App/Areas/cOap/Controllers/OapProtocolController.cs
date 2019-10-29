using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using Ensco.Utilities;
using Ensco.Models;
using System.Collections.Generic;
using DevExpress.Utils;
using Ensco.App.Areas.Oap.Controllers;
using Ensco.Irma.Oap.WebClient.Corp;
using DevExpress.Web.Mvc.UI;
using Ensco.Irma.Oap.Web.Oap.Controllers;
using Ensco.Irma.Oap.Common.Models;
using System.Web.UI;
using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
using Ensco.Irma.Services;
using Ensco.Irma.Models;
using Ensco.Services;
using Ensco.App.Areas.Oap.Models;
using Ensco.Irma.Oap.Common.Extensions;
using Ensco.App.Areas.cOap.Extensions;
using Ensco.OAP.Services;
using Ensco.OAP.Models;
using System.Text;
using DevExpress.Web.Data;
using Ensco.App.App_Start;
using System.Collections.ObjectModel;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System.Web;

namespace Ensco.App.Areas.cOap.Controllers
{
    public class OapProtocolController : OapCorpGridController
    {
        protected OapAuditClient OapAuditClient { get; }
        protected OapChecklistFindingClient OapChecklistFindingClient { get; }
        protected string BaseController { get; set; }

        public OapProtocolController()
        {
            OapAuditClient = new OapAuditClient(GetApiBaseUrl(), Client);

            OapChecklistFindingClient = new OapChecklistFindingClient(GetApiBaseUrl(), Client);

            RegisterClient(new List<Type>()
                            {
                                typeof(PeopleClient),                                
                                typeof(OapChecklistClient)
                            });

            BaseController = "OapProtocol";
        }

        [SiteMapTitle("RigChecklistUniqueId")]
        public ActionResult List(Guid id)
        { 
           ViewBag.Title = Translate("Protocol To Audit");

            Protocol = OapAuditClient.GetCompleteProtocolAsync(id).Result?.Result?.Data;

            return View("GenericList", Protocol);
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {

        }

        public void ConfigureTabs(PageControlSettings settings, HtmlHelper html, ViewContext viewContext)
        {    
            settings.Name = "genericChecklistPage";
            settings.ActivateTabPageAction = ActivateTabPageAction.Click;
            settings.EnableHotTrack = true;
            settings.SaveStateToCookies = true;
            settings.TabAlign = TabAlign.Left;
            settings.TabPosition = TabPosition.Top;
            settings.Width = Unit.Percentage(100);
            settings.CallbackRouteValues = new { Controller = BaseController, Action = "PageControl" };  
            string setPlanningTab = Session["isChecklistIdClick"]?.ToString();
            if (!string.IsNullOrEmpty(setPlanningTab) && setPlanningTab == "True")
            {
                settings.ClientSideEvents.Init = "function (s,e) { s.SetActiveTabIndex(0); }";
                Session["isChecklistIdClick"] = false;
            }           

            var checklistPlanningExecution = settings.TabPages.Add("ChecklistPlanning");
            checklistPlanningExecution.Text = Translate("Planning");
            checklistPlanningExecution.Name = "tabPlanning";
            checklistPlanningExecution.SetContent(() =>
            {
                html.RenderPartial("GenericlistPlanningPartial", Protocol);
                viewContext.Writer.Write("<br/>");
                html.RenderAction("ProtocolAssessorsPartial");
            });

            var checklistExecution = settings.TabPages.Add("ChecklistExecution");
            checklistExecution.Text = Translate("Execution");
            checklistExecution.SetContent(() =>
            {
                var models = Protocol?.ToFlattenedModels();
                viewContext.ViewBag.Protocol = Protocol;
                viewContext.ViewBag.OapChecklistClient = GetClient<OapChecklistClient>();
                html.RenderPartial("GenericlistExecutionPartial", models);

                foreach (var c in Protocol?.Comments.Distinct())
                {
                    viewContext.Writer.Write("<br/>");
                    html.RenderPartial("GenericlistCommentPartial", c);
                }
            });

            var checklistMonitoring = settings.TabPages.Add("ChecklistMonitoring");
            checklistMonitoring.Text = Translate("Monitoring");
            checklistMonitoring.Name = "tabMonitoring";
            var sourceForm = "Oap";

            checklistMonitoring.SetContent(() =>
            {
                html.RenderAction("Control", "Comments", new { Area = "IRMA", sourceForm, sourceFormId = Protocol.Id.ToString() });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("Control", "LessonsLearned", new { Area = "IRMA", sourceForm, SourceFormId = Protocol.RigChecklistUniqueId });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("Control", "Review", new { Area = "IRMA", sourceForm, SourceFormId = Protocol.RigChecklistUniqueId });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("Display", "Findings", new { questions = Protocol?.Questions });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("DisplayRelatedCapa", new { checklistId = Protocol?.Id });
                viewContext.Writer.Write("<br/>");
                html.RenderPartial("GenericlistRelatedSearchQueryPartial", new RelatedSearchQueryModel());
                viewContext.Writer.Write("<br/>");
                html.RenderPartial("GenericlistRelatedSearchPartial",  new List<OapSearchCheckListQuestionFlatModel>());
            });
        }

        [HttpPost]
        public ActionResult PageControl(string controlType)
        {
            return PartialView("GenericlistPageControlPartial", Protocol);
        }

        public void ConfigureGenericlist(HtmlHelper html, ViewContext viewContext)
        {
            var help = Protocol?.OapChecklist?.Help;

            if (help != null && help != string.Empty)
            {
                html.DevExpress().Button(s =>
                {
                    s.Name = "PageHelp";
                    s.Text = "";
                    s.Images.Image.Url = "~/images/Help-icon.png";
                    s.ControlStyle.BackColor = System.Drawing.Color.Transparent;
                    s.Width = 50;
                    s.Height = 50;
                    s.ImagePosition = ImagePosition.Right;
                    s.Style.Add("float", "right");
                }).GetHtml();

                html.DevExpress().PopupControl(settings =>
                {
                    settings.Name = "Help Popup";
                    settings.PopupElementID = "PageHelp";

                    settings.AllowResize = true;
                    settings.ResizingMode = ResizingMode.Live;
                    settings.ShowSizeGrip = ShowSizeGrip.Auto;

                    settings.Width = 700;

                    settings.ShowFooter = false;
                    settings.ShowHeader = false;
                    settings.PopupHorizontalAlign = PopupHorizontalAlign.OutsideLeft;
                    settings.PopupVerticalAlign = PopupVerticalAlign.Middle;

                    settings.ScrollBars = ScrollBars.Auto;

                    settings.SetContent(() =>
                    {
                        viewContext.Writer.Write(Protocol?.OapChecklist?.Help ?? string.Empty);
                    });
                }).GetHtml();
            }
        }

        public virtual bool EnableSave(RigOapChecklist protocol)
        {  
            var isCompleted = protocol.Status.Equals(GridConstants.ChecklistStatus.Completed.ToString(), StringComparison.InvariantCultureIgnoreCase);

            return isCompleted;            
        }

        #region Planning

        public void ConfigurePlanning(FormLayoutSettings<RigOapChecklist> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            var lkpRigList = Ensco.Utilities.UtilitySystem.GetLookupList("Rig");
            if (lkpRigList == null)
            {
                lkpRigList = new LookupListModel<dynamic>();
                lkpRigList.Items = new List<object>();
            }

            var items = (lkpRigList.Items as List<object>)?.Cast<RigModel>();

            var item = items?.SingleOrDefault(it => it.Id.ToString() == Protocol.RigId);

            if (item != null)
                Protocol.RigName = item.Name;



            settings.Name = "planningLayout";
            settings.UseDefaultPaddings = false;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            settings.Items.AddGroupItem(g =>
            {
                g.Caption = Translate("Planning");
                g.ColCount = 5;

                g.Items.Add(i =>
                {

                    i.ShowCaption = DefaultBoolean.False;
                    i.Border.BorderStyle = BorderStyle.None;
                    i.CssClass = "buttonToolbar";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtension().Button(s =>
                    {
                        s.Name = "savePlanningLayout";
                        s.Text = Translate("Save");
                        s.UseSubmitBehavior = false;
                        s.Width = Unit.Pixel(100);                        
                        s.Enabled = Protocol.Status == "Open" &&  Protocol.Assessors.Any(v => v.UserId == UtilitySystem.CurrentUserId);
                    });
                });

                g.Items.Add(i =>
                {
                    i.ShowCaption = DefaultBoolean.False;
                    i.Name = "EmptyLayout";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.SetNestedContent(() => viewContext.Writer.Write("&nbsp;"));
                });

                g.Items.Add(m => m.RigChecklistUniqueId, i =>
                {
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.Width = Unit.Percentage(20);
                    i.Caption = Translate("Protocol Id");
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Protocol.RigChecklistUniqueId.ToString();
                    });
                });

                g.Items.Add(m => m.Title, i =>
                {
                    i.Caption = Translate("OAP Title");
                    i.Width = Unit.Percentage(40);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Height = Unit.Percentage(10);
                    i.NestedExtensionSettings.Enabled = Protocol.Status == "Open";
                });

                g.Items.Add(m => m.OapType, i =>
                {
                    i.Caption = Translate("OAP Type");
                    i.Width = Unit.Percentage(25);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Protocol.OapType;
                    });
                });

                g.Items.Add(m => m.Status, i =>
                {
                    i.Caption = Translate("Status");
                    i.Width = Unit.Percentage(15);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Protocol.Status;
                    });
                });

                g.Items.Add(m => m.ChecklistDateTime, i =>
                {
                    i.Caption = Translate("Assessment Date & Time");
                    i.Width = Unit.Percentage(30);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit;
                    var nsettings = (DateEditSettings)i.NestedExtensionSettings;
                    nsettings.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Enabled = Protocol.Status == "Open";
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                });

                g.Items.Add(m => m.ChecklistDateTimeCompleted, i =>
                {
                    i.Caption = Translate("Date & Time");
                    i.Width = Unit.Percentage(30);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit;
                    var nsettings = (DateEditSettings)i.NestedExtensionSettings;
                    nsettings.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Enabled = Protocol.Status == "Open";
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                });

                g.Items.Add(m => m.RigName, i =>
                {
                    i.Caption = Translate("Rig");
                    i.Width = Unit.Percentage(20);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Protocol.RigName;
                    });
                });

                g.Items.Add(m => m.OapChecklist.OapType.IsIsm, i =>
                {
                    i.Caption = Translate("ISM Certified");
                    i.Width = Unit.Percentage(20);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Protocol.OapChecklist.OapType.IsIsm.ToString();
                    });
                });

            });
        }

        #region Assessors
        public ActionResult ProtocolAssessorsPartial()
        {
            var modelList = MapToViewModel(Protocol.Assessors, perItemAction: perItemMapping);
            ViewBag.RigChecklist = Protocol;

            return PartialView("GenericlistAssessorsPartial", modelList);
        }
        private Action<RigOapChecklistAssessor, ChecklistAssessorGridViewModel> perItemMapping = (assessor, viewModel) =>
        {
            UserModel user = ServiceSystem.GetUser(assessor.UserId);
            if (user == null) return;
            var department = ServiceSystem.GetDeparment(user.Department ?? 0);
            var position = ServiceSystem.GetUserPosition(user.Id);
            TourModel tour = null;
            if (assessor.TourId.HasValue)
                tour = ServiceSystem.GetTour(assessor.TourId.Value);

            viewModel.Name = user.DisplayName;
            viewModel.Department = department?.Name;
            viewModel.Position = position?.Name;
            viewModel.Tour = tour?.Name;
        };
        public ActionResult CreateAssessor(RigOapChecklistAssessor model)
        {
            if (model.IsLead && Protocol.Assessors.Any(a => a.IsLead))
                ViewData["AssessorsErrors"] = "There can be only one lead assessor at a time";
            else if (Protocol.Assessors.Any(a => a.UserId == model.UserId))
                ViewData["AssessorsErrors"] = "This person is already an assessor";
            else                 
                Protocol = OapAuditClient.AddAssessorAsync(Protocol.Id, model).Result?.Result?.Data;

            return ProtocolAssessorsPartial();
        }

        //[ValidateInput(false)]
        public ActionResult DeleteAssessor(RigOapChecklistAssessor model)
        {
            var protocol = Protocol;
            if (protocol != null)
            {
                try
                {
                    var removeModel = protocol.Assessors.SingleOrDefault(a => a.Id == model.Id || a.UserId == model.UserId);
                    if (removeModel != null)
                    {
                        protocol.Assessors.Remove(removeModel);
                        //var existingAssessor = protocol.VerifiedBy.SingleOrDefault(v => v.UserId == removeModel.UserId && v.Role.Equals(GridConstants.VerifierRole.Assessor.ToString()));
                        //if (existingAssessor != null)
                        //{
                        //    protocol.VerifiedBy.Remove(existingAssessor);
                        //}

                        //Protocol = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), checklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.AssessorsErrorsKey, ViewData);
                    }
                }
                catch
                {
                }
            }

            return ProtocolAssessorsPartial();
        }
        //private bool UpdatePlanningLayoutColumns(RigOapChecklist checklist, RigOapChecklist planning, int? locationId)
        //{
        //    var updated = false;

        //    if (checklist == null)
        //    {
        //        return false;
        //    }

        //    if (locationId.HasValue && checklist.LocationId != locationId)
        //    {
        //        checklist.LocationId = locationId.Value;
        //        updated = true;
        //    }

        //    if (checklist.JobId != planning.JobId)
        //    {
        //        checklist.JobId = planning.JobId;
        //        updated = true;
        //    }

        //    if (CompareValues(checklist.Title, planning.Title))
        //    {
        //        checklist.Title = planning.Title;
        //        updated = true;
        //    }

        //    if (CompareValues(checklist.Description, planning.Description))
        //    {
        //        checklist.Description = planning.Description;
        //        updated = true;
        //    }
        //    return updated;
        //}

        //private bool CompareValues(string old, string current)
        //{
        //    if (!string.IsNullOrEmpty(old) && !string.IsNullOrEmpty(current))
        //    {
        //        return !old.Equals(current, StringComparison.InvariantCultureIgnoreCase);
        //    }
        //    else if (string.IsNullOrEmpty(old) && !string.IsNullOrEmpty(current))
        //    {
        //        return true;
        //    }

        //    return false;
        //}
        #endregion


        #endregion

        #region Execution 
        //Configures the execution tab to display the list

        [HttpPost]
        public ActionResult DisplayExecutionGrid()
        {
            return PartialView("GenericlistExecutionPartial", Protocol?.ToFlattenedModels());
        }

        public ActionResult DisplayExecutionChecklistGrid(RigOapChecklist protocol)
        {
            Protocol = protocol;
            return PartialView("GenericlistExecutionPartial", Protocol?.ToFlattenedModels());
        }

        public ActionResult DisplayExecutionComments(RigOapChecklistComment Comments)
        {
            return PartialView("GenericlistCommentPartial", Comments);
        }

        public virtual void ConfigureExecution(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistExecutionGridData(html, viewContext);

            settings.Configure(gridData, html, viewContext);

            settings.SettingsBehavior.MergeGroupsMode = GridViewMergeGroupsMode.Disabled;

            settings.ClientSideEvents.BeginCallback = "function (s, e) { checklistExecutionGridBeginCallback (s,e) }";
            settings.ClientSideEvents.BatchEditStartEditing = "function (s,e) { if (e.focusedColumn.fieldName.substring(0, 7) !== 'YesNoNa') { e.cancel = true;} }";
            settings.ClientSideEvents.DetailRowExpanding = "function (s, e) {  isExecutionGridDetailRowExpanded[e.visibleIndex] = true;  }";
            settings.ClientSideEvents.DetailRowCollapsing = "function (s, e) {  isExecutionGridDetailRowExpanded[e.visibleIndex] = false;  }";

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.ShowDetailButtons = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.SetDetailRowTemplateContent(c => ConfigureExecutionQuestionComments(html, viewContext, c));
            settings.ClientSideEvents.BatchEditConfirmShowing = "function(s, e) {e.cancel = true;}";             

            settings.CommandButtonInitialize = (o, e) =>
            {
                if ((e.ButtonType == ColumnCommandButtonType.Update) || (e.ButtonType == ColumnCommandButtonType.Cancel))
                {
                    e.Visible = false;
                }
            };

            settings.PreRender = ExpandDetailRowsIfNoIsSelected;
            settings.BeforeGetCallbackResult = ExpandDetailRowsIfNoIsSelected;

            settings.PreRender = (sender, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)sender;
                gridView.SortBy(gridView.Columns["Topic"], DevExpress.Data.ColumnSortOrder.Ascending);
            };

        }
        protected virtual void ExpandDetailRowsIfNoIsSelected(object sender, EventArgs args)
        {
            MVCxGridView gridView = (MVCxGridView)sender;

            for (int i = 0; i < gridView.VisibleRowCount; i++)
            {
                string answer = gridView.GetRowValues(i, "YesNoNa0")?.ToString();
                if (answer == "N")
                    gridView.DetailRows.ExpandRow(i);

                var question = Protocol.ToFlattenedModels().Where(c => c.RigQuestionId == (Guid)(gridView.GetRowValues(i, "RigQuestionId"))).FirstOrDefault();

                IOAPServiceDataModel dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
                IEnumerable<CommentModel> comments = dataModel.GetItems(string.Format("QuestionId=\"{0}\" AND SourceFormId=\"{1}\"", question.RigQuestionId, Protocol.Id), "Id").Cast<CommentModel>();
                if (comments.Count() > 0)
                {
                    gridView.DetailRows.ExpandRow(i);
                }
            }
        }

        protected virtual void ConfigureExecutionQuestionComments(HtmlHelper html, ViewContext viewContext, GridViewDetailRowTemplateContainer container)
        {
            var rigQuestionId = DataBinder.Eval(container.DataItem, "RigQuestionId");
            var question = DataBinder.Eval(container.DataItem, "Question");
            html.RenderAction("InlineControl", new { Controller = "Comments", Area = "IRMA", SourceForm = "OAP", SourceFormId = Protocol.Id, QuestionId = rigQuestionId, QuestionText = question });
        }
        public virtual void ConfigureComment(FormLayoutSettings<RigOapChecklistComment> settings, HtmlHelper html, ViewContext viewContext, RigOapChecklistComment model)
        {
            settings.Name = $"cl_{model.Id}";
            settings.Attributes.Add("class", "comments");
            settings.UseDefaultPaddings = false;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            settings.Items.AddGroupItem(g =>
            {
                g.Caption = Translate($"{model.OapChecklistComment.Question}-{model.OapChecklistComment.SubQuestion}", "OapQuestions");
                g.ColCount = 1;
                var item = g.Items.Add(m => m.Comment, i =>
                {
                    i.Name = $"{model.Id}:{model.OapChecklistComment.Id}";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.Memo;
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Height = Unit.Pixel(71);
                });
                item.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            });
        }

        protected virtual GridData InitializeChecklistExecutionGridData(HtmlHelper html, ViewContext viewContext, string batchUpdateAction = "UpdateQuestions")
        {
            var toolbarOptions = new GridToolBarOptions(false);

            var commandOptions = new GridCommandButtonOptions(false);

            var gridData = new GridData(GridConstants.rigChecklistExecutionGrid, BaseController, "List", key: "QuestionId", initializeCallBack: true, callBackRoute: new { Controller = BaseController, Action = "DisplayExecutionGrid" }, toolbarOptions: toolbarOptions, commandButtonOptions: commandOptions, showPager: false)
            {
                Title = "Checklist",
                GroupSetting = new GridGroupBehaviorSetting(groupFormatForMergedGroup: "{1}")
            };

            var radioButtons = GetYesNoNaValues();

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("Group", order:10, allowHeaderFilter: DefaultBoolean.False, allowSort: DefaultBoolean.False,columnAction: c => {
                    c.FieldName = "Group";
                    c.Caption = "Group";
                    c.GroupIndex = 0;
                    c.ReadOnly = true;
                    c.Settings.AllowGroup = DefaultBoolean.True;
                    c.Settings.AllowSort = DefaultBoolean.True;
                    c.SetGroupRowTemplateContent(container => {
                        var graphic = (byte[]) DataBinder.Eval(container.DataItem, "Graphic");
                        var groupHeader = DataBinder.Eval(container.DataItem, "Group").ToString();
                        if (graphic.Length > 0)
                        {
                            html.DevExpress().BinaryImage(s => {
                                s.Properties.ImageWidth = Unit.Pixel(16);
                                s.Properties.ImageHeight = Unit.Pixel(16);
                                s.ContentBytes = graphic;
                                s.Properties.Caption = groupHeader;
                                s.Properties.CaptionSettings.Position = EditorCaptionPosition.Right;
                            }).Render();
                        }
                        else
                        {
                            html.ViewContext.Writer.Write(string.Format("<b>{0}</b>",groupHeader));
                        }
                    });
                }),
                new GridDisplayColumn("SubGroup", order:10, allowHeaderFilter: DefaultBoolean.False, allowSort: DefaultBoolean.False,columnAction: c => {
                    c.FieldName = "SubGroup";
                    c.Caption = "SubGroup";
                    c.GroupIndex = 1;
                    c.SortIndex = 1;
                    c.ReadOnly = true;
                    c.Settings.AllowGroup = DefaultBoolean.True;
                    c.Settings.AllowSort = DefaultBoolean.True;
                    c.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                    c.SetGroupRowTemplateContent(container => {
                        var subGroupHeader = DataBinder.Eval(container.DataItem, "SubGroup").ToString();
                        html.ViewContext.Writer.Write(string.Format("<b>{0}</b>",subGroupHeader));
                    });
                }),
                new GridDisplayColumn("Topic", displayName: "Category", order:30, width:15, columnType: MVCxGridViewColumnType.TextBox , isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False,columnAction: (c) => {
                    c.FieldName = "Topic";
                    c.Settings.AllowCellMerge = DefaultBoolean.False;
                    c.Width =  Unit.Percentage(15);
                }),
                new GridDisplayColumn("Question", order:40,columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False, columnAction: c => {
                    c.FieldName = "Question";
                    c.Caption = "Verification";
                    c.Width = Unit.Percentage(45);
                }),
                new GridDisplayColumn("Comment", displayName: "Comment", order:50,width:20, allowHeaderFilter: DefaultBoolean.False, isReadOnly: false, allowSort: DefaultBoolean.False,  columnAction: (c) => {
                    c.FieldName = "Comment";
                    c.Caption = "Comment".Translate();
                    c.Settings.AllowSort = DefaultBoolean.False;
                    c.EditFormSettings.Visible = DefaultBoolean.False;
                    c.Width =  Unit.Percentage(5);
                    c.SetDataItemTemplateContent(ic =>
                     {
                         html.DevExpress().Button(s =>
                         {
                             s.Name = $"CommentButton_{DataBinder.Eval(ic.DataItem, "QuestionId")}";
                             s.Text = "";
                             s.Images.Image.Url = "~/images/Edit icon.png";
                             s.ControlStyle.BackColor = System.Drawing.Color.Transparent;
                             s.Width = 20;
                             s.Height = 20;
                             s.UseSubmitBehavior = false;
                             s.ClientSideEvents.Click = $"function (s,e) {{ var cell = {GridConstants.rigChecklistExecutionGrid}.GetFocusedCell();   {GridConstants.rigChecklistExecutionGrid}.ExpandDetailRow(cell.rowVisibleIndex); }}";
                         }).GetHtml();

                     });
                }),
                new GridDisplayColumn("QuestionHelp", displayName: " ", order: 60, width: 20, allowHeaderFilter: DefaultBoolean.False, isReadOnly: true, allowSort: DefaultBoolean.False, columnAction: (c) =>
                {
                    c.FieldName = "QuestionHelp";
                    c.Caption = " ";
                    c.Settings.AllowSort = DefaultBoolean.False;
                    c.EditFormSettings.Visible = DefaultBoolean.False;
                    c.Width =  Unit.Percentage(5);

                    c.SetDataItemTemplateContent(ic =>
                    {
                        var help = DataBinder.Eval(ic.DataItem, "QuestionHelp")?.ToString();

                        if (help != null && help != string.Empty)
                        {

                            var rigQuestionId = DataBinder.Eval(ic.DataItem, "QuestionId");

                            html.DevExpress().Button(s =>
                            {
                                s.Name = $"HintButton_{rigQuestionId}";
                                s.Text = "";
                                s.Images.Image.Url = "~/images/Help-icon.png";
                                s.ControlStyle.BackColor = System.Drawing.Color.Transparent;
                                s.Width = 20;
                                s.Height = 20;
                            }).GetHtml();

                            html.DevExpress().PopupControl(settings =>
                            {
                                settings.Name = $"Hint_{rigQuestionId}";
                                settings.PopupElementID = $"HintButton_{rigQuestionId}";
                                settings.Height = 100;
                                settings.MinHeight = 100;
                                settings.MinWidth = 100;
                                settings.Width = 100;
                                settings.ShowCloseButton = true;
                                settings.PreRender = (sender, e) =>
                                {
                                    var popup = (ASPxPopupControl)sender;
                                    popup.ShowShadow = true;
                                };
                                settings.ShowFooter = false;
                                settings.ShowHeader = false;
                                settings.PopupHorizontalAlign = PopupHorizontalAlign.Center;
                                settings.PopupVerticalAlign = PopupVerticalAlign.Above;

                                settings.ScrollBars = ScrollBars.Auto;

                                settings.SetContent(() =>
                                {
                                    viewContext.Writer.Write(DataBinder.Eval(ic.DataItem, "QuestionHelp"));
                                });

                            }).GetHtml();
                        }
                    });
                }),
                new GridDisplayColumn("GroupId", order:200, width:0, displayName: "Group Id", isVisible: false),
                new GridDisplayColumn("SubGroupId", order:200, width:0, displayName: "Sub Group Id", isVisible: false),
                new GridDisplayColumn("RigChecklistId", order:200, width:0, displayName: "RigChecklist", isVisible: false),
                new GridDisplayColumn("QuestionId", order:300, width:0, displayName: "QuestionId", isVisible: false) ,
                new GridDisplayColumn("DisplayType", order:400, width:0, displayName: "DisplayType", isVisible: false),
                new GridDisplayColumn("DisplayNoControl", order:500, width:0, displayName: "DisplayNoControl", isVisible: false)
            };

            gridData.DisplayColumns = displayColumns;

            gridData.Routes.Add(new GridRoute(GridRouteTypes.Batch, new
            {
                Controller = BaseController,
                Action = batchUpdateAction
            }));

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("RigOapChecklistId", displayName:"RigOapChecklistId"),
                                new GridEditLayoutColumn("QuestionId", displayName:"QuestionId")
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns

                    , editMode: GridViewEditingMode.Batch

                    , processLayout: i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                                );

            CreateAnswerColumns(html, viewContext, gridData, "YesNoNa0", 100);

            return gridData;
        }
        protected virtual void CreateAnswerColumns(HtmlHelper html, ViewContext viewContext, GridData gridData, string columnName, int order)
        {
            AddSingleRblQuestionAnswerColumn(html, viewContext, gridData, columnName, order);
        }
        protected virtual void AddSingleRblQuestionAnswerColumn(HtmlHelper html, ViewContext viewContext, GridData gridData, string columnName, int order)
        {

            var rblDisplayColumn = new GridDisplayColumn(columnName, order: order, allowHeaderFilter: DefaultBoolean.False, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, columnAction: c =>
            {
                var radioButtons = GetYesNoNaValues();
                c.Settings.AllowSort = DefaultBoolean.False;
                c.Settings.AllowHeaderFilter = DefaultBoolean.False;
                c.Name = $"{columnName}";

                c.SetHeaderCaptionTemplateContent(hc =>
                {
                    var builder = new StringBuilder();
                    builder.Append(@"<table width=""100%""><tr width=""100%"">");

                    radioButtons.ToList().ForEach(s =>
                    {
                        builder.Append($"<td>{s.DisplayValue}</td>");
                    });

                    builder.Append(@"</table>");

                    viewContext.Writer.Write(builder.ToString());
                });

                var saveEnabled = EnableSave(Protocol);

                var fieldName = $"{columnName}";
                c.FieldName = fieldName;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                c.Width = Unit.Percentage(20);
                c.Settings.ShowEditorInBatchEditMode = false;
                c.SetDataItemTemplateContent(container =>
                {
                    var model = container.DataItem as WebDataRow;
                    var id = DataBinder.Eval(container.DataItem, "QuestionId");
                    var formatType = DataBinder.Eval(container.DataItem, "DisplayType");
                    var name = $"{columnName}_{id}";
                    switch (formatType)
                    {
                        case "RBL":
                            html.DevExpress().RadioButtonList(s =>
                            {
                                s.Width = Unit.Percentage(100);
                                s.Name = name;
                                s.Properties.ValueType = typeof(string);
                                //s.Properties.ValueField = $"{columnName}";
                                //s.ReadOnly = true;
                                s.Enabled = saveEnabled;
                                s.ControlStyle.Border.BorderStyle = BorderStyle.None;
                                s.Style.Add("padding-left", "11px");
                                radioButtons.ToList().ForEach((r) =>
                                {
                                    s.Properties.ItemSpacing = 8;
                                    s.Properties.Items.Add("", r.Value);

                                });
                                s.Properties.RepeatColumns = radioButtons.Count();
                                s.Properties.RepeatDirection = RepeatDirection.Horizontal;
                                s.Properties.ClientSideEvents.SelectedIndexChanged = $"function (s,e) {{ debugger; var val = {s.Name}.GetValue(); var cell = {GridConstants.rigChecklistExecutionGrid}.GetFocusedCell() ;" +
                                                                         $"{GridConstants.rigChecklistExecutionGrid}.batchEditApi.StartEdit(); " +
                                                                         $"{GridConstants.rigChecklistExecutionGrid}.batchEditApi.SetCellValue(cell.rowVisibleIndex, cell.column.fieldName, val); " +
                                                                         $"{GridConstants.rigChecklistExecutionGrid}.batchEditApi.EndEdit(); " +
                                                                         $"{GridConstants.rigChecklistExecutionGrid}.UpdateEdit(); " +
                                                                         $" }}";
                            }).Bind(container.DataItem, fieldName).GetHtml();
                            break;
                        case "DDL":
                        case "TXT":
                            html.DevExpress().TextBox(s =>
                            {
                                s.Width = Unit.Percentage(100);
                                s.Name = name;
                                s.Enabled = saveEnabled;
                                s.ControlStyle.Border.BorderStyle = BorderStyle.None;
                                s.Properties.ClientSideEvents.TextChanged = $"function (s,e) {{ debugger; var val = {s.Name}.GetValue(); var cell = {GridConstants.rigChecklistExecutionGrid}.GetFocusedCell() ;{GridConstants.rigChecklistExecutionGrid}.batchEditApi.StartEdit(); {GridConstants.rigChecklistExecutionGrid}.batchEditApi.SetCellValue(cell.rowVisibleIndex, cell.column.fieldName, val); {GridConstants.rigChecklistExecutionGrid}.batchEditApi.EndEdit(); var t = {GridConstants.rigChecklistExecutionGrid}.batchEditApi.HasChanges();  }}";
                            }).Bind(container.DataItem, fieldName).GetHtml();
                            break;
                        default:
                            viewContext.Writer.Write("Invalid Display Type");
                            break;
                    }

                });
            });

            gridData.DisplayColumns.Add(rblDisplayColumn);
        }
        protected virtual IEnumerable<KeyValueModel> GetYesNoNaValues()
        {
            return new List<KeyValueModel>()
                {
                    new KeyValueModel("Yes", "Y", displayValue: "Yes".Translate()),
                    new KeyValueModel("No", "N", displayValue:  "No".Translate()),
                    new KeyValueModel("NA", "NA", displayValue: "N\\A".Translate())
                };
        }

        [ValidateInput(false)]
        public virtual ActionResult UpdateQuestions(MVCxGridViewBatchUpdateValues<OapGenericCheckListFlatModel, int> values, IList<KeyValueModel> comments)
        {
            try
            {
                var protocol = Protocol;
                var commentsUpdated = false;

                if (comments != null)
                {
                    commentsUpdated = UpdateChecklistComments(protocol, comments);
                }
          
                UpdateBatchEditedValues(protocol, values);

                if (commentsUpdated || values.Update.Any())
                {
                    Protocol = CommonUtilities.UpdateProtocol(OapAuditClient, protocol, GridRouteTypes.Update, CorpProtocolOriginalHashCode, GridConstants.ExecutionErrorsKey, ViewData);
                }
            }
            catch (Exception ex)
            {
                ViewData[GridConstants.ExecutionErrorsKey] = ex.Message;
            }

            return DisplayExecutionPartialView();
        }

        public bool UpdateChecklistComments(RigOapChecklist checklist, IList<KeyValueModel> comments)
        {
            var commentDataChanged = false;

            try
            {
                if (comments == null)
                {
                    return commentDataChanged;
                }
                 
                foreach (var c in comments)
                {
                    if (c.Id == null)
                    {
                        continue;
                    }

                    var keys = c.Id.Split(':');
                    var rigCommentId = Guid.Parse(keys[0]);
                    var commentId = int.Parse(keys[1]);


                    var fc = checklist.Comments.FirstOrDefault(q => q.Id == rigCommentId && q.OapChecklistCommentId == commentId);
                    if ((fc != null) && (fc.Comment ?? string.Empty).GetHashCode() != (c.Value ?? string.Empty).GetHashCode())
                    {
                        fc.Comment = c.Value;
                        commentDataChanged = commentDataChanged || true;
                    }
                }
            }
            catch (Exception) { throw; }

            return commentDataChanged;
        }

        protected virtual void UpdateBatchEditedValues(RigOapChecklist checklist, MVCxGridViewBatchUpdateValues<OapGenericCheckListFlatModel, int> values)
        {
            foreach (var m in values.Update)
            {
                var fq = checklist.Questions.FirstOrDefault(q => q.OapChecklistQuestionId == m.QuestionId);
                if (fq == null)
                {

                    var checklistQuestion = (from g in checklist.OapChecklist.Groups
                                             from sq in g.SubGroups
                                             from t in sq.Topics
                                             from q in t.Questions
                                             where q.Id == m.QuestionId
                                             select q).FirstOrDefault();

                    var nq = new RigOapChecklistQuestion()
                    {
                        RigOapChecklistId = m.RigChecklistId,

                        OapChecklistQuestionId = m.QuestionId,

                        OapChecklistQuestion = checklistQuestion,

                        Comment = m.Comment,

                        Answers = new ObservableCollection<RigOapChecklistQuestionAnswer>(new List<RigOapChecklistQuestionAnswer>()
                                {
                                    new RigOapChecklistQuestionAnswer
                                    {
                                        Ordinal = 0,
                                        Value = m.YesNoNa0
                                    }
                                })
                    };
                    checklist.Questions.Add(nq);
                }
                else
                {
                    fq.Answers[0].Value = m.YesNoNa0;
                    fq.Comment = m.Comment;
                }
            }
        }
        protected virtual PartialViewResult DisplayExecutionPartialView()
        {
            return PartialView("GenericlistExecutionPartial", Protocol?.ToFlattenedModels());
        }
        #endregion

        #region Monitoring

        #region Related Search (Monitoring)
          
        public void ConfigureRelatedSearchQuery(FormLayoutSettings<RelatedSearchQueryModel> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            settings.Name = "relatedSearchLayout";
            settings.UseDefaultPaddings = true;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            var topicList = new List<(int Id, string Topic)>();

            var groupList = Protocol.OapChecklist.Groups.ToList();

            var subGroupList = groupList.Select(g => g.SubGroups).ToList();

            subGroupList.ForEach(sg =>
            {
                var topics = sg.Select(ts => ts.Topics).ToList();

                topics.ForEach(t =>
                {
                    var lst = t.Select(a => (a.Id, a.Topic)).ToList();

                    foreach (var l in lst)
                    {
                        topicList.Add(l);
                    }
                });
            });

           var questionsList = new ObservableCollection<OapChecklistQuestion>();
             

            Protocol.Questions.Select(q =>
            {
                var questions = new OapChecklistQuestion()
                {
                    Id = q.OapChecklistQuestion.Id,
                    Question = topicList.Where(t => t.Id == q.OapChecklistQuestion.OapChecklistTopicId).FirstOrDefault().Topic + " : " + q.OapChecklistQuestion.Question,
                    OapChecklistTopicId = q.OapChecklistQuestion.OapChecklistTopicId
                };

                questionsList.Add(questions);                
               
                return q.OapChecklistQuestion;

            }).ToList();

            settings.Items.AddGroupItem(g =>
            {
                g.Caption = Translate("Related Question Search");

                g.Items.Add(i =>
                {
                    i.Caption = Translate("Search By:");
                    i.CaptionSettings.Location = LayoutItemCaptionLocation.Left;
                    i.FieldName = "SearchBy";
                    i.NestedExtension().RadioButtonList(s =>
                    {
                        s.Properties.RepeatDirection = RepeatDirection.Horizontal;
                        s.Properties.Items.Add("History of Question", "Historical");
                        s.Properties.Items.Add("Mapped Questions", "Mapping");
                        s.ControlStyle.Border.BorderStyle = BorderStyle.None;
                        s.Properties.ValidationSettings.RequiredField.IsRequired = true;
                    });
                });


                g.Items.Add(m => m.QuestionId, i =>
                {
                    i.FieldName = "QuestionId";
                    i.Caption = Translate("Question");
                    i.Width = Unit.Percentage(100);
                    i.NestedExtension().ComboBox(s =>
                    {
                        s.Properties.ValueType = typeof(int);
                        s.Properties.TextField = "Question";
                        s.Properties.ValueField = "Id";
                        s.Properties.DataMember = "Question";
                        s.Width = Unit.Percentage(100);
                        s.Properties.DropDownStyle = DropDownStyle.DropDownList;
                        s.Properties.DataSource = questionsList.OrderBy(p => p.OapChecklistTopicId);
                        s.Properties.ValidationSettings.RequiredField.IsRequired = true;
                        s.PreRender = (a, e) =>
                        {
                            MVCxComboBox c = a as MVCxComboBox;
                            c.Text = "[Select Question]";
                        };
                    });
                });

                g.Items.Add(m => m.FromDate, i =>
                {
                    i.Caption = Translate("From Date");
                    i.Name = "FromDate";
                    i.Width = Unit.Percentage(50);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit;
                    var nsettings = (DateEditSettings)i.NestedExtensionSettings;
                    nsettings.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    nsettings.Properties.ValidationSettings.RequiredField.IsRequired = true;
                });

                g.Items.Add(m => m.ToDate, i =>
                {
                    i.Caption = Translate("To Date");
                    i.Name = "ToDate";
                    i.Width = Unit.Percentage(50);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit;
                    var nsettings = (DateEditSettings)i.NestedExtensionSettings;
                    nsettings.Properties.ClientSideEvents.Init = "function (s,e) { ToDate.SetValue(new Date()); }";
                    nsettings.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    nsettings.Properties.ValidationSettings.RequiredField.IsRequired = true;
                });

                g.Items.Add(i =>
                {
                    i.ShowCaption = DefaultBoolean.False;
                    i.CssClass = "buttonToolbar";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtension().Button(s =>
                    {
                        s.Name = "relatedSearchButton";
                        s.Text = Translate("Search");
                        s.Width = Unit.Pixel(100);
                        s.UseSubmitBehavior = false;
                        s.ClientSideEvents.Click = "function(s,e) { onRelatedQuestionSearchClick (s,e); }";
                    });
                });

            });
        }
        public void ConfigureRelatedSearch(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistRelatedSearchGridData(html);
            settings.Configure(gridData, html, viewContext);
            settings.ClientSideEvents.BeginCallback = "function(s,e) { rigChecklistRelatedSearchGridOnBeginCallback (s,e); }";
            settings.CommandColumn.Visible = false;
        }
        private GridData InitializeChecklistRelatedSearchGridData(HtmlHelper htmlHelper)
        {
            var action = "DisplayRelatedSearch";

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


            var gridData = new GridData(GridConstants.rigChecklistRelatedSearchGrid, BaseController, action, Translate("Related Search"), "AddRelatedSearch", "Add", "search results", key: "RigOapChecklistQuestionId", displayColumnName: "Name", initializeCallBack: true, historicalRow: false); //, showPager: false);

            gridData.ToolBarOptions.DisplayCustomAddNew = false;

            gridData.AddRemoveCustomAddNew();
            gridData.ToolBarOptions.DisplayXlsExport = true;

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("RigId", displayName:"Rig", width:10, columnType:MVCxGridViewColumnType.ComboBox, lookup:oapRigsCombo, editLayoutWidth:50),
                new GridDisplayColumn("ChecklistDateTime", displayName:"Date",width:10, displayFormat: "g"),
                new GridDisplayColumn("RigChecklistUniqueId", displayName:"Protocol or Checklist ID",width:10),
                new GridDisplayColumn("Title", displayName:"Title",width:20),
                new GridDisplayColumn("Ascessor", displayName:"Ascessor",width:10),
                new GridDisplayColumn("Category", displayName:"Category",width:10),
                new GridDisplayColumn("Topic", displayName:"Topic",width:10),
                new GridDisplayColumn("Question", displayName:"Question",width:20),
                new GridDisplayColumn("Answer", displayName:"Answer",width:10),
                new GridDisplayColumn("Findings", displayName:"Findings or NO Answers",width:10)
            };

            gridData.DisplayColumns = displayColumns;

            return gridData;
        }
        public ActionResult DisplayRelatedSearch(RelatedSearchQueryModel searchModel)
        {
            if (searchModel.QuestionId == 0 || searchModel.SearchBy == null)
            {
                return PartialView("GenericlistRelatedSearchPartial", null);
            }
            var relatedQuestionsList = OapAuditClient.GetRelatedQuestionSearchAsync(searchModel.QuestionId, searchModel.FromDate, searchModel.ToDate, searchModel.SearchBy).Result?.Result?.Data.ToList();
            return PartialView("GenericlistRelatedSearchPartial", relatedQuestionsList.ToRelatedQuestionFlattenedModel(searchModel.QuestionId, OapAuditClient, OapChecklistFindingClient));
        }
        #endregion

        #region CAPA Grid (Monitoring)
        public ActionResult DisplayRelatedCapa(Guid? checklistId)
        {
            if (!checklistId.HasValue)
                return PartialView("GenericListRelatedCAPAsPartial", new List<IrmaCapa>());

            var apiClient = GetClient<OapChecklistFindingClient>();
            IEnumerable<IrmaCapa> capas = apiClient.GetCAPAsByChecklistIdAsync(checklistId.Value)?.Result?.Result?.Data;
            foreach (var capa in capas)
            {
                string assignedTo = capa.AssignedTo;
                string[] assignedToList = assignedTo.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder concatenatedString = new StringBuilder();
                // To Loop through
                foreach (string item in assignedToList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int userId = Convert.ToInt32(item);

                        IEnumerable<Person> users = CommonUtilities.GetUsers(GetClient<PeopleClient>());

                        var User = users?.FirstOrDefault(p => p.Id == userId);

                        var assign = User.Name;
                        concatenatedString.Append(assign);
                        concatenatedString.Append(" , ");
                    }

                    capa.AssignedTo = concatenatedString.ToString();
                }

                if (capa.AssignedTo.EndsWith(", "))
                {
                    capa.AssignedTo = capa.AssignedTo.Remove(capa.AssignedTo.Length - 2);
                }
            }
            return PartialView("GenericListRelatedCAPAsPartial", capas);
        }
        public virtual void ConfigureCAPAGrid(GridViewSettings settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            settings.Name = "RelatedCAPAGrid";
            settings.Width = Unit.Percentage(100);
            settings.Caption = Translate("Related CAPAs", "Resources");
            settings.KeyFieldName = "Id";

            settings.CallbackRouteValues = new { Action = "DisplayRelatedCapa", Controller = BaseController, Area = "Oap", checklistId = Protocol.Id };

            var options = new GridViewOptions() { ShowToolbar = true, ShowTitle = false, ShowCommandColumn = false, ShowAddButton = false, AddButtonText = "Add CAPA" };


            Helpers.EnscoHelper.EnscoStandardGrid(htmlHelper, settings, typeof(IrmaCapa), options);

            settings.Columns.Remove(settings.Columns["Id"]);
            settings.Columns.Remove(settings.Columns["CreatedDateTime"]);
            settings.Columns.Remove(settings.Columns["UpdatedDateTime"]);
            settings.Columns.Remove(settings.Columns["UpdatedBy"]);
            settings.Columns.Remove(settings.Columns["CreatedBy"]);
            settings.Columns.Remove(settings.Columns["SiteId"]);
            settings.Columns.Remove(settings.Columns["Source"]);
            settings.Columns.Remove(settings.Columns["SourceId"]);
            settings.Columns.Remove(settings.Columns["SourceUrl"]);

            settings.Columns.Add(i =>
            {
                i.FieldName = "Id";
                i.VisibleIndex = 0;
                i.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                i.SetDataItemTemplateContent(cont =>
                {
                    htmlHelper.DevExpress().HyperLink(link =>
                    {
                        string itemId = DataBinder.Eval(cont.DataItem, "Id").ToString();
                        link.Name = "CAPALink_" + itemId;
                        link.NavigateUrl = Url.Action("Index", "Capa", new { Area = "IRMA", id = itemId });
                        link.Properties.Text = itemId;
                    }).GetHtml();
                });
            });

            settings.Columns["ActionRequired"].VisibleIndex = 1;
            settings.Columns["AssignedTo"].VisibleIndex = 2;
            settings.Columns["DueDate"].VisibleIndex = 3;
            settings.Columns["DateCompleted"].VisibleIndex = 4;
            settings.Columns["Status"].VisibleIndex = 5;

            settings.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;

            settings.SetEditFormTemplateContent(container =>
            {
                var loapChecklistId = Protocol.Id;

                htmlHelper.RenderAction("OpenCapaPartial", new { Controller = "Capa", Area = "IRMA", source = "OAP", sourceId = loapChecklistId, sourceUrl = "/oap/Generic/List/" + loapChecklistId });
            });

        }
        #endregion

        #endregion
    }
}
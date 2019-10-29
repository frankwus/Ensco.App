using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using Ensco.App.App_Start;
using Ensco.Irma.Oap.Common;
using Ensco.Irma.Oap.Common.Controllers;
using Ensco.Irma.Oap.Common.Extensions;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.Web.Oap.Controllers;
using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class FindingsController : OapRigGridController
    {
        public string BaseController { get; set; } = "Findings";             

        protected IEnumerable<RigOapChecklistQuestionFinding> Findings
        {
            get
            {
                return Session[GridConstants.RigChecklistQuestionFindingsKey] as IEnumerable<RigOapChecklistQuestionFinding>;

            }
            set
            {
                if (Session[GridConstants.RigChecklistQuestionFindingsKey] != null)
                {
                    Session.Remove(GridConstants.RigChecklistQuestionFindingsKey);
                }

                Session[GridConstants.RigChecklistQuestionFindingsKey] = value;
            }
        }

        protected IEnumerable<RigOapChecklistQuestion> Questions
        {
            get
            {

                return Session[GridConstants.RigChecklistQuestionsKey] as IEnumerable<RigOapChecklistQuestion>;

            }
            set
            {
                if (Session[GridConstants.RigChecklistQuestionsKey] != null)
                {
                    Session.Remove(GridConstants.RigChecklistQuestionsKey);
                }

                Session[GridConstants.RigChecklistQuestionsKey] = value;
            }
        }

        public FindingsController()
        {
            RegisterClient(new List<Type>()
                            {
                                typeof(OapChecklistFindingClient),
                                typeof(RigOapChecklistClient),
                                typeof(FindingTypeClient),
                                typeof(FindingSubTypeClient),
                                typeof(LookupClient)
                            });
        }

        // GET: Oap/Findings
        public ActionResult Index()
        {
            var client = GetClient<OapChecklistFindingClient>();
            Findings = client.GetAllAsync().Result.Result.Data ?? new ObservableCollection<RigOapChecklistQuestionFinding>();
            Questions = (from rq in Findings
                         select rq.RigOapChecklistQuestion).ToList();
            ViewData["ReadOnly"] = true;
            return View("GenericlistFindingPartial", Findings);
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistFindingsGridData(html);             

            settings.Configure(gridData, html, viewContext);

            var isReadOnly = (bool)(ViewData["ReadOnly"] ?? false);

            settings.CommandButtonInitialize = (o, e) =>
            {
                if ((e.ButtonType == ColumnCommandButtonType.Update) || (e.ButtonType == ColumnCommandButtonType.Cancel))
                {
                    e.Visible = !isReadOnly;
                }
            };

            settings.ClientSideEvents.BeginCallback = $"{GridConstants.rigChecklistFindingGrid}OnBeginCallback";
            //settings.ClientSideEvents.EndCallback = $"{GridConstants.rigChecklistFindingGrid}OnEndCallback";
            settings.CommandColumn.Visible = false;

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false; //true;
             
            settings.SetDetailRowTemplateContent(c =>
            {
                var loapFindingId = (Guid)DataBinder.Eval(c.DataItem, gridData.Key);

                var loapChecklistId = Questions.FirstOrDefault().RigOapChecklistId;             

                if (!c.Grid.IsEditing && !c.Grid.IsNewRowEditing)
                {
                     html.RenderAction("OpenCapaPartial", new { Controller = "Capa", Area = "IRMA", source = "Findings", sourceId = loapFindingId, sourceUrl = "/oap/Generic/List/" + loapChecklistId });                     
                }                
            });

            settings.CellEditorInitialize = (s, e) =>
            {
                e.Editor.ReadOnly = false;
            };

        }

        public ActionResult Display(IEnumerable<RigOapChecklistQuestion> questions)
        {
            Questions = questions;

            Findings = GetQuestionFindings(Questions);

            return TryCatchCollectionDisplayPartialView<RigOapChecklistQuestionFinding>("GenericlistFindingPartial", "RigFindingsErrorsKey", () => Findings);
        }


        private IEnumerable<RigOapChecklistQuestionFinding> GetQuestionFindings(IEnumerable<RigOapChecklistQuestion> questions)
        {
            var findings = new List<RigOapChecklistQuestionFinding>();
            foreach (var q in questions)
            {
                var qFindings = GetClient<OapChecklistFindingClient>().GetAllFindingsForQuestionAsync(q.Id).Result.Result.Data;
                if (qFindings != null)
                {
                    findings.AddRange(qFindings);
                }
            }

            return findings;
        }

        protected virtual bool EnableSave()
        {
            return RigChecklist.VerifiedBy.Any(v => !v.Status?.Equals(GridConstants.WorkflowStatus.Completed.ToString(), StringComparison.InvariantCultureIgnoreCase) ?? true);
        }

        protected virtual GridData InitializeChecklistFindingsGridData(HtmlHelper html)
        {
            var action = "DisplayFindings";

            var gridData = new GridData(GridConstants.rigChecklistFindingGrid, BaseController, action, "Findings", "AddNewFinding", "Add Finding", "search findings", initializeCallBack: true, historicalRow: false, editController: "FindingsPage", key: "Id");

            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (!EnableSave())
            {
                gridData.SetGridReadOnly();
            }

            if (ViewData["FindingType"] == null)
            {
                ViewData["FindingType"] = CommonUtilities.GetFindingTypes(GetClient<FindingTypeClient>());
            }

            if (ViewData["FindingSubType"] == null)
            {
                ViewData["FindingSubType"] = CommonUtilities.GetFindingSubTypes(GetClient<FindingSubTypeClient>());
            }

            if (ViewData["Criticalities"] == null)
            {
                ViewData["Criticalities"] = CommonUtilities.GetCriticalities(GetClient<LookupClient>());
            }

            var questions = (from q in Questions
                             select q.OapChecklistQuestion).ToList();

            var questionsCombo = new GridCombo("QuestionsCombo", Questions, keyColumnName: "Id", displayColumnName: "Question");

            var findingTypes = ViewData["FindingType"] as IEnumerable<FindingType>;

            var findingTypeCombo = new GridCombo("FindingTypeCombo", findingTypes, selectedIndexChangedEvent: "findingTypeOnSelectedeChanged");

            var findingSubTypes = ViewData["FindingSubType"] as IEnumerable<FindingSubType>;


            var criticalities = ViewData["Criticalities"] as IEnumerable<Criticality>;

            var criticalityCombo = new GridCombo("CriticalitiesCombo", criticalities);
            
            //dynamic lkpList = new List<object>();
            //var capaItems = new List<CapaLookupModel>();
            //if (Session["CapaId"] == null)
            //{
            //    lkpList = Ensco.Utilities.UtilitySystem.GetLookupList("Capa");
            //    lkpList.BoundFieldName = "CapaId";
            //    lkpList.DisplayField = "ActionRequired";
            //    Session["CapaId"] = lkpList;
            //}
            //else
            //{
            //    lkpList = Session["CapaId"];
            //}

            //if (lkpList == null)
            //{
            //    lkpList = new LookupListModel<dynamic>();
            //    lkpList.Items = new List<object>();
            //}
            //else
            //{
            //    capaItems.AddRange((lkpList.Items as List<object>)?.Cast<CapaLookupModel>());
            //}


            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("RigChecklistFindingInternalId", order:10, displayName: "Finding Id",isReadOnly: true, columnAction: c => {
                    c.FieldName = "RigChecklistFindingInternalId";
                    c.Caption = Translate("Finding Id");
                    c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    c.SetDataItemTemplateContent(container => {
                    html.DevExpress().HyperLink(hl =>
                        {
                            var id = DataBinder.Eval(container.DataItem, "Id");
                            var internalId = DataBinder.Eval(container.DataItem, "RigChecklistFindingInternalId");
                            var url = Url.Action("List",  gridData.EditController, new RouteValueDictionary(new  { Id = id, checklistId = RigChecklist.Id }));
                            hl.NavigateUrl = HttpUtility.UrlDecode(url);
                            hl.Properties.Text = internalId.ToString();
                        }
                    ).Render();
                        });
                }, width:30,  editLayoutWidth:100),
                new GridDisplayColumn("RigOapChecklistQuestionId", order:20, displayName: Translate("Question"), width:40, columnType: MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: questionsCombo),
                new GridDisplayColumn("FindingTypeId", order:30, displayName: Translate("Finding Type"), width: 20, columnType: MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: findingTypeCombo),
                new GridDisplayColumn("FindingSubTypeId", order:40, displayName:"Finding Sub Type", columnType:MVCxGridViewColumnType.ComboBox,editLayoutWidth:100, columnAction:c => {
                                    c.Name = "FindingSubTypeId";
                                    c.Caption = Translate("Finding Sub Type");
                                    c.FieldName = "FindingSubTypeId";
                                    c.Width = Unit.Percentage(20);
                                    c.EditorProperties().ComboBox(cb =>
                                    {
                                        cb.CallbackRouteValues = new { Controller = BaseController, Action = "GetFindingSubTypes", TextField="Name", ValueField = "Id" };
                                        cb.TextField = "Name";
                                        cb.ValueField = "Id";
                                        cb.DataSource = findingSubTypes;
                                        cb.ClientSideEvents.BeginCallback = "findingSubTypeBeginCallback";
                                        cb.ClientSideEvents.EndCallback = "findingSubTypeEndCallback";
                                        cb.Width = Unit.Percentage(100);
                                    });
                                }),
                new GridDisplayColumn("CriticalityId", order:50, displayName: "Criticality",width: 10,  columnType:MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: criticalityCombo),
                new GridDisplayColumn("FindingDate", order:60, displayName: "Finding Date", width: 20, columnType: MVCxGridViewColumnType.DateEdit, displayFormat:"g", editLayoutWidth:100, isReadOnly: true),
                new GridDisplayColumn("IsRepeat", order:70, displayName: "Repeat", columnType:MVCxGridViewColumnType.CheckBox, width:20, editLayoutWidth:100),
                new GridDisplayColumn("Status", order:80, displayName: "Status", width: 25, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100, isReadOnly: true),
                //new GridDisplayColumn(
                //    "CapaId", order:90, displayName: "CAPA",
                //    width: 30, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100,isVisible:false ),
                    //columnAction: (column) => {
                    //    column.Name = "Capa";
                    //    column.FieldName = "CapaId";
                    //    column.Caption = Translate("CAPA");
                    //    column.SetDataItemTemplateContent( cont => {
                    //        if (int.TryParse(DataBinder.Eval(cont.DataItem,"CapaId")?.ToString(),out int capaId))
                    //        {
                    //            var capa = capaItems.FirstOrDefault(m => m.Id == capaId);
                    //            html.ViewContext.Writer.Write(capa?.Name);
                    //        }
                    //    });
                    //    column.SetEditItemTemplateContent(cont => {
                    //        html.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = column.FieldName, lookup = "Capa", multi = false, selected = DataBinder.Eval(cont.DataItem,"CapaId"), displayField = "ActionRequired" });
                    //    });
                    //}),
                     
                new GridDisplayColumn("Reason", order:100, displayName: "Description", width: 40, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100, isVisible:false),
                new GridDisplayColumn("Id", order:200, displayName: "Finding", width:0, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100, isVisible:false)
            };

            gridData.DisplayColumns = displayColumns;

            gridData.Routes.Add(new GridRoute(GridRouteTypes.Add, new { Controller = BaseController, Action = "CreateFinding" }));
            gridData.Routes.Add(new GridRoute(GridRouteTypes.Update, new { Controller = BaseController, Action = "UpdateFinding" }));
            gridData.Routes.Add(new GridRoute(GridRouteTypes.Delete, new { Controller = BaseController, Action = "DeleteFinding" }));

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("RigOapChecklistQuestionId", displayName:"Question"),
                                new GridEditLayoutColumn("FindingDate", displayName:"Finding Date"),
                                new GridEditLayoutColumn("FindingTypeId", displayName:"Finding Type"),
                                new GridEditLayoutColumn("FindingSubTypeId", displayName:"Finding Sub Type"),
                                new GridEditLayoutColumn("Criticality", displayName:"Criticality"),
                                new GridEditLayoutColumn("Reason", displayName:"Description"),                                  
                                new GridEditLayoutColumn("Status", displayName:"Status"),
                                //new GridEditLayoutColumn("CapaId", displayName:"CAPA"),
                                new GridEditLayoutColumn("Id", displayName:"Id",isVisible:false)
            };

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["Id"] = Guid.Empty;
                e.NewValues["FindingDate"] = DateTime.UtcNow;
                e.NewValues["Status"] = "Open";

                gridData.DefaultNewRowInitializeFields(e);
            };


            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i =>
                    {
                        i.ShowUpdateButton = true;
                        i.ShowCancelButton = true;
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                    , columnCount: 2
                    , emptyLayputItemCount: 0
                    );

            return gridData;
        }

        public ActionResult DisplayFindings()
        {
            var client = GetClient<OapChecklistFindingClient>();        

            Findings = client.GetAllFindingsForChecklistAsync(RigChecklist.Id).Result.Result.Data;

            return PartialView("GenericlistFindingPartial", Findings);
        }

        private IList<RigOapChecklistQuestionFinding> GetQuestionFindings(RigOapChecklist checklist, Guid questionId)
        {
            if (checklist != null)
            {
                var question = checklist.Questions.FirstOrDefault(q => q.Id == questionId);
                if (question != null)
                {
                    var qFindings = GetClient<OapChecklistFindingClient>().GetAllFindingsForQuestionAsync(question.Id).Result.Result.Data;

                    return qFindings;
                }

                return null;
            }

            return null;
        }

        // POST: Checklists/Generic/Create
        [ValidateInput(false)]
        public ActionResult CreateFinding(RigOapChecklistQuestionFinding model)
        {
            return TryCatchCollectionDisplayPartialView<RigOapChecklistQuestionFinding>("GenericlistFindingPartial", "RigFindingsErrorsKey", () =>
            {
                var findings = GetQuestionFindings(RigChecklist, model.RigOapChecklistQuestionId);
                if ((findings != null) && !findings.Any(f => f.Id == Guid.Empty && f.FindingDate == model.FindingDate && f.FindingTypeId == model.FindingTypeId && f.FindingSubTypeId == model.FindingSubTypeId))
                {
                    var client = GetClient<OapChecklistFindingClient>();
                    var response = client.AddFindingAsync(model).Result;

                    if ((response != null) && response.Result.Errors.Any())
                    {
                        CommonUtilities.DisplayGridErrors(response.Result.Errors, GridConstants.RigFindingsErrorsKey, ViewData);
                    }
                    else
                    {
                        RigChecklist = GetClient<RigOapChecklistClient>().GetCompleteChecklistAsync(RigChecklist.Id).Result.Result.Data;
                    }
                }

                return GetQuestionFindings(RigChecklist.Questions);
            });
        }

        [ValidateInput(false)]
        public ActionResult UpdateFinding(RigOapChecklistQuestionFinding model)
        {
            return TryCatchCollectionDisplayPartialView<RigOapChecklistQuestionFinding>("GenericlistFindingPartial", GridConstants.RigFindingsErrorsKey, () =>
            {
                var checklist = RigChecklist;
                if (checklist != null)
                {
                    var findings = GetQuestionFindings(RigChecklist, model.RigOapChecklistQuestionId);
                    if ((findings != null) && findings.Any(f => f.Id == model.Id))
                    {
                        var client = GetClient<OapChecklistFindingClient>();
                        var response = client.UpdateFindingAsync(model).Result;

                        if ((response != null) && response.Result.Errors.Any())
                        {
                            CommonUtilities.DisplayGridErrors(response.Result.Errors, GridConstants.RigFindingsErrorsKey, ViewData);
                        }
                        else
                        {
                            RigChecklist = GetClient<RigOapChecklistClient>().GetCompleteChecklistAsync(RigChecklist.Id).Result.Result.Data;
                        }
                    }
                }

                return GetQuestionFindings(RigChecklist.Questions);
            });
        }

        [ValidateInput(false)]
        public ActionResult DeleteFinding(RigOapChecklistQuestionFinding model)
        {
            return TryCatchCollectionDisplayPartialView<RigOapChecklistQuestionFinding>("GenericlistFindingPartial", GridConstants.RigFindingsErrorsKey, () =>
            {
                var checklist = RigChecklist;
                if (checklist != null)
                {
                    var question = checklist.Questions.FirstOrDefault(q => q.Id == model.RigOapChecklistQuestionId);

                    var qFindings = GetClient<OapChecklistFindingClient>().GetAllFindingsForQuestionAsync(question.Id).Result.Result.Data;

                    if ((question != null) && qFindings.Any(f => f.Id == model.Id))
                    {
                        var client = GetClient<OapChecklistFindingClient>();
                        var response = client.DeleteFindingAsync(model.Id).Result;

                        if ((response != null) && response.Result.Errors.Any())
                        {
                            CommonUtilities.DisplayGridErrors(response.Result.Errors, GridConstants.RigFindingsErrorsKey, ViewData);
                        }
                        else
                        {
                            RigChecklist = GetClient<RigOapChecklistClient>().GetCompleteChecklistAsync(RigChecklist.Id).Result.Result.Data;
                        }
                    }
                }

                return GetQuestionFindings(RigChecklist.Questions);
            });
        }

        public ActionResult GetFindingSubTypes(int findingTypeId, string textField, string valueField)
        {
            return GridExtensionBase.GetComboBoxCallbackResult(p =>
            {
                p.TextField = textField;
                p.ValueField = valueField;
                var findingSubTypes = CommonUtilities.GetFindingTypes(GetClient<FindingTypeClient>()).FirstOrDefault(f => f.Id == findingTypeId)?.SubTypes ?? new ObservableCollection<FindingSubType>();
                p.BindList(findingSubTypes);
            });
        }
    }
}
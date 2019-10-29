using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using Ensco.Irma.Oap.Common.Extensions;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.Web.Oap.Controllers;
using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class FindingsPageController : OapRigGridController
    {
        public string BaseController { get; set; } = "FindingsPage";

        public const string FindingPageErrorsKey = "FindingPageErrorsKey";
        public const string rigFindingPagePreviousFindingsGrid = "gridFindingPagePreviousFindings";

        public FindingsPageController()
        {
            RegisterClient(new List<Type>()
            {
            typeof(OapChecklistFindingClient),
            typeof(RigOapChecklistClient),
            typeof(FindingTypeClient),
            typeof(FindingSubTypeClient),
            typeof(LookupClient),
            typeof(PeopleClient)
            });
        }

        public const string ChecklistIdKey = "ChecklistId"; 
        // GET: Oap/FindingsPage
        public ActionResult List(Guid id)
        { 
            Finding = GetClient<OapChecklistFindingClient>().GetAsync(id).Result.Result.Data;
            return View("FindingPage", Finding);
        }

        protected RigOapChecklistQuestionFinding Finding
        {
            get
            {

                if (Session[GridConstants.RigChecklistQuestionFindingKey] == null)
                {
                    var id = ControllerContext.RequestContext.RouteData.Values["Id"];
                    if (id != null)
                    {
                        if (Guid.TryParse(id.ToString(), out Guid findingId))
                        {
                            Session[GridConstants.RigChecklistQuestionFindingKey] = GetClient<OapChecklistFindingClient>().GetAsync(findingId).Result.Result.Data;
                        }
                    }
                }

                return
                    Session[GridConstants.RigChecklistQuestionFindingKey] as RigOapChecklistQuestionFinding;

            }
            set
            {
                if (Session[GridConstants.RigChecklistQuestionFindingKey] != null)
                {
                    Session.Remove(GridConstants.RigChecklistQuestionFindingKey);
                }

                Session[GridConstants.RigChecklistQuestionFindingKey] = value; 
            }
        }


        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            throw new NotImplementedException();
        }


        public virtual void ConfigurePreviousFindings(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializePreviousFindingsGridData(html);
             
            settings.Configure(gridData, html, viewContext);

            settings.CommandButtonInitialize = (o, e) =>
            {
                if ((e.ButtonType == ColumnCommandButtonType.Update) || (e.ButtonType == ColumnCommandButtonType.Cancel))
                {
                    e.Visible = false;
                }
            };

        }

        protected virtual GridData InitializePreviousFindingsGridData(HtmlHelper html)
        {
            var gridData = new GridData(GridConstants.rigChecklistPreviousFindingGrid, false, true, BaseController)
            {
                Name = rigFindingPagePreviousFindingsGrid
            };

            //gridData.ButtonOptions.DisplayEditButton = false;
            //gridData.ButtonOptions.DisplayDeleteButton = false;

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

            if (ViewData["Users"] == null)
            {
                ViewData["Users"] = CommonUtilities.GetUsers(GetClient<PeopleClient>());
            }

            var users = ViewData["Users"] as IEnumerable<Person>;

            var usersCombo = new GridCombo("UsersCombo", users);

            var findingTypes = ViewData["FindingType"] as IEnumerable<FindingType>;

            var findingTypeCombo = new GridCombo("FindingTypeCombo", findingTypes, selectedIndexChangedEvent: "findingTypeOnSelectedeChanged");

            var findingSubTypes = ViewData["FindingSubType"] as IEnumerable<FindingSubType>;


            var criticalities = ViewData["Criticalities"] as IEnumerable<Criticality>;

            var criticalityCombo = new GridCombo("CriticalitiesCombo", criticalities);



            var displayColumns = new List<GridDisplayColumn>
            {
                 new GridDisplayColumn("RigChecklistFindingInternalId", order:20, displayName:"Finding Id",isReadOnly: true, columnAction: c => {
                    c.FieldName = "RigChecklistFindingInternalId";
                    c.Caption = "Finding Id";
                    c.EditorProperties().HyperLink(hl =>
                        {
                            var url = Url.Action("List",  gridData.EditController, new RouteValueDictionary(new  { Id = "{0}" }));
                            hl.NavigateUrlFormatString = HttpUtility.UrlDecode(url);
                            hl.TextField = "RigChecklistFindingInternalId";
                        }
                    );
                }, width:30,  editLayoutWidth:100),
                new GridDisplayColumn("FindingTypeId", displayName: "Finding Type", width: 40, columnType: MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: findingTypeCombo),
                new GridDisplayColumn("FindingSubTypeId", displayName:"Finding Sub Type", columnType:MVCxGridViewColumnType.ComboBox, columnAction:c => {
                                    c.Name = "FindingSubTypeId";
                                    c.Caption = "Finding Sub Type";
                                    c.FieldName = "FindingSubTypeId";
                                    c.Width = Unit.Percentage(40);
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
                new GridDisplayColumn("CriticalityId", displayName:"Criticality",width: 15,  columnType:MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: criticalityCombo),
                new GridDisplayColumn("Reason", displayName: "Description", width: 40, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100, isVisible:false),
                new GridDisplayColumn("FindingDate", displayName: "Finding Date", width: 25, columnType: MVCxGridViewColumnType.DateEdit, displayFormat:"dd/MM/yyyy hh:mm tt", editLayoutWidth:100, isReadOnly: true),
                new GridDisplayColumn("AssignedUserId", displayName:"Assigned",width: 15,  columnType:MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: usersCombo),
                new GridDisplayColumn("CapaId", displayName: "CAPA", width: 30, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100),
                new GridDisplayColumn("Status", displayName: "Status", width: 30, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100, isReadOnly: true),
                new GridDisplayColumn("Id", displayName: "Finding", width:0, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100, isVisible:false)
            };

            gridData.DisplayColumns = displayColumns;

            //gridData.LayoutColumns = new List<GridEditLayoutColumn>()
            //                {
            //                    new GridEditLayoutColumn("RigOapChecklistQuestionId", displayName:"Question"),
            //                    new GridEditLayoutColumn("FindingDate", displayName:"Finding Date"),
            //                    new GridEditLayoutColumn("FindingTypeId", displayName:"Finding Type"),
            //                    new GridEditLayoutColumn("FindingSubTypeId", displayName:"Finding Sub Type"),
            //                    new GridEditLayoutColumn("Criticality", displayName:"Criticality"),
            //                    new GridEditLayoutColumn("Reason", displayName:"Description"),

            //                    new GridEditLayoutColumn("CapaId", displayName:"CAPA"),
            //                    new GridEditLayoutColumn("Status", displayName:"Status"),
            //                    new GridEditLayoutColumn("Id", displayName:"Id",isVisible:false)
            //}; 

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i =>
                    {
                        i.ShowUpdateButton = false;
                        i.ShowCancelButton = false;
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                    , columnCount: 2
                    , emptyLayputItemCount: 0
                    );

            return gridData;
        }

        public void ConfigureFindingsPage(HtmlHelper htmlHelper, ViewContext viewContext)
        {
            htmlHelper.RenderPartial("FindingPageFindingPartial", Finding);
            viewContext.Writer.Write("<br/>");

            var previousFindings = GetClient<OapChecklistFindingClient>().GetAllQuestionPreviousFindingsAsync(Finding.RigOapChecklistQuestionId).Result.Result.Data ?? new ObservableCollection<RigOapChecklistQuestionFinding>();

            if(previousFindings.Any())
            {
                var currentFinding = previousFindings.FirstOrDefault(f => f.Id == Finding.Id);
                previousFindings.Remove(currentFinding);
            }
            htmlHelper.RenderPartial("FindingPagePreviousFindingsPartial", previousFindings);
            viewContext.Writer.Write("<br/>");

            htmlHelper.RenderPartial("FindingPageVerificationPartial", Finding);
        }

        protected virtual bool EnableSave()
        {
            return RigChecklist.VerifiedBy.Any(v => !v.Status?.Equals(GridConstants.WorkflowStatus.Completed.ToString(), StringComparison.InvariantCultureIgnoreCase) ?? true);
        }


        public virtual void ConfigureFinding(FormLayoutSettings<RigOapChecklistQuestionFinding> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            settings.Name = "findingPageLayout";
            settings.UseDefaultPaddings = false;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            var rigOapChecklist = GetClient<RigOapChecklistClient>().GetAsync(RigChecklist.Id).Result.Result.Data;

            if (ViewData["findingTypes"] == null)
            {
                ViewData["findingTypes"] = GetClient<FindingTypeClient>().GetAllAsync().Result.Result.Data;
            } 

            var findingTypes = ViewData["findingTypes"] as IEnumerable<FindingType>; 

            if (ViewData["criticalities"] == null)
            {
                ViewData["criticalities"] = GetClient<LookupClient>().GetAllCriticalityAsync().Result.Result.Data;
            }

            var criticalities = ViewData["criticalities"] as IEnumerable<Criticality>;

            settings.Items.AddGroupItem(g =>
            {
                g.Caption = "Finding";
                g.ColCount = 3;

                g.Items.Add(i =>
                {

                    i.ShowCaption = DefaultBoolean.False;
                    i.Border.BorderStyle = BorderStyle.None;
                    i.CssClass = "buttonToolbar";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtension().Button(s =>
                    {
                        s.Name = "saveFinding";
                        s.Text = "Save";
                        s.UseSubmitBehavior = true;
                        s.RouteValues = new { Controller = BaseController, Action = "SaveFinding" };
                        s.Width = Unit.Pixel(100);
                        s.Enabled = EnableSave();
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

                g.Items.Add(m => m.RigChecklistFindingInternalId, i =>
                {
                    i.NestedExtensionSettings.Width = Unit.Percentage(33);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.Caption = "Finding Id";  
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Finding.RigChecklistFindingInternalId.ToString();
                    });
                });

                g.Items.Add(m => m.CreatedBy, i =>
                {
                    i.Caption = "Created By";
                    i.Width = Unit.Percentage(33);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Finding.CreatedBy;
                    });
                });


                g.Items.Add(m => m.FindingDate, i =>
                {
                    i.Caption = "Date/Time Created";
                    i.Width = Unit.Percentage(33);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Finding.FindingDate.ToLongDateString();
                    });
                });

                g.Items.Add(m => m.FindingTypeId, i =>
                {
                    i.Width = Unit.Percentage(33);
                    i.NestedExtension().ComboBox(c =>
                    {
                        //c.CallbackRouteValues = new { Controller = BaseController, Action = "FindingTypePartial" };
                        c.Properties.DataSource = findingTypes;
                        c.Properties.ValueField = "Id";
                        c.Properties.TextField = "Name";
                        c.Properties.ClientSideEvents.SelectedIndexChanged = "function(s, e) { debugger; FindingSubTypeId.PerformCallback(); }";
                    });
                    i.NestedExtensionSettings.Width = Unit.Percentage(100); 
                });

                g.Items.Add(m => m.FindingSubTypeId, i =>
                { 
                    i.Width = Unit.Percentage(33);
                    i.SetNestedContent(() => {
                        htmlHelper.RenderPartial("FindingPageFindingSubTypeComboPartial", Finding);
                    });
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                });

                g.Items.Add(m => m.CriticalityId, i =>
                {
                    i.Width = Unit.Percentage(33);
                    i.NestedExtension().ComboBox(c =>
                    {
                        c.Properties.DataSource = criticalities;
                        c.Properties.ValueField = "Id";
                        c.Properties.TextField = "Name";
                    });
                    i.NestedExtensionSettings.Width = Unit.Percentage(100); 
                });

                g.Items.Add(m => m.IsRepeat, i =>
                {
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.CheckBox;
                    i.NestedExtensionSettings.Width = Unit.Percentage(100); 
                    i.ColumnSpan = 3;
                });

                g.Items.Add(i => {
                    i.Name = "ChecklistProtocolNumber";
                    i.Caption = "Checklist/Protocol#";
                    i.Width = Unit.Percentage(33);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = rigOapChecklist.OapChecklist?.ControlNumber ?? string.Empty;
                    }); 
                });

                g.Items.Add(i => {
                    i.Name = "ChecklistProtocolName";
                    i.Caption = "Checklist/Protocol";
                    i.Width = Unit.Percentage(33);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = rigOapChecklist.OapChecklist?.Name ?? string.Empty;
                    });
                    i.ColumnSpan = 2;
                });

                g.Items.Add(m => m.RigOapChecklistQuestion.OapChecklistQuestion.Id, i =>
                {
                    i.Width = Unit.Percentage(33);
                    i.Caption = "Question Id"; 
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);  
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        var questionId = Finding.RigOapChecklistQuestion.OapChecklistQuestionId;
                        s.Text =  (questionId.HasValue) ? questionId.ToString() : string.Empty;
                    });

                });
                g.Items.Add(m => m.RigOapChecklistQuestion.OapChecklistQuestion.Question, i =>
                {
                    i.Width = Unit.Percentage(33); 
                    i.Caption = "Question";   
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Finding.RigOapChecklistQuestion.OapChecklistQuestion.Question;
                    });
                    i.ColumnSpan = 2; 
                });

                g.Items.Add(m => m.Reason, i =>
                {
                    i.Width = Unit.Percentage(100);
                    i.FieldName = "Reason";
                    i.Caption = "Finding Reason"; 
                    i.NestedExtensionSettings.Width = Unit.Percentage(100); 
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.Memo; 
                }); 

                g.Items.Add(m => m.FileName, i =>
                {
                    i.Caption = "File";
                    i.Width = Unit.Percentage(33); 
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = Finding.FileName;
                    });
                });

                g.Items.Add(m => m.UploadedFile, i =>
                {
                    i.Caption = "Upload File";
                    i.Width = Unit.Percentage(66);
                    i.NestedExtension().UploadControl(f => {
                        f.CallbackRouteValues = new { Controller = "FindingsPage", Action = "FileUpload" };
                        f.ShowUploadButton = false;
                        f.AutoStartUpload = true;
                        f.Enabled = true;
                    });
                    i.ColumnSpan = 2;
                });
            });
        }

        public virtual void ConfigureFindingVerification(FormLayoutSettings<RigOapChecklistQuestionFinding> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            settings.Name = "findingVerificationPageLayout";
            settings.UseDefaultPaddings = false;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;


            var rigManager = string.Empty;

            if (Finding.ReviewedByUserId > 0)
            {
                var manager = GetClient<PeopleClient>().GetAsync(Finding.ReviewedByUserId).Result.Result.Data;
                if (manager != null)
                {
                    rigManager = manager.Name;
                }
            }

            settings.Items.AddGroupItem(g =>
            {
                g.Caption = "Finding Verification";
                g.ColCount = 2;
                 
                g.Items.Add(i =>
                {
                    i.Name = "RigManager";
                    i.Caption = "Rig Manager";
                    i.Width = Unit.Percentage(50);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = rigManager; 
                    }); 
                     
                });
               
                g.Items.Add(i =>
                {
                    i.Name = "ReviewReject";
                    i.Caption = "Review/Reject";
                    i.Width = Unit.Percentage(50); 
                    i.SetNestedContent(() =>
                    {
                        if (Finding.Status.Equals("Reviewed") || Finding.Status.Equals("Rejected"))
                        {
                            htmlHelper.ViewContext.Writer.Write($" {Finding.Status} - {Finding.UpdatedDateTime.ToLongDateString()}");
                        }
                        else if (Finding.ReviewedByUserId == UtilitySystem.CurrentUserId) // Review
                        { 
                            htmlHelper.DevExpress().Button(btSettings =>
                            {
                                btSettings.Name = "btnSignatureReview";
                                btSettings.Text = "Review";
                                btSettings.UseSubmitBehavior = false;
                                btSettings.Style.Add("margin-right", "0.8em");
                                btSettings.RouteValues = new { Controller = BaseController, Action = "Review"};
                                btSettings.RenderMode = ButtonRenderMode.Button;
                                btSettings.ClientSideEvents.Click = "function (s,e) { e.processOnServer = confirm('Confirm review?'); }";
                            }).Render();
                            htmlHelper.DevExpress().Button(btSettings =>
                            {
                                btSettings.Name = "btnSignatureReject";
                                btSettings.Text = "Reject";
                                btSettings.UseSubmitBehavior = false;
                                btSettings.RouteValues = new { Controller = BaseController, Action = "Reject"};
                                btSettings.RenderMode = ButtonRenderMode.Button;
                                btSettings.ClientSideEvents.Click = "function (s,e) { e.processOnServer = confirm('Confirm rejection?'); }";
                            }).Render();
                            htmlHelper.ViewContext.Writer.Write("&nbsp;"); 
                        }
                    });
                });


            });
        }


        public virtual void ConfigureFindingType(ComboBoxSettings settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            if (ViewData["findingTypes"] == null)
            {
                ViewData["findingTypes"] = GetClient<FindingTypeClient>().GetAllAsync().Result.Result.Data;
            }

            var findingTypes = ViewData["findingTypes"] as IEnumerable<FindingType>;
            settings.Name = "FindingTypeId";
            //settings.CallbackRouteValues = new { Controller = BaseController, Action = "FindingTypePartial" };
            settings.Properties.DataSource = findingTypes;
            settings.Properties.ValueField = "Id";
            settings.Properties.TextField = "Name";
            settings.Properties.ClientSideEvents.SelectedIndexChanged = "function(s, e) { debugger; FindingSubTypeId.PerformCallback(); }";
        }


        public virtual void ConfigureFindingSubType(ComboBoxSettings settings, HtmlHelper htmlHelper, ViewContext viewContext)
        { 
       

            var findingSubTypes = ViewData["findingSubTypes"] as IEnumerable<FindingSubType>;
            settings.Name = "FindingSubTypeId";
            settings.CallbackRouteValues = new { Controller = BaseController, Action = "FindingSubTypePartial" }; 
            settings.Properties.ValueField = "Id";
            settings.Properties.TextField = "Name";
            settings.Properties.ValueType = typeof(int); 
            settings.Properties.ClientSideEvents.BeginCallback = "function(s, e) { e.customArgs['FindingTypeId'] = FindingTypeId.GetValue(); }";
        }


        [HttpGet]
        public ActionResult Review()
        {
            try
            {
                var finding = Finding;
                finding.Status = "Reviewd";
                finding.UpdatedDateTime = DateTime.UtcNow; 
                var result = GetClient<OapChecklistFindingClient>().UpdateFindingAsync(finding).Result.Result.Data;
                if (!result)
                {
                    //ToDO display the error message
                }
            }
            catch (Exception ex)
            {

            }

            return TryCatchDisplayPartialView("FindingPagePartial", FindingPageErrorsKey, () => Finding);
        }

        [HttpGet]
        public ActionResult Reject(int userId, string comment)
        {
            try
            {
                var finding = Finding;
                finding.Status = "Rejected";
                finding.UpdatedDateTime = DateTime.UtcNow;
                var result = GetClient<OapChecklistFindingClient>().UpdateFindingAsync(finding).Result.Result.Data;
                if (!result)
                {
                    //ToDO display the error message
                }
            }
            catch (Exception ex)
            {

            }

            return TryCatchDisplayPartialView("FindingPagePartial", FindingPageErrorsKey, () => Finding);
        }

        public ActionResult SaveFinding(RigOapChecklistQuestionFinding finding)
        {
            if (UpdateFindingValues(finding))
            {
                var success = GetClient<OapChecklistFindingClient>().UpdateFindingAsync(Finding).Result.Result.Data;
                return List(Finding.Id);
            }

            return TryCatchDisplayPartialView("FindingPagePartial", FindingPageErrorsKey, () => Finding);
        }

        private bool UpdateFindingValues(RigOapChecklistQuestionFinding finding)
        {
            var updated = false;
            if(Finding.FindingTypeId != finding.FindingTypeId)
            {
                Finding.FindingTypeId = finding.FindingTypeId;
                updated = true;
            }

            if (Finding.FindingSubTypeId != finding.FindingSubTypeId)
            {
                Finding.FindingSubTypeId = finding.FindingSubTypeId;
                updated = true;
            }

            if (Finding.CriticalityId != finding.CriticalityId)
            {
                Finding.CriticalityId = finding.CriticalityId;
                updated = true;
            }

            if (Finding.IsRepeat != finding.IsRepeat)
            {
                Finding.IsRepeat = finding.IsRepeat;
                updated = true;
            }

            if (Finding.IsRepeat != finding.IsRepeat)
            {
                Finding.IsRepeat = finding.IsRepeat;
                updated = true;
            }

            if (!string.IsNullOrEmpty(Finding.FileName) && !string.IsNullOrEmpty(finding.FileName))
            {
                if (!Finding.FileName.Equals(finding.FileName))
                {
                    Finding.FileName = finding.FileName;
                    Finding.FileStream = finding.FileStream;
                    updated = true;
                }
            }
            else if(!string.IsNullOrEmpty(Finding.FileName) && string.IsNullOrEmpty(finding.FileName))
            {
                Finding.FileName = finding.FileName;
                Finding.FileStream = finding.FileStream;
                updated = true;
            }
            else if (string.IsNullOrEmpty(Finding.FileName) && !string.IsNullOrEmpty(finding.FileName))
            {
                Finding.FileName = finding.FileName;
                Finding.FileStream = finding.FileStream;
                updated = true;
            }

            return updated;
        }

        public ActionResult FileUpload()
        {
            var uploadedFiles = UploadControlExtension.GetUploadedFiles("UploadedFile");
            if (uploadedFiles.Any())
            {
                var file = uploadedFiles.FirstOrDefault();
                Finding.FileName = file.FileName;
                Finding.FileStream = file.FileBytes;
            }

            return TryCatchDisplayPartialView("FindingPagePartial", FindingPageErrorsKey, () => Finding);
        }


        public ObservableCollection<FindingSubType> GetSubTypes(int findingTypeId)
        {
            return CommonUtilities.GetFindingTypes(GetClient<FindingTypeClient>()).FirstOrDefault(f => f.Id == findingTypeId)?.SubTypes ?? new ObservableCollection<FindingSubType>();
        }
        
        public ActionResult FindingSubTypePartial(int? findingTypeId)
        {
            if (findingTypeId.HasValue)
            {
                Finding.FindingTypeId = findingTypeId.Value;
            }
            return TryCatchDisplayPartialView("FindingPageFindingSubTypeComboPartial", FindingPageErrorsKey,() => Finding);
        }

       
    }
}
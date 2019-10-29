using System;
using System.Linq;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Ensco.Models;
using Ensco.Irma.Oap.WebClient.Corp;
using Ensco.Irma.Oap.Web.Oap.Controllers;
using Ensco.Irma.Models;
using Ensco.Services;
using Ensco.App.Areas.Oap.Models;
using System.Collections.ObjectModel;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Ensco.App.Areas.cOap.Models;
using System.Web.UI.WebControls;
using Ensco.Utilities;
using System.Collections.Generic;
using Ensco.Irma.Oap.Common.Models;
using Ensco.App.Areas.cOap.Extensions;
using Ensco.Irma.Oap.Common.Extensions;

namespace Ensco.App.Areas.cOap.Controllers
{
    public class OapAuditReportController : OapCorpGridController
    {
        private OapChecklistClient OapChecklistClient { get; }
        private OapHierarchyClient OapHierarchyClient { get; }
        private OapAuditClient OapAuditsClient { get; }
        private OapChecklistFindingClient OapChecklistFindingClient { get; }

        public OapAuditReportController() : base()
        {
            OapChecklistClient = new OapChecklistClient(GetApiBaseUrl(), Client);

            OapHierarchyClient = new OapHierarchyClient(GetApiBaseUrl(), Client);

            OapAuditsClient = new OapAuditClient(GetApiBaseUrl(), Client);

            OapChecklistFindingClient = new OapChecklistFindingClient(GetApiBaseUrl(), Client);
        }

        [SiteMapTitle("Id")]
        public ActionResult Index(int Id)
        {
            Session["OapAudit"] = null;
            Session["isAuditIdClick"] = true;


            var audit = OapAuditsClient.GetOapAuditAsync(Id).Result?.Result?.Data;
            var raudit = audit;
            var ap = audit?.OapAuditProtocols?.Where(r => r.RigOapChecklist?.OapChecklist?.OapType?.Code == "AR")?.FirstOrDefault();
            audit?.OapAuditProtocols.Remove(ap);

            Session["OapAudit"] = audit;
            raudit?.OapAuditProtocols.Add(ap);
            return View(raudit);
        }

        public ActionResult Cancel(int id)
        {
            return RedirectToAction("Index", "oapaudit", new { area = "coap" });
        }

        public ActionResult Save(OapAudit audit)
        {
            OapAuditsClient.UpdateOapAuditAsync(audit);
            return RedirectToAction("Index", "oapaudit", new { area = "coap" });
        }

        //public ActionResult Save(OapAudit audit)
        //{
        //    //var response = OapAuditsClient.(model).Result;
        //    OapAuditsClient.UpdateOapAuditAsync(audit);
        //    return RedirectToAction("Index", "oapaudit", new { area = "coap" }); return View("~/coap/oapaudit/Index/");
        //}

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.Name = "OapAuditReportGrid";
        }

        #region - WorkFlow -
        public ActionResult AuditVerificationPartial(IEnumerable<RigOapChecklistVerifier> Verifiers)
        {

            return PartialView("AuditVerificationPartial", Verifiers?.OrderBy(v => v.RigOapChecklistId).ToList() ?? new List<RigOapChecklistVerifier>());

        }

        [HttpGet]
        public ActionResult ValidateOapAudit(int auditId)
        {
            var result = GetClient<OapAuditClient>().ValidateOapAuditAsync(auditId).Result?.Result?.Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Sign(int userId, int order, string Comment, Guid Id)
        {
            // var audit = GetClient<OapAuditClient>().GetOapAuditAsync(Id).Result.Result.Data;
            var RigChecklist = GetClient<OapAuditClient>().GetCompleteProtocolAsync(Id).Result.Result.Data;
            // var RigChecklist = audit.OapAuditProtocols.ToList().Find(s => s.RigOapChecklist.OapChecklist.OapType.Code == "AR")?.RigOapChecklist;
            var checklistId = RigChecklist?.Id ?? Guid.Empty;
            var audit = (OapAudit)Session["audit"];
            try
            {
                var result = GetClient<OapAuditWorkFlowClient>().ProcessWorkflowAsync(new ProcessOapAuditWorkFlow() { Checklist = checklistId, User = userId, Transition = "Sign", Comment = Comment, Order = order, Audit = audit }).Result.Result;

                var nextApprover = RigChecklist.VerifiedBy.OrderBy(v => v.Order).FirstOrDefault(v => v.Order > order);
                Lazy<UserModel> nextApproverUser = new Lazy<UserModel>(() => ServiceSystem.GetUser(nextApprover.UserId), isThreadSafe: false);

                SendOAPReviewEmail(nextApproverUser.Value, audit.Id);

            }
            catch (Exception ex)
            {

            }

            return (audit == null) ? RedirectToAction("Index", "OapAudit") : RedirectToAction("Index", new { id = audit.Id });
        }

        [HttpGet]
        public ActionResult Review(int userId, int order, string Comment, Guid Id)
        {
            var checklistId = Id;
            var audit = (OapAudit)Session["audit"];
            try
            {
                var result = GetClient<OapAuditWorkFlowClient>().ProcessWorkflowAsync(new ProcessOapAuditWorkFlow() { Checklist = checklistId, User = userId, Transition = "Review", Comment = Comment, Order = order, Audit = audit }).Result.Result;

            }
            catch (Exception ex)
            {

            }

            return (audit == null) ? RedirectToAction("Index", "OapAudit") : RedirectToAction("Index", new { id = audit.Id });
        }

        [HttpGet]
        public ActionResult Cancel(int userId, int order, string Comment, Guid Id)
        {
            var audit = (OapAudit)Session["audit"];

            try
            {
                var result = GetClient<OapAuditWorkFlowClient>()
                    .ProcessWorkflowAsync(new ProcessOapAuditWorkFlow() { Checklist = Id, Transition = "Cancel", Order = order, User = userId, Comment = Comment }).Result.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index", new { Id = audit?.Id });
        }

        [HttpGet]
        public ActionResult Reject(int userId, int order, string Comment, Guid Id)
        {
            var checklistId = Id;
            var RigChecklist = GetClient<OapAuditClient>().GetCompleteProtocolAsync(Id).Result.Result.Data;
            var audit = (OapAudit)Session["audit"];
            try
            {
                var result = GetClient<OapAuditWorkFlowClient>().ProcessWorkflowAsync(new ProcessOapAuditWorkFlow() { Checklist = checklistId, User = userId, Transition = "Reject", Comment = Comment, Order = order }).Result.Result;

                var lastApprover = RigChecklist.VerifiedBy.OrderBy(v => v.Order).FirstOrDefault(v => v.Order < order);
                Lazy<UserModel> lastApproverUser = new Lazy<UserModel>(() => ServiceSystem.GetUser(lastApprover.UserId), isThreadSafe: false);
                SendOAPRejectEmail(lastApproverUser.Value, audit.Id);
            }
            catch (Exception ex)
            {
                throw;
            }

            return (audit == null) ? RedirectToAction("Index", "OapAudit") : RedirectToAction("Index", new { id = audit.Id });
        }


        [HttpGet]
        public virtual ActionResult StartAuditWorkflow(int auditId)
        {
            var audit = GetClient<OapAuditClient>().GetOapAuditAsync(auditId).Result.Result.Data;
            var rigChecklist = audit.OapAuditProtocols.ToList().Find(s => s.RigOapChecklist.OapChecklist.OapType.Code == "AR")?.RigOapChecklist;
            var leadAssessor = rigChecklist?.Assessors.FirstOrDefault(a => a.IsLead);

            var result = GetClient<OapAuditWorkFlowClient>().StartWorkflowAsync(new StartOapAuditWorkFlow() { Checklist = rigChecklist.Id, Owner = leadAssessor.UserId, Audit = audit }).Result.Result;

            Lazy<UserModel> user = new Lazy<UserModel>(() => ServiceSystem.GetUser(leadAssessor.UserId), isThreadSafe: false);
            SendOAPReviewEmail(user.Value, auditId);

            return RedirectToAction("Index", "OapAuditReport", new { id = auditId });
        }

        private void SendOAPReviewEmail(UserModel recipient, int Id)
        {
            if (UtilitySystem.Settings.ConfigSettings["SMTPEnabled"] == "True" && recipient != null && recipient != null && recipient.Email != null)
            {
                try
                {
                    var controllerName = Request.RequestContext.RouteData.Values["controller"].ToString(); // This may change due to controllers inheritance
                    string url = Url.Action("Index", controllerName, new { Id = Id }, Request.Url.Scheme);
                    var message = $"Dear {recipient.DisplayName}," +
                        $"<p>You have been assigned the OAP Audit <a href='{url}'>{Id}</a> for review.<br/>" +
                        $"Please click on the link above to be redirected to the checklist page.</p>" +
                        $"<i>This is an automated message from IRMA, please do not reply.</i><br />";

                    MailSender.SendEmail("irma-application@enscoplc.com", recipient.Email, "IRMA - OAP Audit Workflow", message);
                }
                catch (Exception e)
                {
                    // TODO: Implement error handling 
                    Logger.Error(new LogInfo(e.Message));
                    throw;
                }
            }
        }

        private void SendOAPRejectEmail(UserModel recipient, int Id)
        {
            if (UtilitySystem.Settings.ConfigSettings["SMTPEnabled"] == "True" && recipient != null && recipient != null && recipient.Email != null)
            {
                try
                {
                    var controllerName = Request.RequestContext.RouteData.Values["controller"].ToString(); // This may change due to controllers inheritance
                    string url = Url.Action("Index", controllerName, new { Id = Id }, Request.Url.Scheme);
                    var message = $"Dear {recipient.DisplayName}," +
                        $"<p>The OAP Audit <a href='{url}'>{Id}</a> has been rejected.<br/>" +
                        $"Please click on the link above to be redirected to the audit page.</p>" +
                        $"<i>This is an automated message from IRMA, please do not reply.</i><br />";

                    MailSender.SendEmail("irma-application@enscoplc.com", recipient.Email, "IRMA - OAP Audit Workflow", message);
                }
                catch (Exception e)
                {
                    // TODO: Implement error handling 
                    Logger.Error(new LogInfo(e.Message));
                    throw;
                }
            }
        }

        #endregion

        #region Assessor Grid 
        public ActionResult AssessorsPartial()
        {
            var assessorsList = new ObservableCollection<RigOapChecklistAssessor>();

            var audit = (OapAudit)Session["OapAudit"];

            var protocols = OapAuditsClient.GetAuditProtocolsAsync(audit.Id).Result?.Result?.Data;

            foreach (var p in protocols)
            {
                var aList = p.Assessors;

                foreach (var assessor in aList)
                {
                    if (assessorsList.Where(u => u?.UserId == assessor.UserId).Count() == 0)
                        assessorsList.Add(assessor);
                }
            }

            return PartialView("ProtocolsAssessorsPartial", MapToViewModel(assessorsList, perItemAction: perItemMapping));
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
        #endregion

        #region Review Tab 

        protected virtual IEnumerable<OapChecklist> GetProtocols()
        {
            var protocols = OapChecklistClient.GetAllAuditProtocolsAsync().Result?.Result?.Data;
            return protocols;
        }
        public ActionResult ReviewSearchPartial()
        {
            return PartialView("ReviewSearchPartial", new ReviewSearchModel());
        }

        public void ConfigureReviewSearch(FormLayoutSettings<ReviewSearchModel> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {

            if (ViewData["Protocols"] == null)
            {
                ViewData["Protocols"] = GetProtocols();
            }

            var protocols = ViewData["Protocols"] as IEnumerable<OapChecklist>;

            var oapProtocolsCombo = new GridCombo("OapChecklist", protocols);

            settings.Name = "reviewSearchLayout";
            settings.UseDefaultPaddings = true;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;


            settings.Items.AddGroupItem(g =>
            {
                g.Caption = Translate("Previous Protocols and Nonconformities Review");

                g.Items.Add(i =>
                {
                    i.Caption = Translate("Search By:");
                    i.CaptionSettings.Location = LayoutItemCaptionLocation.Left;
                    i.FieldName = "SearchBy";
                    i.NestedExtension().RadioButtonList(s =>
                    {
                        s.Properties.RepeatDirection = RepeatDirection.Vertical;
                        s.Properties.Items.Add("Checklist / Protocols with in date range", 1);
                        s.Properties.Items.Add("Checklist with 'No Answers'", 2);
                        s.Properties.Items.Add("Checklist with blank Answers", 3);
                        s.Properties.Items.Add("Checklist with lot of Yes Answers", 4);
                        //s.Properties.Items.Add("Checklist has Findings", 4);
                        //s.Properties.Items.Add("Checklist has CAPAs", 5);
                        s.Properties.Items.Add("Random checklist or protocols", 5);
                        s.ControlStyle.Border.BorderStyle = BorderStyle.None;
                        s.Properties.ValidationSettings.RequiredField.IsRequired = true;
                    });
                });


                g.Items.Add(m => m.ChecklistId, i =>
                {
                    i.FieldName = "ChecklistId";
                    i.Caption = Translate("Checklist");
                    i.Width = Unit.Percentage(100);
                    i.NestedExtension().ComboBox(s =>
                    {
                        s.Properties.ValueType = typeof(int);
                        s.Properties.TextField = oapProtocolsCombo.DisplayColumnName;
                        s.Properties.ValueField = oapProtocolsCombo.KeyColumnName;
                        s.Width = Unit.Percentage(100);
                        s.Properties.DropDownStyle = DropDownStyle.DropDownList;
                        s.Properties.DataSource = oapProtocolsCombo.DataSource;
                        s.Properties.ValidationSettings.RequiredField.IsRequired = true;
                        s.PreRender = (a, e) =>
                        {
                            MVCxComboBox c = a as MVCxComboBox;
                            c.Text = "[Select Checklist]";
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
                    i.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
                    i.CssClass = "buttonToolbar";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtension().Button(s =>
                    {
                        s.Name = "relatedSearchButton";
                        s.Text = Translate("Search");
                        s.Width = Unit.Pixel(100);
                        s.UseSubmitBehavior = false;
                        s.ClientSideEvents.Click = "function(s,e) { onReviewSearchClick (s,e); }";
                    });
                });
            });
        }

        public ActionResult PreviousProtocolPartial(ReviewSearchModel model)
        {
            if (model.ChecklistId == 0 && model.SearchBy == 0)
            {
                return PartialView("PreviousProtocolPartial", new List<OapPreviousProtocolsFlatModel>());
            }

            var oapChecklists = OapAuditsClient.GetChecklistsForReviewAsync(model.SearchBy, model.ChecklistId, model.FromDate, model.ToDate).Result?.Result?.Data.ToList();

            return PartialView("PreviousProtocolPartial", oapChecklists.ToPreviousProtocolsModel(model, OapAuditsClient, OapChecklistFindingClient));
        }





        public void ConfigureCapaPreviousProtocolGrid(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeCapaPreviousProtocolGridData(html);

            settings.KeyFieldName = "Id";
            settings.Name = "gridPreviousProtocolsCAPAs";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.Configure(gridData, html, viewContext);

            settings.CallbackRouteValues = new { Action = "CapaPreviousProtocolPartial", ckListId = ViewData["SelectedCheckListId"] };

            settings.ClientSideEvents.BeginCallback = "function(s, e) { if(document.getElementById('SelectedCheckListId')==null) {e.processOnServer = false; }else{ e.customArgs['ckListId'] = SelectedCheckListId.value;}}";

            settings.CommandColumn.Visible = false;

        }
        private GridData InitializeCapaPreviousProtocolGridData(HtmlHelper htmlHelper)
        {
            var action = "CapaPreviousProtocolPartial";

            //Finding Type dropdown
            if (ViewData["FindingType"] == null)
            {
                ViewData["FindingType"] = GetClient<FindingTypeClient>().GetAllAsync().Result?.Result?.Data;
            }
            var findingTypes = ViewData["FindingType"] as IEnumerable<FindingType>;
            var oapFindingTypeCombo = new GridCombo("oapFindingTypeCombo", findingTypes);


            //Finding  Sub Type dropdown
            if (ViewData["FindingSubType"] == null)
            {
                ViewData["FindingSubType"] = GetClient<FindingSubTypeClient>().GetAllAsync().Result?.Result?.Data;
            }
            var findingSubTypes = ViewData["FindingSubType"] as IEnumerable<FindingSubType>;
            var oapFindingSubTypeCombo = new GridCombo("oapFindingSubTypeCombo", findingSubTypes);

            //Criticality
            if (ViewData["Criticalities"] == null)
            {
                ViewData["Criticalities"] = GetClient<LookupClient>().GetAllCriticalityAsync().Result?.Result?.Data;
            }
            var criticalities = ViewData["Criticalities"] as IEnumerable<Criticality>;
            var oapCriticalitiesCombo = new GridCombo("oapCriticalitiesCombo", criticalities);


            var gridData = new GridData("gridPreviousProtocolsCAPAs", "OapAuditReport", action, Translate("CAPA from Previous Protocols and Checklists"), "AddRelatedSearch", "Add", "search results", key: "RigOapChecklistQuestionId", displayColumnName: "Name", initializeCallBack: true, historicalRow: false, showPager: false);

            gridData.ToolBarOptions.DisplayCustomAddNew = false;

            gridData.AddRemoveCustomAddNew();
            gridData.ToolBarOptions.DisplayXlsExport = true;

            var displayColumns = new List<GridDisplayColumn>
            {
                //new GridDisplayColumn("ChecklistUniqueId", displayName:"Protocol or Checklist ID",width:10),
                new GridDisplayColumn("FindingId", displayName:"Finding Id",width:5),
                new GridDisplayColumn("Assessor", displayName:"Assessor",width:10, displayFormat: "g"),
                new GridDisplayColumn("FindingType", displayName:"Finding Type",width:10,columnType:MVCxGridViewColumnType.ComboBox,lookup:oapFindingTypeCombo),
                new GridDisplayColumn("FindingSubTypeId", displayName:"Finding SubType",width:10,columnType:MVCxGridViewColumnType.ComboBox,lookup:oapFindingSubTypeCombo),
                new GridDisplayColumn("CriticalityId", displayName:"Criticality",width:10, columnType:MVCxGridViewColumnType.ComboBox,lookup:oapCriticalitiesCombo),
                new GridDisplayColumn("CapaId", displayName:"CapaId",width:10),
                new GridDisplayColumn("ActionRequired", displayName:"ActionRequired",width:10),
                new GridDisplayColumn("AssignedTo", displayName:"Assigned To",width:10),
                new GridDisplayColumn("Status", displayName:"Status",width:10)
            };

            gridData.DisplayColumns = displayColumns;

            return gridData;
        }

        public ActionResult CapaPreviousProtocolPartial(Guid ckListId)
        {
            ViewData["SelectedCheckListId"] = ckListId;
            if (ckListId == null)
            {
                return PartialView("CapaFromPreviousProtocolPartial", new List<OapCapaProtocolsFlatModel>());
            }

            var oapChecklists = OapAuditsClient.GetCompleteProtocolAsync(ckListId).Result?.Result?.Data;


            return PartialView("CapaFromPreviousProtocolPartial", oapChecklists.ToFindingsCapaModel(OapAuditsClient, OapChecklistFindingClient));
        }

        #endregion
    }
}

using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ensco.App.Areas.Oap.Controllers
{
    using Ensco.App.App_Start;
    using Ensco.App.Areas.Oap.Extensions;
    using Ensco.App.Areas.Oap.Models;
    using Ensco.App.Infrastructure;
    using Ensco.Irma.Models;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
    using Ensco.Irma.Oap.WebClient.Rig;
    using Ensco.Irma.Services;
    using Ensco.Models;
    using Ensco.OAP.Models;
    using Ensco.OAP.Services;
    using Ensco.Services;
    using Ensco.Utilities;
    using MvcSiteMapProvider.Web.Mvc.Filters;
    using System.Threading.Tasks;

    [CustomAuthorize]
    public class GenericController : OapRigGridController
    {

        protected string BaseController
        {
            get;

            set;
        }

        public GenericController() : base()
        {
            RegisterClient(new List<Type>()
                            {
                                typeof(RigOapChecklistClient),
                                typeof(RigOapChecklistWorkflowClient),
                                typeof(PeopleClient),
                                typeof(OapChecklistFindingClient),
                                typeof(OapChecklistClient)
                            });

            BaseController = "Generic";
        }

        // GET: Checklists/Generic
        public ActionResult Index()
        {
            var rigChecklist = GetClient<RigOapChecklistClient>().GetAllAsync().Result?.Result?.Data?.FirstOrDefault();

            return List(rigChecklist.Id);
        }

        // GET: Checklists/Generic/List/5
        [SiteMapTitle("RigChecklistUniqueId")]
        public virtual ActionResult List(Guid id)
        {
            RigChecklist = GetRigChecklist(id);

            //Session.Timeout = 120;
            return View("GenericList", RigChecklist);
        }

        [HttpPost]
        public ActionResult Display(RigOapChecklist checklist)
        {
            return View("GenericList", RigChecklist);
        }

        [HttpPost]
        public ActionResult PageControl(string controlType)
        {
            if (!string.IsNullOrEmpty(controlType))
            {
                if (Enum.TryParse<GridConstants.ControlTypeEnum>(controlType, out GridConstants.ControlTypeEnum result))
                {
                    ControlType = result;
                }
            }

            return PartialView("GenericlistPageControlPartial", RigChecklist);
        }

        [HttpGet]
        public ActionResult ValidateChecklist(Guid rigOapChecklistId)
        {
            var result = GetClient<RigOapChecklistClient>().ValidateChecklistAsync(rigOapChecklistId).Result?.Result?.Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Sign(int userId, int order, string Comment)
        {
            var checklistId = RigChecklist?.Id ?? Guid.Empty;

            try
            {
                var result = GetClient<RigOapChecklistWorkflowClient>().ProcessWorkflowAsync(new ProcessWorkFlow() { Checklist = checklistId, User = userId, Transition = "Sign", Comment = Comment, Order = order }).Result.Result;

                var nextApprover = RigChecklist.VerifiedBy.OrderBy(v => v.Order).FirstOrDefault(v => v.Order > order);
                Lazy<UserModel> nextApproverUser = new Lazy<UserModel>(() => ServiceSystem.GetUser(nextApprover.UserId), isThreadSafe: false);                                
                SendOAPReviewEmail(nextApproverUser.Value);

            }
            catch (Exception ex)
            {

            }

            return (checklistId == Guid.Empty) ? RedirectToAction("Index") : RedirectToAction("List", new { id = checklistId });
        }

        [HttpGet]
        public ActionResult Review(int userId, int order, string Comment)
        {
            var checklistId = RigChecklist.Id;

            try
            {
                var result = GetClient<RigOapChecklistWorkflowClient>().ProcessWorkflowAsync(new ProcessWorkFlow() { Checklist = checklistId, User = userId, Transition = "Review", Comment = Comment, Order = order }).Result.Result;

            }
            catch (Exception ex)
            {

            }

            return (checklistId == Guid.Empty) ? RedirectToAction("Index", "GenericDashboard") : RedirectToAction("List", new { id = checklistId });
        }

        [HttpGet]
        public ActionResult Cancel(int userId, int order, string Comment)
        {
            try
            {
                var result = GetClient<RigOapChecklistWorkflowClient>()
                    .ProcessWorkflowAsync(new ProcessWorkFlow() { Checklist = RigChecklist.Id, Transition = "Cancel", Order = order, User = userId, Comment = Comment }).Result.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("List", new { Id = RigChecklist.Id });
        }

        [HttpGet]
        public ActionResult Reject(int userId, int order, string Comment)
        {
            var checklistId = RigChecklist.Id;

            try
            {  
                var result = GetClient<RigOapChecklistWorkflowClient>().ProcessWorkflowAsync(new ProcessWorkFlow() { Checklist = checklistId, User = userId, Transition = "Reject", Comment = Comment, Order = order }).Result.Result;

                var lastApprover = RigChecklist.VerifiedBy.OrderBy(v => v.Order).FirstOrDefault(v => v.Order < order);
                Lazy<UserModel> lastApproverUser = new Lazy<UserModel>(() => ServiceSystem.GetUser(lastApprover.UserId), isThreadSafe: false);
                SendOAPRejectEmail(lastApproverUser.Value);
            }
            catch (Exception ex)
            {
                throw;
            }

            return (checklistId == Guid.Empty) ? RedirectToAction("Index", "GenericDashboard") : RedirectToAction("List", new { id = checklistId });
        }

        [HttpGet]
        public virtual ActionResult StartWorkflow(Guid rigChecklistId)
        {
            var leadAssessor = RigChecklist.Assessors.FirstOrDefault(a => a.IsLead);

            var result = GetClient<RigOapChecklistWorkflowClient>().StartWorkflowAsync(new StartWorkFlow() { Checklist = rigChecklistId, Owner = leadAssessor.UserId }).Result.Result;            
            
            Lazy<UserModel> user = new Lazy<UserModel>(() => ServiceSystem.GetUser(leadAssessor.UserId), isThreadSafe: false);            
            SendOAPReviewEmail(user.Value);           
            
            return RedirectToAction("List", BaseController, new { id = rigChecklistId });
        }

        private GridConstants.ControlTypeEnum ControlType
        {
            get => (GridConstants.ControlTypeEnum)((Session != null) ? Session["ControlType"] ?? GridConstants.ControlTypeEnum.None : GridConstants.ControlTypeEnum.None);
            set
            {
                if (Session["ControlType"] != null)
                {
                    Session.Remove("ControlType");
                }

                Session["ControlType"] = value;
            }
        }

        public IList<OapSearchCheckListQuestionFlatModel> RelatedQuestionList
        {

            get => Session[GridConstants.relatedQuestionListKey] as List<OapSearchCheckListQuestionFlatModel>;
            set
            {
                if (Session[GridConstants.relatedQuestionListKey] != null)
                {
                    Session.Remove(GridConstants.relatedQuestionListKey);
                }

                Session[GridConstants.relatedQuestionListKey] = value;
            }
        }

        #region Api
        // GET: Checklists/Generic/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Checklists/Generic/Edit/5
        [HttpPost]
        public ActionResult Update(RigOapChecklist checklist)
        {
            return PartialView("GenericlistPageControlPartial", checklist);
        }

        // GET: Checklists/Generic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

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

        #region Assessors

        public ActionResult DisplayAssessors(RigOapChecklist planning, int? locationId)
        {
            if (planning != null)
            {
                var checklist = RigChecklist;
                if (UpdatePlanningLayoutColumns(checklist, planning, locationId))
                {
                    RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), checklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.AssessorsErrorsKey, ViewData);
                    return ChecklistAssessorsPartial();
                }
            }

            return ChecklistAssessorsPartial();
        }

        public ActionResult ChecklistAssessorsPartial()
        {
            var modelList = MapToViewModel(RigChecklist.Assessors, perItemAction: perItemMapping);
            ViewBag.RigChecklist = RigChecklist;

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

        [HttpPost]
        public async Task<ActionResult> DeleteAssessor(Guid id)
        {
            var result = await GetClient<RigOapChecklistClient>().DeleteAssessorAsync(id);
            if (result?.Result?.Data == true)
            {
                var assessor = RigChecklist.Assessors.FirstOrDefault(a => a.Id == id);
                RigChecklist.Assessors.Remove(assessor);
            }                

            return ChecklistAssessorsPartial();
        }

        private bool UpdatePlanningLayoutColumns(RigOapChecklist checklist, RigOapChecklist planning, int? locationId)
        {
            var updated = false;

            if (checklist == null)
            {
                return false;
            }

            if (locationId.HasValue && checklist.LocationId != locationId)
            {
                checklist.LocationId = locationId.Value;
                updated = true;
            }

            if (checklist.JobId != planning.JobId)
            {
                checklist.JobId = planning.JobId;
                updated = true;
            }

            //if (checklist.ChecklistDateTime != planning.ChecklistDateTime)
            //{
            //    checklist.ChecklistDateTime = planning.ChecklistDateTime;
            //    updated = true;
            //}

            if (CompareValues(checklist.Title, planning.Title))
            {
                checklist.Title = planning.Title;
                updated = true;
            }

            if (CompareValues(checklist.Description, planning.Description))
            {
                checklist.Description = planning.Description;
                updated = true;
            }
            return updated;
        }

        private bool CompareValues(string old, string current)
        {
            if (!string.IsNullOrEmpty(old) && !string.IsNullOrEmpty(current))
            {
                return !old.Equals(current, StringComparison.InvariantCultureIgnoreCase);
            }
            else if (string.IsNullOrEmpty(old) && !string.IsNullOrEmpty(current))
            {
                return true;
            }

            return false;
        }

        public JsonResult CheckIfLeadExists(bool isLead)
        {
            // Returns "false" (i.e., "not valid") if a user with the specified email address already exists. 
            return Json(!RigChecklist?.Assessors.Any(a => a.IsLead == isLead), JsonRequestBehavior.AllowGet);
        }       

        private bool ValidataAssessorsData(RigOapChecklistAssessor item, ObservableCollection<RigOapChecklistAssessor> assessors, out string errors, bool update = false)
        {
            var sb = new StringBuilder();

            var valid = true;

            errors = string.Empty;

            if (assessors.Any(a => a.IsLead && a.UserId != item.UserId && item.IsLead))
            {
                sb.AppendLine("Another assessor has been assigned as lead.  Please correct the data and save.");
                valid = false;
            }

            if (assessors.Where(a => a.UserId == item.UserId).Count() > (update ? 1 : 0))
            {
                sb.AppendLine("Duplicate assessors.  Please correct the data and save.");
                valid = false;
            }

            errors = sb.ToString();
            return valid;
        }

        public ActionResult CreateAssessor(RigOapChecklistAssessor model)
        {
            if (model.IsLead && RigChecklist.Assessors.Any(a => a.IsLead))
                ViewData["AssessorsErrors"] = "There can be only one lead assessor at a time";
            else if (RigChecklist.Assessors.Any(a => a.UserId == model.UserId))
                ViewData["AssessorsErrors"] = "This person is already an assessor";            
            else
                RigChecklist = GetClient<RigOapChecklistClient>().AddAssessorAsync(RigChecklist.Id, model).Result?.Result?.Data;

            return ChecklistAssessorsPartial();
        }

        private void UpdateVerifiedBy(RigOapChecklistAssessor model, RigOapChecklist checklist)
        {
            if (model.IsLead)
            {
                if ((checklist.VerifiedBy != null))
                {
                    var existingAssessor = checklist.VerifiedBy.SingleOrDefault(v => v.UserId == model.UserId && v.Role.Equals(GridConstants.VerifierRole.Assessor.ToString()));
                    if (existingAssessor == null)
                    {
                        var assessor = checklist.VerifiedBy.SingleOrDefault(v => v.UserId != model.UserId && v.Role.Equals(GridConstants.VerifierRole.Assessor.ToString()));
                        if (assessor != null)
                        {
                            checklist.VerifiedBy.Remove(assessor);
                        }
                        checklist.VerifiedBy.Add(new RigOapChecklistVerifier() { Role = GridConstants.VerifierRole.Assessor.ToString(), RigOapChecklistId = RigChecklist.Id, UserId = model.UserId });
                    }
                }
            }
        }

        //[ValidateInput(false)]
        public ActionResult UpdateAssessor(RigOapChecklistAssessor model)
        {
            var checklist = RigChecklist;
            if (checklist != null)
            {
                try
                {
                    if (ValidataAssessorsData(model, checklist.Assessors, out string errors, true))
                    {
                        var assessor = checklist.Assessors.FirstOrDefault(a => a.Id == model.Id);

                        if (assessor != null)
                        {

                            assessor.IsLead = model.IsLead;

                            if (assessor.UserId != model.UserId)
                            {
                                assessor.UserId = model.UserId;
                            }

                            UpdateVerifiedBy(model, checklist);

                            if (!model.IsLead)
                            {
                                if ((checklist.VerifiedBy != null))
                                {
                                    var removeverifier = checklist.VerifiedBy.SingleOrDefault(v => v.UserId == model.UserId && v.Role.Equals(GridConstants.VerifierRole.Assessor.ToString()));
                                    if (removeverifier != null)
                                    {
                                        checklist.VerifiedBy.Remove(removeverifier);
                                    }
                                }

                            }

                            RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), checklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.AssessorsErrorsKey, ViewData);

                        }

                    }
                    else
                    {
                        ViewData[GridConstants.AssessorsErrorsKey] = "There already a lead defined";
                    }
                }
                catch
                {
                }
            }
            return ChecklistAssessorsPartial();
        }

        //[ValidateInput(false)]
        public ActionResult DeleteAssessor(RigOapChecklistAssessor model)
        {
            var checklist = RigChecklist;
            if (checklist != null)
            {
                try
                {
                    var removeModel = checklist.Assessors.SingleOrDefault(a => a.Id == model.Id || a.UserId == model.UserId);
                    if (removeModel != null)
                    {
                        checklist.Assessors.Remove(removeModel);
                        var existingAssessor = checklist.VerifiedBy.SingleOrDefault(v => v.UserId == removeModel.UserId && v.Role.Equals(GridConstants.VerifierRole.Assessor.ToString()));
                        if (existingAssessor != null)
                        {
                            checklist.VerifiedBy.Remove(existingAssessor);
                        }
                        RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), checklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.AssessorsErrorsKey, ViewData);
                    }
                }
                catch
                {
                }
            }

            return ChecklistAssessorsPartial();
        }

        #endregion

        #region Verifiers

        public ActionResult VerificationGridPartial()
        {
            ViewBag.RigChecklist = RigChecklist;

            return PartialView("GenericlistVerificationPartial", RigChecklist?.VerifiedBy?.OrderBy(v => v.RigOapChecklistId).ToList() ?? new List<RigOapChecklistVerifier>());
        }

        public ActionResult DisplayVerifiers()
        {
            return VerificationGridPartial();
        }

        [ValidateInput(false)]
        public ActionResult UpdateBatchVerifiers(MVCxGridViewBatchUpdateValues<RigOapChecklistVerifier, int> verifiers)
        {
            var checklist = RigChecklist;
            var changed = false;

            // TODO: Add delete logic here
            foreach (var m in verifiers.Insert)
            {
                checklist.VerifiedBy.Add(m);
            }

            foreach (var m in verifiers.Update)
            {
                var existing = checklist.VerifiedBy.SingleOrDefault(a => a.Id == m.Id);
                if (existing != null)
                {
                    existing = m;
                }
            }

            foreach (var m in verifiers.DeleteKeys)
            {
                var existing = checklist.VerifiedBy.SingleOrDefault(a => a.UserId == m);
                if (existing != null)
                {
                    checklist.VerifiedBy.Remove(existing);
                }
            }
            if (changed || verifiers.Insert.Any() || verifiers.Update.Any() || verifiers.DeleteKeys.Any())
            {
                RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), checklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.VerificationErrorsKey, ViewData);
            }

            return VerificationGridPartial();
        }



        #endregion

        #region Execution Checklist

        [HttpPost]
        public ActionResult UpdateComments(string args, IList<KeyValueModel> comments)
        {
            UpdateQuestionAnswer(args);
            if (comments != null)
            {
                UpdateChecklistComments(RigChecklist, comments);

            }

            RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), RigChecklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.AssessorsErrorsKey, ViewData);


            return PartialView("GenericlistExecutionPartial", RigChecklist?.ToFlattenedModels());
        }

        [HttpPost]
        public ActionResult DisplayExecutionGrid()
        {
            return PartialView("GenericlistExecutionPartial", RigChecklist?.ToFlattenedModels());
        }


        public void UpdateQuestionAnswer(string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                var questionValues = args.Split(',');
                var questionId = questionValues[0];
                var questionAnswer = questionValues[1];

                var checklist = RigChecklist;

                var questions = checklist.Questions;

                if ((questions != null) && int.TryParse(questionId, out int qId))
                {
                    var question = questions.SingleOrDefault(q => q.OapChecklistQuestionId == qId);
                    var ans = question.Answers.FirstOrDefault();
                    if (ans != null)
                    {
                        ans.Value = questionAnswer;
                    }
                }
            }
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

                // TODO: Add delete logic here
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


        // POST: Checklists/Generic/Delete/5
        //[HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateQuestions(MVCxGridViewBatchUpdateValues<OapGenericCheckListFlatModel, int> values, IList<KeyValueModel> comments)
        {
            try
            {
                var checklist = RigChecklist;
                var commentsUpdated = false;

                if (comments != null)
                {
                    commentsUpdated = UpdateChecklistComments(checklist, comments);
                }

                // TODO: Add delete logic here
                UpdateBatchEditedValues(checklist, values);


                if (commentsUpdated || values.Update.Any())
                {
                    RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), checklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.ExecutionErrorsKey, ViewData);
                }
            }
            catch (Exception ex)
            {
                ViewData[GridConstants.ExecutionErrorsKey] = ex.Message;
            }

            return DisplayExecutionPartialView();
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
            return PartialView("GenericlistExecutionPartial", RigChecklist?.ToFlattenedModels());
        }

        #endregion

        #region Checklist Findings 

        #endregion

        #endregion

        #region Configurations

        /// <summary>
        /// Configures the Generic List Layout
        /// </summary>
        /// <param name="html"></param>
        /// <param name="viewContext"></param>
        public virtual void ConfigureGenericlist(HtmlHelper html, ViewContext viewContext)
        { 
            var help = RigChecklist?.OapChecklist?.Help;

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
                        viewContext.Writer.Write(RigChecklist?.OapChecklist?.Help ?? string.Empty);
                    });
                }).GetHtml();
            }

            html.DevExpress().ComboBox(s =>
            {
                s.Name = "monitorControls";
                s.ClientVisible = false;
                s.Properties.Items.Add("Lessons Learned", GridConstants.ControlTypeEnum.LessonsLearned);
                s.Properties.Items.Add("Comments", GridConstants.ControlTypeEnum.FormComments);
                s.Properties.Items.Add("Review", GridConstants.ControlTypeEnum.Review);
                s.Properties.Items.Add("Finding", GridConstants.ControlTypeEnum.Finding);
                s.Properties.Items.Add("Related Search", GridConstants.ControlTypeEnum.RelatedSearch);
                s.Properties.DropDownStyle = DropDownStyle.DropDown;
                s.Style.Add("margin-top", "10px");
                s.CallbackRouteValues = new { Controller = BaseController, Action = "Display", checklist = RigChecklist };
                s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s, e) { genericChecklistPage.PerformCallback();}";
            }).GetHtml();
        }

        public virtual void ConfigureTabs(PageControlSettings settings, HtmlHelper html, ViewContext viewContext)
        {

            var Html = html;

            settings.Name = "genericChecklistPage";
            settings.ActivateTabPageAction = ActivateTabPageAction.Click;
            settings.EnableHotTrack = true;
            settings.SaveStateToCookies = true;
            settings.TabAlign = TabAlign.Left;
            settings.TabPosition = TabPosition.Top;
            settings.Width = Unit.Percentage(100);
            settings.CallbackRouteValues = new { Controller = BaseController, Action = "PageControl" };
            settings.ClientSideEvents.BeginCallback = "function (s, e) { if (monitorControls != 'undefined') { e.customArgs['ControlType'] = monitorControls.GetValue(); } }";
            settings.ClientSideEvents.ActiveTabChanging = "function (s,e) { if (e.tab.name == 'tabVerification' || e.tab.name == 'tabMonitoring') { e.reloadContentOnCallback = true; } }";
            string setPlanningTab = Session["isChecklistIdClick"]?.ToString();
            if (!string.IsNullOrEmpty(setPlanningTab) && setPlanningTab == "True")
            {
                settings.ClientSideEvents.Init = "function (s,e) { s.SetActiveTabIndex(0); }";
                //settings.SaveStateToCookies = false;        
            }
            Session["isChecklistIdClick"] = false;

            var checklistPlanningExecution = settings.TabPages.Add("ChecklistPlanning");
            checklistPlanningExecution.Text = Translate("Planning");
            checklistPlanningExecution.Name = "tabPlanning";

            checklistPlanningExecution.SetContent(() =>
            {
                html.RenderPartial("GenericlistPlanningPartial", RigChecklist);

                viewContext.Writer.Write("<br/>");

                html.RenderAction("ChecklistAssessorsPartial");

            });


            var checklistExecution = settings.TabPages.Add("ChecklistExecution");
            checklistExecution.Text = Translate("Execution");
            checklistExecution.SetContent(() =>
            {
                html.RenderPartial("GenericlistExecutionToolbarPartial", RigChecklist);
                viewContext.Writer.Write("<br/>");
                var models = RigChecklist?.ToFlattenedModels();
                viewContext.ViewBag.RigChecklist = RigChecklist;
                viewContext.ViewBag.OapChecklistClient = GetClient<OapChecklistClient>();
                html.RenderPartial("GenericlistExecutionPartial", models);

                //using (html.BeginForm("Comments", BaseController))
                //{
                foreach (var c in RigChecklist?.Comments.Distinct())
                {
                    viewContext.Writer.Write("<br/>");
                    html.RenderPartial("GenericlistCommentPartial", c);
                }
                //};
            });

            var checklistVerification = settings.TabPages.Add("ChecklistVerification");        
            checklistVerification.Text = Translate("Verification");
            checklistVerification.Name = "tabVerification";
            checklistVerification.SetContent(() =>
            {
                html.RenderPartial("GenericlistVerificationToolbarPartial", RigChecklist);
                viewContext.Writer.Write("<br/>");

                html.RenderAction("VerificationGridPartial");
            });

            var checklistMonitoring = settings.TabPages.Add("ChecklistMonitoring");
            checklistMonitoring.Text = Translate("Monitoring");
            checklistMonitoring.Name = "tabMonitoring";
            
            var sourceForm = "Oap"; // String.Format("Oap - Checklist: {0},  ControlNumber: {1}, RigChecklistId: {2}, ChecklistDate: {3} ", RigCheckList.OapChecklist.Name, RigCheckList.OapChecklist.ControlNumber, RigCheckList.RigChecklistUniqueId, RigCheckList.ChecklistDateTime);

            checklistMonitoring.SetContent(() =>
            {
                html.RenderAction("Control", "Comments", new { Area = "IRMA", sourceForm, sourceFormId = RigChecklist.Id.ToString() });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("Control", "LessonsLearned", new { Area = "IRMA", sourceForm, SourceFormId = RigChecklist.RigChecklistUniqueId });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("Control", "Review", new { Area = "IRMA", sourceForm, SourceFormId = RigChecklist?.Id });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("Display", "Findings", new { questions = RigChecklist?.Questions });
                viewContext.Writer.Write("<br/>");
                html.RenderAction("DisplayRelatedCapa", "Generic", new { checklistId = RigChecklist?.Id });
                viewContext.Writer.Write("<br/>");
                html.RenderPartial("GenericlistRelatedSearchQueryPartial", new RelatedSearchQueryModel());
                viewContext.Writer.Write("<br/>");
                html.RenderPartial("GenericlistRelatedSearchPartial", RelatedQuestionList ?? new List<OapSearchCheckListQuestionFlatModel>());
            });
        }

        #region Planning

        public virtual bool EnableSave(RigOapChecklist rigOapChecklist)
        {
            var isPending = rigOapChecklist?.Status?.Equals(GridConstants.ChecklistStatus.Pending.ToString(), StringComparison.InvariantCultureIgnoreCase);
            var isCompleted = rigOapChecklist?.Status?.Equals(GridConstants.ChecklistStatus.Completed.ToString(), StringComparison.InvariantCultureIgnoreCase);

            var assessorsPresent = rigOapChecklist?.Assessors.Any() ?? false;

            return !((isPending ?? false) || (isCompleted ?? false)) && assessorsPresent;
        }

        public virtual void ConfigurePlanning(FormLayoutSettings<RigOapChecklist> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            var lkpList = Ensco.Utilities.UtilitySystem.GetLookupList("Location");
            if (lkpList == null)
            {
                lkpList = new LookupListModel<dynamic>();
                lkpList.Items = new List<object>();
            }

            var items = (lkpList.Items as List<object>)?.Cast<LocationModel>();

            var item = items?.SingleOrDefault(it => it.Id == RigChecklist.LocationId);

            if (item != null)
            {
                RigChecklist.LocationName = item.Name;
            }

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
                        s.UseSubmitBehavior = true;
                        s.Width = Unit.Pixel(100);
                        s.RouteValues = new { Action = "UpdatePlanning", rigOapChecklistId = RigChecklist.Id };
                        s.ClientSideEvents.Click = $"function (s, e) {{ e.customArgs = MVCxClientUtils.GetSerializedEditorValuesInContainer('planningLayout',true); e.customArgs['LocationId'] = $('#LocationId').val(); e.processOnServer = true; }}";
                        s.Enabled = RigChecklist.Status == "Open" && (RigChecklist.VerifiedBy.Any(v => v.UserId == UtilitySystem.CurrentUserId) || RigChecklist.Assessors.Any(v => v.UserId == UtilitySystem.CurrentUserId));
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

                var idCol = g.Items.Add(m => m.RigChecklistUniqueId, i =>
                {
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.Caption = Translate("Checklist Id");
                    //i.RequiredMarkDisplayMode = FieldRequiredMarkMode.Hidden;
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = RigChecklist.RigChecklistUniqueId.ToString();
                    });
                });

                g.Items.Add(m => m.LocationName, i =>
                {
                    lkpList.BoundFieldName = "LocationName";
                    lkpList.FocusedRowChanged = "function(s, e) { var focusedKey = LocationName_treeListPartial.GetFocusedNodeKey(); LocationName_treeListPartial.GetNodeValues(focusedKey, 'Name', function(values) { LocationName.SetText(values); }); LocationName_treeListPartial.GetNodeValues(focusedKey, '" + lkpList.KeyFieldName + "', function(values) { $('#LocationId').val(values);  }); }";
                    lkpList.Name = "LocationName";
                    Session["LocationName"] = lkpList;
                    i.FieldName = "LocationName";
                    i.Caption = Translate("Location");
                    i.Name = "LocationName";
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtension().DropDownEdit(s =>
                    {
                        s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                        s.Properties.ValidationSettings.Display = DevExpress.Web.Display.Dynamic;
                        s.Properties.ValidationSettings.SetFocusOnError = true;
                        s.Properties.ClientSideEvents.DropDown = "function (s, e) {var locId = LocationId.value; LocationName_treeListPartial.SetFocusedNodeKey(locId); }";
                        s.Width = Unit.Percentage(100);
                        s.Enabled = RigChecklist.Status == "Open";

                        s.SetDropDownWindowTemplateContent(c =>
                        {
                            var selectedId = RigChecklist.LocationId;
                            var selectedValue = RigChecklist.LocationName;
                            htmlHelper.ViewContext.Writer.Write(selectedValue);
                            htmlHelper.RenderAction("TreeLookupPartial", "Common", new { Area = "Common", name = i.FieldName, lookup = "Location", multi = false, selected = selectedId });
                        });

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
                    nsettings.Enabled = RigChecklist.Status == "Open";
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
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
                        s.Text = RigChecklist.Status;
                    });
                });

                g.Items.Add(m => m.Title, i =>
                {
                    i.Caption = Translate("Title");
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Height = Unit.Percentage(10);
                    i.NestedExtensionSettings.Enabled = RigChecklist.Status == "Open";
                });

                g.Items.Add(m => m.Description, i =>
                {
                    i.Caption = Translate("Description");
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.Memo;
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Height = Unit.Pixel(71);
                    i.NestedExtensionSettings.Enabled = RigChecklist.Status == "Open";
                });

                g.Items.Add(m => m.JobId, i =>
                {   
                    i.Caption = Translate("Job");
                    i.Width = Unit.Percentage(30);
                    i.FieldName = "JobId";
                    i.Name = "Job";
                    var readOnly = RigChecklist.Status == "Open";
                    i.SetNestedContent(() => htmlHelper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = i.FieldName, lookup = "Job", multi = false, selected = RigChecklist.JobId, readOnly = !readOnly }));

                });
            });
        }

        public ActionResult UpdatePlanning(Guid rigOapChecklistId, RigOapChecklist model)
        {
            RigChecklist.ChecklistDateTime = model.ChecklistDateTime;
            RigChecklist.Description = model.Description;
            RigChecklist.JobId = model.JobId;
            RigChecklist.Title = model.Title;
            RigChecklist.LocationId = model.LocationId;

            var result = GetClient<RigOapChecklistClient>().UpdateRigChecklistAsync(RigChecklist).Result?.Result?.Data;

            return RedirectToAction("List",new { id = rigOapChecklistId });
        }


        #endregion

        #region Execution
        public virtual void ConfigureExecutionToolbar(FormLayoutSettings<RigOapChecklist> settings, HtmlHelper html, ViewContext viewContext, RigOapChecklist model)
        {
            settings.Name = "ExecutionToolbar";
            settings.UseDefaultPaddings = false;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            var item = settings.Items.Add(i =>
            {
                i.ShowCaption = DefaultBoolean.False;
                i.CssClass = "buttonToolbar";
                i.Width = Unit.Percentage(100);
                i.NestedExtension().Button(s =>
                {
                    s.Name = "applyExecutionChanges";
                    s.Text = "Save";
                    s.UseSubmitBehavior = false;
                    s.Width = Unit.Pixel(100);
                    s.ClientSideEvents.Click = "onTabToolBarClick";
                    s.Enabled = EnableSave(model);
                });
            });

        }

        //Configures the execution tab to display the list
        public virtual void ConfigureExecution(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistExecutionGridData(html, viewContext);

            settings.Configure(gridData, html, viewContext);

            settings.SettingsBehavior.MergeGroupsMode = GridViewMergeGroupsMode.Disabled;

            settings.ClientSideEvents.BeginCallback = $"{GridConstants.rigChecklistExecutionGrid}BeginCallback";
            settings.ClientSideEvents.BatchEditStartEditing = "function (s,e) { if (e.focusedColumn.fieldName.substring(0, 7) !== 'YesNoNa') { e.cancel = true;} }";
            settings.ClientSideEvents.DetailRowExpanding = "function (s, e) {  isExecutionGridDetailRowExpanded[e.visibleIndex] = true;  }";
            settings.ClientSideEvents.DetailRowCollapsing = "function (s, e) {  isExecutionGridDetailRowExpanded[e.visibleIndex] = false;  }";

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.ShowDetailButtons = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.SetDetailRowTemplateContent(c => ConfigureExecutionQuestionComments(html, viewContext, c));
            settings.ClientSideEvents.BatchEditConfirmShowing = "function(s, e) {e.cancel = true;}";            
            settings.SettingsPager.PageSize = 100; // Makes checklist show all items without scrollbar

            settings.CommandButtonInitialize = (o, e) =>
            {
                if ((e.ButtonType == ColumnCommandButtonType.Update) || (e.ButtonType == ColumnCommandButtonType.Cancel))
                {
                    e.Visible = false;
                }
            };            

            settings.CustomColumnSort = (s, e) =>
            {
                if (e.Column != null && e.Column.FieldName == "SubGroup")
                {
                    int order1, order2; 
                    if (int.TryParse(e.GetRow1Value("SubGroupOrder").ToString(),out order1) && int.TryParse(e.GetRow2Value("SubGroupOrder").ToString(), out order2))
                    {
                        e.Result = order1 < order2 ? -1 : (order1 == order2 ? 0 : 1);
                        e.Handled = true;
                    }                    
                } else if (e.Column != null && e.Column.FieldName == "Question")
                {
                    int order1, order2;
                    if (int.TryParse(e.GetRow1Value("QuestionOrdinal").ToString(), out order1) && int.TryParse(e.GetRow2Value("QuestionOrdinal").ToString(), out order2))
                    {
                        e.Result = order1 < order2 ? 1 : -1; 
                        e.Handled = true;
                    }
                }

            };

            settings.PreRender = ExpandDetailRowsIfNoIsSelected;
            settings.BeforeGetCallbackResult = ExpandDetailRowsIfNoIsSelected;

        }

        protected virtual void ExpandDetailRowsIfNoIsSelected(object sender, EventArgs args)
        {
            MVCxGridView gridView = (MVCxGridView)sender;

            for (int i = 0; i < gridView.VisibleRowCount; i++)
            {
                string answer = gridView.GetRowValues(i, "YesNoNa0")?.ToString();
                if (answer == "N")
                    gridView.DetailRows.ExpandRow(i);
                //else
                //gridView.DetailRows.CollapseRow(i);


                var question = RigChecklist.ToFlattenedModels().Where(c => c.RigQuestionId == (Guid)(gridView.GetRowValues(i, "RigQuestionId"))).FirstOrDefault();
                //var question = RigChecklist.Questions.Where(c => c.OapChecklistQuestionId == Convert.ToInt32(gridView.GetRowValues(i, "QuestionId"))).FirstOrDefault();

                IOAPServiceDataModel dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
                IEnumerable<CommentModel> comments = dataModel.GetItems(string.Format("QuestionId=\"{0}\" AND SourceFormId=\"{1}\"", question.RigQuestionId, RigChecklist.Id), "Id").Cast<CommentModel>();
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
            html.RenderAction("InlineControl", new { Controller = "Comments", Area = "IRMA", SourceForm = "OAP", SourceFormId = RigChecklist.Id, QuestionId = rigQuestionId, QuestionText = question });
        }
                
        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistAssessorsGridData(html, viewContext);
            settings.Configure(gridData, html, viewContext);
            settings.ClientSideEvents.BeginCallback = $"{GridConstants.rigChecklistAssessorGrid}BeginCallback";
            settings.ClientSideEvents.EndCallback = $"{GridConstants.rigChecklistAssessorGrid}EndCallback";
            settings.SettingsExport.BeforeExport = (sender, e) =>
            {
                MVCxGridView gridView = sender as MVCxGridView;
                if (sender == null)
                    return;
                gridView.Columns["UserId"].Visible = false;
            };
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

        #endregion

        #region Monitoring

        public virtual void ConfigureMonitoring(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {

        }

        #endregion

        #region Verification        

        public virtual void ConfigureVerifiers(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistVerifiersGridData(html);
            settings.Configure(gridData, html, viewContext);

            settings.CommandButtonInitialize = (o, e) =>
            {
                if ((e.ButtonType == ColumnCommandButtonType.Update) || (e.ButtonType == ColumnCommandButtonType.Cancel))
                {
                    e.Visible = false;
                }
            };
        }


        #endregion

        #region Related Search 
        public void ConfigureRelatedSearchQuery(FormLayoutSettings<RelatedSearchQueryModel> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            settings.Name = "relatedSearchLayout";
            settings.UseDefaultPaddings = true;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            var topicList = new List<(int Id, string Topic)>();

            var groupList = RigChecklist.OapChecklist.Groups.ToList();

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

            RigChecklist.Questions.Select(q =>
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
                        s.ClientSideEvents.Click = "onRelatedSearchClick";
                    });
                });

            });
        }
        #endregion

        #endregion

        #region Initialize GridData
        private GridData InitializeChecklistAssessorsGridData(HtmlHelper html, ViewContext viewContext)
        {
            var action = "DisplayAssessors";

            //User Lookup Details
            if (ViewData["Users"] == null)
            {
                ViewData["Users"] = CommonUtilities.GetUsers(GetClient<PeopleClient>());
            }
            var users = ViewData["Users"] as IEnumerable<Person>;
            var userCombo = new GridCombo("UserCombo", users);

            //Position Lookup Details
            var lkpList = Ensco.Utilities.UtilitySystem.GetLookupList("Position");
            if (lkpList == null)
            {
                lkpList = new LookupListModel<dynamic>();
                lkpList.Items = new List<object>();
            }
            var items = (lkpList.Items as List<object>)?.Cast<PositionModel>();
            if (ViewData["Position"] == null)
            {
                ViewData["Position"] = items;
            }
            var positions = ViewData["Position"];
            var positionsCombo = new GridCombo("PositionModel", positions);

            //Department Lookup Details
            var lkpDepartmentList = Ensco.Utilities.UtilitySystem.GetLookupList("Department");
            if (lkpDepartmentList == null)
            {
                lkpDepartmentList = new LookupListModel<dynamic>();
                lkpDepartmentList.Items = new List<object>();
            }
            var deptItems = (lkpDepartmentList.Items as List<object>)?.Cast<DepartmentModel>();
            if (ViewData["Departments"] == null)
            {
                ViewData["Departments"] = deptItems;
            }
            var departments = ViewData["Departments"];
            var departmentsCombo = new GridCombo("DepartmentModel", departments);

            //Tour Lookup Details
            var lkpTourList = IrmaServiceSystem.GetLookupList(IrmaConstants.IrmaLookupLists.Tour);
            if (lkpTourList == null)
            {
                lkpTourList = new LookupListModel<dynamic>();
                lkpTourList.Items = new List<object>();
            }
            var tourItems = (lkpTourList.Items as List<object>)?.Cast<TourModel>();
            if (ViewData["Tour"] == null)
            {
                ViewData["Tour"] = tourItems;
            }
            var tour = ViewData["Tour"];
            var tourCombo = new GridCombo("TourModel", tour);



            //Grid Definition
            var gridData = new GridData(GridConstants.rigChecklistAssessorGrid, BaseController, action, "Assessor", "AddNewAssessor", "Add Assessor", "search assessor", key: "Id", displayColumnName: "Name", initializeCallBack: true, historicalRow: false);

            gridData.ToolBarOptions.DisplayXlsExport = true;

            if (!EnableSave(RigChecklist))
            {
                gridData.SetGridReadOnly();
            }

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("Name", displayName: "Assessor", columnType: MVCxGridViewColumnType.TextBox, width:20),
                new GridDisplayColumn("UserId", displayName: Translate("Assessor"), columnType:MVCxGridViewColumnType.ComboBox, lookup:userCombo, editLayoutWidth:100,
                columnAction:  c =>{
                    c.FieldName = "UserId";
                    c.Caption = Translate("Assessor");
                    c.Width = Unit.Percentage(10);
                    c.Name = "Assessor";
                    c.CellStyle.HorizontalAlign = HorizontalAlign.Left;

                    c.SetEditItemTemplateContent((ic) =>
                      {
                         var userId = DataBinder.Eval(ic.DataItem, "UserId");
                         html.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = c.FieldName, lookup = "Passport", multi = false, selected = userId  });
                      });
                  }, allowHeaderFilter: DefaultBoolean.True,  isVisible: false),


                new GridDisplayColumn("PositionId", displayName: Translate("Position"), columnType:MVCxGridViewColumnType.ComboBox, lookup:positionsCombo, isReadOnly: true,editLayoutWidth:100, width:20),
                new GridDisplayColumn("DepartmentId", displayName: Translate("Department"),columnType:MVCxGridViewColumnType.ComboBox, lookup:departmentsCombo, isReadOnly: true,editLayoutWidth:100, width:20),
                new GridDisplayColumn("TourId", displayName: Translate("Tour"), isReadOnly: true,editLayoutWidth:100,columnType:MVCxGridViewColumnType.ComboBox, lookup:tourCombo, width:20),
                new GridDisplayColumn("IsLead", displayName: Translate("Lead"), columnType: MVCxGridViewColumnType.CheckBox,  editLayoutWidth:100),
                new GridDisplayColumn("Role", displayName: Translate("Role"),width:0, isVisible: false),
                new GridDisplayColumn("RigOapChecklistId", displayName: "RigChecklist",width:0, isVisible: false),
                new GridDisplayColumn("Id", displayName: "RigChecklistAssessor",width:0, isVisible: false)
            };

            gridData.DisplayColumns = displayColumns;

            gridData.Routes.Add(new GridRoute(GridRouteTypes.Add, new { Controller = BaseController, Action = "CreateAssessor" }));
            gridData.Routes.Add(new GridRoute(GridRouteTypes.Update, new { Controller = BaseController, Action = "UpdateAssessor" }));
            gridData.Routes.Add(new GridRoute(GridRouteTypes.Delete, new { Controller = BaseController, Action = "DeleteAssessor" }));

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("UserId", displayName:"Assessor"),
                                new GridEditLayoutColumn("IsLead", displayName: "Lead"),
                                new GridEditLayoutColumn("TourId", displayName:"Tour", isVisible:false),
                                new GridEditLayoutColumn("Role", displayName:"Role", isVisible:false),
                                new GridEditLayoutColumn("Id", displayName:"Id", isVisible:false),
                                new GridEditLayoutColumn("RigOapChecklistId", displayName:"RigOapChecklist", isVisible:false),
            };

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["Id"] = Guid.NewGuid();
                e.NewValues["Role"] = "Assessor";
                e.NewValues["RigOapChecklistId"] = RigChecklist?.Id;
                e.NewValues["IsLead"] = false;
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

        protected virtual GridData InitializeChecklistVerifiersGridData(HtmlHelper htmlHelper)
        {

            if (ViewData["Users"] == null)
            {
                ViewData["Users"] = CommonUtilities.GetUsers(GetClient<PeopleClient>());
            }

            var users = ViewData["Users"] as IEnumerable<Person>;
            var userCombo = new GridCombo("UserCombo", users);

            var gridData = new GridData(GridConstants.rigChecklistVerifierGrid, historicalRow: false);

            var rolesCombo = new GridCombo("RolesCombo", new List<object> { new { id = GridConstants.VerifierRole.Assessor, Name = "Assessor" }, new { id = GridConstants.VerifierRole.OIM, Name = "OIM" } });

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("Role", displayName: "Role", width:15, isReadOnly:true, allowHeaderFilter: DefaultBoolean.False, allowSort: DefaultBoolean.False, columnType: MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: rolesCombo),
                new GridDisplayColumn("UserId", displayName: "Verifier", width:20, isReadOnly:true, allowHeaderFilter: DefaultBoolean.False, allowSort: DefaultBoolean.False,  columnType: MVCxGridViewColumnType.ComboBox, editLayoutWidth:100, lookup: userCombo),
                new GridDisplayColumn("SignedOn", displayName: "Review/Sign", width: 25, columnAction: column => {
                    column.FieldName = "SignedOn";
                    column.Caption = Translate("Review / Sign");
                    column.Settings.AllowSort = DefaultBoolean.False;
                    column.Settings.AllowHeaderFilter = DefaultBoolean.False;
                    column.Width = Unit.Percentage(20);

                    column.SetDataItemTemplateContent(c =>
                            {
                                var assigneeId = (int) (DataBinder.Eval(c.DataItem, "UserId") ?? 0);

                                if (!Enum.TryParse<GridConstants.VerifierRole>((string) (DataBinder.Eval(c.DataItem, "Role") ?? ""), out GridConstants.VerifierRole role ))
                                {
                                    role = GridConstants.VerifierRole.None;
                                }


                                var signed =  (DateTime)(DataBinder.Eval(c.DataItem, "SignedOn") ?? DateTime.MinValue);
                                if (signed != DateTime.MinValue)
                                {
                                    htmlHelper.ViewContext.Writer.Write(signed.ToLongDateString());

                                }
                                else if (!string.IsNullOrEmpty(RigChecklist.Status) && RigChecklist.Status.Equals(GridConstants.rigChecklistPendingWorkFlowStatus))  // Process only if the checklist is in workflow
                                {
                                    if (assigneeId == UtilitySystem.CurrentUserId && role == GridConstants.VerifierRole.Assessor) // Sign
                                    {
                                         htmlHelper.DevExpress().Button(btSettings =>
                                        {
                                            btSettings.Name = "btnSignatureSign_" + c.VisibleIndex;
                                            btSettings.Text = "Sign";
                                            btSettings.UseSubmitBehavior = false;
                                            //btSettings.ClientSideEvents.Click = string.Format("function(s, e) {{ document.location='{0}'; }}", string.Format("{0}?AssigneeId={3}&Status=sign", Url,assigneeId));
                                            btSettings.RouteValues = new { Controller = BaseController, Action = "Sign", userId = assigneeId };
                                            btSettings.RenderMode = ButtonRenderMode.Button;
                                        }).Render();
                                    }
                                    else if (assigneeId == UtilitySystem.CurrentUserId && role == GridConstants.VerifierRole.OIM) // Review
                                    {
                                        var comments =  (string) (DataBinder.Eval(c.DataItem, "Comment") ?? "");

                                        htmlHelper.DevExpress().Button(btSettings =>
                                        {
                                            btSettings.Name = "btnSignatureReview_" + c.VisibleIndex;
                                            btSettings.Text = "Review";
                                            btSettings.UseSubmitBehavior = false;
                                            btSettings.Style.Add("margin-right","0.8em");
                                            btSettings.RouteValues = new { Controller = BaseController , Action = "Review", userId = assigneeId};
                                            btSettings.RenderMode = ButtonRenderMode.Button;
                                            btSettings.ClientSideEvents.Click = "function (s,e) { e.processOnServer = confirm('Confirm review?'); }";
                                        }).Render();
                                        htmlHelper.DevExpress().Button(btSettings =>
                                        {
                                            btSettings.Name = "btnSignatureReject_" + c.VisibleIndex;
                                            btSettings.Text = "Reject";
                                            btSettings.UseSubmitBehavior = false;
                                            btSettings.RouteValues = new { Controller = BaseController, Action = "Reject", userId = assigneeId, comment = comments};
                                            btSettings.RenderMode = ButtonRenderMode.Button;
                                            btSettings.ClientSideEvents.Click = "function (s,e) { e.processOnServer = confirm('Confirm rejection?'); }";
                                        }).Render();
                                        htmlHelper.ViewContext.Writer.Write("&nbsp;");
                                        //htmlHelper.DevExpress().TextBox(txt =>
                                        //{
                                        //    txt.Name = "ReviewComment_" + c.VisibleIndex;
                                        //    txt.Width = Unit.Percentage(100);
                                        //    txt.Style.Add("margin-top", "10px;");
                                        //    txt.Properties.NullText = "Reviewer Comments";
                                        //}).Render();
                                    }
                                    else
                                    {
                                        htmlHelper.ViewContext.Writer.Write(string.Empty);
                                    }
                                }
                                else
                                {
                                    htmlHelper.ViewContext.Writer.Write(string.Empty);
                                }
                            });
                }),
                new GridDisplayColumn("Status", displayName: "Workflow Status", allowHeaderFilter: DefaultBoolean.False, allowSort: DefaultBoolean.False, width: 20, editLayoutWidth:100, isReadOnly: true),
                new GridDisplayColumn("Comment", displayName: "Comment", allowHeaderFilter: DefaultBoolean.False, allowSort: DefaultBoolean.False, width: 40, columnType: MVCxGridViewColumnType.TextBox, editLayoutWidth:100),
                new GridDisplayColumn("RigOapChecklistId", displayName: "RigChecklist", width:0, isVisible: false),
                new GridDisplayColumn("Id", displayName: "RigChecklistVerifier", width:0, isVisible: false)
            };

            gridData.DisplayColumns = displayColumns;


            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["RigOapChecklistId"] = RigChecklist?.Id;
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

        protected virtual IEnumerable<KeyValueModel> GetYesNoNaValues()
        {
            return new List<KeyValueModel>()
                {
                    new KeyValueModel("Yes", "Y", displayValue: "Yes".Translate()),
                    new KeyValueModel("No", "N", displayValue:  "No".Translate()),
                    new KeyValueModel("NA", "NA", displayValue: "N\\A".Translate())
                };
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
                //new GridDisplayColumn("QuestionOrdinal", displayName: " ", order:20, width:5, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("Topic", displayName: "Category", order:30, width:15, columnType: MVCxGridViewColumnType.TextBox , isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False,columnAction: (c) => {
                    c.FieldName = "Topic";
                    c.Settings.AllowCellMerge = DefaultBoolean.False;
                    c.Width =  Unit.Percentage(15);
                }),
                new GridDisplayColumn("Question", order:40,columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False, columnAction: c => {
                    c.FieldName = "Question";
                    c.Caption = "Verification";
                    c.Width = Unit.Percentage(45);
                    c.SortIndex = 2;
                    c.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
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
                           //  s.ClientSideEvents.Click = $"function (s,e) {{ let cell = {GridConstants.rigChecklistExecutionGrid}.GetFocusedCell() ; if (isExecutionGridDetailRowExpanded[cell.rowVisibleIndex] != true) {{ {GridConstants.rigChecklistExecutionGrid}.batchEditApi.SetCellValue(cell.rowVisibleIndex, 'DisplayNoControl', {"false"}); {GridConstants.rigChecklistExecutionGrid}.ExpandDetailRow(cell.rowVisibleIndex);  }} else {{ {GridConstants.rigChecklistExecutionGrid}.CollapseDetailRow(cell.rowVisibleIndex); }}; }}";
                             s.ClientSideEvents.Click = $"function (s,e) {{ var cell = {GridConstants.rigChecklistExecutionGrid}.GetFocusedCell() ;   {GridConstants.rigChecklistExecutionGrid}.batchEditApi.SetCellValue(cell.rowVisibleIndex, 'DisplayNoControl', {"false"}); {GridConstants.rigChecklistExecutionGrid}.ExpandDetailRow(cell.rowVisibleIndex); }}";
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

                var saveEnabled = EnableSave(RigChecklist);

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

                                //s.Properties.ClientSideEvents.GotFocus = $"function (s,e) {{ debugger; var val = {s.Name}.GetValue(); var cell = {rigChecklistExecutionGrid}.GetFocusedCell() ; if ((val === 'N')) {{ if (isExecutionGridDetailRowExpanded[cell.rowVisibleIndex] != true) {{ {rigChecklistExecutionGrid}.ExpandDetailRow(cell.rowVisibleIndex); }} }} else {{ if (isExecutionGridDetailRowExpanded[cell.rowVisibleIndex] == true) {{ {rigChecklistExecutionGrid}.CollapseDetailRow(cell.rowVisibleIndex); }} }} {rigChecklistExecutionGrid}.batchEditApi.StartEdit(); {rigChecklistExecutionGrid}.batchEditApi.SetCellValue(cell.rowVisibleIndex, cell.column.fieldName, val); {rigChecklistExecutionGrid}.batchEditApi.EndEdit(); var t = {rigChecklistExecutionGrid}.batchEditApi.HasChanges(); }}";
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

        #endregion

        #region Question Comments

        public void ConfigureChecklistQuestionComment(GridViewSettings settings, HtmlHelper html, ViewContext viewContext, Guid rigChecklistQuestionId)
        {
            var gridData = InitializeChecklistQuestionCommentGridData(html, rigChecklistQuestionId);
            settings.Configure(gridData, html, viewContext);

            //settings.Settings.VerticalScrollableHeight = 150;
            //settings.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;

            settings.CommandButtonInitialize = (o, e) =>
            {
                if ((e.ButtonType == ColumnCommandButtonType.Edit) || (e.ButtonType == ColumnCommandButtonType.Delete))
                {
                    e.Visible = false;
                }
            };
        }

        private GridData InitializeChecklistQuestionCommentGridData(HtmlHelper htmlHelper, Guid rigChecklistQuestionId)
        {
            var action = "DisplayChecklistQuestionComment";

            if (ViewData["Users"] == null)
            {
                ViewData["Users"] = CommonUtilities.GetUsers(GetClient<PeopleClient>());
            }

            var users = ViewData["Users"] as IEnumerable<Person>;
            var userCombo = new GridCombo("UserCombo", users);

            var gridData = new GridData($"rigChecklistQuestionCommentGrid_{rigChecklistQuestionId}", BaseController, action, "Question Comments", "AddQuestionComment", "Add", "search question comments", historicalRow: false);


            gridData.CallBackRoute = new { Controller = BaseController, Action = action, rigOapChecklistQuestionId = rigChecklistQuestionId };

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("OapChecklistQuestionCommentId", displayName:"Comment Id", order:10, width:10,editLayoutWidth:50, isReadOnly:true),
                                new GridDisplayColumn("CommentDate", displayName:"Comment Date", order:20, columnType:MVCxGridViewColumnType.DateEdit, width:15, isReadOnly:true),
                                new GridDisplayColumn("Comment", displayName:"Comment", order:30, width:60,editLayoutWidth:50, isReadOnly:false),
                                new GridDisplayColumn("CommentBy", displayName:"Comment By", order:40, columnType: MVCxGridViewColumnType.ComboBox, isReadOnly:true, width:10, lookup: userCombo),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", order:100, width:0, columnType:MVCxGridViewColumnType.DateEdit, isVisible:false, isReadOnly:true),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", order:110, width:0, columnType:MVCxGridViewColumnType.DateEdit, isVisible:false, isReadOnly:true),
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", order:120, width:0, isVisible:false, isReadOnly:true ),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", order:130, width:0, isVisible:false, isReadOnly:true ),
                                new GridDisplayColumn("Id", displayName:"Id", order:140, width:0, isVisible:false, isReadOnly:true ),
                                new GridDisplayColumn("RigOapChecklistQuestionId", displayName:"Question Id", order:140, width:0, isVisible:false, isReadOnly:true )
                            };


            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = "CreateChecklistQuestionComment" }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = "UpdateChecklistQuestionComment" }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = "DeleteChecklistQuestionComment" }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("CommentDate", displayName:"Comment Date",width:100, isVisible: false),
                                new GridEditLayoutColumn("Comment", displayName:"Comment",width:100),
                                new GridEditLayoutColumn("CommentBy", displayName:"Comment By",width:100, isVisible: false),
                                new GridEditLayoutColumn("Id", displayName:"Id",width:100, isVisible:false),
                                new GridEditLayoutColumn("RigOapChecklistQuestionId", displayName:"Question Id",width:100, isVisible:false),
                                new GridEditLayoutColumn("CreatedDateTime", displayName:"Created Date",width:100, isVisible:false),
                                new GridEditLayoutColumn("UpdatedDateTime", displayName:"Updated Date",width:100, isVisible:false),
                                new GridEditLayoutColumn("CreatedBy", displayName:"Created By",width:100, isVisible:false ),
                                new GridEditLayoutColumn("UpdatedBy", displayName:"Updated By",width:100, isVisible:false),

                            };

            gridData.RowInitializeEvent = (s, e) =>
            {
                e.NewValues["Id"] = Guid.NewGuid();
                e.NewValues["RigOapChecklistQuestionId"] = rigChecklistQuestionId;
                e.NewValues["CommentDate"] = DateTime.UtcNow;
                gridData.DefaultNewRowInitializeFields(e);
                if (users != null)
                {
                    var currentUser = users.FirstOrDefault(p => p.Id == UtilitySystem.CurrentUserId);

                    e.NewValues["CommentBy"] = currentUser?.Id ?? -1;
                }

            };

            gridData.FormLayout = new GridEditFormLayout(gridData.LayoutColumns
                    , i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                    , columnCount: 2
                    , emptyLayputItemCount: 1);

            return gridData;
        }

        public ActionResult DisplayChecklistQuestionComment(Guid rigOapChecklistQuestionId)
        {
            ViewData[GridConstants.RigChecklistQuestionIdKey] = rigOapChecklistQuestionId;
            return PartialView("GenericlistQuestionCommentPartial", GetQuestionComments(rigOapChecklistQuestionId));
        }

        public ObservableCollection<RigOapChecklistQuestionComment> GetQuestionComments(Guid rigOapChecklistQuestionId)
        {
            var question = RigChecklist?.Questions.SingleOrDefault(q => q.Id == rigOapChecklistQuestionId);

            if (question.Comments == null)
            {
                question.Comments = new ObservableCollection<RigOapChecklistQuestionComment>();
            }

            return question.Comments;
        }

        public ActionResult CreateChecklistQuestionComment(RigOapChecklistQuestionComment model)
        {
            var comments = GetQuestionComments(model.RigOapChecklistQuestionId);
            comments.Add(model);
            RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), RigChecklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.QuestionCommentsErrorsKey, ViewData);
            return DisplayChecklistQuestionComment(model.RigOapChecklistQuestionId);
        }

        public ActionResult UpdateChecklistQuestionComment(RigOapChecklistQuestionComment model)
        {
            var comments = GetQuestionComments(model.RigOapChecklistQuestionId);
            var existingModel = comments.FirstOrDefault(c => c.Id == model.Id);
            if (existingModel != null)
            {
                existingModel.Comment = model.Comment;
                existingModel.CommentBy = model.CommentBy;
                existingModel.CommentDate = model.CommentDate;
                existingModel.UpdatedDateTime = model.UpdatedDateTime;
                existingModel.UpdatedBy = model.UpdatedBy;

                RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), RigChecklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.QuestionCommentsErrorsKey, ViewData);
            }
            return DisplayChecklistQuestionComment(model.RigOapChecklistQuestionId);
        }

        public ActionResult DeleteChecklistQuestionComment(RigOapChecklistQuestionComment model)
        {
            var comments = GetQuestionComments(model.RigOapChecklistQuestionId);
            var existingModel = comments.FirstOrDefault(c => c.Id == model.Id);
            if (existingModel != null)
            {
                comments.Remove(existingModel);

                RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), RigChecklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.QuestionCommentsErrorsKey, ViewData);
            }
            return DisplayChecklistQuestionComment(model.RigOapChecklistQuestionId);
        }

        #endregion

        #region Related Search 

        public void ConfigureRelatedSearch(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistRelatedSearchGridData(html);
            settings.Configure(gridData, html, viewContext);
            settings.ClientSideEvents.BeginCallback = $"{GridConstants.rigChecklistRelatedSearchGrid}OnBeginCallback";
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


            var gridData = new GridData(GridConstants.rigChecklistRelatedSearchGrid, BaseController, action, Translate("Related Search"), "AddRelatedSearch", "Add", "search results", key: "RigOapChecklistQuestionId", displayColumnName: "Name", initializeCallBack: true, historicalRow: false, showPager: false);

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
            var relatedQuestionsList = GetClient<RigOapChecklistClient>().GetRelatedQuestionSearchAsync(searchModel.QuestionId, searchModel.FromDate, searchModel.ToDate, searchModel.SearchBy).Result?.Result?.Data.ToList();
            return PartialView("GenericlistRelatedSearchPartial", relatedQuestionsList.ToRelatedQuestionFlattenedModel(searchModel.QuestionId, GetClient<RigOapChecklistClient>(), GetClient<OapChecklistFindingClient>()));

        }

        #endregion

        #region CAPA Grid (Monitoring)
        public virtual void ConfigureCAPAGrid(GridViewSettings settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            settings.Name = "RelatedCAPAGrid";
            settings.Width = Unit.Percentage(100);
            settings.Caption = Translate("Related CAPAs", "Resources");
            settings.KeyFieldName = "Id";

            settings.CallbackRouteValues = new { Action = "DisplayRelatedCapa", Controller = "Generic", Area = "Oap", checklistId = RigChecklist.Id };

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
                var loapChecklistId = RigChecklist.Id;

                htmlHelper.RenderAction("OpenCapaPartial", new { Controller = "Capa", Area = "IRMA", source = "OAP", sourceId = loapChecklistId, sourceUrl = "/oap/Generic/List/" + loapChecklistId });
            });

        }
        #endregion        


        [HttpPost]
        public ActionResult UpdateChecklistAnswers(IEnumerable<OapChecklistAnswerPostbackModel> checklistAnswers)
        {
            IList<RigOapChecklistQuestionAnswer> answerList = new List<RigOapChecklistQuestionAnswer>();

            foreach (var answer in checklistAnswers)
            {
                var newAnswer = new RigOapChecklistQuestionAnswer()
                {
                    RigOapChecklistQuestionId = answer.QuestionId,
                    Ordinal = answer.AnswerOrdinal,
                    Value = answer.Value,
                    CreatedDateTime = DateTime.UtcNow
                };
                answerList.Add(newAnswer);
            }

            RigOapChecklistClient client = GetClient<RigOapChecklistClient>();
            var result = client.UpdateChecklistAnswersAsync(answerList.AsEnumerable()).Result?.Result?.Data;

            // Update RigChecklist in Session cache
            if (result == true && RigChecklist != null)
            {
                var oldAnswers = from question in RigChecklist.Questions
                                       from answers in question.Answers
                                       select answers;

                foreach (var answer in oldAnswers)
                {
                    var newAnswer = answerList.FirstOrDefault(a => a.RigOapChecklistQuestionId == answer.RigOapChecklistQuestionId && a.Ordinal == answer.Ordinal);
                    if (newAnswer != null)
                        answer.Value = newAnswer.Value;
                }
                
            }


            return RedirectToAction("List", new { Id = RigChecklist.Id });
        }

        private void SendOAPReviewEmail(UserModel recipient)
        {
            if (UtilitySystem.Settings.ConfigSettings["SMTPEnabled"] == "True" && recipient != null && recipient != null && recipient.Email != null) {
                try
                {
                    var controllerName = Request.RequestContext.RouteData.Values["controller"].ToString(); // This may change due to controllers inheritance
                    string url = Url.Action("List", controllerName, new { Id = RigChecklist.Id }, Request.Url.Scheme);
                    var message = $"Dear {recipient.DisplayName}," +
                        $"<p>You have been assigned the OAP checklist <a href='{url}'>{RigChecklist.RigChecklistUniqueId}</a> for review.<br/>" +
                        $"Please click on the link above to be redirected to the checklist page.</p>" +
                        $"<i>This is an automated message from IRMA, please do not reply.</i><br />";

                    MailSender.SendEmail("irma-application@enscoplc.com", recipient.Email, "IRMA - OAP Workflow", message);
                }
                catch (Exception e)
                {
                    // TODO: Implement error handling 
                    Logger.Error(new LogInfo(e.Message));
                    throw;
                }
            }
        }

        private void SendOAPRejectEmail(UserModel recipient)
        {
            if (UtilitySystem.Settings.ConfigSettings["SMTPEnabled"] == "True" && recipient != null && recipient != null && recipient.Email != null)
            {
                try
                {
                    var controllerName = Request.RequestContext.RouteData.Values["controller"].ToString(); // This may change due to controllers inheritance
                    string url = Url.Action("List", controllerName, new { Id = RigChecklist.Id }, Request.Url.Scheme);
                    var message = $"Dear {recipient.DisplayName}," +
                        $"<p>The OAP checklist <a href='{url}'>{RigChecklist.RigChecklistUniqueId}</a> has been rejected.<br/>" +
                        $"Please click on the link above to be redirected to the checklist page.</p>" +
                        $"<i>This is an automated message from IRMA, please do not reply.</i><br />";

                    MailSender.SendEmail("irma-application@enscoplc.com", recipient.Email, "IRMA - OAP Workflow", message);
                }
                catch (Exception e)
                {
                    // TODO: Implement error handling 
                    Logger.Error(new LogInfo(e.Message));
                    throw;
                }
            }
        }
    }
}
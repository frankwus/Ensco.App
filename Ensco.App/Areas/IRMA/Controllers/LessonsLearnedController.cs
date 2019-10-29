using Ensco.OAP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensco.OAP.Models.ViewModels;
using Ensco.OAP.Models;
using Ensco.Services;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using System.IO;
using System.Web.UI;
using Ensco.Irma.Models;
using Ensco.Irma.Services;
using Ensco.Utilities;
using Ensco.App.App_Start;
using Ensco.App.Infrastructure;
using static Ensco.OAP.Services.OAPServiceSystem;
using Ensco.Models;
using MvcSiteMapProvider.Web.Mvc.Filters;

namespace Ensco.App.Areas.IRMA.Controllers
{
    [CustomAuthorize]
    public class LessonsLearnedController : Controller
    {
        IOAPServiceDataModel dataModel = null;

         
        // GET: IRMA/LessonsLearned
        public ActionResult Index()
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            var lessonsLearned = dataModel.GetAllItems().Cast<LessonLearnedModel>().ToList();            
            return View(lessonsLearned);
        }

        public PartialViewResult IndexLessonsGridView()
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            var lessonsLearned = dataModel.GetAllItems().Cast<LessonLearnedModel>().ToList();

            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearnedType);
            LessonLearnedViewBag(ViewBag);
            foreach (var lesson in lessonsLearned)
            {
                if (lesson.TypeId != null)
                {
                    lesson.Type = dataModel.GetItem(string.Format("Id={0}", lesson.TypeId), "Id");
                }                
            }              
            return PartialView("IndexLessonsGridView", lessonsLearned);
        }

        public PartialViewResult IndexLessonsAddGridView(LL_IndexGridRowModel rowModel)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            
            BusinessUnitModel objBU = ServiceSystem.GetBusinessUnitByRigId(UtilitySystem.Settings.RigId);

            if (ModelState.IsValid)
            {
                var newLesson = new LessonLearnedModel()
                {
                    DateStarted = DateTimeOffset.Now,
                    Title = rowModel.Title,
                    ImpactLevel = rowModel.ImpactLevel,
                    Status = LessonsLearnedStatus.Open.ToString(),
                    SourceForm = "Lessons Learned",
                    SourceRigFacility = UtilitySystem.Settings.RigId,
                    SourceBU = objBU.Id,
                    TypeId = rowModel.TypeId
                };
                dataModel.Add(newLesson);
            }
            else
            {
                ViewData["UpdateError"] = true;
            }
            return IndexLessonsGridView();
        }

        [SiteMapTitle("Id")]
        public ActionResult Edit(int id)
        {
            LessonLearnedModel lessonLearned = OAPServiceSystem.GetLessonLearned(id);
            Session["LessonLearnedModel"] = lessonLearned;
            ViewBag.LessonStatus = lessonLearned.Status;

            LessonLearnedViewBag(ViewBag);
            ViewBag.IsEditable = lessonLearned.IsEditable;
            ViewBag.ImpactLevels = Enum.GetNames(typeof(LessonsLearnedImpact)).ToList();

            if (lessonLearned != null)
            {
                return View("LessonForm", lessonLearned);
            }
            return RedirectToAction("Index");
        }

        private void LessonLearnedViewBag(dynamic viewBag)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearnedType);
            IEnumerable<LessonLearnedType> types = dataModel.GetAllItems().Cast<LessonLearnedType>();
            viewBag.LessonTypes = types;
        }

        public RedirectToRouteResult Save([ModelBinder(typeof(DevExpressEditorsBinder))]LessonLearnedModel formLessonModel)
        {
            LessonLearnedModel lessonLearned = (LessonLearnedModel)Session["LessonLearnedModel"];
            List<LessonLearnedOriginatorModel> originators = lessonLearned.Originators;

            if(originators.Count == 0)
            {                
                return RedirectToAction("Edit", new { Id = lessonLearned.Id });
            }
            
            lessonLearned.TypeId = formLessonModel.TypeId;
            lessonLearned.Title = formLessonModel.Title;
            lessonLearned.SourceBU = formLessonModel.SourceBU;
            lessonLearned.SourceRigFacility = formLessonModel.SourceRigFacility;
            lessonLearned.Topic = formLessonModel.Topic;
            lessonLearned.ImpactLevel = formLessonModel.ImpactLevel;
            lessonLearned.eMocId = formLessonModel.eMocId;
            lessonLearned.Description = formLessonModel.Description;

            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            dataModel.Update(lessonLearned);

            return RedirectToAction("Edit",new { Id = lessonLearned.Id });
        }

        public ActionResult Cancel(int id)
        {
            LessonLearnedModel lessonLearned = OAPServiceSystem.GetLessonLearned(id);
            lessonLearned.Status = "Canceled";
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            dataModel.Update(lessonLearned);
            return RedirectToAction("Index");
        }

        public ActionResult Close(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            LessonLearnedModel lessonLearned = OAPServiceSystem.GetLessonLearned(id.Value);
            lessonLearned.Status = "Closed";
            Session["LessonLearnedModel"] = lessonLearned;
            dataModel.Update(lessonLearned);

            return RedirectToAction("Index");
        }

        public ActionResult SubmitForPreApproval()
        {
            LessonLearnedModel lessonLearned = (LessonLearnedModel)Session["LessonLearnedModel"];
            if (lessonLearned == null)
                return RedirectToAction("Index");
            var preApprovals = lessonLearned.Approvals.Where(a => a.Type == (int)ApprovalModel.ApprovalType.LessonsLearnedPreApproval);
            var approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);

            foreach (var preApproval in preApprovals)
            {
                preApproval.Status = (int)IrmaConstants.ApprovalStatus.PendingReview;
                approvalDataModel.Update(preApproval);
            }
            lessonLearned.Status = "Pending Review";
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            dataModel.Update(lessonLearned);

            return RedirectToAction("Edit", new { Id=lessonLearned.Id });
        }

        public ActionResult SubmitForApproval()
        {
            LessonLearnedModel lessonLearned = (LessonLearnedModel)Session["LessonLearnedModel"];
            if (lessonLearned == null)
                return RedirectToAction("Index");
            var approvals = lessonLearned.Approvals.Where(a => a.Type == (int)ApprovalModel.ApprovalType.LessonsLearnedApproval);
            var approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);

            foreach (var approval in approvals)
            {
                approval.Status = (int)IrmaConstants.ApprovalStatus.PendingApproval;
                approvalDataModel.Update(approval);
            }
            lessonLearned.Status = "Pending Approval";
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            dataModel.Update(lessonLearned);

            return RedirectToAction("Edit", new { Id = lessonLearned.Id });
        }

        #region Originators
        public ActionResult LessonsLearnedOriginatorsPartial()
        {
            LessonLearnedModel model = (LessonLearnedModel)Session["LessonLearnedModel"];
            ViewBag.IsEditable = model.IsEditable;
            foreach (var originator in model.Originators)
            {
                var user = ServiceSystem.GetUser(originator.PassportId);
                originator.Position = (int)user.Position;
            }
            return PartialView("LessonLearnedOriginatorsPartial", model.Originators);            
        }

        public ActionResult LessonsLearnedAddOriginatorsPartial(LL_EditOriginatorsGridRowModel rowModel)
        {
            // Add Originator logic
            var lessonLearnedModel = (LessonLearnedModel)Session["LessonLearnedModel"];
            List<LessonLearnedOriginatorModel> originators = lessonLearnedModel.Originators;
            if (ModelState.IsValid)
            {
                LessonLearnedOriginatorModel newOriginator = new LessonLearnedOriginatorModel() { LessonId = lessonLearnedModel.Id, PassportId = rowModel.PassportId, RigFacility = rowModel.RigFacility };
                var user = ServiceSystem.GetUser(rowModel.PassportId);
                newOriginator.Position = (int)user.Position;
                if (Ensco.Utilities.UtilitySystem.Settings.RigId != 1)
                {
                    newOriginator.RigFacility = Ensco.Utilities.UtilitySystem.Settings.RigId;
                }
                dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearnedOriginator);
                newOriginator = dataModel.Add(newOriginator);
                originators.Add(newOriginator);
            }
            else
            {
                ViewData["UpdateError"] = true;
            }

            return LessonsLearnedOriginatorsPartial();
            //return PartialView("LessonLearnedOriginatorsPartial", originators);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult LessonsLearnedDeleteOriginatorsPartial(int Id)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearnedOriginator);
            var originator = (LessonLearnedOriginatorModel)dataModel.GetItem(string.Format("Id={0}", Id), "Id");
            if (originator != null)
            {
                dataModel.Delete(originator);
                var lessonLearnedModel = (LessonLearnedModel)Session["LessonLearnedModel"];
                lessonLearnedModel.Originators.RemoveAll(o => o.Id == originator.Id);
            }
                
            return LessonsLearnedOriginatorsPartial();
        }
        #endregion

        #region PreApproval
        public PartialViewResult LessonsLearnedPreApprovalReviewPartial()
        {
            var model = (LessonLearnedModel)Session["LessonLearnedModel"];
            var approvals = model.Approvals.Where(a => a.Type == (int)ApprovalModel.ApprovalType.LessonsLearnedPreApproval);
            return PartialView("LessonsLearnedPreApprovalReviewPartial", approvals);
        }

        public ActionResult LessonsLearnedPreApprovalReviewAddPartial(ApprovalModel rowModel)
        {
            var lessonLearnedModel = (LessonLearnedModel)Session["LessonLearnedModel"];
            if (ModelState.IsValid)
            {
                rowModel.RequestInfo = Url.Action("PreApprovalProcessApproval");
                OAPServiceSystem.AddPreApprover(rowModel, lessonLearnedModel);
                Session["LessonLearnedModel"] = lessonLearnedModel;
            }
            else
            {
                ViewData["UpdateError"] = true;
            }
            return LessonsLearnedPreApprovalReviewPartial();
        }

        public ActionResult LessonsLearnedPreApprovalReviewDeletePartial(int Id)
        {
            IIrmaServiceDataModel serviceSystem = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            var approver = (ApprovalModel)serviceSystem.GetItem(string.Format("Id={0}", Id), "Id");
            if (approver != null)
            {
                serviceSystem.Delete(approver);
                var lessonLearnedModel = (LessonLearnedModel)Session["LessonLearnedModel"];
                lessonLearnedModel.Approvals.RemoveAll(o => o.Id == approver.Id);
            }
            return LessonsLearnedPreApprovalReviewPartial();
        }

        public ActionResult PreApprovalProcessApproval(int Id, int ApprovalId, int Approver, int Status, string Comment)
        {
            string comment = Comment == "null" ? "" : Comment; // DevExpress sending the 'null' string when textbox is empty
            LessonLearnedModel lessonLearned = (LessonLearnedModel)Session["LessonLearnedModel"];
            IIrmaServiceDataModel serviceSystem = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            ApprovalModel approval = serviceSystem.GetItem(string.Format("Id={0}", ApprovalId), "Id");

            var approvalInSessionModel = lessonLearned.Approvals.FirstOrDefault(a => a.Id == approval.Id);
            if (approvalInSessionModel != null)
            {
                approvalInSessionModel.Status = Status;
                approvalInSessionModel = null;
            }                
            approval.Status = Status;
            approval.ApprovedDate = DateTime.Now;
            approval.ApproverComments = comment;
            switch (Status)
            {
                case (int)IrmaConstants.ApprovalStatus.Reviewed:
                    var approvedCount = lessonLearned.GetPreApprovals().Where(a => a.Status == (int)IrmaConstants.ApprovalStatus.Reviewed).Count();
                    var totalCount = lessonLearned.GetPreApprovals().Count();
                    if (approvedCount == totalCount) // Proceeds with the workflow
                        lessonLearned.Status = "Final Review";
                    break;
                case (int)IrmaConstants.ApprovalStatus.Rejected:
                    lessonLearned.Status = "Rejected";
                    break;
            }
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            dataModel.Update(lessonLearned);
            serviceSystem.Update(approval);

            return RedirectToAction("Edit", new { Id = Id });
        }

        #endregion

        #region Approval
        public ActionResult LessonsLearnedApprovalPartial()
        {
            var model = (LessonLearnedModel)Session["LessonLearnedModel"];
            var approvals = model.Approvals.Where(a => a.Type == (int)ApprovalModel.ApprovalType.LessonsLearnedApproval);
            return PartialView("LessonsLearnedApprovalPartial",approvals);
        }

        [HttpPost]
        public ActionResult LessonsLearnedApprovalAddPartial(ApprovalModel rowModel)
        {            
            if (ModelState.IsValid)
            {
                var lessonModel = (LessonLearnedModel)Session["LessonLearnedModel"];
                rowModel.RequestInfo = Url.Action("ApprovalProcessApproval");
                OAPServiceSystem.AddApprover(rowModel, lessonModel);
            } else
            {
                ViewData["UpdateError"] = true;
            }
            return LessonsLearnedApprovalPartial();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult LessonsLearnedApprovalDeletePartial(int Id)
        {
            IIrmaServiceDataModel serviceSystem = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);

            var approver = (ApprovalModel)serviceSystem.GetItem(string.Format("Id={0}", Id), "Id");
            if (approver != null)
            {
                serviceSystem.Delete(approver);
                var lessonLearnedModel = (LessonLearnedModel)Session["LessonLearnedModel"];
                lessonLearnedModel.Approvals.RemoveAll(a => a.Id == Id);
            }
            return LessonsLearnedApprovalPartial();
        }

        public ActionResult ApprovalProcessApproval(int Id, int ApprovalId, int Approver, int Status, string Comment)
        {
            string comment = (Comment == "null") ? "" : Comment; // DevExpress sending the 'null' string when textbox is empty
            LessonLearnedModel lessonLearned = (LessonLearnedModel)Session["LessonLearnedModel"];
            IIrmaServiceDataModel serviceSystem = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            ApprovalModel approval = serviceSystem.GetItem(string.Format("Id={0}", ApprovalId), "Id"); 

            approval.ApprovedDate = DateTime.Now;
            approval.ApproverComments = Comment;
            approval.Status = Status;
            lessonLearned.Approvals.FirstOrDefault(a => a.Id == ApprovalId).Status = Status;
            switch (Status)
            {
                case (int)IrmaConstants.ApprovalStatus.Approved:
                    var approvedCount = lessonLearned.GetApprovals().Where(a => a.Status == (int)IrmaConstants.ApprovalStatus.Approved).Count();
                    var totalCount = lessonLearned.GetApprovals().Count();
                    if (approvedCount == totalCount) // Proceeds with the workflow / auto close
                        lessonLearned.Status = "Closed";
                    break;

                case (int)IrmaConstants.ApprovalStatus.Rejected:
                    lessonLearned.Status = "Rejected";
                    approval.ApproverComments = Comment;
                    break;
            }
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            dataModel.Update(lessonLearned);
            serviceSystem.Update(approval);

            return RedirectToAction("Edit", new { Id = Id });
        }

        #endregion

        #region Control Definition
        public PartialViewResult Control(string SourceForm, int SourceFormId)
        {
            ViewBag.SourceForm = SourceForm;
            ViewBag.SourceFormId = SourceFormId;

            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            var lessonsLearned = dataModel.GetItems(string.Format("SourceForm=\"{0}\" AND SourceFormId={1}", SourceForm, SourceFormId), "Id").Cast<LessonLearnedModel>().ToList();
                        
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearnedType);
            LessonLearnedViewBag(ViewBag);
            foreach (var lesson in lessonsLearned)
            {
                if (lesson.TypeId != null)
                {
                    lesson.Type = dataModel.GetItem(string.Format("Id={0}", lesson.TypeId), "Id");
                }
            }
            return PartialView("Control", lessonsLearned);
        }

        public PartialViewResult ControlAddGridRow(LL_IndexGridRowModel rowModel, string SourceForm, int SourceFormId)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearned);
            BusinessUnitModel objBU = ServiceSystem.GetBusinessUnitByRigId(UtilitySystem.Settings.RigId);
            if (ModelState.IsValid)
            {
                var newLesson = new LessonLearnedModel()
                {
                    DateStarted = DateTimeOffset.Now,
                    Title = rowModel.Title,
                    ImpactLevel = rowModel.ImpactLevel,
                    SourceRigFacility = UtilitySystem.Settings.RigId,
                    SourceBU = objBU.Id,
                    TypeId = rowModel.TypeId,
                    Status = LessonsLearnedStatus.Open.ToString(),
                    SourceForm = SourceForm,
                    SourceFormId = SourceFormId,
                    SourceFormURL = Request.UrlReferrer.AbsoluteUri
                };
                newLesson = dataModel.Add(newLesson);
                dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.LessonLearnedOriginator);
                LessonLearnedOriginatorModel originator = new LessonLearnedOriginatorModel();
                originator.LessonId = newLesson.Id; 
                string key = HttpContext.Session.SessionID + "_UserSessionInfo";
                UserSession userSession = (UserSession)HttpContext.Session[key];
                originator.PassportId = userSession.UserId;
                originator.RigFacility = UtilitySystem.Settings.RigId;
                dataModel.Add(originator);
            }
            else
            {
                ViewBag.UpdateError = string.Join(",", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            }
            return Control(SourceForm, SourceFormId);
        }
        #endregion
    }
}

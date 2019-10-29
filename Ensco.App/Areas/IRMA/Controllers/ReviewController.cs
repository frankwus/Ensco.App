using Ensco.OAP.Models;
using Ensco.OAP.Services;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Ensco.Models;
using Ensco.App.Infrastructure;
using Ensco.App.App_Start;
using MvcSiteMapProvider.Web.Mvc.Filters;

namespace Ensco.App.Areas.IRMA.Controllers
{
    [CustomAuthorize]
    public class ReviewController : Controller
    {
        private IOAPServiceDataModel dataModel;

        // GET: IRMA/Review
        public ActionResult Index()
        {
            return View();
        }

        [SiteMapTitle("id")]
        public ActionResult Edit(int id)
        {
            ReviewModel review = OAPServiceSystem.GetReview(id);
            return View(review);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Comment")]ReviewModel model)
        {
            ReviewModel review = OAPServiceSystem.GetReview(model.Id);
            review.Comment = model.Comment;
            OAPServiceSystem.UpdateReview(review);
            return Edit(model.Id);
        }

        #region Control

        public ActionResult GetAllReviews()
        {             
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Review);
            IEnumerable<ReviewModel> reviews = dataModel.GetAllItems().Cast<ReviewModel>();

            foreach (ReviewModel review in reviews)
            {
                review.Attachments = ServiceSystem.GetAttachments("Review", review.Id.ToString());
            }

            return PartialView("GetAllReviews", reviews);
        }

        public ActionResult Control(string SourceForm, string SourceFormId)
        {
            ViewBag.SourceForm = SourceForm;
            ViewBag.SourceFormId = SourceFormId;
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Review);
            IEnumerable<ReviewModel> reviews = dataModel.GetItems(string.Format("SourceForm=\"{0}\" AND SourceFormId=\"{1}\"", SourceForm, SourceFormId), "Id").Cast<ReviewModel>();

            foreach (ReviewModel review in reviews)
            {
                review.Attachments = ServiceSystem.GetAttachments("Review", review.Id.ToString());
            }

            return PartialView("Control",reviews);
        }

        [HttpPost]
        public ActionResult ControlAddReview(ReviewModel reviewModel, string SourceForm, string SourceFormId)
        {
            if (ModelState.IsValid)
            {
                dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Review);
                BusinessUnitModel businessUnit = ServiceSystem.GetBusinessUnitByRigId(UtilitySystem.Settings.RigId);
                ReviewModel newReview = new ReviewModel()
                {
                    RigId = UtilitySystem.Settings.RigId,
                    Comment = reviewModel.Comment,
                    ReviewerPassportId = reviewModel.ReviewerPassportId,
                    RequestedBy = UtilitySystem.CurrentUserId,
                    SourceBU = businessUnit.Id,
                    SourceForm = SourceForm,
                    SourceFormId = SourceFormId.ToString(),
                    DateCreated = DateTimeOffset.Now
                };
                dataModel.Add(newReview);
                ServiceSystem.AddTask(sourceForm: "Review", sourceFormId: SourceFormId, assigneeUserId: newReview.ReviewerPassportId, 
                    dueDate: reviewModel.DueDate, sourceFormUrl: Request.UrlReferrer.PathAndQuery);

            } else
            {
                ViewData["UpdateError"] = true;
            }

            return Control(SourceForm, SourceFormId);
        }

        [HttpPost]
        public ActionResult ControlSignReview(int Id, string Comment, string returnUrl=null)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Review);
            ReviewModel review = dataModel.GetItem(string.Format("Id={0}", Id), "Id");

            review.ReviewDate = DateTimeOffset.Now;
            review.Comment = Comment;
            dataModel.Update(review);

            return Redirect(returnUrl ?? "Index");
        }

        #endregion
    }
}
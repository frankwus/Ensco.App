using Ensco.App.App_Start;
using Ensco.App.Infrastructure;
using Ensco.Models;
using Ensco.OAP.Models;
using Ensco.OAP.Services;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.IRMA.Controllers
{
    [CustomAuthorize]
    public class CommentsController : Controller
    {
        IOAPServiceDataModel dataModel;

        // GET: IRMA/Review
        public ActionResult Index()
        {
            return View();
        }

        // GET: IRMA/Comments
        public ActionResult GetAllComments()
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
            IEnumerable<CommentModel> comments = dataModel.GetAllItems().Cast<CommentModel>();
            foreach (CommentModel comment in comments)
            {
                UserModel user = ServiceSystem.GetUser((int)comment.CommenterPassport); // Populates position field
                comment.Position = (int)user.Position;
            }
            return PartialView("GetAllComments", comments);
        }


        #region Comments Control
        public ActionResult Control(string sourceForm, string sourceFormId, bool readOnly = false)
        {
            // This will return records with QuestionId = null
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
            IEnumerable<CommentModel> comments = dataModel.GetItems(string.Format("SourceForm=\"{0}\" AND SourceFormId=\"{1}\" AND QuestionId=null",sourceForm, sourceFormId), "Id").Cast<CommentModel>();
            foreach (CommentModel comment in comments)
            {
                UserModel user = ServiceSystem.GetUser((int)comment.CommenterPassport); // Populates position field
                comment.Position = (int)user.Position;
            }
            ViewBag.SourceForm = sourceForm;
            ViewBag.SourceFormId = sourceFormId;
            ViewBag.ReadOnly = readOnly;

            return PartialView("Control",comments);
        }

        public ActionResult ControlAddRow(CommentModel commentModel, string sourceForm, string sourceFormId)
        {
            ViewBag.SouceForm = sourceForm;
            ViewBag.SourceFormId = sourceFormId;
            if (ModelState.IsValid)
            {
                dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);

                string key = HttpContext.Session.SessionID + "_UserSessionInfo";
                UserSession userSession = (UserSession)HttpContext.Session[key];

                CommentModel newComment = new CommentModel
                {
                    Comment = commentModel.Comment,
                    CommentDate = DateTimeOffset.Now,
                    CommenterPassport = userSession.UserId,
                    Rig = UtilitySystem.Settings.RigId,
                    SourceForm = sourceForm,
                    SourceFormId = sourceFormId
                };

                newComment = dataModel.Add(newComment);

            } else
            {
                ViewData["UpdateError"] = true;
            }
            return Control(sourceForm, sourceFormId);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ControlDeleteRow(int Id, string sourceForm, string sourceFormId)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
            CommentModel comment = dataModel.GetItem(string.Format("Id={0}",Id), "Id");
            if (comment != null)
                dataModel.Delete(comment);
            return Control(sourceForm, sourceFormId);
        }


        public ActionResult InlineControl(string SourceForm, string SourceFormId, string QuestionId, string QuestionText, bool readOnly = false)
        {
            ViewBag.ReadOnly = readOnly;
            ViewBag.SourceForm = SourceForm;
            ViewBag.SourceFormId = SourceFormId;
            ViewBag.QuestionId = QuestionId;
            ViewBag.QuestionText = QuestionText;

            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
            IEnumerable<CommentModel> comments = dataModel.GetItems(string.Format("SourceFormId=\"{0}\" AND QuestionId=\"{1}\"", SourceFormId, QuestionId), "Id").Cast<CommentModel>();
            foreach (CommentModel comment in comments)
            {
                UserModel user = ServiceSystem.GetUser(comment.CommenterPassport); // Populates position field
                comment.Position = (int)user.Position;
            }
            return PartialView("InlineControl", comments);
        }

        [HttpPost]
        public ActionResult InlineControlAdd(CommentModel rowModel, string SourceForm, string SourceFormId, string QuestionId, string QuestionText)
        {
            if (ModelState.IsValid)
            {
                dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
                CommentModel newComment = new CommentModel()
                {
                    SourceForm = SourceForm,
                    SourceFormId = SourceFormId,
                    QuestionId = QuestionId,
                    CommentDate = DateTimeOffset.Now,
                    CommenterPassport = GetUserPassportId(),
                    QuestionText = QuestionText,
                    Comment = rowModel.Comment,
                    Rig = UtilitySystem.Settings.RigId
                };

                newComment = dataModel.Add(newComment);
            } else
            {
                ViewData["UpdateError"] = true;
            }
            return InlineControl(SourceForm, SourceFormId, QuestionId, QuestionText);
        }

        [HttpPost]
        public ActionResult InlineControlUpdate(CommentModel rowModel, string SourceForm, string SourceFormId, string QuestionId, string QuestionText)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
            CommentModel comment = dataModel.GetItem(string.Format("Id={0}", rowModel.Id), "Id");
            if (comment != null)
            {
                comment.Comment = rowModel.Comment;
                comment.CommentDate = DateTimeOffset.Now;
                comment.CommenterPassport = GetUserPassportId();
                dataModel.Update(comment);
            }

            return InlineControl(SourceForm, SourceFormId, QuestionId, QuestionText);
        }

        [HttpPost]
        public ActionResult InlineControlDelete(int Id, string SourceForm, string SourceFormId, string QuestionId, string QuestionText)
        {
            dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
            CommentModel comment = dataModel.GetItem(string.Format("Id={0}", Id), "Id");
            if (comment != null)
                dataModel.Delete(comment);

            return InlineControl(SourceForm, SourceFormId, QuestionId, QuestionText);
        }
        
        #endregion


        private int GetUserPassportId()
        {
            string key = HttpContext.Session.SessionID + "_UserSessionInfo";
            UserSession userSession = (UserSession)HttpContext.Session[key];
            return userSession.UserId;
        }
    }
}
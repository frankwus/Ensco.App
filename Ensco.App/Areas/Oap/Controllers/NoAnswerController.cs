using AutoMapper;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.Web.Oap.Controllers;
using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;

namespace Ensco.App.Areas.Oap.Controllers
{
    
    public class NoAnswerController : OapRigController
    {
        public NoAnswerController()
        {
            RegisterClient(new List<Type>()
                            {
                                typeof(RigOapChecklistClient),
                                typeof(RigOapChecklistQuestionNoAnswerCommentClient),
                                typeof(PeopleClient) 
                            });
        }


        protected  RigOapChecklistQuestion Question
        {
            get
            {

                return Session[GridConstants.RigChecklistQuestionKey] as RigOapChecklistQuestion;

            }
            set
            {
                if (Session[GridConstants.RigChecklistQuestionKey] != null)
                {
                    Session.Remove(GridConstants.RigChecklistQuestionKey);
                }

                Session[GridConstants.RigChecklistQuestionKey] = value;
            }
        }

        protected virtual bool EnableSave()
        {
            return !RigChecklist.Status.Equals(GridConstants.WorkflowStatus.Completed.ToString(), StringComparison.InvariantCultureIgnoreCase) && !RigChecklist.Status.Equals(GridConstants.WorkflowStatus.Pending.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
        

        public ActionResult Control(int oapChecklistId, int oapChecklistQuestionId, Guid rigOapChecklistId, bool showLastNoAnswerControl = false)
        {
            var oapChecklistClient = GetClient<OapChecklistClient>();
            RigOapChecklistQuestionNoAnswerComment noAnswer = null;
            if (showLastNoAnswerControl)
            {
                noAnswer = oapChecklistClient.GetAllQuestionNoAnswersAsync(oapChecklistId, oapChecklistQuestionId).Result?.Result.Data?.LastOrDefault();
            }
            else
            {
                noAnswer = oapChecklistClient.GetFirstQuestionOpenNoAnswersAsync(oapChecklistId, oapChecklistQuestionId).Result?.Result?.Data;
            }
            
            // If there is no existing no answer, create a new one
            if (noAnswer == null)
            {
                int userId = UtilitySystem.CurrentUserId;
                noAnswer = createNoAnswer(oapChecklistId, oapChecklistQuestionId, rigOapChecklistId, userId);
            }                

            // Mapping to view model
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RigOapChecklistQuestionNoAnswerComment, NoAnswerControlViewModel>());
            var mapper = config.CreateMapper();
            var viewModel = mapper.Map<NoAnswerControlViewModel>(noAnswer);
            viewModel.StartUserId = noAnswer.StartCommentBy;
            viewModel.EndUserId = noAnswer.EndCommentBy;
            viewModel.Attachments = ServiceSystem.GetAttachments("Oap - No Answer Control", viewModel.Id.ToString());

            // Replace user ids by user names
            if (viewModel.StartCommentBy != null)
                viewModel.StartCommentBy = ServiceSystem.GetUser(int.Parse(viewModel.StartCommentBy))?.DisplayName;

            if (viewModel.EndCommentBy != null)
                viewModel.EndCommentBy = ServiceSystem.GetUser(int.Parse(viewModel.EndCommentBy))?.DisplayName;

            return PartialView("NoAnswerCommentPartial", viewModel);
        }

        [HttpPost]
        public ActionResult Update(
            [Bind(Include= "Id,Comment,IsStopWorkAuthorityExercised,WasAbletoCorrectImmediately,Correction")]RigOapChecklistQuestionNoAnswerComment model)
        {
            var oapChecklistClient = GetClient<OapChecklistClient>();

            try
            {
                var updatedModel = oapChecklistClient.UpdateNoAnswerAsync(model).Result?.Result?.Data;
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        [HttpPost]
        public async Task<ActionResult> Close(
            Guid noAnswerId, int oapChecklistQuestionId, Guid rigOapChecklistId, string Correction, bool IsStopWorkAuthorityExercised,
            string Comment, bool WasAbletoCorrectImmediately)
        {
            var oapChecklistClient = GetClient<OapChecklistClient>();
            var noAnswer = oapChecklistClient.GetNoAnswerByIdAsync(noAnswerId).Result?.Result?.Data;

            try
            {
                noAnswer.ClosureRigOapChecklistId = rigOapChecklistId;
                noAnswer.EndDateTime = DateTime.UtcNow;
                noAnswer.Comment = Comment;
                noAnswer.Correction = Correction;
                noAnswer.EndCommentBy = UtilitySystem.CurrentUserId;
                noAnswer.IsStopWorkAuthorityExercised = IsStopWorkAuthorityExercised;
                noAnswer.WasAbletoCorrectImmediately = WasAbletoCorrectImmediately;

                await oapChecklistClient.UpdateNoAnswerAsync(noAnswer);

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult ReviewsPartial()
        {
            return PartialView("ReviewsPartial");
        }


        private RigOapChecklistQuestionNoAnswerComment createNoAnswer(int oapChecklistId, int oapChecklistQuestionId, Guid rigOapChecklistId, int userId)
        {
            var oapChecklistClient = GetClient<OapChecklistClient>();
            var noAnswer = oapChecklistClient.AddQuestionNoAnswerAsync(oapChecklistId, oapChecklistQuestionId, rigOapChecklistId, userId).Result?.Result?.Data;

            if (noAnswer == null) throw new Exception("Unable to create No Answer, please check OAP API");

            return noAnswer;
        }
    }

    public class NoAnswerControlViewModel
    {
        public Guid Id { get; set; }
        public int OapChecklistQuestionId { get; set; }
        public Nullable<Guid> SourceRigOapChecklistId { get; set; }
        public Nullable<Guid> ClosureRigOapChecklistId { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Comment { get; set; }
        public int StartUserId { get; set; }
        public string StartCommentBy { get; set; }
        public Nullable<DateTime> EndDateTime { get; set; }
        public int? EndUserId { get; set; }
        public string EndCommentBy { get; set; }
        public Nullable<bool> WasAbletoCorrectImmediately { get; set; }
        public Nullable<bool> IsStopWorkAuthorityExercised { get; set; }
        public string Correction { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
        public string Status => (EndDateTime != null || EndDateTime == DateTime.MinValue) ? "Closed" : "Open";
    }
}
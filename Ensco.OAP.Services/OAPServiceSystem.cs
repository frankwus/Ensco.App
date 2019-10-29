using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ensco.Services;
using Ensco.Irma.Data;
using Ensco.OAP.Models;
using Ensco.OAP.Models.ViewModels;
using Ensco.Irma.Services;
using Ensco.Irma.Models;
using Ensco.Utilities;
using Ensco.Models;

namespace Ensco.OAP.Services
{
    public static class OAPServiceSystem
    {
        public enum OAPDataModelType
        {
            LessonLearned,
            LessonLearnedApproval,
            LessonLearnedOriginator,
            LessonLearnedType,
            Comment,
            Review
        };


        public static IOAPServiceDataModel GetServiceModel(OAPDataModelType modelType)
        {
            IOAPServiceDataModel dataModel = null;
            switch (modelType)
            {
                case OAPDataModelType.LessonLearned:
                    dataModel = new OAPServiceDataModel<LessonLearnedModel, OAP_LessonsLearned>();
                    break;
                case OAPDataModelType.LessonLearnedOriginator:
                    dataModel = new OAPServiceDataModel<LessonLearnedOriginatorModel, OAP_LessonsLearnedOriginators>();
                    break;
                case OAPDataModelType.Comment:
                    dataModel = new OAPServiceDataModel<CommentModel, OAP_Comment>();
                    break;
                case OAPDataModelType.LessonLearnedType:
                    dataModel = new OAPServiceDataModel<LessonLearnedType, OAP_LessonsLearnedType>();
                    break;
                case OAPDataModelType.Review:
                    dataModel = new OAPServiceDataModel<ReviewModel, OAP_Review>();
                    break;
            }

            return dataModel;
        }

        #region Lessons Learned logics
        public static LessonLearnedModel GetLessonLearned(int id)
        {
            var dataModel = GetServiceModel(OAPDataModelType.LessonLearned);
            LessonLearnedModel lessonLearned = dataModel.GetItem(string.Format("Id={0}", id), "Id");

            IIrmaServiceDataModel approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);

            var approvals = approvalDataModel.GetItems(
                string.Format("RequestItemId={0}", lessonLearned.Id),"Id").Cast<ApprovalModel>().ToList();
            foreach (var approver in approvals)
            {
                var user = ServiceSystem.GetUser((int)approver.Approver);
                approver.Position = (int)user.Position;
                approver.Initialize();
            }
            lessonLearned.Approvals = approvals;

            dataModel = OAPServiceSystem.GetServiceModel(OAPDataModelType.LessonLearnedType);
            LessonLearnedType lessonType = (LessonLearnedType)dataModel.GetItem(string.Format("Id={0}",lessonLearned.TypeId),"Id");
            lessonLearned.Type = lessonType;

            lessonLearned.Attachments = ServiceSystem.GetAttachments("Lessons Learned", lessonLearned.Id.ToString());

            dataModel = GetServiceModel(OAPDataModelType.LessonLearnedOriginator);
            var originators = dataModel.GetItems(string.Format("LessonId={0}", lessonLearned.Id), "Id").Cast<LessonLearnedOriginatorModel>().ToList();
            foreach (var originator in originators)
            {
                var user = ServiceSystem.GetUser(originator.PassportId);
                originator.Position = (int)user.Position;
            }
            lessonLearned.Originators = originators;

            return lessonLearned;
        }

        public static void AddPreApprover(ApprovalModel approvalModel, LessonLearnedModel lessonLearned)
        {
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            var approver = ServiceSystem.GetUser((int)approvalModel.Approver);

            approvalModel.Type = (int)ApprovalModel.ApprovalType.LessonsLearnedPreApproval;
            approvalModel.RequestItemId = lessonLearned.Id;
            approvalModel.Name = lessonLearned.Title;
            approvalModel.Requester = UtilitySystem.CurrentUserId;
            approvalModel.RequestedDate = DateTime.Now;
            approvalModel.Position = (int)approver.Position;
            approvalModel.Sequence = lessonLearned.Approvals.Count == 0 ? 1 : lessonLearned.Approvals.Max(a => a.Sequence) + 1;

            approvalModel = dataModel.Add(approvalModel);
            lessonLearned.Approvals.Add(approvalModel);
        }

        public static void AddApprover(ApprovalModel approvalModel, LessonLearnedModel lessonLearned)
        {
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            var approver = ServiceSystem.GetUser((int)approvalModel.Approver);

            approvalModel.Type = (int)ApprovalModel.ApprovalType.LessonsLearnedApproval;
            approvalModel.RequestItemId = lessonLearned.Id;
            approvalModel.Name = lessonLearned.Title;
            approvalModel.Requester = UtilitySystem.CurrentUserId;
            approvalModel.RequestedDate = DateTime.Now;
            approvalModel.Position = (int)approver.Position;

            approvalModel = dataModel.Add(approvalModel);
            lessonLearned.Approvals.Add(approvalModel);

        }
        #endregion

        #region Comments control logics

        //public IEnumerable<CommentModel> GetComments(Guid guid)
        //{

        //}

        #endregion

        #region Reviews logics

        public static ReviewModel AddReview(ReviewModel model)
        {
            IOAPServiceDataModel dataModel = GetServiceModel(OAPDataModelType.Review);

            model.DateCreated = DateTimeOffset.UtcNow;
            model.RigId = UtilitySystem.CurrentRigId;
            model.RequestedBy = UtilitySystem.CurrentUserId;

            return dataModel.Add(model);
        }

        public static ReviewModel GetReview(int id)
        {
            IOAPServiceDataModel dataModel = GetServiceModel(OAPDataModelType.Review);
            ReviewModel review = dataModel.GetItem(string.Format("Id={0}", id), "Id");

            var user = ServiceSystem.GetUser(review.ReviewerPassportId);
            review.Reviewer = user.DisplayName;

            review.Attachments = ServiceSystem.GetAttachments("Review", review.Id.ToString());
            return review;
        }

        public static bool UpdateReview(ReviewModel model)
        {
            IOAPServiceDataModel dataModel = GetServiceModel(OAPDataModelType.Review);
            return dataModel.Update(model);
        }

        #endregion
    }
}

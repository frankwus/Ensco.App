using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class GridConstants
    {
        public const string rigChecklistPendingWorkFlowStatus = "Pending";
        public const string relatedQuestionListKey = "RelatedQuestionListKey";

        #region GridName Constants
        public const string rigChecklistAssessorGrid = "checklistAssessorGrid";
        public const string rigChecklistExecutionGrid = "checklistExecutionGrid";
        public const string rigChecklistVerifierGrid = "checklistVerifierGrid";
        public const string rigChecklistRelatedSearchGrid = "rigChecklistRelatedSearchGrid";
        public const string rigChecklistBAAssetsGrid = "rigChecklistBAAssetsGrid";
        #endregion

        public const string RigChecklistQuestionKey  = "rigChecklistQuestion";

        public const string NoAnswerCommentErrorsKey = "NoAnswerCommentErrors";


        public const string AssessorsErrorsKey  = "AssessorsErrors";
        public const string ExecutionErrorsKey= "ExecutionErrors";
        public const string VerificationErrorsKey  = "VerificationErrors";
        public const string QuestionCommentsErrorsKey = "QuestionCommentErrors";
        public const string RigChecklistQuestionInlineCommentErrorsKey = "RigCheckListQuestionInlineCommentErrors";

        public const string RelatedSearchErrorsKey = "RelatedSearchErrors";


        public const string rigChecklistFindingGrid = "checklistFindingGrid";
        public const string rigChecklistFindingsCapaGrid = "checklistFindingsCapaGrid";

        public const string rigChecklistPreviousFindingGrid = "checklistPreviousFindingGrid";

        public const string RigFindingsErrorsKey  = "RigFindingsErrors";

        public const string RigOapChecklistGroupAssetsErrorsKey = "RigOapChecklistGroupAssetsErrors";

        public const string RigOapChecklistThirdPartyJobsErrorsKey = "RigOapChecklistThirdPartyJobsErrors";

        public const string RigOapChecklistWorkInstructionsErrorsKey = "RigOapChecklistWorkInstructionsErrors";

        public const string RigChecklistQuestionFindingsKey = "rigChecklistQuestionFindings";

        public const string RigChecklistQuestionIdKey = "RigChecklistQuestionId";

        public const string RigChecklistQuestionsKey = "rigChecklistQuestions";

        public const string PreviousFindingsErrorsKey = "PreviousFindingsErrors";

        public const string RigChecklistQuestionFindingKey = "rigChecklistQuestionFinding";

        public const string QuestionScoringErrorsKey  = "QuestionScoringErrors";


        public const string PreviousProtocolErrorsKey = "PreviousProtocolErrorsKey";

        public const string CapaPreviousProtocolErrorsKey = "CapaPreviousProtocolErrorsKey";







        [DefaultValue(None)]
        public enum VerifierRole
        {
            None,
            Assessor,
            OIM,
            RigManager,
            Master
        }


        [DefaultValue(Pending)]
        public enum WorkflowStatus
        {
            Pending,
            Completed,
            Rejected
        }

        [DefaultValue(Open)]
        public enum ChecklistStatus
        {
            Open,
            Pending,
            Completed
        }

        [DefaultValue(None)]
        public enum ControlTypeEnum
        {
            None,
            LessonsLearned,
            Review,
            FormComments,
            Finding,
            RelatedSearch
        }

        [DefaultValue(None)]
        public enum FindingTypeEnum
        {
            None,            
            Nonconformity,
            MajorNonconformity,
            Observation,
            Recommendation
        }
    }
}
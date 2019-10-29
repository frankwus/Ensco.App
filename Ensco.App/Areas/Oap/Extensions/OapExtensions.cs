using System.Collections.Generic;
using System.Linq;

namespace Ensco.App.Areas.Oap.Extensions
{
    using Ensco.App.Areas.Oap.Controllers;
    using Ensco.App.Areas.Oap.Models;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.WebClient.Rig;
    using Ensco.OAP.Models;
    using Ensco.OAP.Services;
    using StructureMap;
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Westwind.Globalization;

    public static class OapExtensions
    {
        public static IList<OapGenericCheckListFlatModel> ToFlattenedModels(this RigOapChecklist rigchecklist)
        {
            var questions = rigchecklist.Questions;

            var genericFlatModelList = new List<OapGenericCheckListFlatModel>();

            if (questions.Any())
            {
                genericFlatModelList = (from g in rigchecklist.OapChecklist.Groups
                                        from sg in g.SubGroups
                                        from t in sg.Topics
                                        from q in t.Questions
                                        select new OapGenericCheckListFlatModel(rigchecklist.Id,
                                                                                rigchecklist.OapChecklist.Id,
                                                                                g.Id,
                                                                                g.Name,
                                                                                g.Order,
                                                                                g.OapGraphic?.Image ?? new byte[0],
                                                                                sg.Id,
                                                                                sg.Name,
                                                                                sg.Order,
                                                                                sg.IsVisible,
                                                                                sg.IsPlantMonitoring,
                                                                                sg.IsThirdPartyActivities,
                                                                                sg.IsWorkInstructions,
                                                                                t.Topic,
                                                                                questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id)?.Id,
                                                                                q.Id,
                                                                                q.Question,
                                                                                q.Order,
                                                                                q.Help,
                                                                                questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id)?.Comment,
                                                                                q.Weight,
                                                                                q.Maximum,
                                                                                q.Criticality,
                                                                                q.Section,
                                                                                q.BasicCauseClassification,
                                                                                questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id)?.Answers?.ToDictionary(),
                                                                                questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id)?.OapChecklistQuestion?.OapChecklistQuestionDataType?.Code ?? "RBL",
                                                                                questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id)?.OapChecklistQuestion?.OapFrequency?.Name ?? "")
                ).OrderBy(x => x.GroupOrder).ThenBy(x => x.SubGroupOrder).ThenBy(x => x.QuestionOrdinal).ToList();
            }
            return genericFlatModelList;
        }

        public static IDictionary<int, string> ToDictionary(this ObservableCollection<RigOapChecklistQuestionAnswer> answers)
        {
            var values = new Dictionary<int, string>();

            foreach (var ans in answers)
            {
                values.Add(ans.Ordinal, ans.Value);
            }

            return values;
        }

        public static IList<OapSearchCheckListQuestionFlatModel> ToRelatedQuestionFlattenedModel(this IEnumerable<RigOapChecklistQuestion> rigRelatedQuestionList, int questionId, RigOapChecklistClient checklistClient, OapChecklistFindingClient checklistFindingClient)
        {
            var layoutList = new List<OapSearchCheckListQuestionFlatModel>();
            foreach (var item in rigRelatedQuestionList)
            {
                var checklist = checklistClient.GetCompleteChecklistAsync(item.RigOapChecklistId).Result.Result.Data;

                if (checklist == null)
                {
                    continue;
                }

                var rigQuestion = checklist.Questions.FirstOrDefault(rq => rq.Id == item.Id);

                var qFindings = checklistFindingClient.GetAllFindingsForQuestionAsync(rigQuestion.Id).Result.Result.Data;

                var rigQuestionAnswers = rigQuestion?.Answers?.ToList();

                StringBuilder sb = new StringBuilder();
                sb.Append("{ ");
                foreach (var questionAnswer in rigQuestionAnswers)
                {
                    var answer = questionAnswer.Value;
                    answer = (answer == "Y") ? "YES".Translate() : ((answer == "N") ? "NO".Translate() : " ");
                    sb.Append(answer);

                    if (rigQuestionAnswers.Count > 1)
                    {
                        sb.Append(", ");
                    }

                }

                sb.Append(" }");
                var answers = sb.ToString();

                var gst = (from g in checklist.OapChecklist.Groups
                           from sg in g.SubGroups
                           from t in sg.Topics
                           from q in t.Questions
                           where q.Id == rigQuestion.OapChecklistQuestionId
                           select new { Group = g, SubGroup = sg, Topic = t }).FirstOrDefault();

                var relatedQuestion = new OapSearchCheckListQuestionFlatModel(checklist.Id, checklist.RigId.ToString(), checklist.RigChecklistUniqueId, checklist.OapchecklistId, checklist.ChecklistDateTime, checklist.Title, gst?.Group?.Name, gst?.SubGroup?.Name, gst?.Topic?.Topic, item.OapChecklistQuestionId ?? 0, rigQuestion?.Question, answers, qFindings.FirstOrDefault()?.FindingType?.Name);

                if (!layoutList.Any(l => l.RigOapChecklistId == relatedQuestion.RigOapChecklistId && l.QuestionId == relatedQuestion.QuestionId))
                {
                    layoutList.Add(relatedQuestion);
                }

            }
            return layoutList;
        }

        public static IList<OapProtocolScoringFlatModel> ToProtocolScoringFlattenedModel(this RigOapChecklist rigchecklist)
        {
            var questions = rigchecklist.Questions;

            var protocolScoringFlatModelList = new List<OapProtocolScoringFlatModel>();

            if (questions.Any())
            {
                protocolScoringFlatModelList = (from g in rigchecklist.OapChecklist.Groups
                                                from sg in g.SubGroups
                                                from t in sg.Topics
                                                from q in t.Questions
                                                select new OapProtocolScoringFlatModel(questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id).Id, rigchecklist.Id, rigchecklist.OapChecklist.Id, g.Name, t.Topic, q.Id, q.Question, q.YesValue, q.EditNoValue, questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id).NoValue, q.Maximum, 0, 0, q.Weight, questions.FirstOrDefault(rq => rq.OapChecklistQuestionId == q.Id)?.Answers?.ToDictionary())
                ).ToList();
            }
            return protocolScoringFlatModelList;
        }

        public static IList<OIMBehavioralBasedSafetyFlatModel> ToBBSFlattenedModel(RigOapChecklistClient rigOapChecklistClient, string checklistType, string checklistSubType, string status, string verifierRole)
        {
            var checklist = rigOapChecklistClient.GetAllInWorkflowByTypeSubTypeCodeStatusAsync(checklistType, checklistSubType, status, verifierRole).Result?.Result?.Data;

            var bbsFSOFlatModelList = new List<OIMBehavioralBasedSafetyFlatModel>();

            checklist.ToList().ForEach((c) =>
            {

                var group = c.OapChecklist.Groups?.FirstOrDefault(g => g.OapChecklistId == c.OapChecklist.Id);

                var questions = c.Questions;

                var nonConformityCount = 0;
                if (questions.Any())
                {
                    nonConformityCount = questions.Count(f => f.Answers.FirstOrDefault()?.Value == "N");
                }

                var flatList = new OIMBehavioralBasedSafetyFlatModel(c.Id,
                                                                     c.OapChecklist.Id,
                                                                     c.PositionId,
                                                                     c.OwnerId,
                                                                     c.LocationId,
                                                                     c.ChecklistDateTime,
                                                                     nonConformityCount);


                bbsFSOFlatModelList.Add(flatList);
            });

            return bbsFSOFlatModelList;

        }

        public static IList<RigOapChecklistGroupAssetModel> ToGroupAssetModels(this ObservableCollection<RigOapChecklistGroupAsset> checklistAssets, int groupId, OapChecklistAssetDataManagementClient oapChecklistAssetDataManagementClient)
        {
            if (groupId == 0)
            {
                return new List<RigOapChecklistGroupAssetModel>();
            }

            var assets = oapChecklistAssetDataManagementClient.GetAllByGroupAsync(groupId).Result.Result.Data;

            if ((assets == null) || (assets?.Any() == false))
            {
                return new List<RigOapChecklistGroupAssetModel>();
            }

            var assetModels = (from ca in checklistAssets
                               let asset = assets.FirstOrDefault(a => a.Id == ca.AssetId)
                               select new RigOapChecklistGroupAssetModel(ca.Id, groupId, ca.AssetId, asset?.OapSubSystem?.OapSystem?.SystemGroupId ?? 0, asset?.OapSubSystem?.OapSystem?.SystemGroup?.Name, asset?.OapSubSystem?.OapSystemId ?? 0, asset?.OapSubSystem?.OapSystem?.Name, asset?.OapSubSystemId ?? 0, asset.OapSubSystem.Name, ca.Value)).ToList();

            return assetModels;
        }

        public static bool HasComments(this OapGenericCheckListFlatModel question, RigOapChecklist rigOapChecklist)
        {
            IOAPServiceDataModel dataModel = OAPServiceSystem.GetServiceModel(OAPServiceSystem.OAPDataModelType.Comment);
            IEnumerable<CommentModel> comments = dataModel.GetItems(string.Format("QuestionId=\"{0}\" AND SourceFormId=\"{1}\"", question.QuestionId, rigOapChecklist.Id.ToString()), "Id").Cast<CommentModel>();

            return comments.Count() > 0;
        }

        public static bool HasActiveNoAnswerControl(this OapGenericCheckListFlatModel question, OapChecklistClient oapChecklistClient, int checklistId, int oapChecklistQuestionId)
        {
            var noAnswer = oapChecklistClient.GetFirstQuestionOpenNoAnswersAsync(checklistId, oapChecklistQuestionId).Result?.Result?.Data;
            
            return noAnswer != null;
        }

        public static bool ChecklistIsRelatedToPreviousNoAnswer(this OapGenericCheckListFlatModel question, OapChecklistClient oapChecklistClient, Guid rigOapChecklistId, int oapChecklistId, int oapChecklistQuestionId)
        {
            var noAnswer = oapChecklistClient.GetAllQuestionNoAnswersAsync(oapChecklistId, oapChecklistQuestionId).Result?.Result?.Data;

            return noAnswer.Where(n => n.SourceRigOapChecklistId == rigOapChecklistId || n.ClosureRigOapChecklistId == rigOapChecklistId).Count() > 0;
        }

        public static bool IsEditable(this RigOapChecklist checklist)
        {
            return checklist.Status == "Open";
        }
    }
}
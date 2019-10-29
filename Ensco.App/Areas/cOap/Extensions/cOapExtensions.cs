using System.Text;
using System.Linq;
using System.Collections.Generic;
using Ensco.App.Areas.cOap.Models;
using Ensco.App.Areas.Oap.Models;
using Ensco.Irma.Oap.Common.Extensions;
using Ensco.Irma.Oap.WebClient.Corp;
using System.Collections.ObjectModel;

namespace Ensco.App.Areas.cOap.Extensions
{

    public static class cOapExtensions
    {

        public static IList<OapGenericCheckListFlatModel> ToFlattenedModels(this RigOapChecklist protocol)
        {
            var questions = protocol.Questions;

            var genericFlatModelList = new List<OapGenericCheckListFlatModel>();

            if (questions.Any())
            {
                genericFlatModelList = (from g in protocol.OapChecklist.Groups
                                        from sg in g.SubGroups
                                        from t in sg.Topics
                                        from q in t.Questions
                                        select new OapGenericCheckListFlatModel(protocol.Id,
                                                                                protocol.OapChecklist.Id,
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

        public static IList<OapSearchCheckListQuestionFlatModel> ToRelatedQuestionFlattenedModel(this IEnumerable<RigOapChecklistQuestion> rigRelatedQuestionList, int questionId, OapAuditClient auditClient, OapChecklistFindingClient checklistFindingClient)
        {
            var layoutList = new List<OapSearchCheckListQuestionFlatModel>();
            foreach (var item in rigRelatedQuestionList)
            {
                var protocol = auditClient.GetCompleteProtocolAsync(item.RigOapChecklistId).Result.Result.Data;

                if (protocol == null)
                {
                    continue;
                }

                var rigQuestion = protocol.Questions.FirstOrDefault(rq => rq.Id == item.Id);

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

                var gst = (from g in protocol.OapChecklist.Groups
                           from sg in g.SubGroups
                           from t in sg.Topics
                           from q in t.Questions
                           where q.Id == rigQuestion.OapChecklistQuestionId
                           select new { Group = g, SubGroup = sg, Topic = t }).FirstOrDefault();

                var relatedQuestion = new OapSearchCheckListQuestionFlatModel(protocol.Id, protocol.RigId.ToString(), protocol.RigChecklistUniqueId, protocol.OapchecklistId, protocol.ChecklistDateTime, protocol.Title, gst?.Group?.Name, gst?.SubGroup?.Name, gst?.Topic?.Topic, item.OapChecklistQuestionId ?? 0, rigQuestion?.OapChecklistQuestion.Question, answers, qFindings.FirstOrDefault()?.FindingType?.Name);

                if (!layoutList.Any(l => l.RigOapChecklistId == relatedQuestion.RigOapChecklistId && l.QuestionId == relatedQuestion.QuestionId))
                {
                    layoutList.Add(relatedQuestion);
                }

            }
            return layoutList;
        }


        public static IList<OapPreviousProtocolsFlatModel> ToPreviousProtocolsModel(this IEnumerable<RigOapChecklist> rigChecklistList, ReviewSearchModel model, OapAuditClient auditClient, OapChecklistFindingClient checklistFindingClient)
        {
            var layoutList = new List<OapPreviousProtocolsFlatModel>();

            foreach (var item in rigChecklistList)
            {
                var protocol = auditClient.GetCompleteProtocolAsync(item.Id).Result?.Result?.Data;

                if (protocol == null)
                {
                    continue;
                }


                var count = 0;
                var question = protocol.Questions.ToList();

                question.ForEach((q) =>
                {

                    var v = q.Answers.Where(a => a.RigOapChecklistQuestionId == q.Id).FirstOrDefault().Value;

                    if (v == "N" || v == "NA")
                    {
                        count++;

                    }

                });

                var previousProtocols = new OapPreviousProtocolsFlatModel(protocol.Id, protocol.RigChecklistUniqueId, protocol.OapChecklist.OapType.Name, protocol.Title, protocol.Assessors?.Where(a => a.IsLead == true)?.FirstOrDefault()?.User?.Name, protocol.ChecklistDateTime.ToString("dd-MMM-yyyy"), count.ToString(), protocol.Status, "Review");

                if (!layoutList.Any(l => l.ChecklistId == previousProtocols.ChecklistId))
                {
                    layoutList.Add(previousProtocols);
                }
            }

            return layoutList;
        }

        public static IList<OapCapaProtocolsFlatModel> ToFindingsCapaModel(this RigOapChecklist checkList, OapAuditClient auditClient, OapChecklistFindingClient checklistFindingClient)
        {
            var layoutList = new List<OapCapaProtocolsFlatModel>();

            var ckListFindings = checklistFindingClient.GetAllFindingsForChecklistAsync(checkList.Id).Result?.Result?.Data;

            foreach (var finding in ckListFindings)
            {
                var capa = checklistFindingClient.GetCAPAsByFindingIdAsync(finding.RigChecklistFindingInternalId).Result?.Result?.Data;

                var capaProtocol = new OapCapaProtocolsFlatModel(checkList.Id, checkList.RigChecklistUniqueId, checkList.Assessors?.Where(a => a.IsLead == true)?.FirstOrDefault()?.User?.Name,
                    finding.RigChecklistFindingInternalId, finding.FindingTypeId , finding.FindingSubTypeId, finding.CriticalityId, 0, " "
                    , finding.AssignedUser?.Name, finding?.Status, "Review");

                if (!layoutList.Any(l => l.FindingId == capaProtocol.FindingId))
                {
                    layoutList.Add(capaProtocol);
                }
            }

            return layoutList;
        }
    }
}
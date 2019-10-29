using Ensco.Irma.Oap.Common.Extensions;
using System;

namespace Ensco.App.Areas.Oap.Models
{
    public class OapSearchCheckListQuestionFlatModel
    {
        public OapSearchCheckListQuestionFlatModel()
        {

        }

        public OapSearchCheckListQuestionFlatModel(Guid rigOapChecklistId, string rigId, int rigChecklistUniqueId, int oapchecklistId, DateTime checklistDateTime, string title, string ascessor, string category, string topic, int questionId, string Question, string answer, string findings)
        {
            RigOapChecklistId = rigOapChecklistId;
            RigId = rigId;
            RigChecklistUniqueId = rigChecklistUniqueId;
            OapchecklistId = oapchecklistId;
            ChecklistDateTime = checklistDateTime;
            Title = title?.Translate();
            Ascessor = ascessor;
            Category = category?.Translate();
            Topic = topic;
            QuestionId = questionId;
            this.Question = Question?.Translate();
            Answer = answer;
            Findings = findings;
        }

        public Guid RigOapChecklistId { get; }
        public string RigId { get; }

        public int RigChecklistUniqueId { get; }

        public int OapchecklistId { get; }

        public DateTime ChecklistDateTime { get; }

        public string Title { get; }

        public string Ascessor { get; }

        public string Category { get; }

        public string Topic { get; }

        public int QuestionId { get; set; }

        public string Question { get; }

        public string Answer { get; }

        public string Findings { get; }


    }
}
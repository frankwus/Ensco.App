using Ensco.Irma.Oap.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Ensco.App.Areas.Oap.Models
{
    public class OapProtocolScoringFlatModel
    {
        public OapProtocolScoringFlatModel()
        {

        }

        public OapProtocolScoringFlatModel(Guid id,Guid rigChecklistId, int checklistId, string group, string topic, int questionId, string Question, int yesValue, bool editNoValue, int noValue, int maxScore, double score, double averageScore, double weight, IDictionary<int, string> yesNoNakvs = null)
        {
            Id = id;
            RigChecklistId = rigChecklistId;
            ChecklistId = checklistId;
            Group = group?.Translate();
            Topic = topic?.Translate();
            QuestionId = questionId;
            this.Question = Question?.Translate();
            YesValue = yesValue;
            EditNoValue = editNoValue;
            NoValue = noValue;
            MaxScore = maxScore;        
            Weight = weight;

            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(0))
            {
                YesNoNa0 = yesNoNakvs[0];
            }

            if(YesNoNa0 == "Y")
            {
                score = (weight * yesValue);
            }
            else
            {
                score = (weight * NoValue);
            }

            Score = score;

            averageScore = Math.Round((score / maxScore) * 100);

            AverageScore = averageScore;           
        }

        public Guid Id { get; }
        public Guid RigChecklistId { get; }
        public int ChecklistId { get; } 
        public string Group { get; }
        public string Topic { get; }
        public int QuestionId { get; set; }
        public string Question { get; }
        public int YesValue { get; }
        public bool EditNoValue { get; }        
        public int NoValue { get; set; }
        public int MaxScore { get; }
        public double Score { get; }
        public double AverageScore { get; }       
        public double Weight { get; }
        public string YesNoNa0 { get; }
    }
}
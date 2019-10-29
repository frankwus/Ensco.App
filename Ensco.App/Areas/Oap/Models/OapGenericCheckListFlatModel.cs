using Ensco.Irma.Oap.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Ensco.App.Areas.Oap.Models
{
    public class OapGenericCheckListFlatModel
    {
        public OapGenericCheckListFlatModel()
        {
             
        }


        public OapGenericCheckListFlatModel(Guid rigChecklistId, int checklistId,int groupId, string group, int groupOrder, byte[] graphic, int subGroupId, string subGroup, int subGroupOrder, bool displaySubGroup,bool isPlantMonitoring, bool IsThirdPartyActivities, bool IsWorkInstructions, string topic,Guid? rigQuestionId, int questionId, string question, string questionOrdinal, string questionHelp, string comment, double weight, int score, int? criticality = null, string section = "", string basicCauseClassification ="", IDictionary<int,string> yesNoNakvs = null, string displayType = "RBL", string frequency = "")
        {
            RigChecklistId = rigChecklistId;
            ChecklistId = checklistId;
            GroupId = groupId;
            GroupOrder = groupOrder;
            Graphic = graphic;
            SubGroupId = subGroupId;
            SubGroupOrder = subGroupOrder;
            Group = group?.Translate();
            SubGroup = subGroup?.Translate();
            DisplaySubGroup = displaySubGroup;
            IsPlantMonitoring = isPlantMonitoring;
            this.IsThirdPartyActivities = IsThirdPartyActivities;
            this.IsWorkInstructions = IsWorkInstructions;
            Topic = topic?.Translate();
            RigQuestionId = rigQuestionId;
            QuestionId = questionId;
            this.Question = question?.Translate();
            QuestionOrdinal = questionOrdinal;
            QuestionHelp = questionHelp?.Translate();
            YesNoNaKeyValues = yesNoNakvs;
            DisplayType = displayType;
            Frequency = frequency;
            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(0))
            {
                YesNoNa0 = yesNoNakvs[0];
                //if (YesNoNa0.Equals("N",StringComparison.InvariantCultureIgnoreCase))
                //{
                //    DisplayNoControl = true;
                //}
            }
            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(1))
            {
                YesNoNa1 = yesNoNakvs[1];
            }
            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(2))
            {
                YesNoNa2 = yesNoNakvs[2];
            }
            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(3))
            {
                YesNoNa3 = yesNoNakvs[3];
            }
            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(4))
            {
                YesNoNa4 = yesNoNakvs[4];
            }
            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(5))
            {
                YesNoNa5 = yesNoNakvs[5];
            }
            if ((yesNoNakvs != null) && yesNoNakvs.ContainsKey(6))
            {
                YesNoNa6 = yesNoNakvs[6];
            }

            Comment = comment;
            Criticality = criticality;
            Section = section;
            this.BasicCauseClassification = basicCauseClassification;

            Weight = weight;
            Score = Score;
        }

        public Guid RigChecklistId { get; }

        public int ChecklistId { get; }
        public int GroupId { get; }
        public int GroupOrder { get; }
        public byte[] Graphic { get; }
        public int SubGroupId { get; }
        public int SubGroupOrder { get; }
        public string Group { get; } 

        public string SubGroup { get; }

        public bool DisplaySubGroup { get; }
        public bool IsPlantMonitoring { get; }
        public bool IsThirdPartyActivities { get; }
        public bool IsWorkInstructions { get; }
        public string Topic { get; }
        public Guid? RigQuestionId { get; }
        public int QuestionId { get; set; }

        public string Question { get; }

        public string QuestionOrdinal { get; }

        public string QuestionHelp { get; }

        public IDictionary<int, string> YesNoNaKeyValues { get; set; }

        public string DisplayType { get; }
        public string Frequency { get; }
        public string Comment { get; set; }

        public bool DisplayNoControl { get; set; }

        public int? Criticality { get; }
        public string Section { get; }
        public string BasicCauseClassification { get; }

        public double Weight { get; }
        public int Score { get; }
        public string YesNoNa { get; set; }

        public string YesNoNa0
        {
            get;

            set;
        }

        public string YesNoNa1
        {
            get;

            set;
        }

        public string YesNoNa2
        {
            get;

            set;
        }
        public string YesNoNa3
        {
            get;

            set;
        }
        public string YesNoNa4
        {
            get;

            set;
        }
        public string YesNoNa5
        {
            get;

            set;
        }
        public string YesNoNa6
        {
            get;

            set;
        }

    }
}
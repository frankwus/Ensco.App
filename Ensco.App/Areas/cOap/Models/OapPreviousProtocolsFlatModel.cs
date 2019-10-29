using Ensco.Irma.Oap.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Ensco.App.Areas.cOap.Models
{
    public class OapPreviousProtocolsFlatModel
    {
        public OapPreviousProtocolsFlatModel()
        {

        }
        public OapPreviousProtocolsFlatModel(Guid checklistId, int checklistUniqueId, string oapType, string title, string assessor, string checklistDateTime, string findings, string status, string action)
        {
            ChecklistId = checklistId;
            ChecklistUniqueId = checklistUniqueId;
            OapType = oapType;           
            Title = title;
            Assessor = assessor;
            ChecklistDateTime = checklistDateTime;
            Findings = findings;
            Status = status;
            Action = action;          
        }
        public Guid ChecklistId { get; }
        public int ChecklistUniqueId { get; }
        public string OapType { get; }
        public string Title { get; }
        public string Assessor { get; }
        public string ChecklistDateTime { get; }
        public string Findings { get; }
        public string Status { get; }
        public string Action { get; }       
    }
}
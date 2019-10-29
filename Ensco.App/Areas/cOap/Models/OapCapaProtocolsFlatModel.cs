using Ensco.Irma.Oap.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Ensco.App.Areas.cOap.Models
{
    public class OapCapaProtocolsFlatModel
    {
        public OapCapaProtocolsFlatModel()
        {

        }

        public OapCapaProtocolsFlatModel(Guid rigChecklistId, int checklistUniqueId, string assessor, int findingId, int findingType, int findingSubTypeId, int criticalityId, int capaId, string actionRequired, string assignedTo, string status, string reviewed)
        {
            RigChecklistId = rigChecklistId;
            ChecklistUniqueId = checklistUniqueId;
            Assessor = assessor;
            FindingId = findingId;
            FindingType = findingType;
            FindingSubTypeId = findingSubTypeId;
            CriticalityId = criticalityId;
            CapaId = capaId;
            ActionRequired = actionRequired;
            AssignedTo = assignedTo;
            Status = status;
            Reviewed = reviewed;
        }
        public Guid RigChecklistId { get; }
        public int ChecklistUniqueId { get; }
        public string Assessor { get; }
        public int FindingId { get; }
        public int FindingType { get; }
        public int FindingSubTypeId { get; }
        public int CriticalityId { get; }
        public int? CapaId { get; }
        public string ActionRequired { get; }
        public string AssignedTo { get; }
        public string Status { get; }
        public string Reviewed { get; }

    }
}
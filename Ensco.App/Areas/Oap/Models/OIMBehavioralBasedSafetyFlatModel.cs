using System;
using System.Collections.Generic;

namespace Ensco.App.Areas.Oap.Models
{
    public class OIMBehavioralBasedSafetyFlatModel
    {
        public OIMBehavioralBasedSafetyFlatModel()
        {

        }

        public OIMBehavioralBasedSafetyFlatModel(Guid rigChecklistId, int checklistId, int? positionId, int? ownerId, int? locationId, DateTime date, int nonConformity)
        {
            RigChecklistId = rigChecklistId;
            ChecklistId = checklistId;
            PositionId = positionId;
            OwnerId = ownerId;
            LocationId = locationId;
            Date = date;
            NonConformity = nonConformity;
        }

        public Guid RigChecklistId { get; }
        public int ChecklistId { get; } 
        public int? PositionId { get; }
        public int? OwnerId { get; }        
        public int? LocationId { get; }
        public DateTime Date { get; }
        public int NonConformity { get; }       
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Ensco.App.Areas.cOap.Models
{
    public class AuditPlanCVTComplianceModel
    {
         
        public int BUId { get; set; }
        public int RigId { get; set; } 
        public DateTime LastCompleted { get; set; }
        public DateTime DueDate { get; set; }      
        public string NextSchedule { get; set; }
        public string Compliant { get; set; }
    }
}
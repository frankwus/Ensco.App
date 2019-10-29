using System;
using System.ComponentModel.DataAnnotations;

namespace Ensco.App.Areas.cOap.Models
{
    public class AuditPlanSummaryModel
    {
         
        public int BUId { get; set; }
        public int RigId { get; set; } 
        public string Protocol { get; set; }      
        public int ProtocolCompleteCount { get; set; }            
        public double ProtocolCompletePercentage { get; set; }
        public string Findings { get; set; }
        public int FindingsCompleteCount { get; set; }
        public double FindingsCompletePercentage { get; set; }
        public string Capa { get; set; }
        public int CapaCompleteCount { get; set; }
        public double CapaCompletePercentage { get; set; }

    }
}
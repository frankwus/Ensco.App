using System;
using System.ComponentModel.DataAnnotations;

namespace Ensco.App.Areas.cOap.Models
{
    public class AuditPlanRACProtocolModel
    {
         
        public int BUId { get; set; }
        public int RigId { get; set; }
        public string AuditType { get; set; }
        public string Element { get; set; }
        public int ProtocolId { get; set; }
        public string Protocol { get; set; }             
        public string OapLevel { get; set; } 
        public DateTime DateCompleted { get; set; }
        public string TotalFindings { get; set; }
        public string OpenFindings { get; set; }
        public string TotalCapa { get; set; }
        public string OpenCapa { get; set; }
    }
}
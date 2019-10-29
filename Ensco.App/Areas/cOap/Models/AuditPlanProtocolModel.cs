using System;
using System.ComponentModel.DataAnnotations;

namespace Ensco.App.Areas.cOap.Models
{
    public class AuditPlanProtocolModel
    {
         
        public int BUId { get; set; }
        public int RigId { get; set; }
        public int Id { get; set; }
        public string Protocol { get; set; }      
        public string OapCategory { get; set; }            
        public string OapLevel { get; set; }
        public int OwnerId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Findings { get; set; }
        public string status { get; set; }    
    }
}
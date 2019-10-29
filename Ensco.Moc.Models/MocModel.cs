using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Moc.Models
{
    public class MocModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        public string MocNumber { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public bool RiskAssessmentOnly { get; set; }
        public bool TemporaryChange { get; set; }
        public int CreatedBy { get; set; }
        public int AssignedTo { get; set; }
        public int Location { get; set; }
        public bool RequireReview { get; set; } 
        public bool RequireApproval { get; set; }
        public int Priority { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateEffective { get; set; }
        public DateTime DateExpired { get; set; }
        public DateTime DateClosed { get; set; }
        public bool EamsCreated { get; set; }
        public bool EamsUpdated { get; set; }
        public int Sequence { get; set; }
        public List<ApprovalModel> Approvals { get; set; }

        public MocModel()
        {

        }
    }
}

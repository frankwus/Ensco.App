using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Moc.Models
{
    public class ApprovalModel
    {
        public enum ApprovalType { Review, Approval};

        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        public int MocId { get; set; }
        public ApprovalType Type { get; set; }
        public int UserId { get; set; }
        public DateTime DateNeeded { get; set; }
        public DateTime DateFinished { get; set; }
    }
}

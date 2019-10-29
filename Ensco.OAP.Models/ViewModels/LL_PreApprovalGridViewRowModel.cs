using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models.ViewModels
{
    public class LL_PreApprovalGridViewRowModel
    {
        [Column(Visible = false)]
        public int Id { get; set; }

        [Column(LookupList = "Passport", DisplayName = "Passport")]
        public int ApproverPassportId { get; set; }

        [Column(LookupList = "Position")]
        public int Position { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        public DateTime DueDate { get; set; }
        public string OriginatorComments { get; set; }
        public string Status { get; set; }

        [Column(DisplayName = "Reviewer Comments")]
        public string ApproverComments { get; set; }
        [Column(Visible = false)]
        public DateTimeOffset DateCompleted { get; set; }
    }
}

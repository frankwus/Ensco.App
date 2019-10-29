using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Irma
{
    public class IrmaHomeTasksCapaModel
    {
        [Column(Visible = true, Key = true, DisplayName = "Source")]
        public int id { get; set; }
        public string Criticality { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly)]
        public Nullable<System.DateTime> DueDate { get; set; }
        public string Action_Required { get; set; }
        public string Status { get; set; }
        [Column(Visible = false)]
        public int id1 { get; set; }
        [Column(Visible = false)]
        public string assigneeUserId { get; set; }
        [Column(Visible = false)]
        public string AssignorUserId { get; set; }
    }
}

using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Irma
{
    public class IrmaHomeTasksTaskModel
    {
        [Column(Visible = true, Key = true, DisplayName = "Source")]
        public Nullable<int> id { get; set; }
        public string page { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string AssignedBy { get; set; }
        public string Comment { get; set; }
        [Column(Visible = false)]
        public Nullable<int> IsoId { get; set; }
        [Column(Visible = false)]
        public string UserId { get; set; }
    }
}

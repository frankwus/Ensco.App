using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Jobs
{
    public class PermitModel
    {
        [Column(Visible = false, Key =true)]
        public Nullable<int> JobId { get; set; }
        public string JobTitle { get; set; }
        public Nullable<int> CW { get; set; }
        public Nullable<int> HW { get; set; }
        public Nullable<int> CSE { get; set; }
        public Nullable<int> EI { get; set; }
        public Nullable<int> GT { get; set; }
        [Column(Visible = false)]
        public int PermitId { get; set; }
        public string PermitTitle { get; set; }
        public Nullable<int> LocationId { get; set; }
        [Column(Visible = false)]
        public string Action { get; set; }
        [Column(Visible = false)]
        public int id1 { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Status { get; set; }

    }
}

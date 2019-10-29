using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Jobs
{
    public class JobsHomeModel
    {
        public List<JobSummaryModel> Summary { get; set; }
        public DataTableModel Permits { get; set; }
        public DataTableModel ActiveLongTermIsolations { get; set; }
        public DataTableModel ActiveShortTermIsolations { get; set; }
        public DataTableModel PermitAuthorities { get; set; }
        public DataTableModel IsolationAuthorities { get; set; }
    }
}

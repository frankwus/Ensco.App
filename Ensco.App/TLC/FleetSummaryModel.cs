using Ensco.Models;
using Ensco.TLC.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.TLC
{
    public class FleetSummaryModel
    {
        [Column(LookupList ="Passport")]
        public Nullable<int> PassportId { get; set; }
        public string EnscoPassportNo { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string RigName { get; set; }
        public int TotalCapBooks { get; set; }
        public int CapBooksApproved { get; set; }
        public int CapBooksInProgress { get; set; }
        public int CapBooksPendingApproval { get; set; }
        public int CapBooksOverdue { get; set; }
        public int CapBooksNotStarted { get; set; }
        public int CapBooksExpiringNext3Months { get; set; }
        public DataTableModel FleetSummaryRig { get; set; }
        public IQueryable<CAP_FleetSummaryTotalsModel> Totals { get; set; }

        // Fleet Summary
        public CAP_FleetSummaryEmployeeTotalsModel EmpTotals { get; set; }
        public DataTableModel FleetSummaryEmployee { get; set; }
        public IQueryable<CAP_FleetSummaryEmployeeTotalsModel> EmployeeTotals { get; set; }

    }
}
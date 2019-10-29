using Ensco.Irma.Models;
using Ensco.Models;
using Ensco.TLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.TLC
{
    public class ManageCapBookModel
    {
        public string RigName { get; set; }
        public long SelectedPersonnelId { get; set; }
        public string Passport { get; set; }
        public long CompId { get; set; }
        public DataTableModel PersonalSummary { get; set; }
        public DataTableModel Competency { get; set; }
        public DataTableModel KSAs { get; set; }
        public DataTableModel Additional { get; set; }
        public DataTableModel Expired { get; set; }
        public DataTableModel AssessorGuide { get; set; }
        public IQueryable<CAP_BookModel> CapBooks { get; set; }

        // Show All the Cap Books that need approvals
        public DataTableModel CapBookApprovals { get; set; }
        public List<ApprovalModel> Approvals { get; set; }

        public string[] SelectedKSAs { get; set; } //semi-colon seperated email addresses


        public ManageCapBookModel()
        {
            RigName = Utilities.UtilitySystem.Settings.RigName;
        }
    }
}
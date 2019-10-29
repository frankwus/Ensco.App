using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigAdminManageModel
    {
        public List<RigModel> Rigs { get; set; }
        public Nullable<int> SelectedRigId { get; set; }
        public Nullable<int> SelectedPersonnelRigId { get; set; }
        public Nullable<int> SelectedPersonnelCorpId { get; set; }
        public DataTableModel Assets { get; set; }
        public List<dynamic> Personnel { get; set; }
        public List<dynamic> CorpPersonnel { get; set; }
        public List<dynamic> Compliance { get; set; }
        public List<dynamic> Requirements { get; set; }
        public List<dynamic> FieldsVisible { get; set; }
        public RigRequirementsModel SelectedRequirement { get; set; }
        public RigControlModel Control { get; set; }
        public int MaxPOB { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeFormat { get; set; }
        [Column(LookupList ="PassportEmail")]
        public string[] PobSummaryEmailList { get; set; } //semi-colon seperated email addresses
        public DataTableModel AdminUsers { get; set; }
        public DataTableModel IsolationLocks { get; set; }

        // Rig Phase 1 settings
        [Column(LookupList="PassportOIM")]
        public int CurrentOIM { get; set; }
        [Column(LookupList = "PassportMaster")]
        public int CurrentMaster { get; set; }
        public bool IsRigInBrazil { get; set; }
        public bool IsClientRequireSignColdWorkPermit { get; set; }
        public bool IsClientRequireSignHotWorkPermit { get; set; }
        public bool IsClientRequireSignConfinedWorkPermit { get; set; }
        public bool ChooseClientSignAtTimeOfPermit { get; set; }

        [Column(LookupList = "PassportEmail")]
        public string[] DailySummaryEmailList { get; set; } 

        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.TimeOnly)]
        public DateTime EmailTime { get; set; }

        public DataTableModel JobCodes { get; set; }

        //[Column(LookupList="Location")]
        //public Nullable<int> Location { get; set; }

        public RigAdminManageModel()
        {
        }
    }
}

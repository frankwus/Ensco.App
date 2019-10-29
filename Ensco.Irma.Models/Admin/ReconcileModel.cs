using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Admin
{
    public class ReconcileModel
    {
        [Column(Visible = false, Key = true)]
        public int PassportId { get; set; }
        public string Passport { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DisplayName { get; set; }
        [Column(LookupList ="Company")]
        public Nullable<int> Company { get; set; }
        [Column(LookupList = "Department")]
        public Nullable<int> Department { get; set; }
        [Column(LookupList = "Position")]
        public Nullable<int> Position { get; set; }
        public string Email { get; set; }
        [Column(LookupList = "Nationality")]
        public Nullable<int> Nationality { get; set; }
        public string PrimaryPhone { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PersonalEmail { get; set; }
        [Column(Visible = false)]
        public int ReconId { get; set; }
        [Column(Visible = false)]
        public int MatchGroup { get; set; }
    }
}

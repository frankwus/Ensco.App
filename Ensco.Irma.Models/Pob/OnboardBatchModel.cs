using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class OnboardBatchModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int CrewChangeId { get; set; }
        [Column(Visible = false)]
        public int CrewFlightId { get; set; }

        [Column(Visible =false, LookupList ="Passport")]
        public Nullable<int> Name { get; set; }

        public string Passport { get; set; }
        public string DisplayName { get; set; }
        [Column(LookupList = "Position")]
        public Nullable<int> Position { get; set; }
        [Column(LookupList = "Company")]
        public Nullable<int> Company { get; set; }
        [Column(LookupList = "UserType")]
        public Nullable<int> CompanyType { get; set; }
        [Column(LookupList = "Nationality")]
        public Nullable<int> Nationality { get; set; }
        [Column(LookupList = "Tour")]
        public Nullable<int> Tour { get; set; }
        [Column(LookupList = "RigCrew")]
        public Nullable<int> Crew { get; set; }
        [Column(LookupList = "Room")]
        public Nullable<int> Room { get; set; }
        [Column(LookupList = "RoomBed")]
        public Nullable<int> Bed { get; set; }
        [Column(LookupList = "MusterStation")]
        public Nullable<int> MusterStation1 { get; set; }
        [Column(LookupList = "MusterStation")]
        public Nullable<int> MusterStation2 { get; set; }
        [Column(LookupList = "LifeBoat")]
        public Nullable<int> PrimaryLifeBoat { get; set; }
        [Column(LookupList = "LifeBoat")]
        public Nullable<int> SecondaryLifeBoat { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEstimatedLeave { get; set; }
        [Column(LookupList = "Passport")]
        public Nullable<int> Authentication { get; set; }
        public string Optional1 { get; set; }
        public string Optional2 { get; set; }
        public string Optional3 { get; set; }
        public string Optional4 { get; set; }
        [Column(Visible = false)]
        public int Status { get; set; }
        [Column(Visible =false)]
        public bool Selected { get; set; }
        public OnboardBatchModel()
        {
            DateStart = DateTime.Now;
            DateEstimatedLeave = DateTime.Now;
        }
    }
}

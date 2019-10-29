using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobBoardingModel
    {
        [Column(LookupList ="CrewChange")]
        public Nullable<int> CrewChange { get; set; }
        [Column(LookupList = "FlightManifest")]
        public Nullable<int> FlightManifest { get; set; }
        [Column(Visible = false)]
        public bool Crew { get; set; }
        [Column(Visible = false)]
        public List<OnboardIndividualPobModel> OnboardPersons { get; set; }
        public List<OffboardPobModel> OffboardPersons { get; set; }

        [Column(LookupList = "CrewPob", DisplayName = "Select Person from Crew Change")]
        public Nullable<int> PobCrewChange { get; set; }
        [Column(LookupList ="Passport", DisplayName = "From Database")]
        public Nullable<int> PobPassport { get; set; }
        public OnboardIndividualPobModel SelectedPob { get; set; }
        public SelectPobModel PobSelect { get; set; }

        public int CrewChangeId { get; set; }
        public int CrewFlightId { get; set; }
        public bool BatchOnboard { get; set; }
        public PobBoardingModel()
        {
            OnboardPersons = new List<OnboardIndividualPobModel>();
            OffboardPersons = new List<OffboardPobModel>();
            PobSelect = new SelectPobModel();
            SelectedPob = new OnboardIndividualPobModel();
            Crew = false;
            BatchOnboard = false;
        }
    }
}

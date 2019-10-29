using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobSummaryReportModel
    {
        public PobSummaryModel Summary { get; set; }
        public List<PobSummaryByCompanyTypeModel> SummaryByCompanyType { get; set; }
        public List<PersonnelOnOffBoardModel> ArrivalLog { get; set; }
        public List<PersonnelOnOffBoardModel> DepartureLog { get; set; }
        public List<PobCrewBreakdownModel> CrewBreakdown { get; set; }
        public List<LifeBoatComplianceReportModel> LifeboatBreakdown { get; set; }
        public int BedsAvailable { get; set; }
        public int Essential { get; set; }
        public int EssentialNo { get; set; }
        public int ShortService { get; set; }
        public string RigName { get; set; }
    }
}

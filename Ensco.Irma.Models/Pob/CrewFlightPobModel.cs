using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class CrewFlightPobModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        [Column(Visible =false)]
        public int RigId { get; set; }
        [Column(Visible = false)]
        public int CrewFlightId { get; set; }
        [Column(LookupList = "Passport", Visible = false)]
        public int PassportId { get; set; }
        [Column(LookupList = "PassportNotCrewOnboard", DisplayName = "Name", Visible = false)]
        public int PassportIdArriving { get; set; }
        [Column(LookupList = "PassportCrewOnboard", DisplayName = "Name", Visible = false)]
        public int PassportIdDeparting { get; set; }
        [Column(HyperLinkField = "PassportId", HyperLinkType = ColumnAttribute.HyperLinkFieldType.Passport)]
        public string Passport { get; set; }
        [Column(LookupList = "Position")]
        public int Position { get; set; }
        [Column(LookupList = "Company")]
        public int Company { get; set; }
        [Column(LookupList = "Nationality")]
        public int Nationality { get; set; }
        public double BodyWeight { get; set; }
        public double BagWeight { get; set; }
        public short Bags { get; set; }
        public string AirlineFlight { get; set; }
        public string Location { get; set; }
        public string Terminal { get; set; }
        public string HomeAirport { get; set; }
        [Column(DisplayName ="Time", DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        public System.DateTime ArrivalDepartureTime { get; set; }
        [Column(LookupList = "PobPlanningStatus")]
        public int Status { get; set; }
        
        public CrewFlightPobModel()
        {
        }
        public CrewFlightPobModel Copy()
        {
            return (CrewFlightPobModel)this.MemberwiseClone();
        }

        public void Assign(CrewFlightPobModel model)
        {
            PropertyInfo[] props = model.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object value = prop.GetValue(model);
                if (value != null)
                {
                    prop.SetValue(this, value);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ensco.Irma.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class POB_FlightPobViewArriving
    {
        public int CrewFlightId { get; set; }
        public int Id { get; set; }
        public int Name { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<int> Company { get; set; }
        public Nullable<int> Nationality { get; set; }
        public Nullable<double> BodyWeight { get; set; }
        public Nullable<double> BagWeight { get; set; }
        public Nullable<short> Bags { get; set; }
        public string HomeAirport { get; set; }
        public string AirlineFlight { get; set; }
        public string ArrivalLocation { get; set; }
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        public string ArrivalTerminal { get; set; }
        public Nullable<int> Status { get; set; }
    }
}

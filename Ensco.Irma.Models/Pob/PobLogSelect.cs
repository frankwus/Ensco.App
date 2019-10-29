using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobLogSelect
    {
        public enum LogType { Arrival, Departure, ArrivalDeparture };
        public LogType Type { get; set; }
        public int CrewChangeId { get; set; }
        public int FlightId { get; set; }
        public PobLogSelect(LogType type)
        {
            Type = type;
        }
    }
}

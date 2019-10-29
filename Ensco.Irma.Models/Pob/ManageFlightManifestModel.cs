using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class ManageFlightManifestModel
    {
        public DataTableModel FlightManifests { get; set; }
        public CrewFlightModel SelectedManifest { get; set; }
        public CrewFlightModel FlightParkingLot { get; set; }
        public static bool IsEditing { get; set; }
        public static bool IsNewRowEditing { get; set; }

        public static bool IsPobEditing { get; set; }
        public static bool IsNewRowPobEditing { get; set; }

        public bool IsOffboarding { get; set; }
        public ManageFlightManifestModel()
        {
            SelectedManifest = null;
            IsEditing = false;
            IsNewRowEditing = false;
            IsPobEditing = false;
            IsNewRowPobEditing = false;
        }
    }
}

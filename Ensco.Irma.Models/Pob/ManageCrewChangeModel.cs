using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class ManageCrewChangeModel
    {
        public DataTableModel CrewChanges { get; set; }
        public CrewChangeModel SelectedCrewChange { get; set; }
        public static CrewFlightModel SelectedFlight { get; set; }

        public static bool IsEditing { get; set; }
        public static bool IsNewRowEditing { get; set; }

        public static bool IsPobEditing { get; set; }
        public static bool IsNewRowPobEditing { get; set; }

        public ManageCrewChangeModel()
        {
            SelectedCrewChange = null;
            SelectedFlight = null;
            IsEditing = false;
            IsNewRowEditing = false;
            IsPobEditing = false;
            IsNewRowPobEditing = false;
        }

     }
}

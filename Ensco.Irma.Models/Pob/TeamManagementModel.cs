using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class TeamManagementModel
    {
        public DataTableModel Teams { get; set; }

        public TeamModel SelectedTeam { get; set; }
    }
}

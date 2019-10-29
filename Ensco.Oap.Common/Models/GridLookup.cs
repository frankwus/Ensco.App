using DevExpress.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridLookup : GridCombo
    {
        public GridLookup(GridLookupSettings gridLookupSettings)
        {
            GridLookupSettings = gridLookupSettings;
        }

        public GridData Data { get; set; }

        public bool ShowColumnHeader {get; set;}

        public bool EnableRowHotTrack { get; set; }

        public GridLookupSettings GridLookupSettings { get; }
    }
}
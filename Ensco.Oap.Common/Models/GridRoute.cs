using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridRoute
    {
        public GridRoute(GridRouteTypes type, object route)
        {
            Type = type;
            Route = route;
        }

        public GridRouteTypes Type { get; }

        public object Route { get; }
    }
}
using Ensco.Irma.Oap.Common.Models;
using System.Collections.Generic;

namespace Ensco.Irma.Oap.Common
{
    public static class CommonDataLists
    {
        public static GridCombo GetCriticalityCombo()
        {
            return new GridCombo()
            {
                Name = "OapCriticality",
                DataSource = new List<dynamic>
                {
                    new {Id = 1, Name = "1"},
                    new {Id = 2, Name = "2"},
                    new {Id = 3, Name = "3"}
                },
                KeyColumnName = "Id",
                DisplayColumnName = "Name",
                ValueColumnName = "Id"
            };
        }
    }
}

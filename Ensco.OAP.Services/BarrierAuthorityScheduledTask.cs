using Ensco.Irma.Models;
using Ensco.Irma.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Services
{
    public class BarrierAuthorityScheduledTask
    {


        public static void Test()
        {
            IEnumerable<RigPersonnelModel> rigPersonnel = IrmaServiceSystem.GetOnboardedByDate(DateTime.Parse("2018-11-29 14:31:00"));
        }

    }
}

using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Services
{
    public class JobLookupListData : ILookupList
    {
        public dynamic GetLookupList(string name)
        {
            try
            {
                JobConstants.LookupLists type = (JobConstants.LookupLists)Enum.Parse(typeof(JobConstants.LookupLists), name);

                return JobServiceSystem.GetLookupList(type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

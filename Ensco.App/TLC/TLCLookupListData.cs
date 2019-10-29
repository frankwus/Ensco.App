using Ensco.TLC.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.App.TLC
{
    public class TLCLookupListData : ILookupList
    {
        public dynamic GetLookupList(string name)
        {
            try
            {
                TlcConstants.TlcLookupLists type = (TlcConstants.TlcLookupLists)Enum.Parse(typeof(TlcConstants.TlcLookupLists), name);

                return TlcManageLookupLists.GetLookupList(type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }    
}

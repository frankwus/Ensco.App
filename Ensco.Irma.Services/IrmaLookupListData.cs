using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Services
{
    public class IrmaLookupListData : ILookupList
    {
        public dynamic GetLookupList(string name)
        {
            try
            {
                IrmaConstants.IrmaLookupLists type = (IrmaConstants.IrmaLookupLists)Enum.Parse(typeof(IrmaConstants.IrmaLookupLists), name);

                return IrmaServiceSystem.GetLookupList(type);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}

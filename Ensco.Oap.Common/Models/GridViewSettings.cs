using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridSettings
    {
        public string BeginCallBack { get; set; }

        public string EndCallBack { get; set; }

        public ASPxGridViewCommandButtonEventHandler CommandButtonInitializer {get; set;}
    }
}

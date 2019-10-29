using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.OAP.Services;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class MasterOversightController : GenericController
    {
        public MasterOversightController()
        {
            BaseController = "MasterOversight";

            RegisterClient(typeof(DPAClient));
        }

        public int Columns { get; set; } = 4;

        public override ActionResult List(Guid id)
        {
            ViewBag.Title = "Master Oversight Checklist";
            return base.List(id);
        }        
    }
}
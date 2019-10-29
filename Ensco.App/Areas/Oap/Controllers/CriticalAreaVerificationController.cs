using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class CriticalAreaVerificationController : GenericController
    {

        [SiteMapTitle("RigChecklistUniqueId")]
        public override ActionResult List(Guid id)
        {
            return base.List(id);
        }

    }
}
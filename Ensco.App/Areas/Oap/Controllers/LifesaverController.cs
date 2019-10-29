using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class LifesaverController : GenericController
    {
        public LifesaverController() : base()
        {
            BaseController = "Lifesaver";
        }

        [SiteMapTitle("RigChecklistUniqueId")]
        public override ActionResult List(Guid id)
        {
            ViewBag.Title = "Life Saver Checklist";
            return base.List(id);
        }
    }
}
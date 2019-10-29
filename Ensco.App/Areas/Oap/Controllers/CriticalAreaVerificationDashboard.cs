using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Rig;
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
    using System.Web.Routing;
    using System.Web;
    using Ensco.Models;
    using System.Linq;

    [Route("CAV")]
    public class CriticalAreaVerificationDashboardController : GenericDashboardController
    {
        public CriticalAreaVerificationDashboardController() : base()
        {
            GridDataChecklist.Title = "Open CAV Checklists";
            ChecklistType = "CAV";
            GridDataChecklist = new GridData(oapChecklistGridName, "CriticalAreaVerificationDashboard", "RigChecklists", "Open CAV", "AddNewRigChecklist", "Add Checklist", "search checklists", initializeCallBack: true);
        } 

        
        public override ActionResult Index()
        {
            var data = GetRigChecklistData(true);
            return View("CriticalAreaVerificationDashboard", data);
        }

        public override ActionResult RigChecklists()
        {
            var data = GetRigChecklistData(true);
            return PartialView("GenericDashboardPartial", data);
            
        }

        protected override IEnumerable<OapChecklist> GetChecklistData(bool useTypeSubTypeCode = false)
        {
            return base.GetChecklistData(true);
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            base.Configure(settings, html, viewContext);
        }   
    }
}
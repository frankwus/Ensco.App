using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Rig;
    using System.Collections.Generic;

    public class MasterOversightDashboardController : BarrierAuthorityDashboardController   {
        

        public MasterOversightDashboardController() : base()
        {
            ChecklistType = "RO";
            ChecklistSubType = "MO";
            GridDataChecklist = new GridData(oapChecklistGridName, 
                "MasterOversightDashboard", "RigChecklists", "Open Master Oversight Checklists", 
                "AddNewRigChecklist", "Add Checklist", "search checklists", initializeCallBack: true, editController: "MasterOversight");
            
        }
        
        public override ActionResult Index()
        {
            var data = GetRigChecklistData(true);
            return View("MasterOversightDashboard", data);
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
    }
}
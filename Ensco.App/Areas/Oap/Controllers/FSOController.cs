using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Ensco.App.Areas.Oap.Models;
using Ensco.App.Infrastructure;
using Ensco.Irma.Oap.Common.Extensions;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.WebClient.Rig;
using Westwind.Globalization;

namespace Ensco.App.Areas.Oap.Controllers
{

    public class FSOController : GenericController
    {

        public FSOController(): base()
        {
            BaseController = "FSO";
        }

        public override ActionResult List(Guid id)
        {
            RigChecklist = GetRigChecklist(id);
            ViewBag.Title = Translate("FSO Checklist");
            return View("GenericList", RigChecklist);

        }        

        protected override IEnumerable<KeyValueModel> GetYesNoNaValues()
        {   
            return new List<KeyValueModel>()
                {
                    new KeyValueModel("Safe", "Y", displayValue: "Safe".Translate()),
                    new KeyValueModel("Unsafe", "N", displayValue:  "Unsafe".Translate()),
                    new KeyValueModel("NA", "NA", displayValue: "N/A".Translate())
                };
        }

    }
}
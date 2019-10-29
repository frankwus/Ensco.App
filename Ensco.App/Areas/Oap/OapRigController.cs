namespace Ensco.Irma.Oap.Web.Oap.Controllers
{
    using Ensco.Irma.Oap.Common.Controllers;
    using Ensco.Irma.Oap.WebClient.Rig;
    using System;

    public abstract class OapRigController : UIBaseController
    {
        protected const string rigChecklistKey = "RigChecklist";
        protected const string rigChecklistHashCodeKey = "RigCheckListHashCode";

        protected WebClient.Rig.GetAllModel GetAllModels()
        {
            return new WebClient.Rig.GetAllModel() { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow };
        }

        protected override string ApiBaseUrlName { get; set; } = "RigWebApiUrl";

        protected RigOapChecklist RigChecklist
        {
            get
            {

                if (Session[rigChecklistKey] == null)
                {
                    var id = ControllerContext.RequestContext.RouteData.Values["Id"];
                    if (id != null)
                    {
                        Session[rigChecklistKey] = GetRigChecklist(Guid.Parse(id.ToString()));
                    }

                }

                return
                    Session[rigChecklistKey] as RigOapChecklist;

            }
            set
            {
                if (Session[rigChecklistKey] != null)
                {
                    Session.Remove(rigChecklistKey);
                } 

                if (value != null)
                {
                    Session[rigChecklistKey] = value;
                    RigChecklistOriginalHashCode = value.GetHashCode();
                }
            }
        }

        protected RigOapChecklist GetRigChecklist(Guid id)
        {
            return GetClient<RigOapChecklistClient>().GetCompleteChecklistAsync(id).Result?.Result?.Data;
        }

        protected int RigChecklistOriginalHashCode
        {
            get
            {
                var val = Session[rigChecklistHashCodeKey];
                if (val != null)
                {
                    return int.Parse(val.ToString());
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                Session[rigChecklistHashCodeKey] = value;
            }
        }
    }
}
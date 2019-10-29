namespace Ensco.Irma.Oap.Web.Oap.Controllers
{
    using Ensco.Irma.Oap.Common.Controllers;
    using Ensco.Irma.Oap.WebClient.Corp;
    using System;

    public abstract class OapCorpController : UIBaseController
    {
        protected const string protocolKey = "OapCorpProtocol";
        protected const string protocolHashCodeKey = "OapCorpProtocolHashCode";

        protected GetAllModel GetAllModelsCorp()
        {
            return new GetAllModel() { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow };
        }

        protected override string ApiBaseUrlName { get; set; } = "CorpWebApiUrl";

        protected RigOapChecklist Protocol
        {
            get
            {

                if (Session[protocolKey] == null)
                {
                    var id = ControllerContext.RequestContext.RouteData.Values["Id"];
                    if (id != null)
                    {
                        Session[protocolKey] = GetCompleteProtocol(Guid.Parse(id.ToString()));
                    }

                }

                return
                    Session[protocolKey] as RigOapChecklist;

            }
            set
            {
                if (Session[protocolKey] != null)
                {
                    Session.Remove(protocolKey);
                }

                if (value != null)
                {
                    Session[protocolKey] = value;
                    CorpProtocolOriginalHashCode = value.GetHashCode();
                }
            }
        }

        protected RigOapChecklist GetCompleteProtocol(Guid id)
        {
            return GetClient<OapAuditClient>().GetCompleteProtocolAsync(id).Result?.Result?.Data;
        }

        protected int CorpProtocolOriginalHashCode
        {
            get
            {
                var val = Session[protocolHashCodeKey];
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
                Session[protocolHashCodeKey] = value;
            }
        }
    }
}
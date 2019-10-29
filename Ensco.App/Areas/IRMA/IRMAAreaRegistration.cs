using System.Web.Mvc;

namespace Ensco.App.Areas.IRMA
{
    public class IRMAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "IRMA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "IRMA_default",
                "IRMA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
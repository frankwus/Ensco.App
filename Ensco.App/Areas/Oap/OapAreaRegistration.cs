using System.Web.Mvc;

namespace Ensco.App.Areas.Oap
{
    public class OapAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Oap";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Oap_default",
                "Oap/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
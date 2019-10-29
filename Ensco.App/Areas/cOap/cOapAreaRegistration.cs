using System.Web.Mvc;

namespace Ensco.App.Areas.cOap
{
    public class cOapAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "cOap";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "cOap_default",
                "cOap/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
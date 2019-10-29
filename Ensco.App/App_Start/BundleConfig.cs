using System.Web;
using System.Web.Optimization;

namespace Ensco.App
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui{version}.min.js",
                "~/Scripts/cldr.js",
                "~/Scripts/cldr/event.js",
                "~/Scripts/cldr/supplemental.js",
                "~/Scripts/cldr/unresolved.js",
                "~/Scripts/globalize.js",
                "~/Scripts/globalize/message.js",
                "~/Scripts/globalize/number.js",
                "~/Scripts/globalize/currency.js",
                "~/Scripts/globalize/date.js",
                "~/Scripts/globalize.js",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/enscoApp.js",
                "~/Scripts/ace.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.min.js",
            //          "~/Scripts/respond.min.js",
            //          "~/Scripts/metro.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/jquery-ui.min.css",
                      "~/Content/site.css",
                      "~/Content/oap.css"));

        }
    }
}

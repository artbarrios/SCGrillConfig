using System.Web;
using System.Web.Optimization;

namespace SCGrillConfig
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/flowchart/css").Include(
                      "~/Content/flowchart/jquery.flowchart.css",
                      "~/Content/flowchart/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/flowchart").Include(
                      "~/Scripts/jquery-ui-{version}.js",
                      "~/Scripts/flowchart/jquery.flowchart.min.js",
                      "~/Scripts/panzoom/jquery.panzoom.js",
                      "~/Scripts/panzoom/detect-browser.js"));

            bundles.Add(new StyleBundle("~/Content/datetimepicker/css").Include(
                      "~/Content/bootstrap-datetimepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js"));

        }
    }
}

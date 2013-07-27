using System.Web.Optimization;

namespace AS.ToolKit.Web.App_Start {
    public class BundleConfig {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datepicker.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootmetro").Include(
                "~/Scripts/bootmetro-panorama.js",
                "~/Scripts/bootmetro-pivot.js",
                "~/Scripts/bootmetro-charms.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                "~/Content/css/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/bootstrap-responsive.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootmetro").Include(
                "~/Content/css/bootmetro.css",
                "~/Content/css/bootmetro-responsive.css",
                "~/Content/css/bootmetro-icons.css",
                "~/Content/css/bootmetro-ui-light.css",
                "~/Content/css/datepicker.css"));
        }
    }
}
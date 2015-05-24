namespace Portal
{
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// This class defines a Bundle Config
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register bundle
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js",
                      "~/Scripts/nav.js",
                      "~/Scripts/hover_pack.js",
                      "~/Scripts/jquery-1.11.1.min.js",
                      "~/Scripts/jquery.etalage.min.js",
                      "~/Scripts/easyResponsiveTabs.js",
                      "~/Scripts/responsiveslides.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                       "~/Content/etalage.css",
                       "~/Content/style.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                     "~/Content/themes/base/all.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}

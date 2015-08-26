using System.Web;
using System.Web.Optimization;

namespace Blog
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

            bundles.Add(new ScriptBundle("~/bundles/uikit").Include(
                      "~/Scripts/uikit.js",
                      "~/Scripts/components/sticky.js",
                      "~/Scripts/syntaxhighlighter_3.0.83/scripts/shCore.js",
                      "~/Scripts/syntaxhighlighter_3.0.83/scripts/shBrushCSharp.js",
                      "~/Scripts/syntaxhighlighter_3.0.83/scripts/shBrushJScript.js",
                      "~/Scripts/syntaxhighlighter_3.0.83/scripts/shBrushSql.js",
                      "~/Scripts/syntaxhighlighter_3.0.83/scripts/shBrushXml.js",
                      "~/Scripts/syntaxhighlighter_3.0.83/scripts/shBrushPowerShell.js",
                      "~/Scripts/syntaxhighlighter_3.0.83/scripts/shBrushCss.js",
                      "~/Scripts/jquery.lazyload.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                      "~/ckeditor/ckeditor.js",
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/calendar.js",
                      "~/Scripts/jquery.ui.datepicker-cc-fa.js",
                      "~/Scripts/components/nestable.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                       "~/Content/jquery-ui-1.11.3.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/uikit.rtl.css",
                      "~/Content/site.css",
                       "~/Scripts/syntaxhighlighter_3.0.83/styles/shCore.css",
                       "~/Scripts/syntaxhighlighter_3.0.83/styles/shCoreDefault.css"));
        }
    }
}

using System.Web;
using System.Web.Optimization;
using System;
namespace Blog
{
    public class BundleConfig
    {
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            ignoreList = new IgnoreList();
            if (ignoreList == null)
                throw new ArgumentNullException(nameof(ignoreList));
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                         "~/Scripts/bootstrap.min.js",
                         "~/Scripts/jquery-2.2.0.min.js",
                          "~/Scripts/jquery-ui-1.11.4.min.js",
                          "~/Scripts/timetransfer.js",
                          "~/Scripts/Meterial/material.min.js",
                          "~/Scripts/Meterial/ripples.min.js"
                       ));
            bundles.Add(new ScriptBundle("~/bundles/DashboardJs").Include(
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/jquery-2.2.0.min.js"
                  ));
            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                       "~/Scripts/FileUpload/*.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/gallery").Include(
                       "~/Scripts/Gallery/jquery.touchSwipe.min.js"
                     ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/css/Home-half-slider.css",
                  "~/Content/css/fonts.css",
                  "~/Content/css/jquery-ui.min.css",
                  "~/Content/css/jquery-ui.structure.min.css",
                  "~/Content/css/jquery-ui.theme.min.css",
                  "~/Content/css/bootstrap.min.css",
                "~/Content/css/Meterial/ripples.min.css",
                "~/Content/css/Meterial/bootstrap-material-design.min.css",
                "~/Content/css/blog.css"
                ));
            bundles.Add(new StyleBundle("~/Content/css/admin").Include(
                "~/Content/css/page/login.css",
                "~/Content/css/page/signup.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css/dashboard").Include(
                "~/Content/css/page/dashboard.css",

              "~/Content/css/bootstrap.min.css",
                "~/Content/css/fonts.css"
                ));
            bundles.Add(new StyleBundle("~/Content/css/error").Include(
                "~/Content/css/page/error.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jquery/TextEditor").Include(
                "~/Scripts/TextEditor/ckeditor.js"
                ));
        }
    }
}
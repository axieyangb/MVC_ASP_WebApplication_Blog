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
                throw new ArgumentNullException("ignoreList");
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
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       
                         "~/Scripts/bootstrap.min.js",
                         "~/Scripts/jquery.js",
                         "~/Scripts/jquery-ui.js",
                          "~/Scripts/jquery-ui.min.js",
                          "~/Scripts/timetransfer.js"

                       ));

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                       "~/Scripts/FileUpload/*.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/gallery").Include(
                       "~/Scripts/Gallery/jquery.touchSwipe.min.js"
                     ));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css",
                "~/Content/css/blog.css",
                "~/Content/css/blog.min.css",
                "~/Content/css/bootstrap.css",
                "~/Content/css/bootstrap.min.css",
                 "~/Content/css/Home-half-slider.css",
                  "~/Content/css/fonts.css",
                  "~/Content/css/jquery-ui.min.css",
                  "~/Content/css/jquery-ui.css",
                  "~/Content/css/jquery-ui.structure.css",
                  "~/Content/css/jquery-ui.structure.min.css",
                  "~/Content/css/jquery-ui.theme.css",
                  "~/Content/css/jquery-ui.theme.min.css"

                ));
            bundles.Add(new StyleBundle("~/Content/css/admin").Include(
                "~/Content/css/page/login.css",
                "~/Content/css/page/signup.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css/dashboard").Include(
                "~/Content/css/page/dashboard.css",
              "~/Content/css/bootstrap.css",
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
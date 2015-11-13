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
                       "~/Scripts/bootstrap.js",
                         "~/Scripts/bootstrap.min.js",
                         "~/Scripts/jquery.js",
                         "~/Scripts/jquery.min.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css",
                "~/Content/css/blog.css",
                "~/Content/css/blog.min.css",
                "~/Content/css/bootstrap.css",
                "~/Content/css/bootstrap.min.css",
                  "~/Content/css/fonts.css"
                ));
            bundles.Add(new StyleBundle("~/Content/css/admin").Include(
                "~/Content/css/page/login.css",
                "~/Content/css/page/signup.css"
                 
                ));
            bundles.Add(new StyleBundle("~/Content/css/dashboard").Include(
                "~/Content/css/page/dashboard.css"
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
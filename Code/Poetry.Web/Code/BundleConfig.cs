using System.Web.Optimization;

namespace Poetry.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jplugin").Include(
                "~/scripts/jquery.min.js",
                "~/scripts/bootstrap.min.js",
                "~/scripts/layout.js",
                "~/scripts/jsviews.js",
                // "~/Scripts/datepicker/WdatePicker.js",
                "~/scripts/jplugin.js",
                "~/Scripts/index.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Scripts/select2/select2.js",
                "~/scripts/select2/select2_locale_zh-CN.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/frontjs").Include(
                "~/scripts/jquery.min.js",
                "~/scripts/bootstrap.min.js",
                "~/Scripts/popwin.js",
                "~/Scripts/scrollMove.js",
                "~/scripts/jsviews.js",
                "~/scripts/jplugin.js"
             ));


            bundles.Add(new StyleBundle("~/Content/css/frontcss").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/css.css"
            ));



            bundles.Add(new StyleBundle("~/Content/css/css").Include(
              "~/Content/css/bootstrap.css",
              "~/Content/css/font-awesome.min.css",
              "~/Content/css/components.css",
              "~/Content/css/layout.css",
              "~/Content/css/light.css",
              "~/Content/css/custom.css",
              "~/Content/css/plugins.css"
              ));

            bundles.Add(new StyleBundle("~/Content/css/wecss").Include(
                "~/Content/css/portry.css",
               "~/Content/css/zqui.css",
               "~/Content/css/grow.css",
               "~/Content/css/iconfont.css",
               "~/Content/css/poetry.css"
               ));

            bundles.Add(new ScriptBundle("~/bundles/wejs").Include(
               "~/scripts/jquery.min.js",
               "~/scripts/jsviews.js",
               "~/scripts/jplugin.js",
               //"~/scripts/hhSwipe.js",
               "~/Scripts/waterFall.js",
               "~/Scripts/common.js"
            ));

        }
    }
}
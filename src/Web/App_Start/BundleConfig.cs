using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                         "~/Scripts/bootstrap/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/bootstrap.css",
                      "~/adminlte/css/adminlte.min.css",
                      "~/adminlte/fontawesome-free/css/all.min.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                                  "~/Scripts/vue.js"));

            bundles.Add(new ScriptBundle("~/bundles/vuePd").Include(
                       "~/Scripts/vue.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/axios").Include(
                    "~/Scripts/axios/axios.js"));

            bundles.Add(new ScriptBundle("~/bundles/Apis").Include(
                    "~/Scripts/api/Apis.js"));
        }
    }
}

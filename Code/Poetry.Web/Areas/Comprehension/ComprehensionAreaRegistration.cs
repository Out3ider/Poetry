using System.Web.Mvc;

namespace Poetry.Web.Areas.Comprehension
{
    public class ComprehensionAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public override string AreaName => "Comprehension";

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Comprehension_default",
                "Comprehension/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Poetry.Web.Areas.Comprehension.Controllers" }
            );
        }
    }
}
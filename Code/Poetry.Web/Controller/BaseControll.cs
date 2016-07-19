namespace Poetry.Web.Controller
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    public abstract class BaseControll : System.Web.Mvc.Controller
    {
        /// <summary>
        /// 无名控制器
        /// </summary>
        /// <param name="actionName"></param>
        protected override void HandleUnknownAction(string actionName)
        {
            this.View(actionName).ExecuteResult(this.ControllerContext);
        }
    }
}
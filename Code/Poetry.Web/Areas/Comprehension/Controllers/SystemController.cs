
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poetry.Model;
using Poetry.Web.Controller;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web.Areas.Comprehension.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemController : BaseControll
    {
        private void CheckLogin()
        {
            PageHelper.CheckAdmin(HttpContext.Response, ProjectType.微感悟);
        }


        /// <summary>
        /// 无名控制器
        /// </summary>
        /// <param name="actionName"></param>
        protected override void HandleUnknownAction(string actionName)
        {
            CheckLogin();
            this.View(actionName).ExecuteResult(this.ControllerContext);
        }

        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
    }
}
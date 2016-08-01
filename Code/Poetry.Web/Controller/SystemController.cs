using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poetry.Model;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web.Controller
{
    public class SystemController : BaseControll
    {

        private void CheckLogin()
        {
            PageHelper.CheckAdmin(HttpContext.Response, ProjectType.发现身边先锋);
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

        public ActionResult Login()
        {
            return View();
        }
    }
}
using Sail.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sail.Common;
using Poetry.Model;

namespace Poetry.Web.Controller
{
    /// <summary>
    /// 微信端控制器
    /// </summary>
    public class WeChatController : BaseControll
    {
        //添加首页读取方法，可以使在开发过程中使用 “我要推荐”不需要注册。
        public ActionResult Index() {
            if (WebHelper.CurrentUser == null)
            {
                WebHelper.CurrentUser = new DataContext().GetModel<Admin>(x => x.LoginId == "admin");
            }
            return View();
        }
    }
}
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
    public class LeftMenu
    {
        /// <summary>
        /// 菜单项
        /// </summary>
        public IList<HMenuItem> Menus { set; get; }

        /// <summary>
        /// 首页地址
        /// </summary>
        public string IndexUrl { set; get; }
        /// <summary>
        /// 当前菜单id
        /// </summary>
        public string CurrentId { get; set; }
    }

    /// <summary>
    /// 通用帮助类
    /// </summary>
    public class UtilityController : System.Web.Mvc.Controller
    {
        /// <summary>
        /// 输出菜单页面
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Menu(string currentId, ProjectType type)
        {
            using (var db = new DataContext())
            {
                var index = WebHelper.GetRootUrl() + "system/index";
                switch (type)
                {
                    case ProjectType.微感悟:
                        index = WebHelper.GetRootUrl() + "Comprehension/system/index";
                        break;
                }
                return PartialView("Menu", new LeftMenu
                {
                    IndexUrl = index,
                    Menus = GroupController.LoadMenu(db, type),
                    CurrentId = currentId
                });
            }
        }

        /// <summary>
        /// 不同的用户组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Group(ProjectType type) => PartialView("_GroupInfo", type);

        /// <summary>
        /// 后台登录页
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult SysLogin(ProjectType type) => PartialView("_SystemLogin", type);

        /// <summary>
        /// 管理员
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult AdminInfo(ProjectType type) => PartialView("_AdminInfo", type);

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Member() => PartialView("_Member");
    }
}
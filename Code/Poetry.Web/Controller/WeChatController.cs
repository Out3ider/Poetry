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
    /// <summary>
    /// 微信端控制器
    /// </summary>
    public class WeChatController : BaseControll
    {
        private ActionResult NeedLogin(Func<Admin, ActionResult> act)
        {
            var user = WebHelper.CurrentUser as Admin;
            if (user.IsNull()) HttpContext.Response.Redirect("login");
            return act(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Rankinglist()
        {
            var sql = @"select DISTINCT t.*  , count(1) OVER (PARTITION  by t.Id)  as count 
from T_County t left JOIN T_News n on n.County = t.Id 
where n.IsChecked = 1 and n.IsPublish = 1
order by count DESC";

            var db = new DataContext();
            var rank = db.GetDataset(sql).Tables[0].ToDictionary().ToJson();

            var rankNews = db.GetPageList<News>(1, 5, Clip.Where<News>(x => x.IsChecked == true && x.IsPublish == true),
                Clip.OrderBy<News>(x => x.Liked.Desc())).Data.Cast<News>().ToList();
            ViewBag.Rank = rank;
            return View(rankNews);
        }

        public ActionResult Input()
        {
            return NeedLogin(user => View(user));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public class WechatController : BaseControll
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        private ActionResult NeedLogin(Antlr.Runtime.Misc.Func<Admin, ActionResult> act)
        {
            var user = WebHelper.CurrentUser as Admin;

            if (user == null) HttpContext.Response.Redirect("login");
            else
            {
                if (user.Icon.IsNull()) user.Icon = PageHelper.DefaultHeadIcon;
            }

            return act(user);
        }

        /// <summary>
        /// 微信首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (var db = new DataContext())
            {
                var sql = @"
select COUNT(1) from T_Senses where IsPublish=1
UNION ALL
select sum( Views) from T_Senses  where IsPublish =1
UNION ALL
select count(1) from T_UserPrize
UNION ALL
select sum( Liked) from T_Senses  where IsPublish =1

";
                var rows = db.GetDataset(sql).Tables[0].Rows.Cast<DataRow>().ToList();
                var model = Tuple.Create(rows[0][0], rows[1][0], rows[2][0], rows[3][0]);
                return View(model);
            }
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Center() => NeedLogin(user => View(user));

        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Modify() => NeedLogin(user => View(user));

        /// <summary>
        /// 发表感悟
        /// </summary>
        /// <returns></returns>
        public ActionResult Recommend() => NeedLogin(user =>
        {
            using (var db = new DataContext())
            {
                ViewBag.IsGet = !UserPrize.IsCanGet(db, user);
                return View(user);
            }

        });


        /// <summary>
        /// 直播列表页面（兼详情）
        /// </summary>
        /// <returns></returns>
        public ActionResult RaftingList()
        {
            ViewBag.CountyId = 0;
            using (var db = new DataContext())
            {
                var user = WebHelper.CurrentUser as Admin;
                if (user != null)
                {
                    user.Group = user.GetUserGroup(db, ProjectType.微感悟);
                    if (user.Group != null) ViewBag.CountyId = user.Group.County.Id;
                }
                var isAdmin = (user?.IsSuperAdmin ?? false) || (user?.Group != null);
                return View(isAdmin);
            } 
        }

        /// <summary>
        /// 登录退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            WebHelper.CurrentUser = null;
            return View("login");
        }

        /// <summary>
        /// 我的奖品
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPrize() => NeedLogin(user =>
        {
            var db = new DataContext();
            var rankNews = db.GetList<UserPrize>(Clip.Where<UserPrize>(x => x.PrizeUser.UserId == user.UserId), Clip.OrderBy<UserPrize>(x => x.CreateTime.Desc()));
            return View(rankNews);
        });


        /// <summary>
        /// 摇一摇
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Shake(int id) => NeedLogin(user =>
        {
            using (var db = new DataContext())
            {
                ViewBag.IsGet = db.GetCount<UserPrize>(x => x.SensesId == id) > 0;
                return View(user);
            }
        });

        /// <summary>
        /// 县区导航
        /// </summary>
        /// <returns></returns>
        public ActionResult CountyHead()
        {
            var sql = @"select u.County,count(1) 
from T_Senses s left JOIN T_Admin u on s.Creater = u.UserId
GROUP BY u.County";
            using (var db = new DataContext())
            {
                var counts = db.GetDataset(sql).Tables[0].Rows.Cast<DataRow>()
                       .Select(x => Tuple.Create(x[0].ToString().ToInt(), x[1].ToString().ToInt())).ToList();
                var model = db.GetList<County>().OrderBy(x => x.OrderByNo)
                    .Select(x => Tuple.Create(x.Id, x.Name, "", counts.Find(c => c.Item1 == x.Id)?.Item2 ?? 0)).ToList();

                model.Insert(0, Tuple.Create(0, "全部县区", "", counts.Sum(x => x.Item2)));

                return PartialView("_CountyList", model);
            }


        }

        /// <summary>
        /// 点赞排行榜县区导航
        /// </summary>
        /// <returns></returns>
        public ActionResult RankingCountyHead()
        {
            var sql = @"select u.County,sum(Liked) 
from T_Senses s left JOIN T_Admin u on s.Creater = u.UserId
GROUP BY u.County";
            using (var db = new DataContext())
            {
                var counts = db.GetDataset(sql).Tables[0].Rows.Cast<DataRow>()
                       .Select(x => Tuple.Create(x[0].ToString().ToInt(), x[1].ToString().ToInt())).ToList();
                var model = db.GetList<County>().OrderBy(x => x.OrderByNo)
                    .Select(x => Tuple.Create(x.Id, x.Name, "icon icon-hongxin-o", counts.Find(c => c.Item1 == x.Id)?.Item2 ?? 0)).ToList();

                model.Insert(0, Tuple.Create(0, "全部县区", "icon icon-hongxin-o", counts.Sum(x => x.Item2)));

                return PartialView("_CountyList", model);
            }


        }

    }
}

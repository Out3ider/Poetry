using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Poetry.Model;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    public class MemberNewsController : BaseController<News>
    {
        /// <summary>
        /// 排序
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<News>(x => x.CreateTime.Desc());


        private string getRandom(Random rnd, int max)
        {
            var i = rnd.Next(1, max);
            return VirtualPathUtility.ToAbsolute($"~/Content/demoPic/image_{i.ToString().PadLeft(3, '0')}.jpg");
        }


        /// <summary>
        /// 动态创建演示数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Demo()
        {
            return HandleHelper.TryAction(db =>
            {
                var countys = db.GetList<County>();
                var random = new Random();

                50.Times(i =>
                {
                    var title = VirtualPathUtility.ToAbsolute($"~/Content/demoPic/image_{(i + 1).ToString().PadLeft(3, '0')}.jpg");
                    var news = new News
                    {
                        County = countys[random.Next(1, countys.Count)],
                        Type = (OrgType)random.Next(1, 6),
                        Content = "抗洪抢险事迹" + i,
                        CreaterId = WebHelper.CurrentAdmin?.UserId,
                        CreaterName = WebHelper.CurrentAdmin?.UserName,
                        Referral = HumanName.GetName(),
                        PartyMember = HumanName.GetName(),
                        NewsDate = DateTime.Now,
                        Organization = "机构名称" + i,
                        Icon = title,
                        Images = $"{title},",
                        IsChecked = true,
                    };
                    2.Times(x => news.Images += getRandom(random, 90) + ",");
                    db.Save(news);
                });

            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="e"></param>
        /// <param name="o"></param>
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            var model = o as News;
            var user = WebHelper.CheckAdmin<Admin>();
            if (e == ControllerEvent.Put)
            {
                if (model?.Id == 0)
                {
                    model.CreaterId = user.UserId;
                    model.CreaterName = user.UserName;
                    model.IsChecked = true;
                    model.IsPublish = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip MakeWhere(string key)
        {
            Clip where = null;
            if (key.IsNotNull()) where &= Clip.Where<News>(x => x.PartyMember.Like(key) || x.Referral.Like(key) || x.Organization.Like(key) || x.Content.Like(key)).Bracket();
            return where;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="county"></param>
        /// <param name="checkeStatus"></param>
        /// <param name="publishStatus"></param>
        /// <returns></returns>
        public AjaxResult GetList(int pageIndex, int pageSize, string key, int type, int county, string checkeStatus, string publishStatus) => HandleHelper.TryAction(db =>
        {
            var where = MakeWhere(key);
            if (type > 0) where &= Clip.Where<News>(x => x.Type == (OrgType)type);
            if (county > 0) where &= Clip.Where<News>(x => x.County.Id == county);
            if (checkeStatus.IsNotNull()) where &= Clip.Where<News>(x => x.IsChecked == (checkeStatus == "True"));
            if (publishStatus.IsNotNull()) where &= Clip.Where<News>(x => x.IsPublish == (publishStatus == "True"));
            var result = db.GetPageList<News>(pageIndex, pageSize, where, OrderBy);
            var userIds = result.Data.Cast<News>().Select(x => x.CreaterId as object).Distinct().ToArray();
            var users = userIds.Length == 0 ? new List<Admin>() : db.GetList<Admin>(c => c.UserId.In(userIds));
            result.Data = result.Data.Cast<News>().Select(x =>
            {
                dynamic d = x.ToDynamic();
                d.ReferralPhone = x.CreaterId.IsNull() ? "" : users.Find(c => c.UserId == x.CreaterId)?.Phone ?? "";
                return d;
            }).ToList();
            return result;
        });

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult CheckPass([FromBody] string id) => ActToNews(id, model =>
        {
            model.IsChecked = true;
            model.IsPublish = Param.Default.IsAutoPublish;
        });

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Publish([FromBody] string id) => ActToNews(id, model => { model.IsPublish = true; });

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult UnPublish([FromBody] string id) => ActToNews(id, model => { model.IsPublish = false; });

        /// <summary>
        /// 对某实体操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static AjaxResult ActToNews(string id, Action<News> action) => HandleHelper.TryAction(db =>
        {
            var model = db.GetModelById<News>(id);
            action(model);
            db.Save(model);
        });
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Poetry.Model;
using Poetry.Web.Code;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    public class ContentController : ApiController
    {

        [HttpGet]
        public AjaxResult Get(string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var model = db.GetModelById<News>(id);
                var user = WebHelper.CurrentUser as Admin;

                if (model.IsNotNull())
                {
                    model.Views += 1;
                    db.Save(model);
                }
                var result = model.ToDynamic();
                var userId = user?.UserId;

                if (userId.IsNull())
                    result.isLike = false;
                else
                {
                    var like = db.GetModel<UserLike>(x => x.UserId == userId && x.NewsId == model.Id);
                    result.isLike = like != null;
                }

                result.ImageList = model.GetImages();
                result.Comments = model.GetComments(db);
                return result;

            });
        }


        /// <summary>
        /// 获取优秀事迹列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetList(int pageIndex, int pageSize)
        {
            return HandleHelper.TryAction(db =>
            {
                var where = Clip.Where<News>(x => x.IsChecked == true);
                var orderBy = Clip.OrderBy<News>(x => x.CreateTime.Desc());
                return db.GetPageList<News>(pageIndex, pageSize, where, orderBy);
            });
        }

         

        /// <summary>
        /// 前台查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="countyId"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Query(int pageIndex, int pageSize, int countyId, string sort)
        {
            var userId = (WebHelper.CurrentUser as Admin)?.UserId ?? new Guid().ToString("D");

            var cols = SqlHelper.Select<News, County>((t, c) => new
            {
                c.Name,
                t.CreateTime,
                t.Id,
                t.PartyMember,
                t.Organization,
                t.Icon,
                t.NewsDate,
                t.Type,
                t.Referral,
                t.Content,
                t.Comments,
                t.Views,
                t.Liked
            });
            var tableName = $" T_News t left JOIN T_County c on c.id = t.County left JOIN T_UserLike u on u.NewsId = t.id  and u.UserId = '{userId}'";

            var where = Clip.Where<News>(x => x.IsChecked == true && x.IsPublish == true);
            if (countyId > 0)
                where &= Clip.Where<County>(x => x.Id == countyId, "c.");

            var orderBy = Clip.OrderBy<News>(x => x.CreateTime.Desc(), "");
            if (sort.IsNotNull()) orderBy = sort.ToField("").Desc;

            var sql = SqlStatement .Select(cols,
                "count(1) over () as counts".ToField(""),
                "case  when  u.id is null then 0 else 1 end as isLike".ToField(""))
                .From(tableName).Where(where)
                .OrderBy(orderBy);

            return HandleHelper.TryAction(db =>
            {
                var datatable = db.GetPageDatatableByCustom(pageIndex, pageSize, sql);
                var result = new PageResult(pageIndex, pageSize)
                {
                    Count = datatable.Rows.Count == 0 ? 0 : (int)datatable.Rows[0]["counts"],
                    Data = datatable.ToDictionary()
                };
                return result;
            });
        }



        /// <summary>
        /// 用户赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Like([FromBody]string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var model = db.GetModelById<News>(id);
                if (model == null) return 0;
                model.Liked++;
                db.RunTran(t =>
                {
                    var user = WebHelper.CurrentUser as Admin;
                    if (user == null) throw new SailCommonException("点赞之前必须登录");
                    try
                    {
                        db.Save(new UserLike { NewsId = model.Id, UserId = user.UserId });
                        db.Save(model);
                    }
                    catch (SailDbUniquenessException)
                    {
                        throw new SailCommonException("您已经点过赞了");
                    }
                });

                return model.Liked;
            });
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conent"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Comment(string id, [FromBody] string conent)
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CheckUser<Admin>();
                var news = db.GetModelById<News>(id);
                if (news.IsNull()) throw new SailCommonException("无效事迹");
                var model = new NewsComment
                {
                    Content = conent,
                    CreaterId = user.UserId,
                    CreaterName = user.UserName,
                    IsChecked = Param.Default.IsCommentNeedCheck,
                    NewsId = news.Id
                };
                db.RunTran(t =>
                {
                    db.Save(model);
                    model.UpdateNews(db);
                });
                model = db.GetModelById<NewsComment>(model.Id);
                return model;

            });
        }
    }
}
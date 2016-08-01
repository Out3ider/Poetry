using System.Linq;
using System.Web.Http;
using Poetry.Model;
using Poetry.Web.Utility;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    /// <summary>
    /// 微信端微感悟
    /// </summary>
    public class WeChatSensesController : BaseController<Senses>
    {
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            var model = o as Senses;
            var user = WebHelper.CheckUser<Admin>();
            switch (e)
            {
                case ControllerEvent.Put:
                    model?.Init(user);
                    break;
                case ControllerEvent.Delete:
                    model?.DeleteInfos(db);
                    break;
            }

        }

        [HttpGet]
        public AjaxResult Demo()
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CurrentAdmin as Admin;
                100.Times(x =>
                {

                    var model = new Senses()
                    {
                        SensesTitle = HumanName.GetName() + "发了啥点感悟",
                        Content = HumanName.GetName(),
                        Creater = user,
                        CreaterId = user?.UserId,
                        CreaterName = user?.UserName,
                        IsPublish = true,
                        IsChecked = true
                    };
                    db.Save(model);
                });

            });
        }

        protected override Clip OrderBy => Clip.OrderBy<Senses>(x => x.CreateTime.Desc());
        [HttpGet]
        public AjaxResult MySenses(int pageIndex, int pageSize)
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CheckUser<Admin>();
                var where = Clip.Where<Senses>(x => x.Creater.UserId == user.UserId);
                return db.GetPageList<Senses>(pageIndex, pageSize, where, OrderBy);
            });

        }

        [HttpGet]
        public override AjaxResult Get(string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var model = db.GetModelById<Senses>(id);
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
                    var like = db.GetModel<ReaderLike>(x => x.UserId == userId && x.SensesId == model.Id);
                    result.isLike = like != null;
                }

                //result.ImageList = model.GetImages();
                result.CommentList = model.GetComments(db);
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
                var model = db.GetModelById<Senses>(id);
                if (model == null) return 0;
                model.Liked++;
                db.RunTran(t =>
                {
                    var user = WebHelper.CurrentUser as Admin;
                    if (user == null) throw new SailCommonException("点赞之前必须登录");
                    try
                    {
                        db.Save(new ReaderLike() { SensesId = model.Id, UserId = user.UserId });
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
                var news = db.GetModelById<Senses>(id);
                if (news.IsNull()) throw new SailCommonException("无效感悟");
                var model = new SensesComment
                {
                    Content = conent,
                    Creater = user,
                    CreaterId = user.UserId,
                    CreaterName = user.UserName,
                    IsChecked = ComprehensionParam.Default.IsCommentNeedCheck,
                    SensesId = news.Id
                };
                if(BadWords.CheckBadWords(model.Content)) throw  new SailCommonException("评论内容含有敏感词");
                db.RunTran(t =>
                { 
                    db.Save(model);
                    model.UpdateSenses(db);
                });
                model = db.GetModelById<SensesComment>(model.Id);
                return model;

            });
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public AjaxResult DeleteCommon([FromBody] string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CheckUser<Admin>();
                var comm = db.GetModelById<SensesComment>(id);
                if (comm.IsNull()) throw new SailCommonException("无效评论");
                db.RunTran(t =>
                {
                    var model = db.GetModelById<Senses>(comm.SensesId);
                    if (model.IsNull()) throw new SailCommonException("无效感悟");
                    db.DeleteById<SensesComment>(id);
                    comm.UpdateSenses(db);
                });
            });
        }

        /// <summary>
        /// 滚动直播
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="countyId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult List(int pageIndex, int pageSize, int countyId, string key = "")
        {
            return HandleHelper.TryAction(db =>
            {
                var where = Clip.Where<Senses>(x => x.IsPublish == true);
                if (countyId > 0) where &= "County".ToField("Creater.") == countyId;
                if (key.IsNotNull())
                    where &= Clip.Where<Senses>(x => x.SensesTitle.Like(key) || x.Creater.UserName.Like(key)).Bracket();

                return db.GetPageList<Senses>(pageIndex, pageSize, where, OrderBy);
            });
        }

        /// <summary>
        /// 精华帖
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="countyId"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult ListStick(int pageIndex, int pageSize, int countyId)
        {
            return HandleHelper.TryAction(db =>
            {
                var where = Clip.Where<Senses>(x => x.IsPublish == true);

                where &= Clip.Where<Senses>(x => x.IsStick == true);

                if (countyId > 0) where &= "County".ToField("Creater.") == countyId;

                var orderby = Clip.OrderBy<Senses>(x => x.IsStick.Desc() && x.OrderByNo.Desc() && x.CreateTime.Desc());
                return db.GetPageList<Senses>(pageIndex, pageSize, where, orderby);
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="countyId"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Ranking(int pageIndex, int pageSize, int countyId)
        {
            return HandleHelper.TryAction(db =>
            {
                var where = Clip.Where<Senses>(x => x.IsChecked == true && x.IsPublish == true);
                if (countyId > 0) where &= "County".ToField("Creater.") == countyId;
                return db.GetPageList<Senses>(pageIndex, pageSize, where,
                    Clip.OrderBy<Senses>(x => x.Liked.Desc()));
            });

        }
    }
}
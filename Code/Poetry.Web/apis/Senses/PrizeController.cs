using System;
using System.Collections.Generic;
using Poetry.Model;
using Sail.Common;
using System.Linq;
using System.Web.Http;
using Poetry.Web.Utility;
using Sail.Web;

namespace Poetry.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class PrizeController : BaseController<Prize>
    {
        /// <summary>
        /// 排序
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<Prize>(x => x.Operator.Asc() && x.Probability.Asc());

        /// <summary>创建默认where条件，可以被覆盖</summary>
        /// <param name="key">The key.</param>
        /// <returns>Clip.</returns>
        protected override Clip MakeWhere(string key)
        {
            Clip where = null;
            if (key.IsNotNull()) where &= Clip.Where<Prize>(x => x.Title.Like(key));
            return where;
        }

        /// <summary>
        /// 后台获取奖品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AjaxResult GetList(int pageIndex, int pageSize, string key, int type)
        {

            return HandleHelper.TryAction(db =>
            {
                var where = MakeWhere(key);
                if (type >= 0) where &= Clip.Where<Prize>(x => x.Operator == (MobileOperator)type);
                return db.GetPageList<Prize>(pageIndex, pageSize, where, OrderBy);
            });
        }

        /// <summary>
        /// 用户奖品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult UserPrizeList(int pageIndex, int pageSize, int type)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = null;
                if (type >= 0) where &= Clip.Where<UserPrize>(x => x.Prize.Operator == (MobileOperator)type);
                return db.GetPageList<UserPrize>(pageIndex, pageSize, where,
                    Clip.Where<UserPrize>(x => x.CreateTime.Desc()));
            });
        }

        private static Tuple<double, int> Get(List<Tuple<double, int>> src)
        {
            var rnd = new Random();
            src = src.OrderByDescending(x => x.Item1).ToList();
            var result = src.First();
            var total = (int)(src.Sum(x => x.Item1) * 1024);
            var randomNumber = (float)rnd.Next(0, total) / 1024;
            for (var i = 0; i < src.Count; i++)
            {
                var item = src[i];
                var pre = src.Take(i).Sum(x => x.Item1);
                var next = src.Take(i + 1).Sum(x => x.Item1);
                if (randomNumber >= pre && randomNumber < next)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 随机生成一条感悟奖品
        /// </summary>
        /// <param name="db"></param>
        /// <param name="senseId"></param>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static UserPrize GetRandom(DataContext db, int senseId, Admin user, ProjectType type)
        {
            var radio = 1000;
            var userId = user.UserId;
            var @operator = user.MobileOperator;


            var list = $"Prizess_{type}_{@operator}".GetCache(() => db.GetList<Prize>(x => x.ProjectType == type && x.Operator == @operator));

            var result = list.Where(x => x.CurrentStock < x.TotalStock || x.TotalStock == 0).ToList();


            var userlist = db.GetList<UserPrize>(x => x.PrizeUser.UserId == userId && x.Prize.ProjectType == type)
                .GroupBy(x => x.Prize.Id)
                .Select(x => new { PrizeId = x.Key, Count = x.Count() }).ToList();

            if (userlist.Sum(x => x.Count) >= ComprehensionParam.Default.MaxPrize)
                throw new SailCommonException("对不起，您没有抽取到任何奖品");

            userlist.ForEach(u =>
            {
                result.RemoveAll(x => x.MaxNumber > 0 && u.Count >= x.MaxNumber && x.Id == u.PrizeId);
            });

            if (result.Count == 0) throw new SailCommonException("对不起，您没有抽取到任何奖品");

            var prizes = result.Select(x => Tuple.Create((double)x.Probability / radio, x.Id)).ToList();
            var countProbability = result.Sum(x => x.Probability);
            if (countProbability < radio)
            {
                prizes.Add(Tuple.Create((double)(radio - countProbability) / radio, 0));
            }

            var src = Get(prizes);
            var model = list.Find(x => x.Id == src.Item2);
            if (model == null || model.Id == 0) throw new SailCommonException("对不起，您没有抽取到任何奖品");
            var userpiz = new UserPrize
            {
                Prize = model,
                PrizeUser = user,
                SensesId = senseId
            };
            db.RunTran(t =>
            {
                db.Save(userpiz);
                model.CurrentStock++;
                db.Save(model);
            });
            return userpiz;
        }

        /// <summary>
        /// 抽奖了
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Lottery([FromBody]int id)
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CheckUser<Admin>();
                if (db.GetCount<UserPrize>(x => x.SensesId == id) > 0) throw new SailCommonException("本次感悟已抽过奖了");

                var senses = db.GetModelById<Senses>(id);

                if (senses.Creater.UserId != user.UserId) throw new SailCommonException("无效链接，无法抽奖");


                return GetRandom(db, id, user, ProjectType.微感悟);
            });
        }

        [HttpGet]
        public AjaxResult Demo(string text)
        {
            return HandleHelper.TryAction(db =>
            {
                return BadWords.CheckBadWords(text);
            });
        }
    }
}
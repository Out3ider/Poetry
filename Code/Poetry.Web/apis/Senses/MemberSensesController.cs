using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using Poetry.Model;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    public class MemberSensesController : BaseController<Senses>
    {
        /// <summary>
        /// 排序
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<Senses>(x => x.CreateTime.Desc());



        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="e"></param>
        /// <param name="o"></param>
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            var model = o as Senses;
            var user = WebHelper.CheckAdmin<Admin>();
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip MakeWhere(string key)
        {
            Clip where = null;
            if (key.IsNotNull()) where &= Clip.Where<Senses>(x => x.SensesTitle.Like(key) || x.Creater.UserName.Like(key)).Bracket();
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
        /// <param name="stickStatus"></param>
        /// <param name="publishStatus"></param>
        /// <returns></returns>
        public AjaxResult GetList(int pageIndex, int pageSize, string key, int county, string stickStatus, string publishStatus) => HandleHelper.TryAction(db =>
      {
          var where = MakeWhere(key);
          if (county > 0) where &= "County".ToField("Creater.") == county;
          if (stickStatus.IsNotNull()) where &= Clip.Where<Senses>(x => x.IsStick == (stickStatus == "True"));
          if (publishStatus.IsNotNull()) where &= Clip.Where<Senses>(x => x.IsPublish == (publishStatus == "True"));
          var orderby = Clip.OrderBy<Senses>(x => x.IsStick.Desc() && x.OrderByNo.Desc() && x.CreateTime.Desc());
          var result = db.GetPageList<Senses>(pageIndex, pageSize, where, orderby);
          result.Data = result.Data.Cast<Senses>().Select(x =>
          {
              dynamic d = x.ToDynamic();
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
        /// 精华
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult SetStick([FromBody] string id) => ActToNews(id, model => model.IsStick = true);

        /// <summary>
        /// 取消精华
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult UnStick([FromBody] string id) => ActToNews(id, model => model.IsStick = false);

        /// <summary>
        /// 对某实体操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static AjaxResult ActToNews(string id, Action<Senses> action) => HandleHelper.TryAction(db =>
        {
            var model = db.GetModelById<Senses>(id);
            action(model);
            db.Save(model);
        });
    }
}
using System.Web.Http;
using Poetry.Model;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    public class SenseCommentController : BaseController<SensesComment>
    {
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            base.BeforeAction(db, e, o);
            var model = o as SensesComment;
            if (e == ControllerEvent.Put)
            {
                var user = WebHelper.CheckUser<Admin>();
                if (model?.Id == 0)
                {
                    model.CreaterId = user.UserId;
                    model.CreaterName = user.UserName;
                }
            }
        }

        protected override Clip MakeWhere(string key)
        {
            return key.IsNull() ? null : Clip.Where<SensesComment>(x => x.CreaterName.Like(key) || x.Content.Like(key)).Bracket();
        }

        protected override Clip OrderBy => Clip.OrderBy<SensesComment>(x => x.CreateTime.Desc());


        [HttpGet]
        public AjaxResult GetList(int pageIndex, int pageSize, string key, string status)
        {

            return HandleHelper.TryAction(db =>
            {
                var where = MakeWhere(key);
                if (status.IsNotNull())
                {
                    var isChecked = status == "True";
                    where &= Clip.Where<SensesComment>(x => x.IsChecked == isChecked);
                }
                return db.GetPageList<SensesComment>(pageIndex, pageSize, where, OrderBy);
            });

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult CheckPass([FromBody] string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var model = db.GetModelById<SensesComment>(id);
                model.IsChecked = true;
                db.RunTran(x =>
                {
                    db.Save(model);
                    model.UpdateSenses(db);
                });


            });
        }

        [HttpDelete]
        public override AjaxResult Delete([FromBody]string id)
        {
            return HandleHelper.TryAction(db =>
            {
                db.RunTran(x =>
                {
                    var model = db.GetModelById<SensesComment>(id);
                    db.DeleteById<SensesComment>(id);
                    model.UpdateSenses(db);
                });


            });
        }
    }
}
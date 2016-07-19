using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Ivony.Logs;
using Poetry.Model;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    /// <summary>
    /// 超级管理员
    /// </summary>
    public class AdminController : BaseController<Admin>
    {
        /// <summary>
        /// 操作前
        /// </summary>
        /// <param name="db"></param>
        /// <param name="e"></param>
        /// <param name="o"></param> 
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            base.BeforeAction(db, e, o);
            if (e == ControllerEvent.Put)
            {
                var model = o as Admin;
                if (model != null && model.UserId.IsNull())
                {
                    model.Type = UserType.系统管理员;
                    model.Password = Admin.DefaultPassword;
                }
            }
        }



        protected override Clip MakeWhere(string key)
        {
            var where = Clip.Where<Admin>(x => x.LoginId != "admin" && x.Type == UserType.系统管理员);
            if (key.IsNotNull()) where &= Clip.Where<Admin>(x => x.UserName.Like(key));
            return where;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Login([FromBody] string str)
        {
            return HandleHelper.TryAction(db =>
            {
                var value = DynamicJson.Parse(str);
                var result = Common.Login<Admin>(db, value.LoginId.Trim(), value.Password.Trim());
                if (result.Status == Common.LoginStatus.成功)
                {

                    WebHelper.CurrentAdmin = result.User;
                    var user = WebHelper.CurrentAdmin as Admin;
                    if (user.Type != UserType.系统管理员) throw new SailCommonException("无效用户");
                    return result.User;
                }

                throw new SailCommonException(result.Status.ToString());
            });
        }


        [HttpPost]
        public AjaxResult ChangePwd([FromBody] string value) => HandleHelper.TryAction(db =>
        {
            var cpd = DynamicJson.Parse(value);
            var user = WebHelper.CurrentAdmin;
            if (user.IsNull()) throw new SailCommonException("请先登录");
            if (cpd.NewPwd != cpd.RptPwd) throw new SailCommonException("新密码与重复密码不一致");
            if (cpd.NewPwd == Admin.DefaultPassword) throw new SailCommonException("不能使用默认密码");
            Common.ChangePwd<Admin>(db, user.LoginId, cpd.OldPwd, cpd.NewPwd, false);
        });


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Logout() => HandleHelper.TryAction(() =>
        {
            WebHelper.CurrentAdmin = null;
        });

        public AjaxResult Get() => HandleHelper.TryAction(() => WebHelper.CurrentAdmin);




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IList<HMenuItem> LoadMenu()
        {
            var user = WebHelper.CheckAdmin<Admin>();
            var menutext = FileHelper.ReadTextFile("~/config/menu.json".FullFileName());
            var list = menutext.JsonToObj<List<HMenuItem>>().ToList();
            HMenuItem.Filter(list, "", user);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Admin CheckLogin()
        {
            using (var db = new DataContext())
            {
#if DEBUG
                if (WebHelper.CurrentAdmin.IsNull())
                    WebHelper.CurrentAdmin = db.GetModel<Admin>(Clip.Where<Admin>(x => x.LoginId == "admin"));
#endif
                if (WebHelper.CurrentAdmin.IsNull())
                    HttpContext.Current.Response.Redirect("~/system/login");

                var user = WebHelper.CheckAdmin<Admin>();
                return user;
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult ResetPwd([FromBody]string id)
        {
            return HandleHelper.TryAction(db =>
            {
                WebHelper.CheckAdmin<Admin>();
                var user = db.GetModelById<Admin>(id);
                user.ResetPassword(db);
            });
        }
    }

}

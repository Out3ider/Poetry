using System.Web.Http;
using Poetry.Model;
using Poetry.Web.Utility;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    /// <summary>
    /// 微信注册用户
    /// </summary>
    public class WxUserController : BaseController<Admin>
    {
        protected override Clip MakeWhere(string key)
        {
            var where = Clip.Where<Admin>(x => x.Type == UserType.普通用户);
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

                    WebHelper.CurrentUser = result.User;
                    var user = WebHelper.CurrentUser as Admin;
                    return result.User;
                }

                throw new SailCommonException(result.Status.ToString());
            });
        }

        [HttpPost]
        public AjaxResult ChangePwd([FromBody] string value) => HandleHelper.TryAction(db =>
        {
            var cpd = DynamicJson.Parse(value);
            var user = WebHelper.CurrentUser;
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
            WebHelper.CurrentUser = null;
        });

        [HttpPost]
        public AjaxResult Register([FromBody]string value) => HandleHelper.TryAction(db =>
        {
            WebHelper.CurrentUser = null;
            var user = db.LoadModalByJson<Admin>(value, "");
            if (db.GetCount<Admin>(Clip.Where<Admin>(x => x.LoginId == user.Phone)) > 0)
            {
                throw new SailCommonException("手机号已被使用，无法注册");
            }
            string code = DynamicJson.Parse(value).Code;
            if (!VerifyCode.CheckCode(user.Phone, (int)VerifyType.注册用户, code)) throw new SailCommonException("验证码错误");

            db.RunTran(t =>
            {
                BeforeAction(db, ControllerEvent.Put, user);
                user.LoginId = user.Phone;
                user.Type = UserType.普通用户;

                db.Save(user);
                AfterAction(db, ControllerEvent.Put, user);
            });
            WebHelper.CurrentUser = user;
        });


        /// <summary>
        /// 忘记密码验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult ForgetPwdCode(string mobile) => HandleHelper.TryAction(db =>
        {
            var count = db.GetCount<Admin>(Clip.Where<Admin>(x => x.LoginId == mobile));
            if (count == 0) throw new SailCommonException("该号码未注册,不能修改密码");
            var code = VerifyCode.GetVerifycodes(mobile, (int)VerifyType.找回密码);
            code.PushSms(db);
            return code;
        });

        /// <summary>
        /// 注册校验码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult RegCode(string mobile) => HandleHelper.TryAction(db =>
        {
            var count = db.GetCount<Admin>(Clip.Where<Admin>(x => x.LoginId == mobile));
            if (count > 0) throw new SailCommonException("该号码已被使用，不能注册");
            var code = VerifyCode.GetVerifycodes(mobile, (int)VerifyType.注册用户);
            code.PushSms(db);
            return code;
        });


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


        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult ForgetPwd([FromBody]string value)
        {
            return HandleHelper.TryAction(db =>
            {
                var json = DynamicJson.Parse(value);
                var phone = (string)json.Phone;
                var pwd = json.NewPwd;
                var code = (string)json.Code;
                if (!VerifyCode.CheckCode(phone, (int)VerifyType.找回密码, code)) throw new SailCommonException("无效验证码");
                var user = db.GetModel<Admin>(Clip.Where<Admin>(x => x.LoginId == phone));
                if (user.IsNull()) throw new SailCommonException("该号码未注册,不能修改密码");
                user.Password = pwd;
                db.Save(user);
                return user;
            });
        }


    }
}
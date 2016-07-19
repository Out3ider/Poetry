using System.Web.Http;
using Poetry.Model;
using Poetry.Web.Utility;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    /// <summary>
    /// ΢��ע���û�
    /// </summary>
    public class WxUserController : BaseController<Admin>
    {
        protected override Clip MakeWhere(string key)
        {
            var where = Clip.Where<Admin>(x => x.Type == UserType.��ͨ�û�);
            if (key.IsNotNull()) where &= Clip.Where<Admin>(x => x.UserName.Like(key));
            return where;
        }


        /// <summary>
        /// ��¼
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
                if (result.Status == Common.LoginStatus.�ɹ�)
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
            if (user.IsNull()) throw new SailCommonException("���ȵ�¼");
            if (cpd.NewPwd != cpd.RptPwd) throw new SailCommonException("���������ظ����벻һ��");
            if (cpd.NewPwd == Admin.DefaultPassword) throw new SailCommonException("����ʹ��Ĭ������");
            Common.ChangePwd<Admin>(db, user.LoginId, cpd.OldPwd, cpd.NewPwd, false);
        });


        /// <summary>
        /// �˳���¼
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
                throw new SailCommonException("�ֻ����ѱ�ʹ�ã��޷�ע��");
            }
            string code = DynamicJson.Parse(value).Code;
            if (!VerifyCode.CheckCode(user.Phone, (int)VerifyType.ע���û�, code)) throw new SailCommonException("��֤�����");

            db.RunTran(t =>
            {
                BeforeAction(db, ControllerEvent.Put, user);
                user.LoginId = user.Phone;
                user.Type = UserType.��ͨ�û�;

                db.Save(user);
                AfterAction(db, ControllerEvent.Put, user);
            });
            WebHelper.CurrentUser = user;
        });


        /// <summary>
        /// ����������֤��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult ForgetPwdCode(string mobile) => HandleHelper.TryAction(db =>
        {
            var count = db.GetCount<Admin>(Clip.Where<Admin>(x => x.LoginId == mobile));
            if (count == 0) throw new SailCommonException("�ú���δע��,�����޸�����");
            var code = VerifyCode.GetVerifycodes(mobile, (int)VerifyType.�һ�����);
            code.PushSms(db);
            return code;
        });

        /// <summary>
        /// ע��У����
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult RegCode(string mobile) => HandleHelper.TryAction(db =>
        {
            var count = db.GetCount<Admin>(Clip.Where<Admin>(x => x.LoginId == mobile));
            if (count > 0) throw new SailCommonException("�ú����ѱ�ʹ�ã�����ע��");
            var code = VerifyCode.GetVerifycodes(mobile, (int)VerifyType.ע���û�);
            code.PushSms(db);
            return code;
        });


        /// <summary>
        /// ��������
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
        /// ��������
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
                if (!VerifyCode.CheckCode(phone, (int)VerifyType.�һ�����, code)) throw new SailCommonException("��Ч��֤��");
                var user = db.GetModel<Admin>(Clip.Where<Admin>(x => x.LoginId == phone));
                if (user.IsNull()) throw new SailCommonException("�ú���δע��,�����޸�����");
                user.Password = pwd;
                db.Save(user);
                return user;
            });
        }


    }
}
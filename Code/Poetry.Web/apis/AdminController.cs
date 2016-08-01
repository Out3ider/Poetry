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
        public override AjaxResult GetList(int pageIndex, int pageSize)
        {
            throw new SailCommonException("禁用");
        }

        public override AjaxResult GetList(int pageIndex, int pageSize, string key)
        {
            throw new SailCommonException("禁用");
        }

        [HttpDelete]
        public override AjaxResult Delete([FromBody]string id)
        {
            return HandleHelper.TryAction(db =>
            {
                db.DeleteById<UserGroup>(id);
            });
        }

        /// <summary>
        /// 保存管理员信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public override AjaxResult Put(string id, [FromBody] string value) => HandleHelper.TryAction(db =>
        {
            var model = db.LoadModalByJson<UserGroup>(value, id);
            db.RunTran(t =>
            {
                db.Save(model);
                var where = nameof(UserGroup.Admin).ToField() == model.Admin.UserId &&
                            Clip.Where<UserGroup>(x => x.Type == model.Type);
                var count = db.GetCount<UserGroup>(where);
                if (count > 1)
                    throw new SailCommonException("此用户已经是管理员");
            });
            return model;
        });

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
                var type = (ProjectType) (int.Parse(value.Type.ToString()));
                var result = Common.Login<Admin>(db, value.LoginId.Trim(), value.Password.Trim());

                if (result.Status == Common.LoginStatus.成功)
                {
                    var user = result.User as Admin;
                    if (!user.IsSuperAdmin)
                    {
                        var group = user.GetUserGroup(db, type);
                        if (group == null) throw new SailCommonException("无效管理员");
                        user.Group = group;
                    }
                    WebHelper.CurrentAdmin = user;
                    return result.User;
                }
                throw new SailCommonException(result.Status.ToString());
            });
        }



        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AjaxResult GetList(int pageIndex, int pageSize, ProjectType type)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = Clip.Where<UserGroup>(x => x.Type == type);
                //if (key.IsNotNull())
                //    where &= Clip.Where<UserGroup>(x => x.Admin.UserName.Like(key) || x.Admin.LoginId.Like(key)).Bracket();
                return db.GetPageList<UserGroup>(pageIndex, pageSize, where,
                    Clip.OrderBy<UserGroup>(x => x.UserRole.Desc()));
            });
        }






        /// <summary>
        /// 管理员退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Logout() => HandleHelper.TryAction(() =>
        {
            WebHelper.CurrentAdmin = null;
        });



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IList<HMenuItem> LoadMenu(string menuName)
        {
            var user = WebHelper.CheckAdmin<Admin>();
            var menutext = FileHelper.ReadTextFile($"~/config/{menuName}.json".FullFileName());
            var list = menutext.JsonToObj<List<HMenuItem>>().ToList();
            HMenuItem.Filter(list, "", user);
            return list;
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
                var usergroup = db.GetModelById<UserGroup>(id);
                var user = usergroup.Admin;
                user.ResetPassword(db);
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
    }

}

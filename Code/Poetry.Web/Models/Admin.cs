using System;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    [HTable]
    public class Admin : IModel, IUser
    {
        public static string DefaultPassword => "123456";

        /// <summary>
        /// 用户ID
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string UserId { set; get; }

        /// <summary>
        /// 登录名
        /// </summary>
        [HColumn(Length = 50)]
        public string LoginId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [HColumn(Length = 50)]
        public string UserName { set; get; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [HColumn(Length = 2000)]
        public string Password { get; set; }

        /// <summary>
        /// 运营商
        /// </summary>
        [HColumn]
        public MobileOperator MobileOperator { set; get; }


        /// <summary>
        /// 移动运营商
        /// </summary>
        public string MobileOperatorStr => MobileOperator.ToString();


        public UserGroup Group { set; get; }

        /// <summary>
        /// 县区
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public County County { set; get; }



        /// <summary>
        /// 备注
        /// </summary>
        [HColumn(Length = 2000)]
        public string Memo { set; get; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string Phone { set; get; }



        /// <summary>
        /// 用户头像
        /// </summary>
        [HColumn(Length = 200)]
        public string Icon { set; get; }


        /// <summary>
        /// 所在单位或居住地
        /// </summary>
        [HColumn(Length = 50)]
        [Form(IsRequired = true)]
        public string OrgName { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }


        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="powerid"></param>
        /// <returns></returns>
        public bool IsHasPower(string powerid)
        {
            if (Group.IsNull()) return false;
            if (Group?.UserRole.RoleId == 1) return true;
            return Group?.UserRole.Powers?.Select(x => x.Key).Contains(powerid) ?? false;
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuperAdmin => LoginId == "admin";

        /// <summary>
        /// 重置密码为默认密码
        /// </summary>
        /// <param name="db"></param>
        public void ResetPassword(IDataContext db)
        {
            Password = DefaultPassword;
            db.Save(this);
        }


        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="db"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public UserGroup GetUserGroup(IDataContext db, ProjectType type)
        {
            return db.GetModel<UserGroup>(x => x.Admin.UserId == UserId && x.Type == type);
        }

    }
}
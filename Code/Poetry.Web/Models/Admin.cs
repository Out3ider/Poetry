using System.Collections.Generic;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{


    /// <summary>
    /// 系统管理员
    /// </summary>
    [HTable]
    public class Admin : Member
    {
        public static string DefaultPassword => "123456";

        /// <summary>
        /// 用户类型
        /// </summary>
        [HColumn]
        public UserType Type { set; get; }


        [HColumn(ColumnName = "C_Group")]
        public Group Group { set; get; }

        /// <summary>
        /// 关联县区
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

        


        public override bool IsHasPower(string powerid)
        {
            if (Group.IsNull()) return false;
            if (Group.RoleId == 1) return true;
            return Group?.Powers?.Select(x => x.Key).Contains(powerid) ?? false;
        }

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
        /// 搜索用户
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="countyId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public PageResult SearchUser(IDataContext db, int pageIndex, int pageSize, int countyId, string key)
        {
            Clip where = Clip.Where<Admin>(x => x.Type == UserType.普通用户);
            if (key.IsNotNull()) where &= Clip.Where<Admin>(x => x.LoginId.Like(key) || x.Phone.Like(key) || x.UserName.Like(key)).Bracket();
            return db.GetPageList<Admin>(pageIndex, pageSize, where, Clip.OrderBy<Admin>(x => x.UserName.Asc()));
        }

    }
}
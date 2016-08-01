using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 用户组和用户关联表
    /// </summary>
    public class UserGroup : IModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string Id { set; get; }

        /// <summary>
        ///  用户组
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        [ModelData(Key = "prop", Value = "RoleId")]

        public Group UserRole { set; get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        [HColumn]
        [ModelData(Key = "id", Value = "UserId")]
        [ModelData(Key = "name", Value = "UserName")]
        public Admin Admin { set; get; }

        /// <summary>
        /// 所属县区
        /// </summary>
        [HColumn]
        [ModelData(Key = "prop", Value = "Id")]
        public County County { set; get; }

        /// <summary>
        /// 项目类型
        /// </summary>
        [HColumn]
        public ProjectType Type { set; get; }
    }
}
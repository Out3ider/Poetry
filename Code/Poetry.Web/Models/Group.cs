using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 用户组
    /// </summary>
    [HTable]
    public class Group : Role
    {
        /// <summary>
        /// 管理员类型
        /// </summary>
        [HColumn]
        public ProjectType ProjectType { set; get; }


    }
}
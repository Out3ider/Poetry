using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 新闻类型
    /// </summary>
    public class NewsType : DictBase
    {
        /// <summary>
        /// 是否禁用
        /// </summary>
        [HColumn]
        public bool IsDisabled { set; get; }
    }
}
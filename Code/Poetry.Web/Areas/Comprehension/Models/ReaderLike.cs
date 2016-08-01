using System;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 用户赞记录
    /// </summary>
    public class ReaderLike : IModel
    {
        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int Id { set; get; }

        /// <summary>
        /// 优秀事迹id
        /// </summary>
        [HColumn(IsNotNeedUniqueness = true)]
        public int SensesId { set; get; }

        /// <summary>
        /// 用户id
        /// </summary>
        [HColumn(IsUniqueness = true, IsGuid = true)]
        public string UserId { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }
    }
}
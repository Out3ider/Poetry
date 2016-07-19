using System;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// �û��޼�¼
    /// </summary>
    public class UserLike : IModel
    {
        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int Id { set; get; }

        /// <summary>
        /// �����¼�id
        /// </summary>
        [HColumn(IsNotNeedUniqueness = true)]
        public int NewsId { set; get; }

        /// <summary>
        /// �û�id
        /// </summary>
        [HColumn(IsUniqueness = true, IsGuid = true)]
        public string UserId { set; get; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }
    }
}
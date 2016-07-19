using Sail.Common;

namespace Poetry.Model
{
    public abstract class DictBase : IDict
    {
        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int Id { set; get; }

        /// <summary>
        /// 县区名称
        /// </summary>
        [HColumn(Length = 50, IsUniqueness = true, Remark = "名称")]
        public string Name { set; get; }

        /// <summary>
        /// 排序号
        /// </summary>
        [HColumn(Length = 14, Precision = 2)]
        public decimal OrderByNo { set; get; }
    }
}
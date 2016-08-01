using System;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 奖品
    /// </summary>
    public class Prize : IModel
    {

        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int Id { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 项目类型
        /// </summary>
        [HColumn]
        public ProjectType ProjectType { set; get; }

        /// <summary>
        /// 运营商
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public MobileOperator Operator { set; get; }

        /// <summary>
        /// 运营商
        /// </summary>
        public string OperatorStr => Operator.ToString();

        /// <summary>
        /// 描述
        /// </summary>
        [HColumn(Length = 500)]
        public string Description { set; get; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string Title { set; get; }

        /// <summary>
        /// 随机几率(千分几率)
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, CustomValidate = "pinteger", Tips = "抽取出现的几率，0-1000之间,例如1%的几率就填10，10%的几率就填100，若所有奖品加起来几率低于1000，则可能会轮空，抽取不到任何奖品")]
        public int Probability { set; get; }

        /// <summary>
        /// 总数量
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, CustomValidate = "pinteger", Tips = "所有人最多可抽取总数")]
        public int TotalStock { set; get; }

        /// <summary>
        /// 每人上限数量
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, CustomValidate = "pinteger", Tips = "每人最多可抽取总数")]
        public int MaxNumber { set; get; }

        /// <summary>
        /// 已抽取数量
        /// </summary>
        [HColumn]

        public int CurrentStock { set; get; }
    }
}
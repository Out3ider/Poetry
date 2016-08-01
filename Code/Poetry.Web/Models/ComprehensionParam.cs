using System;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ComprehensionParam : BaseConfig<ComprehensionParam>, IModel
    {
        /// <summary>
        /// 参数id
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string ParamId { set; get; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string SiteName { set; get; }

        /// <summary>
        /// 评论无需审核
        /// </summary>
        [HColumn]
        [Form(Tips = "")]
        public bool IsCommentNeedCheck { set; get; }

        /// <summary>
        /// 感悟自动发布
        /// </summary>
        [HColumn]
        [Form]
        public bool IsAutoPublish { set; get; }

        /// <summary>
        /// 每人最多领取次数
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, CustomValidate = "pinteger")]
        public int MaxPrize { set; get; }

        /// <summary>
        /// 最大参与抽奖人数
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, CustomValidate = "pinteger")]
        public int MaxPrizeUser { set; get; }

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static ComprehensionParam Get(IDataContext db) => db.GetModel<ComprehensionParam>(x => x.ParamId != new Guid().ToString());

    }
}
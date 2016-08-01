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
        /// ����id
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string ParamId { set; get; }

        /// <summary>
        /// ϵͳ����
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string SiteName { set; get; }

        /// <summary>
        /// �����������
        /// </summary>
        [HColumn]
        [Form(Tips = "")]
        public bool IsCommentNeedCheck { set; get; }

        /// <summary>
        /// �����Զ�����
        /// </summary>
        [HColumn]
        [Form]
        public bool IsAutoPublish { set; get; }

        /// <summary>
        /// ÿ�������ȡ����
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, CustomValidate = "pinteger")]
        public int MaxPrize { set; get; }

        /// <summary>
        /// ������齱����
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, CustomValidate = "pinteger")]
        public int MaxPrizeUser { set; get; }

        /// <summary>
        /// ��ȡϵͳ����
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static ComprehensionParam Get(IDataContext db) => db.GetModel<ComprehensionParam>(x => x.ParamId != new Guid().ToString());

    }
}
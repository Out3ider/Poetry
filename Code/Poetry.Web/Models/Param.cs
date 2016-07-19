using System;
using Sail.Common;

namespace Poetry.Model
{
    public class Param : BaseConfig<Param>, IModel
    {
        /// <summary>
        /// ����id
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string ParamId { set; get; }

        /// <summary>
        /// ����
        /// </summary>
        [HColumn(Length = 200)]
        [Form]
        public string Host { set; get; }

        /// <summary>
        /// ��վ����
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string SiteName { set; get; }

        /// <summary>
        /// app id
        /// </summary>
        [HColumn(Length = 200)]
        public string AppId { set; get; }

        /// <summary>
        /// app secret
        /// </summary>
        [HColumn(Length = 200)]
        public string Secret { set; get; }


        /// <summary>
        /// ����ƽ̨��ַ
        /// </summary>
        [HColumn(Length = 50)]
        [Form(IsRequired = true, Tips = "����ƽ̨�ķ�������ַ �磺127.9.0.1")]
        public string SmsServer { get; set; }

        /// <summary>
        /// ���ŷ���˿�
        /// </summary>
        [HColumn(Length = 5)]
        [Form(IsRequired = true, CustomValidate = "pinteger", Tips = "����ƽ̨�Ķ˿ں� ��:63333")]
        public string SmsPort { set; get; }


        /// <summary>
        /// �����������
        /// </summary>
        [HColumn]
        [Form(Tips = "")]
        public bool IsCommentNeedCheck { set; get; }

        /// <summary>
        /// ��˺��Զ�����
        /// </summary>
        [HColumn]
        [Form]
        public bool IsAutoPublish { set; get; }

        /// <summary>
        /// ��ȡϵͳ����
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Param Get(IDataContext db) => db.GetModel<Param>(Clip.Where<Param>(x => x.ParamId != new Guid().ToString()));


    }
}
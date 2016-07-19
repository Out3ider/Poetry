using System;
using Sail.Common;

namespace Poetry.Model
{
    public class Param : BaseConfig<Param>, IModel
    {
        /// <summary>
        /// 参数id
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string ParamId { set; get; }

        /// <summary>
        /// 域名
        /// </summary>
        [HColumn(Length = 200)]
        [Form]
        public string Host { set; get; }

        /// <summary>
        /// 网站名称
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
        /// 短信平台地址
        /// </summary>
        [HColumn(Length = 50)]
        [Form(IsRequired = true, Tips = "短信平台的服务器地址 如：127.9.0.1")]
        public string SmsServer { get; set; }

        /// <summary>
        /// 短信服务端口
        /// </summary>
        [HColumn(Length = 5)]
        [Form(IsRequired = true, CustomValidate = "pinteger", Tips = "短信平台的端口号 如:63333")]
        public string SmsPort { set; get; }


        /// <summary>
        /// 评论无需审核
        /// </summary>
        [HColumn]
        [Form(Tips = "")]
        public bool IsCommentNeedCheck { set; get; }

        /// <summary>
        /// 审核后自动发布
        /// </summary>
        [HColumn]
        [Form]
        public bool IsAutoPublish { set; get; }

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Param Get(IDataContext db) => db.GetModel<Param>(Clip.Where<Param>(x => x.ParamId != new Guid().ToString()));


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Poetry.Web.Utility;
using Sail.Common;
using Sail.Web;

namespace Poetry.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Senses : NewsBase
    {

        /// <summary>
        /// 感悟标题
        /// </summary>
        [HColumn]
        public string SensesTitle { set; get; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        [HColumn]
        public Admin Creater { set; get; }

        /// <summary>
        /// 是否精华
        /// </summary>
        [HColumn]
        public bool IsStick { set; get; }

        /// <summary>
        /// 排序号
        /// </summary>
        [HColumn(Length = 14, Precision = 2)]
        [Form(CustomValidate = "pdecimal", Tips = "感悟排序按照从大到小的顺序")]
        public decimal OrderByNo { set; get; }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IList<SensesComment> GetComments(DataContext db) =>
            db.GetList<SensesComment>(x => x.IsChecked == true && x.SensesId == this.Id, x => x.CreateTime.Desc());

        /// <summary>
        /// 删除相关信息
        /// </summary>
        /// <param name="db"></param>
        public void DeleteInfos(IDataContext db)
        {
            var id = this.Id;
            db.Delete<UserPrize>(Clip.Where<UserPrize>(x => x.SensesId == id));
            db.Delete<ReaderLike>(Clip.Where<ReaderLike>(x => x.SensesId == id));
            db.Delete<SensesComment>(Clip.Where<SensesComment>(x => x.SensesId == id));
        }

        /// <summary>
        /// 初始化发布人姓名什么
        /// </summary>
        /// <param name="user"></param>
        public void Init(Admin user)
        {
            var model = this;
            if (model.Id == 0)
            {
                model.CreaterId = user.UserId;
                model.CreaterName = user.UserName;
                model.Creater = user;
                model.IsChecked = true;
                model.IsPublish = ComprehensionParam.Default.IsAutoPublish; ;
            }
            if (BadWords.CheckBadWords(model.SensesTitle)) throw new SailCommonException("标题含有敏感词");
            if (BadWords.CheckBadWords(model.Content)) throw new SailCommonException("内容含有敏感词");
            
        }
    }
}
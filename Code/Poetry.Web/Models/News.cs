using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Sail.Common;
using Sail.Web;

namespace Poetry.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class News : UserInfoBase
    { 
        /// <summary>
        /// 标题图片
        /// </summary>
        [HColumn(Length = 200)]
        public string Icon { set; get; }

        /// <summary>
        /// 党员姓名
        /// </summary>
        [HColumn(Length = 50)]
        public string PartyMember { set; get; }

        /// <summary>
        /// 发生时间
        /// </summary>
        [HColumn]
        public DateTime NewsDate { set; get; }

        /// <summary>
        /// 引用
        /// </summary>
        [HColumn(Length = 50)]
        public string Referral { set; get; }

        /// <summary>
        /// 图片
        /// </summary>
        [HColumn]
        public string Images { set; get; }

        /// <summary>
        /// 图片列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetImages()
        {
            return Images?.Split(',').Where(x => x.IsNotNull()).Select(x => x).ToList();
        }

        /// <summary>
        /// 县区id
        /// </summary>
        [HColumn]
        public County County { set; get; }

        /// <summary>
        /// 类型
        /// </summary>
        [HColumn]
        public OrgType Type { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeStr => Type.ToString(); 
        /// <summary>
        /// 基层党组织
        /// </summary>
        [HColumn(Length = 50)]
        public string Organization { set; get; } 
        /// <summary>
        /// 是否审核
        /// </summary>
        [HColumn]
        public bool IsChecked { set; get; } 
        /// <summary>
        /// 点赞
        /// </summary>
        [HColumn]
        public int Liked { set; get; }

        /// <summary>
        /// 查看次数
        /// </summary>
        [HColumn]
        public int Views { set; get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [HColumn]
        public bool IsPublish { set; get; }  
        /// <summary>
        /// 评论数
        /// </summary>
        [HColumn]
        public int Comments { set; get; }

        /// <summary>
        /// 事迹内容
        /// </summary>
        [HColumn]
        public string Content { set; get; }

        /// <summary>
        /// where条件
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static Clip MakeWhere(int year, int month, string prefix = "t.")
        {
            var where = Clip.Where<News>(x => x.IsChecked == true, prefix);
            if (year > 0 && month == 0) where &= "CreateTime".DateBetween(new DateTime(year, 1, 1).ToDateString(),
                    new DateTime(year, 12, 31).ToDateString(), prefix);
            else if ((year > 0 && month > 0)) where &= "CreateTime".DateBetween(new DateTime(year, month, 1).ToDateString(),
                    new DateTime(year, month, 1).GetLastDayOfMonth().ToDateString(), prefix);
            return where;
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IList<NewsComment> GetComments(DataContext db) => db.GetList<NewsComment>(x => x.IsChecked == true && x.NewsId == this.Id);
    }
}
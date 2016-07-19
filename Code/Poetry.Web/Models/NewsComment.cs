using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 事迹评论
    /// </summary>
    public class NewsComment : UserInfoBase
    {
        /// <summary>
        /// 关联的新闻Id
        /// </summary>
        [HColumn]
        public int NewsId { set; get; }

        /// <summary>
        /// 回答内容
        /// </summary>
        [HColumn(Length = 500)]
        public string Content { set; get; }

        /// <summary>
        /// 是否审核通过
        /// </summary>
        [HColumn]
        public bool IsChecked { set; get; }

        /// <summary>
        /// 更新新闻的评论数
        /// </summary>
        /// <param name="db"></param>
        public void UpdateNews(IDataContext db)
        {
            var sql = $@"update t  set Comments = (SELECT count(1) from T_NewsComment c where c.NewsId = t.Id)  from T_News t  where t.id ={{0}}";
            db.ExecuteSql(sql, NewsId);
        }
    }
}
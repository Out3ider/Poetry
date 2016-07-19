using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// �¼�����
    /// </summary>
    public class NewsComment : UserInfoBase
    {
        /// <summary>
        /// ����������Id
        /// </summary>
        [HColumn]
        public int NewsId { set; get; }

        /// <summary>
        /// �ش�����
        /// </summary>
        [HColumn(Length = 500)]
        public string Content { set; get; }

        /// <summary>
        /// �Ƿ����ͨ��
        /// </summary>
        [HColumn]
        public bool IsChecked { set; get; }

        /// <summary>
        /// �������ŵ�������
        /// </summary>
        /// <param name="db"></param>
        public void UpdateNews(IDataContext db)
        {
            var sql = $@"update t  set Comments = (SELECT count(1) from T_NewsComment c where c.NewsId = t.Id)  from T_News t  where t.id ={{0}}";
            db.ExecuteSql(sql, NewsId);
        }
    }
}
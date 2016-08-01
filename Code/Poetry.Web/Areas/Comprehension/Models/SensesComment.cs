using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 感悟评论
    /// </summary>
    public class SensesComment : UserInfoBase
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [HColumn]
        public Admin Creater { set; get; }
        /// <summary>
        /// 关联的感悟Id
        /// </summary>
        [HColumn]
        public int SensesId { set; get; }

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
        /// 更新感悟的评论数
        /// </summary>
        /// <param name="db"></param>
        public void UpdateSenses(IDataContext db)
        {
            var sql = $@"update t  set Comments = (SELECT count(1) from T_SensesComment c where c.SensesId = t.Id)  from T_Senses t  where t.id ={{0}}";
            db.ExecuteSql(sql, SensesId);
        }
    }
}
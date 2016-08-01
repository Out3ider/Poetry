using Sail.Common;
using System;

namespace Poetry.Model
{
    /// <summary>
    /// 用户中奖信息
    /// </summary>
    public class UserPrize : IModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int Id { set; get; }

        /// <summary>
        /// 获取人
        /// </summary>
        [HColumn]
        public Admin PrizeUser { set; get; }

        /// <summary>
        /// 奖品
        /// </summary>
        [HColumn]
        public Prize Prize { set; get; }

        /// <summary>
        /// 关联的感悟Id
        /// </summary>
        [HColumn]
        public int SensesId { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }


        /// <summary>
        /// 能否抽奖
        /// </summary>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsCanGet(IDataContext db, IUser user)
        {

            var isUserGet = db.GetCount<UserPrize>(nameof(UserPrize.PrizeUser).ToField() == user.UserId) <
                               ComprehensionParam.Default.MaxPrize;//用户单人抽奖上限

            var isMaxGet = db.GetCount<UserPrize>() < ComprehensionParam.Default.MaxPrizeUser; //活动总抽奖上限

            return isUserGet && isMaxGet;
        }
    }
}
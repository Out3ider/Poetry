using System.Collections.Generic;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 检查手机号的
    /// </summary>
    public class MobileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<MobileOperator, List<string>> Mobile = new Dictionary<MobileOperator, List<string>>
        {
            [MobileOperator.移动] = new List<string> { "134", "135", "136", "137", "138", "139", "141", "142", "147", "148", "150", "151", "152", "154", "157", "158", "159", "178", "182", "183", "184", "187", "188", "1705", },
            [MobileOperator.联通] = new List<string> { "130", "131", "132", "144", "145", "155", "156", "176", "185", "186", "1707", "1708", "1709", },
            [MobileOperator.电信] = new List<string> { "133", "153", "173", "177", "180", "181", "189", "1700" },
        };

        /// <summary>
        /// 检验运营商
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static MobileOperator Check(string phone)
        {
            if (phone.Length != 11) throw new SailCommonException("无效手机号码");
            if (Mobile[MobileOperator.电信].Any(phone.StartsWith)) return MobileOperator.电信;
            if (Mobile[MobileOperator.联通].Any(phone.StartsWith)) return MobileOperator.联通;
            if (Mobile[MobileOperator.移动].Any(phone.StartsWith)) return MobileOperator.移动;
            return MobileOperator.其他;
        }
    }
}
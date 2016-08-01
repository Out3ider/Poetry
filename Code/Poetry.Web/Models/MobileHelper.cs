using System.Collections.Generic;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// ����ֻ��ŵ�
    /// </summary>
    public class MobileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<MobileOperator, List<string>> Mobile = new Dictionary<MobileOperator, List<string>>
        {
            [MobileOperator.�ƶ�] = new List<string> { "134", "135", "136", "137", "138", "139", "141", "142", "147", "148", "150", "151", "152", "154", "157", "158", "159", "178", "182", "183", "184", "187", "188", "1705", },
            [MobileOperator.��ͨ] = new List<string> { "130", "131", "132", "144", "145", "155", "156", "176", "185", "186", "1707", "1708", "1709", },
            [MobileOperator.����] = new List<string> { "133", "153", "173", "177", "180", "181", "189", "1700" },
        };

        /// <summary>
        /// ������Ӫ��
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static MobileOperator Check(string phone)
        {
            if (phone.Length != 11) throw new SailCommonException("��Ч�ֻ�����");
            if (Mobile[MobileOperator.����].Any(phone.StartsWith)) return MobileOperator.����;
            if (Mobile[MobileOperator.��ͨ].Any(phone.StartsWith)) return MobileOperator.��ͨ;
            if (Mobile[MobileOperator.�ƶ�].Any(phone.StartsWith)) return MobileOperator.�ƶ�;
            return MobileOperator.����;
        }
    }
}
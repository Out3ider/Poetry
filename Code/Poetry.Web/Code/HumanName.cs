using System;
using System.Text;

namespace Poetry.Web
{
    public class HumanName
    {
        private static readonly string[] Surname = @"
��,Ǯ,��,��,��,��,֣,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,ʩ,��,��,��,��,��,��,κ,��,��,
��,л,��,��,��,ˮ,�,��,��,��,��,��,��,��,��,��,³,Τ,��,��,��,��,��,��,��,��,Ԭ,��,��,��,ʷ,��,
��,��,�,Ѧ,��,��,��,��,��,��,��,��,��,��,��,��,��,��,ʱ,��,Ƥ,��,��,��,��,��,Ԫ,��,��,��,ƽ,��,
��,��,��,��,Ҧ,��,տ,��,��,ë,��,��,��,��,��,�,��,��,��,��,̸,��,é,��,��,��,��,��,��,ף,��,��,
��,��,��,��,ϯ,��,��,ǿ,��,·,¦,Σ,��,ͯ,��,��,÷,ʢ,��,��,��,��,��,��,��,��,��,��,��,��,��,��,
��,��,֧,��,��,��,¬,Ī,��,��,��,��,��,��,Ӧ,��,��,��,��,��,��,��,��,��,��,��,��,ʯ,��,��,ť,��,
��,��,��,��,��,½,��,��,��,��,�,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,
��,��,ɽ,��,��,��,�,��,ȫ,ۭ,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,ղ,��,��,
Ҷ,��,˾,��,۬,��,��,��,ӡ,��,��,��,��,ۢ,��,��,��,��,��,��,׿,��,��,��,��,��,��,��,��,��,��,˫,
��,ݷ,��,��,̷,��,��,��,��,��,��,��,Ƚ,��,۪,Ӻ,�S,�,ɣ,��,�,ţ,��,ͨ,��,��,��,��,ۣ,��,��,ũ,
��,��,ׯ,��,��,��,��,��,Ľ,��,��,ϰ,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,
��,��,��,��,��,»,��,��,ŷ,�,��,��,ε,Խ,��,¡,ʦ,��,��,��,��,��,��,��,��,��,��,��,��,��,��,��,
��,��,ɳ,ؿ,��,��,��,��,��,��,��,��,��,��,��,��,��,��,Ȩ,��,��,��,��,��,
��ٹ,˾��,�Ϲ�,ŷ��,�ĺ�,���,����,����,����,�ʸ�,ξ��,����,�̨,��ұ,����,���,����,����,̫��,
����,����,����,��ԯ,���,����,����,����,Ľ��,˾ͽ,˾��".Replace("\r\n", "").Split(',');

        private static readonly Random Rnd = new Random((int)DateTime.Now.ToFileTimeUtc());

        private static string RandomName()
        {
            string str = "";
            int count = Rnd.Next(1, 3);
            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    int aCode = 1601 + Rnd.Next(999);
                    string strtemp = Encoding.Default.GetString(new[] { (byte)(aCode / 100 + 160), (byte)(aCode % 100 + 160) });
                    if (strtemp != "?")
                    {
                        str += strtemp;
                        break;
                    }
                }

            }
            return str;
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <returns></returns>
        public static string GetName()
        {
            var name = Surname[Rnd.Next(0, Surname.Length)];
            name += RandomName();
            return name;
        }
    }
}
using System;
using System.Text;

namespace Poetry.Web
{
    public class HumanName
    {
        private static readonly string[] Surname = @"
赵,钱,孙,李,周,吴,郑,王,冯,陈,褚,卫,蒋,沈,韩,杨,朱,秦,尤,许,何,吕,施,张,孔,曹,严,华,金,魏,陶,姜,
戚,谢,邹,喻,柏,水,窦,章,云,苏,潘,葛,奚,范,彭,郎,鲁,韦,昌,马,苗,凤,花,方,俞,任,袁,柳,丰,鲍,史,唐,
费,岑,薛,雷,贺,倪,汤,滕,殷,罗,毕,郝,常,乐,于,时,傅,皮,卞,齐,康,伍,余,元,卜,顾,孟,平,黄,和,穆,萧,
尹,姚,邵,湛,汪,祁,毛,狄,米,贝,臧,计,成,戴,谈,宋,茅,庞,熊,纪,舒,屈,项,祝,董,梁,杜,阮,蓝,闵,席,季,
贾,路,娄,江,童,颜,郭,梅,盛,林,刁,钟,徐,丘,骆,高,夏,蔡,田,樊,胡,凌,霍,虞,万,柯,管,卢,莫,裘,缪,解,
宗,丁,宣,邓,单,洪,包,石,崔,龚,程,嵇,邢,滑,裴,陆,荣,翁,荀,羊,惠,甄,麴,封,芮,储,靳,邴,段,富,巫,焦,
谷,侯,全,郗,班,秋,伊,宫,宁,仇,栾,甘,厉,祖,武,刘,景,詹,龙,叶,司,郜,黎,薄,印,宿,白,怀,蒲,邰,赖,卓,
屠,蒙,乔,阴,闻,翟,谭,姬,申,冉,雍,桑,桂,濮,牛,扈,燕,尚,温,庄,晏,柴,瞿,阎,慕,连,习,艾,鱼,向,古,易,
戈,廖,庾,步,耿,文,广,东,欧,利,越,隆,师,巩,聂,敖,冷,辛,阚,那,简,饶,曾,沙,鞠,关,蒯,查,荆,游,竺,盖,
司马,上官,欧阳,夏侯,诸葛,闻人,东方,赫连,皇甫,尉迟,公羊,澹台,公冶,淳于,单于,太叔,申屠,公孙,仲孙,
轩辕,令狐,钟离,宇文,长孙,慕容,司徒,司空".Replace("\r\n", "").Split(',');

        private static readonly Random Rnd = new Random((int)DateTime.Now.ToFileTimeUtc());

        private static string RandomName()
        {
            var str = "";
            var count = Rnd.Next(1, 3);
            for (var i = 0; i < count; i++)
            {
                while (true)
                {
                    var aCode = 1601 + Rnd.Next(999);
                    var strtemp = Encoding.Default.GetString(new[] { (byte)(aCode / 100 + 160), (byte)(aCode % 100 + 160) });
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
        /// 获取随机姓名
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
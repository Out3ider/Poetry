using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{

    ///社区

    [HTable]
    public class Community : DictBase
    {
        /// <summary>
        /// 标题图片
        /// </summary>
        [HColumn(Length = 200)]
        public string Icon { set; get; }     //list中显示图片
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
            return Images?.Split(',').Where(x => x.IsNotNull()).Select(x => x ).ToList();
        }

        /// <summary>
        /// 县区id
        /// </summary>

        [HColumn]
        public County County
        {
            set; get;
        }
        [HColumn]
        public string Describe
        {
            set; get;
        }


        public static IList<KeyValuePair<string, string>> GetAll(IDataContext db)
        {
            return db.GetList<Community>()
                    .OrderBy(x => x.OrderByNo)
                    .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name))
                    .ToList();
        }
    }
}
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 县区
    /// </summary>
    [HTable]
    public class County : DictBase
    {
      
                public static IList<KeyValuePair<string, string>> GetAll(IDataContext db)
        {
            return db.GetList<County>()
                    .OrderBy(x => x.OrderByNo)
                    .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name))
                    .ToList();
        }
    }
    
}
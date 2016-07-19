using System.Collections.Generic;
using System.Linq;
using Poetry.Model;
using Sail.Common;

namespace Poetry.Web
{
    public class BaseDictController<T> : BaseController<T> where T : DictBase
    {
        protected override Clip OrderBy => Clip.OrderBy<T>(x => x.OrderByNo.Asc());

        protected override Clip MakeWhere(string key)
        {
            Clip where = null;
            if (key.IsNotNull()) where = Clip.Where<T>(x => x.Name.Like(key));
            return where;
        }

        public static List<T> GetAll()
        {
            using (var db = new DataContext())
            {
                return db.GetList<T>().OrderBy(x => x.OrderByNo).ToList();
            }

        }

        public static List<KeyValuePair<string, string>> GetAllKv()
        {
            using (var db = new DataContext())
            {
                return db.GetList<T>().OrderBy(x => x.OrderByNo).Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name)).ToList();
            }

        }

    }
}
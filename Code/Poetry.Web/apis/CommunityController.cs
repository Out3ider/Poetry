using Poetry.Model;
using Poetry.Web;
using Sail.Common;

namespace Poetry.Web
{
    /// <summary>
    /// 社区控制器
    /// </summary>
    public class CommunityController : BaseDictController<Community>
    {


        public AjaxResult GetList(int pageIndex, int pageSize, string key, string county, string describe)
        {
            return HandleHelper.TryAction(db =>
            {
                var clip = MakeWhere(key);
                if (county.IsNotNull()) clip &= nameof(Community.County).ToField("") == county;
                if (describe.IsNotNull()) clip &= Clip.Where<Community>(x => x.Describe.Like(describe));
                //clip &= Clip.Where<Community>(x => x.Name == county);
                return db.GetPageList<Community>(pageIndex, pageSize, clip, OrderBy);
            });
        }
    }
}
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
                if (county.IsNotNull()) clip &= nameof(Community.County).ToField("") == county;//对象是一个实体 county所以我们可以用== 来判断
                if (describe.IsNotNull()) clip &= Clip.Where<Community>(x => x.Describe.Like(describe));//此处对象为字符串，所以我们用.like 方法来判断
                //clip &= Clip.Where<Community>(x => x.Name == county);
                return db.GetPageList<Community>(pageIndex, pageSize, clip, OrderBy);
            });
        }
    }
}
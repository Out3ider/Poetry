using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Poetry.Model;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupController : BaseController<Group>
    {
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            base.BeforeAction(db, e, o);
            var role = o as Group;
            switch (e)
            {
                case ControllerEvent.Put:
                    if (role.RoleName == "超级管理员") throw new SailCommonException("您自定义的用户组不能叫【超级管理员】");
                    break;
                case ControllerEvent.Delete:
                    if (db.GetCount<Admin>(new Field("C_Group") == role.RoleId) > 0)
                        throw new SailCommonException("有用户正在使用这个用户组，无法删除");
                    break;
            }
        }

        protected override Clip MakeWhere(string key)
        {
            var where = Clip.Where<Group>(x => x.RoleId != 1);
            if (key.IsNotNull()) where &= Clip.Where<Group>(x => x.RoleName.Like(key));
            return where;
        }

        [HttpGet]
        public AjaxResult Menus(string url) => HandleHelper.TryAction(db => LoadMenu(db, url));


        internal static List<HMenuItem> LoadMenu(IDataContext db, string url)
        {
            var user = WebHelper.CheckAdmin<Admin>();
            var menutext = FileHelper.ReadTextFile("~/config/menu.json".FullFileName());
            var list = menutext.JsonToObj<List<HMenuItem>>().ToList();
            Filter(list, url, user);
            return list;
        }

        public static void Filter(List<HMenuItem> list, string MenuIdOrUrl, IPower user = null)
        {
            MenuIdOrUrl = MenuIdOrUrl.ToLower();
            list.ForEach(s =>
            {
                if (s.Url.IsNull()) s.Url = "";
                if (s.Url.IndexOf("~", StringComparison.Ordinal) == 0) s.Url = System.Web.VirtualPathUtility.ToAbsolute(s.Url);
                s.Url = s.Url.ToLower();
                s.Selected = (s.Url.Equals(MenuIdOrUrl)) || s.ItemId.Equals(MenuIdOrUrl);
                s.IsHasPower = user.IsNotNull() && (user.IsSuperAdmin || user.IsHasPower(s.ItemId));
                if (s.SubItems.Any())
                {
                    Filter(s.SubItems, MenuIdOrUrl, user);
                    s.IsHasPower = s.SubItems.Any(sb => sb.IsHasPower);
                    s.Selected = s.SubItems.Any(sb => sb.Selected);
                }
            });
            list.RemoveAll(x => !x.IsHasPower);
        }


        [HttpGet]
        public AjaxResult Menus() => HandleHelper.TryAction(db => LoadMenu(db, ""));




        [HttpGet]
        public AjaxResult All() => HandleHelper.TryAction(db =>
        {
            return db.GetList<Group>().Select(x => new { x.RoleName, x.RoleId });
        });

        public static List<KeyValuePair<string, string>> AllItem()
        {
            using (var db = new DataContext())
            {
                return db.GetList<Group>()
                    .Select(x => new KeyValuePair<string, string>(x.RoleId.ToString(), x.RoleName))
                    .ToList();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        /// <summary>增删改查之前执行,可以被覆盖</summary>
        /// <param name="db">The database.</param>
        /// <param name="e">The e.</param>
        /// <param name="o">The o.</param>
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            base.BeforeAction(db, e, o);
            var role = o as Group;
            switch (e)
            {
                case ControllerEvent.Put:
                    if (role?.RoleName == "超级管理员") throw new SailCommonException("您自定义的用户组不能叫【超级管理员】");
                    break;
                case ControllerEvent.Delete:
                    if (db.GetCount<UserGroup>(new Field("UserRole") == role?.RoleId) > 0)
                        throw new SailCommonException("有用户正在使用这个用户组，无法删除");
                    break;
            }
        }

        /// <summary>创建默认where条件，可以被覆盖</summary>
        /// <param name="key">The key.</param>
        /// <returns>Clip.</returns>
        protected override Clip MakeWhere(string key)
        {
            var where = Clip.Where<Group>(x => x.RoleId != 1);
            if (key.IsNotNull()) where &= Clip.Where<Group>(x => x.RoleName.Like(key));
            return where;
        }


        internal static List<HMenuItem> LoadMenu(IDataContext db, ProjectType type)
        {
            var user = WebHelper.CheckAdmin<Admin>();
            var menuName = "menu";
            switch (type)
            {
                case ProjectType.微感悟:
                    menuName = "senses";
                    break;
            }
            var menutext = FileHelper.ReadTextFile($"~/config/{menuName}.json".FullFileName());
            var list = menutext.JsonToObj<List<HMenuItem>>().ToList();
            Filter(list, user);
            return list;
        }

        /// <summary>
        /// 过滤菜单权限
        /// </summary>
        /// <param name="list"></param>
        /// <param name="user"></param>
        public static void Filter(List<HMenuItem> list, IPower user = null)
        {

            list.ForEach(s =>
            {
                if (s.Url.IsNull()) s.Url = "";
                if (s.Url.IndexOf("~", StringComparison.Ordinal) == 0) s.Url = System.Web.VirtualPathUtility.ToAbsolute(s.Url);
                s.Url = s.Url.ToLower();
                s.IsHasPower = (user?.IsSuperAdmin ?? false) || (user?.IsHasPower(s.ItemId) ?? false);
                if (s.SubItems.Any())
                {
                    Filter(s.SubItems, user);
                    s.IsHasPower = s.SubItems.Any(sb => sb.IsHasPower);
                    s.Selected = s.SubItems.Any(sb => sb.Selected);
                }
            });
            list.RemoveAll(x => !x.IsHasPower);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Menus(ProjectType type) => HandleHelper.TryAction(db => LoadMenu(db, type));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetList(int pageIndex, int pageSize, string key, ProjectType type)
        {
            return HandleHelper.TryAction(db =>
            {
                var where = Clip.Where<Group>(x => x.ProjectType == type);
                where &= MakeWhere(key);
                return db.GetPageList<Group>(pageIndex, pageSize, where, OrderBy);
            });
        }

        public override AjaxResult GetList(int pageIndex, int pageSize, string key)
        {
            throw new SailCommonException("屏蔽鸟");
        }

        /// <summary>
        /// 所有用户组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> All(ProjectType type)
        {
            using (var db = new DataContext())
            {
                var group = db.GetModel<Group>(x => x.RoleName == "超级管理员");
                var list = db.GetList<Group>(x => x.ProjectType == type);
                list.Insert(0, group);
                return list.Select(x => new KeyValuePair<string, string>(x.RoleId.ToString(), x.RoleName))
                   .ToList();
            }
        }
    }
}
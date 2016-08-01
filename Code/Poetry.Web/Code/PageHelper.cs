using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Poetry.Model;
using Sail.Web;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Sail.Common;


namespace Poetry.Web
{

    public static class PageHelper
    {
        /// <summary>
        /// 后台用户登录确认
        /// </summary>
        /// <param name="response"></param>
        /// <param name="type"></param>
        public static void CheckAdmin(HttpResponseBase response, ProjectType type)
        {
            var user = WebHelper.CurrentAdmin as Admin;
#if DEBUG
            if (WebHelper.CurrentAdmin.IsNull())
                WebHelper.CurrentAdmin = new DataContext().GetModel<Admin>(Clip.Where<Admin>(x => x.LoginId == "admin"));
#endif
            var group = user?.Group?.UserRole;
            if (user == null
                || (group == null && !user.IsSuperAdmin)
                || (group != null && group.RoleId != 1 && group.ProjectType != type)
                ) response.Redirect("login");
        }


        public static string GetPre(this string src, string type = "")
        {
            var result = "";
            if (src.IsNotNull() && src.IndexOf("/uploads/") >= 0)
            {
                var lastIndex = src.LastIndexOf('/');
                result = $"{src.Substring(0, lastIndex)}/{type}pre{src.Substring(lastIndex)}";
            }
            return result;
        }

        /// <summary>
        /// 用户默认头像
        /// </summary>
        public static string DefaultHeadIcon => $"{WebHelper.GetRootUrl()}Content/images/default.jpg";



        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector));
        }


        public static void ForEach<T>(IEnumerable<T> list, Action<int, T> act)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            var enumerable = list as IList<T> ?? list.ToList();
            for (var i = 0; i < enumerable.Count; i++)
            {
                var e = enumerable[i];
                act(i, e);
            }
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary) expando.Add(item);
            return (ExpandoObject)expando;
        }

        public static Clip Where<T>(this T model, Expression<Func<T, bool>> exp, string prefix = "t.") where T : IModel
        {
            return Clip.Where(exp, prefix);
        }

        /// <summary>
        /// datable转换动态数据集
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> ToDictionary(this DataTable dt)
        {
            return (from DataRow dr in dt.Rows select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col])).ToList();
        }


        /// <summary>
        /// 菜单id
        /// </summary>
        public const string ItemId = nameof(ItemId);
        /// <summary>
        /// 页面标题
        /// </summary>
        public const string PageTitle = nameof(PageTitle);
        /// <summary>
        /// 页面副标题
        /// </summary>
        public const string PageTips = nameof(PageTips);
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public const string CurrentUser = nameof(CurrentUser);

        /// <summary>
        /// 设定页面菜单、标题什么的
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemId"></param>
        /// <param name="title"></param>
        /// <param name="tips"></param>
        /// <returns></returns>
        public static Admin SetPage(this WebPageBase page, string itemId, string title, string tips)
        {
            page.PageData[ItemId] = itemId;
            page.PageData[PageTitle] = title;
            page.PageData[PageTips] = tips;
            return WebHelper.CurrentUser as Admin;// page.PageData[CurrentUser];
        }

        private static TagBuilder BuilderOption(string text, string value)
        {
            var tag = new TagBuilder("option");
            tag.SetInnerText(text);
            tag.MergeAttribute("value", value);
            return tag;
        }

        /// <summary>
        /// 输出select的options
        /// </summary>
        /// <param name="items"></param>
        /// <param name="defaultText"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static HtmlString RenderOptions(this IList<KeyValuePair<string, string>> items, string defaultText = "", string defaultValue = "")
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(defaultText)) sb.Append(BuilderOption(defaultText, defaultValue));
            if (items.IsNotNull()) items.ForEach(item => { sb.Append(BuilderOption(item.Value, item.Key).ToString()); });
            return new HtmlString(sb.ToString());
        }


        /// <summary>
        /// 输出枚举到options
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultText"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static HtmlString RenderEnum(this Type type, string defaultText = "", string defaultValue = "")
        {
            return
                type.GetEnumItems()
                    .OrderBy(x => x.Key)
                    .Select(x => new KeyValuePair<string, string>(x.Key.ToString(), x.Value))
                    .ToList()
                    .RenderOptions(defaultText, defaultValue);
        }


        /// <summary>
        /// 生成枚举的列表数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> EnumList(this Type type)
        {
            return type.GetEnumItems().OrderBy(x => x.Key).Select(x => new KeyValuePair<string, string>(x.Key.ToString(), x.Value)).ToList();
        }

        /// <summary>
        /// 生成数字
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static HtmlString NumberTo(this int start, int end)
        {
            var items = new List<KeyValuePair<string, string>>();
            for (var i = start; i <= end; i++)
            {
                items.Add(new KeyValuePair<string, string>(i.ToString(), i.ToString()));
            }
            return items.RenderOptions();
        }


        /// <summary>
        /// 导出csv
        /// </summary>
        /// <param name="titles"></param>
        /// <param name="fileName"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static HttpResponseMessage ExportToCsv(string[] titles, string fileName, Action<StringBuilder> act)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var str = new StringBuilder();
            titles.ForEach(s =>
            {
                str.Append(s + ",");
            });
            str.AppendLine("");
            act(str);
            var filename = fileName + ".csv";
            response.Content = new StreamContent(ExportHelper.GenerateStreamFromString(str.ToString()));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = HttpUtility.UrlEncode(filename)
            };
            return response;
        }
        public static void AppendCsv(this StringBuilder sb, object text)
        {
            sb.AppendFormat("{0},", text);
        }
    }
}
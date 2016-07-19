using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sail.Common;

namespace Poetry.Web.Code
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class SqlHelper
    {
        public static Field Select<T>(Expression<Func<T, object>> expression) where T : IModel
        {
            var exp = expression.Body as NewExpression;
            return ExpandColumn(exp);
        }

        private static Field ExpandColumn(NewExpression exp)
        {
            return exp.Arguments.Select(x =>
            {
                var data = x as MemberExpression;
                var param = data?.Expression as ParameterExpression;
                return $"{param?.Name}.{data?.Member.Name}";
            }).JoinToString().ToField("");
        }

        public static Field Select<T1, T2>(Expression<Func<T1, T2, object>> expression) where T1 : IModel where T2 : IModel
        {
            var exp = expression.Body as NewExpression;
            return ExpandColumn(exp);
        }

        public static Field Select<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> expression) where T1 : IModel where T2 : IModel where T3 : IModel
        {
            var exp = expression.Body as NewExpression;
            return ExpandColumn(exp);
        }

        public static Field Select<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, object>> expression) where T1 : IModel where T2 : IModel where T3 : IModel where T4 : IModel
        {
            var exp = expression.Body as NewExpression;
            return ExpandColumn(exp);
        }
    }
}
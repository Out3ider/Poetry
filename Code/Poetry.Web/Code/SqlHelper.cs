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


        private static Field ExpandSelectColumn(NewExpression exp)
        {
            return exp.Arguments.Select((x, index) =>
            {
                var data = x as MemberExpression;
                var param = data?.Expression as ParameterExpression;
                var table = data?.Member.DeclaringType.GetHTable();
                var col = table?.Columns.Find(c => c.Property == data?.Member.Name)?.ColumnName;
                var columnName = exp.Members[index].Name;
                var asName = (col == columnName ? "" : $" as {columnName}");
                return $"{param?.Name}.{col}{asName}";
            }).JoinToString().ToField("");
        }

        private static Field ExpandJoinColumn(NewExpression exp)
        {
            return exp.Arguments.Select((x, index) =>
            {
                var data = x as MemberExpression;
                var param = data?.Expression as ParameterExpression;
                var table = data?.Member.DeclaringType.GetHTable();
                var col = table?.Columns.Find(c => c.Property == data?.Member.Name)?.ColumnName;
                var columnName = exp.Members[index].Name;
                var asName = (col == columnName ? "" : $" as {columnName}");
                return $"{param?.Name}.{col}{asName}";
            }).JoinToString().ToField("");
        }


        

        public static Field Select<T1, T2>(Expression<Func<T1, T2, object>> expression) where T1 : IModel where T2 : IModel
        {
            var exp = expression.Body as NewExpression;
            return ExpandSelectColumn(exp);
        }
 
       
    }
}
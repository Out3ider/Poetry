using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Poetry.Model;
using Sail.Common;



namespace Poetry.Model
{
    /// <summary>
    /// ������
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormAttribute : Attribute
    {
        /// <summary>
        /// ��ǩ
        /// </summary>
        public string Label { set; get; }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        public string Tips { set; get; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsRequired { set; get; }

        /// <summary>
        /// �Ƿ�ֻ��
        /// </summary>
        public bool IsReadOnly { set; get; }

        /// <summary>
        /// �Զ�����֤
        /// </summary>
        public string CustomValidate { set; get; }

        /// <summary>
        /// ��ʽ
        /// </summary>
        public string Class { set; get; }


    }

    /// <summary>
    /// ����data���ԣ����Դ��ڶ��
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ModelDataAttribute : Attribute
    {
        /// <summary>
        /// data-����
        /// </summary>
        public string Key { set; get; }

        /// <summary>
        /// data-ֵ
        /// </summary>
        public string Value { set; get; }
    }

}

namespace System.Web.WebPages
{
    using Poetry.Model;


    /// <summary>
    /// �������ر�
    /// </summary>
    public class Element
    {
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public string Label { set; get; }

        /// <summary>
        /// �ؼ�id
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        public string Tips { set; get; }

        /// <summary>
        /// �Զ�����֤
        /// </summary>
        public string ValidateCustom { set; get; }

        /// <summary>
        /// �Ƿ�ֻ��
        /// </summary>
        public bool IsReadOnly { set; get; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsRequired { set; get; }

        /// <summary>
        /// css��
        /// </summary>
        public string Class { set; get; }

        /// <summary>
        /// data����
        /// </summary>
        public Dictionary<string, string> Data { set; get; }

        /// <summary>
        /// ���data-*=*
        /// </summary>
        /// <returns></returns>
        public HtmlString RenderData()
        {
            var sb = new StringBuilder();
            if (Data.IsNull()) return new HtmlString("");
            foreach (var data in Data)
            {
                sb.Append($" data-{data.Key}='{data.Value}' ");
            }
            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// ���id
        /// </summary>
        /// <returns></returns>
        public HtmlString RenderId()
        {
            var element = this;
            return new HtmlString($"id='{element.Id}'");
        }


        /// <summary>
        /// ���class
        /// </summary>
        /// <param name="controlClass"></param>
        /// <returns></returns>
        public IHtmlString RenderClass(string controlClass = " form-control ")
        {
            return new HtmlString($"class='{controlClass} {Class} '");
        }

        /// <summary>
        /// ���ȫ������
        /// </summary>
        /// <param name="controllCalss"></param>
        /// <returns></returns>
        public IHtmlString Attr(string controllCalss = "form-control")
        {
            var str = $"{RenderId()} {RenderClass(controllCalss)} {RenderData()} {RenderPlaceholder()} {(IsReadOnly ? "readonly='readonly'" : "")}  ";
            return new HtmlString(str);
        }

        /// <summary>
        /// ���placeholder
        /// </summary>
        /// <returns></returns>
        public IHtmlString RenderPlaceholder()
        {
            return new HtmlString($"placeholder=\"������{this.Label}\"");
        }

        /// <summary>
        /// ����
        /// </summary>
        public Element()
        {
            Data = new Dictionary<string, string>();
        }

        /// <summary>
        /// ����
        /// </summary>
        public Element(string label, string id) : this()
        {
            Label = label;
            Id = id;
        }

        /// <summary>
        /// ����
        /// </summary>
        public Element(string label, string id, string className) : this(label, id)
        {
            Class = className;
        }

        /// <summary>
        /// ����
        /// </summary>
        public Element(string label, string id, bool isrequired, string className) : this(label, id, className)
        {
            IsRequired = isrequired;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="expression"></param>
        /// <param name="customlabel"></param>
        /// <returns></returns>
        public static Element Create<TModel>(Expression<Func<TModel, Object>> expression, string customlabel = "")
        {


            if (expression == null) throw (new ArgumentNullException(nameof(expression)));
            MemberExpression operation = null;

            if (expression.Body is MemberExpression)
                operation = (MemberExpression)expression.Body;
            else if (expression.Body is UnaryExpression)
                operation = (MemberExpression)(((UnaryExpression)expression.Body).Operand);


            var typeName = expression.Parameters[0].Type.FullName + "_" + expression.Body;

            return typeName.GetCache(() =>
            {
                var prefix = "";
                var preFixLable = "";
                var memberName = operation?.Member.Name;
                var modelType = operation?.Member.DeclaringType;

                if (operation?.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    var ma = (operation.Expression) as MemberExpression;
                    prefix = ma?.Member.Name + ".";
                    var piPrefix = ReflectionExtensions.GetProperties(ma?.Member.DeclaringType).Find(x => x.Name.Equals(ma?.Member.Name));
                    preFixLable = piPrefix.GetSummary();
                }


                var ps = ReflectionExtensions.GetProperties(modelType);
                var pi = ps.Find(x => x.Name.Equals(memberName));
                var mc = pi.GetAttribute<FormAttribute>();
                var lable = mc?.Label;
                if (lable.IsNull()) lable = pi.GetSummary();
                var result = new Element
                {
                    Label = customlabel.IsNotNull() ? customlabel : (preFixLable + lable),
                    Id = prefix + pi.Name,
                    IsRequired = mc?.IsRequired ?? false,
                    Class = mc?.Class ?? "",

                    Tips = mc?.Tips ?? "",
                    ValidateCustom = mc?.CustomValidate,
                    IsReadOnly = mc?.IsReadOnly ?? false,
                };
                var validate = new List<string>();
                if (result.IsRequired)
                {
                    validate.Add("required");
                }
                if ((mc?.CustomValidate ?? "").IsNotNull())
                {
                    validate.Add($"custom[{mc?.CustomValidate}]");
                }
                if (validate.Any()) result.Class += $" validate[{validate.JoinToString()}]";

                var datas = pi.GetCustomAttributes(typeof(ModelDataAttribute), true).Cast<ModelDataAttribute>().ToList();
                if (datas.Any()) datas.ForEach(x =>
                {
                    result.Data[x.Key] = x.Value;

                });
                return result;
            });
        }
    }
}
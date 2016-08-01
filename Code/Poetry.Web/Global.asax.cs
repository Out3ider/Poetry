using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Poetry.Model;
using Poetry.Web.Utility;
using Sail.Common;
using Sail.Web;
namespace Poetry.Web
{


    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "WeChat", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Poetry.Web.Controller" }
               );

            DatabaseScheme.InitDatabase();
            VerifyCode.Init(typeof(VerifyType)); //初始化验证码类型
            InitData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            using (var db = new DataContext())
            {
                if (db.GetCount<County>() == 0)
                {
                    ForEach(new[] { "市级", "无为县", "芜湖县", "繁昌县", "南陵县", "镜湖区", "弋江区", "鸠江区", "三山区", "经开区", "大桥区", },
                        (i, name) => db.Save(new County { Name = name, OrderByNo = i + 1 }));
                }


                if (db.GetCount<Admin>() == 0)
                {
                    db.Save(new Group { RoleName = "超级管理员" });
                    db.Save(new Admin { LoginId = "admin", Password = "admin", UserName = "超级管理员" });
                }


                if (db.GetCount<Param>() == 0)
                {
                    db.Save(new Param
                    {
                        AppId = "wx366a13dfa1da94e0",
                        Secret = "0f8059760687d0dba0d913465d63b093",
                        Host = "",
                        SiteName = "发现身边先锋",
                    });
                }

                if (db.GetCount<ComprehensionParam>() == 0)
                {
                    db.Save(new ComprehensionParam
                    {
                        SiteName = "微感悟",
                        IsAutoPublish = true,
                        IsCommentNeedCheck = false,
                    });
                }

                if (db.GetCount<Prize>() == 0)
                {
                    new Dictionary<MobileOperator, Tuple<int, int>[]>
                    {
                        [MobileOperator.联通] = new[] { Tuple.Create(100, 1000) },
                        [MobileOperator.电信] = new[] { Tuple.Create(100, 50), Tuple.Create(30, 950) },
                        [MobileOperator.移动] = new[] { Tuple.Create(100, 50), Tuple.Create(30, 750), Tuple.Create(10, 200) },

                    }.ForEach(x =>
                    {
                        x.Value.ForEach(c =>
                        {
                            db.Save(new Prize
                            {
                                MaxNumber = 999999,
                                Title = $"{c.Item1}M{x.Key}流量",
                                ProjectType = ProjectType.微感悟,
                                Operator = x.Key,
                                Probability = c.Item2,
                                TotalStock = 999999
                            });
                        });
                    });
                }
                ComprehensionParam.Config(ComprehensionParam.Get(db));
                BadWords.Config();
                Param.Config(Param.Get(db));

            }
        }




        //带索引的foreach
        void ForEach<T>(IEnumerable<T> list, Action<int, T> act)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            var enumerable = list as IList<T> ?? list.ToList();
            for (var i = 0; i < enumerable.Count; i++)
            {
                var e = enumerable[i];
                act(i, e);
            }
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            SailSession.Session.UpdateTime();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
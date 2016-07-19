#使用说明

##一、修改web.config
>1、<del>
    把[*appSettings*]项中的[*IModel*]值修改为实际项目的Model包名，并且取消注释
</del>
<br />
>2、如果使用mysql数据库，把相关mysql的dll注释取消
<br />
>3、[*appSettings*]项中的[*SessionTimeOut*]值决定了Session的超时分钟数
<br />
>4、[*appSettings*]项中的[*IsEnableSqlLog*]值的True或False决定了是否记录sql语句到日志文件
<br />


##二、给项目添加一个global.asax文件
###1、在[*Application_Start*]方法中加入如下两行以注册webapi路由和初始化数据库：

<pre><code>
WebApiConfig.Register(GlobalConfiguration.Configuration);
DatabaseScheme.InitDatabase();
</code></pre>

###2、 <del>加入覆盖的Init方法 </del>
<pre>
<del>	public override void Init()</del>
<del>		{                                                                                               </del>
<del>			this.PostAuthenticateRequest += (s, e) =>                                                   </del>
<del>			{                                                                                           </del>
<del>				//让webapi也能访问session                                                                </del>
<del>				System.Web.HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);  </del>
<del>			};                                                                                          </del>
<del>			base.Init();                                                                                </del>
<del>		}                                                                                               </del>
</pre>

###3、在[*Application_BeginRequest*]方法中加入
<pre><code>
    SailSession.Session.UpdateTime();
 </code></pre>

##三、修改connfig\conn.config文件
>把相应的连接字符串取消注释，并且修改数据库名称和数据库登录密码，默认sqlserver是使用windows认证登录的。
<br />
<br />
>**这里的数据库如果不存在，系统会在第一次运行的时候自动创建。**
<br />
<br>
>**如果以后修改实体类，那么编译后第一次运行时，会自动修改数据库结构

##四、添加api目录,并添加相应的Controller
>可以继承**`BaseController<T>
    `**
    <br />
>默认提供如下几个方法:
<pre><code>
    public class BaseController<t> : ApiController where T : Sail.Common.IModel
	{
		//事件执行后
		public virtual void AfterAction(DataContext db, ControllerEvent e, object o);	  
	
	    //事件执行前
		public virtual void BeforeAction(DataContext db, ControllerEvent e, object o);
	 
	    //保存对象，根据id是否为0判断新增或更新数据
		[HttpPut]
		public virtual AjaxResult Put(int id, string value);
	 
	    //删除方法
		[HttpDelete]
		public virtual AjaxResult Delete(int id);
	    
	    //根据id获取对象
		[HttpGet]
		public virtual AjaxResult Get(int id);
	    //分页获取列表
		[HttpGet]
		public virtual AjaxResult GetList(int pageIndex, int pageSize);
	    //有条件分页获取列表
		[HttpGet]
		public virtual AjaxResult GetList(int pageIndex, int pageSize, string key);
	
	    //生成排序字段，默认id倒序,可重写
		public virtual Clip MakeOrderBy();
		   
	    //按照过滤条件过滤，默认无效，可重写
		public virtual Clip MakeWhere(string key);
	    
	}
</code></pre>

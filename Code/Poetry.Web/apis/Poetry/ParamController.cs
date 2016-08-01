using System.Web.Http;
using Poetry.Model;
using Sail.Common;
using Poetry.Web;

namespace Poetry.Web
{
    /// <summary>
    /// 发现身边先锋系统参数控制器
    /// </summary>
    public class ParamController : ApiController
    {
        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Get() => HandleHelper.TryAction(db => Param.Default);


        /// <summary>
        /// 保存系统参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public AjaxResult Save([FromBody]string value) => HandleHelper.TryAction(db =>
        {
            var model = Param.Default;
            model = db.LoadModalByJson<Param>(value, model.IsNull() ? "" : model.ParamId);
            db.Save(model);
            Param.Config(model);
            return model;
        });

    }


}
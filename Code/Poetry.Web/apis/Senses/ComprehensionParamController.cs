using System.Web.Http;
using Poetry.Model;
using Poetry.Web.Utility;
using Sail.Common;

namespace Poetry.Web
{
    /// <summary>
    /// ΢����ϵͳ����
    /// </summary>
    public class ComprehensionParamController : ApiController
    {        /// <summary>
             /// ��ȡϵͳ����
             /// </summary>
             /// <returns></returns>
        [HttpGet]
        public AjaxResult Get() => HandleHelper.TryAction(db => ComprehensionParam.Default);


        /// <summary>
        /// ����ϵͳ����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public AjaxResult Save([FromBody]string value) => HandleHelper.TryAction(db =>
        {
            var model = ComprehensionParam.Default;
            model = db.LoadModalByJson<ComprehensionParam>(value, model.IsNull() ? "" : model.ParamId);
            db.Save(model);
            ComprehensionParam.Config(model);
            BadWords.Config();
            return model;
        });


    }
}
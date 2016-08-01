using System.Web.Http;
using Poetry.Model;
using Sail.Common;
using Poetry.Web;

namespace Poetry.Web
{
    /// <summary>
    /// ��������ȷ�ϵͳ����������
    /// </summary>
    public class ParamController : ApiController
    {
        /// <summary>
        /// ��ȡϵͳ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Get() => HandleHelper.TryAction(db => Param.Default);


        /// <summary>
        /// ����ϵͳ����
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
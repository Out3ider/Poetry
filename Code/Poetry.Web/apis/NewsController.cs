using System;
using System.Collections.Generic;
using Poetry.Model;
using Sail.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using Sail.Web;

namespace Poetry.Web
{
    public class WeChatNewsController : BaseController<News>
    {

        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            var model = o as News;
            var user = WebHelper.CheckUser<Admin>();
            if (e == ControllerEvent.Put)
            {
                if (model?.Id == 0)
                {
                    model.CreaterId = user.UserId;
                    model.Referral = user.UserName;
                    model.CreaterName = user.UserName;
                }
            }
        }


        protected override Clip OrderBy => Clip.OrderBy<News>(x => x.CreateTime.Desc());


    }
}
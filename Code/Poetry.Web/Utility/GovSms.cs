using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using Poetry.Model;
using Sail.Common;
using Sail.Web;

namespace Poetry.Web.Utility
{
    /// <summary>
    /// 政府短信平台
    /// </summary>
    public class GovSms
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string BuildeParam(IDictionary<string, object> dict)
        {
            return dict.Select(item => $"{item.Key}={item.Value}").JoinToString("&");
        }


        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="content"></param>
        /// <param name="mobilePhones"></param>
        public static bool SendSms(string mobilePhones, string content)
        {
            if (mobilePhones == null) throw new ArgumentNullException(nameof(mobilePhones));
            var param = new Dictionary<string, object>
            {
                ["MessageFlag"] = "123",
                ["ModuleName"] = "VIP",
                ["MobilePhones"] = mobilePhones,
                ["Content"] = HttpUtility.UrlEncode(content, Encoding.GetEncoding("GBK")),

            };

            var strUrl = $"http://{Param.Default.SmsServer}:{Param.Default.SmsPort}/ISendSMS.jsp";
            try
            {
                strUrl += $"?{BuildeParam(param)}";
                var client = new HttpClient
                {
                    BaseAddress = new Uri($"http://{Param.Default.SmsServer}:{Param.Default.SmsPort}")
                };
                var p = BuildeParam((param));

                //var result = client.GetStringAsync($"/ISendSMS.jsp?{p}").Result.Trim();
                var result = WebClientHelper.GetHttp(strUrl, Encoding.UTF8).Trim();
                return result.ToUpper() == "OK";
            }
            catch (Exception)
            {
                return false;

            }
        }
    }
}
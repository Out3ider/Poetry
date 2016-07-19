using System;
using System.Collections.Generic;
using Sail.Common;

namespace Poetry.Web.Utility
{
    public enum VerifyType
    {
        注册用户,
        找回密码
    }

    /// <summary>
    /// 验证码
    /// </summary>
    public class VerifyCode
    {

        /// <summary>
        /// 验证码缓存
        /// </summary>
        static List<VerifyCode> VerifyCodes { get; } = new List<VerifyCode>();

        /// <summary>
        /// 验证码类型
        /// </summary>
        static Dictionary<int, string> VerifyCategory { get; } = new Dictionary<int, string>();

        /// <summary>
        /// 初始化验证码
        /// </summary>
        /// <param name="type"></param>
        public static void Init(Type type)
        {
            if (!type.IsEnum) throw new Exception("请使用枚举类型注册验证码");
            VerifyCategory.Clear();
            type.GetEnumItems().ForEach(x =>
            {
                VerifyCategory[x.Key] = x.Value;
            });
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static VerifyCode GetVerifycodes(string mobile, int type)
        {
            //移除所有超时的
            VerifyCodes.RemoveAll(x => x.IsTimeout);
            var code = VerifyCodes.Find(x => x.Mobile == mobile && x.Type == type);
            if (code.IsNull())
            {
                code = new VerifyCode(mobile, type);
                VerifyCodes.Add(code);
            }
            return code;
        }

        /// <summary>
        /// 超时(分钟)
        /// </summary>
        static int TimeOut => 10;

        /// <summary>
        /// 发送超时(分钟) 就是每n分钟最多发送一次
        /// </summary>
        static int SendTimeOut => 1;

        /// <summary>
        /// 长度4位
        /// </summary>
        const int codeLength = 6;

        /// <summary>
        /// 验证码内容元素
        /// </summary>
        const string textArray = "1234567890";


        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { set; get; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public int Type { private set; get; }


        /// <summary>
        /// 请求的手机号，防止获取验证码后，手机号不匹配
        /// </summary>
        public string Mobile { private set; get; }

        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime StartTime { set; get; }

        /// <summary>
        /// 上次发送时间
        /// </summary>
        public DateTime LastSendTime { set; get; }

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsTimeout => MinutesAgo > TimeOut;

        /// <summary>
        /// 多少分钟前生成的验证码
        /// </summary>
        /// <returns></returns>
        public int MinutesAgo => (DateTime.Now - StartTime).Minutes;


        /// <summary>
        /// 多少分钟前发送
        /// </summary>
        /// <returns></returns>
        public int SendMinutesAgo => (DateTime.Now - LastSendTime).Minutes;


        /// <summary>
        /// 初始化验证码
        /// </summary>
        public VerifyCode(string mobile, int type)
        {
            Mobile = mobile;
            Type = type;
            StartTime = DateTime.Now;
            var random = new Random((int)DateTime.Now.Ticks);
            for (var i = 0; i < codeLength; i++)
                Code += textArray.Substring(random.Next() % textArray.Length, 1);
        }

        /// <summary>
        /// 校验验证码，如果成功则重置验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool CheckCode(string mobile, int type, string code)
        {
            var vcode = GetVerifycodes(mobile, type);
            if (vcode.Code != code) return false;
            VerifyCodes.RemoveAll(x => x.Mobile == mobile && x.Type == type);
            return true;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="db"></param>
        public void PushSms(IDataContext db)
        {
            if ((DateTime.Now - LastSendTime).Minutes < 1) throw new SailCommonException("一分钟之内不能重复发送验证码");
            string str = $"您申请{ VerifyCategory[Type] }的验证码为:{Code}，该验证码{TimeOut}分钟内有效。";
            var phone = Mobile;
            if (phone.IsNotNull())
            {
                if (!GovSms.SendSms(phone, str)) throw new SailCommonException("短信发送失败，请与系统管理员联系");
                LastSendTime = DateTime.Now;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Sail.Common;

namespace Poetry.Web.Utility
{
    public enum VerifyType
    {
        ע���û�,
        �һ�����
    }

    /// <summary>
    /// ��֤��
    /// </summary>
    public class VerifyCode
    {

        /// <summary>
        /// ��֤�뻺��
        /// </summary>
        static List<VerifyCode> VerifyCodes { get; } = new List<VerifyCode>();

        /// <summary>
        /// ��֤������
        /// </summary>
        static Dictionary<int, string> VerifyCategory { get; } = new Dictionary<int, string>();

        /// <summary>
        /// ��ʼ����֤��
        /// </summary>
        /// <param name="type"></param>
        public static void Init(Type type)
        {
            if (!type.IsEnum) throw new Exception("��ʹ��ö������ע����֤��");
            VerifyCategory.Clear();
            type.GetEnumItems().ForEach(x =>
            {
                VerifyCategory[x.Key] = x.Value;
            });
        }

        /// <summary>
        /// ��ȡ��֤��
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static VerifyCode GetVerifycodes(string mobile, int type)
        {
            //�Ƴ����г�ʱ��
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
        /// ��ʱ(����)
        /// </summary>
        static int TimeOut => 10;

        /// <summary>
        /// ���ͳ�ʱ(����) ����ÿn������෢��һ��
        /// </summary>
        static int SendTimeOut => 1;

        /// <summary>
        /// ����4λ
        /// </summary>
        const int codeLength = 6;

        /// <summary>
        /// ��֤������Ԫ��
        /// </summary>
        const string textArray = "1234567890";


        /// <summary>
        /// ��֤��
        /// </summary>
        public string Code { set; get; }

        /// <summary>
        /// ��֤������
        /// </summary>
        public int Type { private set; get; }


        /// <summary>
        /// ������ֻ��ţ���ֹ��ȡ��֤����ֻ��Ų�ƥ��
        /// </summary>
        public string Mobile { private set; get; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime StartTime { set; get; }

        /// <summary>
        /// �ϴη���ʱ��
        /// </summary>
        public DateTime LastSendTime { set; get; }

        /// <summary>
        /// �Ƿ�ʱ
        /// </summary>
        public bool IsTimeout => MinutesAgo > TimeOut;

        /// <summary>
        /// ���ٷ���ǰ���ɵ���֤��
        /// </summary>
        /// <returns></returns>
        public int MinutesAgo => (DateTime.Now - StartTime).Minutes;


        /// <summary>
        /// ���ٷ���ǰ����
        /// </summary>
        /// <returns></returns>
        public int SendMinutesAgo => (DateTime.Now - LastSendTime).Minutes;


        /// <summary>
        /// ��ʼ����֤��
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
        /// У����֤�룬����ɹ���������֤��
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
        /// ���Ͷ���
        /// </summary>
        /// <param name="db"></param>
        public void PushSms(IDataContext db)
        {
            if ((DateTime.Now - LastSendTime).Minutes < 1) throw new SailCommonException("һ����֮�ڲ����ظ�������֤��");
            string str = $"������{ VerifyCategory[Type] }����֤��Ϊ:{Code}������֤��{TimeOut}��������Ч��";
            var phone = Mobile;
            if (phone.IsNotNull())
            {
                if (!GovSms.SendSms(phone, str)) throw new SailCommonException("���ŷ���ʧ�ܣ�����ϵͳ����Ա��ϵ");
                LastSendTime = DateTime.Now;
            }
        }
    }
}
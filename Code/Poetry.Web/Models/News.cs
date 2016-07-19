using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Sail.Common;
using Sail.Web;

namespace Poetry.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class News : UserInfoBase
    { 
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        [HColumn(Length = 200)]
        public string Icon { set; get; }

        /// <summary>
        /// ��Ա����
        /// </summary>
        [HColumn(Length = 50)]
        public string PartyMember { set; get; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [HColumn]
        public DateTime NewsDate { set; get; }

        /// <summary>
        /// ����
        /// </summary>
        [HColumn(Length = 50)]
        public string Referral { set; get; }

        /// <summary>
        /// ͼƬ
        /// </summary>
        [HColumn]
        public string Images { set; get; }

        /// <summary>
        /// ͼƬ�б�
        /// </summary>
        /// <returns></returns>
        public IList<string> GetImages()
        {
            return Images?.Split(',').Where(x => x.IsNotNull()).Select(x => x).ToList();
        }

        /// <summary>
        /// ����id
        /// </summary>
        [HColumn]
        public County County { set; get; }

        /// <summary>
        /// ����
        /// </summary>
        [HColumn]
        public OrgType Type { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeStr => Type.ToString(); 
        /// <summary>
        /// ���㵳��֯
        /// </summary>
        [HColumn(Length = 50)]
        public string Organization { set; get; } 
        /// <summary>
        /// �Ƿ����
        /// </summary>
        [HColumn]
        public bool IsChecked { set; get; } 
        /// <summary>
        /// ����
        /// </summary>
        [HColumn]
        public int Liked { set; get; }

        /// <summary>
        /// �鿴����
        /// </summary>
        [HColumn]
        public int Views { set; get; }

        /// <summary>
        /// �Ƿ���ʾ
        /// </summary>
        [HColumn]
        public bool IsPublish { set; get; }  
        /// <summary>
        /// ������
        /// </summary>
        [HColumn]
        public int Comments { set; get; }

        /// <summary>
        /// �¼�����
        /// </summary>
        [HColumn]
        public string Content { set; get; }

        /// <summary>
        /// where����
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static Clip MakeWhere(int year, int month, string prefix = "t.")
        {
            var where = Clip.Where<News>(x => x.IsChecked == true, prefix);
            if (year > 0 && month == 0) where &= "CreateTime".DateBetween(new DateTime(year, 1, 1).ToDateString(),
                    new DateTime(year, 12, 31).ToDateString(), prefix);
            else if ((year > 0 && month > 0)) where &= "CreateTime".DateBetween(new DateTime(year, month, 1).ToDateString(),
                    new DateTime(year, month, 1).GetLastDayOfMonth().ToDateString(), prefix);
            return where;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IList<NewsComment> GetComments(DataContext db) => db.GetList<NewsComment>(x => x.IsChecked == true && x.NewsId == this.Id);
    }
}
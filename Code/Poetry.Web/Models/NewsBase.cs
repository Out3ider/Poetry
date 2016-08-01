using System;
using System.Collections.Generic;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{
    public abstract class NewsBase : UserInfoBase
    {
       
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        [HColumn(Length = 200)]
        public string Icon { set; get; }

  

        /// <summary>
        /// ͼƬ
        /// </summary>
        [HColumn]
        public string Images { set; get; }

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
        /// ͼƬ�б�
        /// </summary>
        /// <returns></returns>
        public IList<string> GetImages()
        {
            return Images?.Split(',').Where(x => x.IsNotNull()).Select(x => x).ToList();
        }

        public IList<string> ImagesList => GetImages();
    }
}
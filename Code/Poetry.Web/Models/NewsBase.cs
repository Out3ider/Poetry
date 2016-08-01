using System;
using System.Collections.Generic;
using System.Linq;
using Sail.Common;

namespace Poetry.Model
{
    public abstract class NewsBase : UserInfoBase
    {
       
        /// <summary>
        /// 标题图片
        /// </summary>
        [HColumn(Length = 200)]
        public string Icon { set; get; }

  

        /// <summary>
        /// 图片
        /// </summary>
        [HColumn]
        public string Images { set; get; }

        /// <summary>
        /// 是否审核
        /// </summary>
        [HColumn]
        public bool IsChecked { set; get; }

        /// <summary>
        /// 点赞
        /// </summary>
        [HColumn]
        public int Liked { set; get; }

        /// <summary>
        /// 查看次数
        /// </summary>
        [HColumn]
        public int Views { set; get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [HColumn]
        public bool IsPublish { set; get; }

        /// <summary>
        /// 评论数
        /// </summary>
        [HColumn]
        public int Comments { set; get; }

        /// <summary>
        /// 事迹内容
        /// </summary>
        [HColumn]
        public string Content { set; get; }

        /// <summary>
        /// 图片列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetImages()
        {
            return Images?.Split(',').Where(x => x.IsNotNull()).Select(x => x).ToList();
        }

        public IList<string> ImagesList => GetImages();
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using Sail.Common;

namespace Poetry.Web.Utility
{
    /// <summary>
    /// 敏感词
    /// </summary>
    public class BadWords
    {

        /// <summary>
        /// 
        /// </summary>
        public const string FileName = "~/config/敏感词.txt";
        /// <summary>
        /// 敏感词列表
        /// </summary>
        public static List<string> CurrentBadWords { set; get; } = new List<string>();


        /// <summary>
        /// 检查是否有关键字
        /// </summary>
        /// <param name="text"></param>
        /// <returns>有关键字就返回true，否则返回false</returns>
        public static bool CheckBadWords(string text) => CurrentBadWords.Any(text.Contains);


        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void Config(string fileName = FileName)
        {
            var path = fileName.FullFileName();
            var list = FileHelper.ReadTextFile(path).Split(new[] { '\r', '\n' });
            CurrentBadWords.Clear();
            CurrentBadWords = list.Where<string>(x => x.IsNotNull()).ToList();
        }

    }
}
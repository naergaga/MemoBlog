using MemoBlog.Data;
using MemoBlog.Models.emoji;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MemoBlog.Common
{
    public class EmoticonCommon
    {
        /// <summary>
        /// 获取本地未添加表情
        /// </summary>
        /// <param name="serverPath">文件夹</param>
        /// <param name="emList">服务器上的表情</param>
        /// <returns>待添加表情</returns>
        public static List<string> GetFileList(string serverPath,List<Emoticon> emList)
        {
            //从本地文件夹中读取
            var fileArr = Directory.GetFiles(serverPath+"/images/emoji");
            var list = new List<String>();
            foreach (var item in fileArr)
            {
                string fileName = Path.GetFileName(item);
                if (!Contains(emList, fileName))
                {
                    list.Add(fileName);
                }
            }
            return list;
        }

        //判断表情是否在list
        public static bool Contains(List<Emoticon> emList,string path)
        {
            foreach (var item in emList)
            {
                if (item.Path==path)
                {
                    return true;
                }
            }
            return false;
        }

        //判断表情是否在list
        public static Emoticon GetByTitle(List<Emoticon> emList, string title)
        {
            foreach (var item in emList)
            {
                if (item.Title == title)
                {
                    return item;
                }
            }
            return null;
        }

        //parse content
        public static string ParseContent(List<Emoticon> emList, string content)
        {
            Regex reg = new Regex(@"\[\w+\]");
            MatchCollection coll = reg.Matches(content);

            if (coll.Count==0)
            {
                return content;
            }

            Match item1=null;
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < coll.Count; i++)
            {
                int start = item1 == null ? 0 : item1.Index+item1.Length;
                item1 = coll[i];
                var length = item1.Index - start;
                sb.Append(content.Substring(start, length));

                String emStr = item1.Value.Substring(1, item1.Value.Length - 2);
                var emItem = GetByTitle(emList, emStr);
                if (emItem == null)
                {
                    sb.Append(item1.Value);
                    continue;
                }
                sb.AppendFormat("<img src='/images/emoji/{0}'>", emItem.Path);
            }
            sb.Append(content.Substring(item1.Index + item1.Value.Length));
            return sb.ToString();
        }

        public static string GetImagePath(string webRootPath)
        {
            return webRootPath + "/images/emoji";
        }
    }
}

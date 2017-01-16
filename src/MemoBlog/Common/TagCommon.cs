using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoBlog.Models;

namespace MemoBlog.Common
{
    public class TagCommon
    {
        public static List<string> GetTags(string tags)
        {
            var arr1 = tags.Trim().Split(';');
            var list = new List<string>();
            for (int i = 0; i < arr1.Length; i++)
            {
                var str = arr1[i].Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    list.Add(str);
                }
            }
            return list;
        }

        public static bool Contains(IEnumerable<Tag> taglist, string item)
        {
            foreach (var tag in taglist)
            {
                if (tag.Name == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

using MemoBlog.Models;
using MemoBlog.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Common
{
    public class CommentCommon
    {

        public static List<CommentView> ParseList(List<Comment> list)
        {
            bool[] taked = new bool[list.Count()];
            List<CommentView> list2 = new List<CommentView>();
            for (int i=0;i<list.Count();i++)
            {
                var item = list[i];
                if (item.Pid==0)
                {
                    list2.Add(new CommentView(item));
                    taked[i] = true;
                }
            }

            for (int i = 0; i < list2.Count(); i++)
            {
                FindSubList(list2[i],list, taked);
            }
            return list2;
        }

        private static void FindSubList(CommentView cv,List<Comment> list, bool[] taked)
        {
            cv.SubList = new List<CommentView>();
            for (int i = 0; i < list.Count(); i++)
            {
                var item = list[i];
                if (!taked[i]&&item.Pid==cv.Id)
                {
                    var cvNew = new CommentView(item);
                    cv.SubList.Add(cvNew);
                    taked[i] = true;
                    FindSubList(cvNew, list, taked);
                }
            }
        }
    }
}

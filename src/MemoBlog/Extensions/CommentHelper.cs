using MemoBlog.Common;
using MemoBlog.Models.emoji;
using MemoBlog.Models.View;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoBlog.Extensions
{
    public static class CommentHelper
    {


        public static HtmlString RenderCommentItem(this IHtmlHelper helper, List<CommentView> list, string userName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                RenderItem(sb, item, userName);
            }
            return new HtmlString(sb.ToString());
        }

        private static void RenderItem(StringBuilder sb, CommentView cv,string userName)
        {
            string str = "<a data-pid={2} href='javascript:void(0)' class='delCommentBtn'>删除</a>";

            sb.Append("<div class='commentItem'>");
            sb.AppendFormat("<div>" +
                "<div>{1}</div>" +
                "<div class='right'>{0}" +
                (userName==cv.User.UserName?str:string.Empty) +
                "<a data-pid='{2}' href='javascript:void(0)' class='addCommentBtn'>回复</a></div>" +
                "</div>", cv.User.UserName, cv.Content,cv.Id);
            foreach (var item in cv.SubList)
            {
                RenderItem(sb, item, userName);
            }
            sb.Append("</div>");
        }

        public static HtmlString ParseContent(this IHtmlHelper helper, List<Emoticon> emList, string content)
        {
             return new HtmlString(EmoticonCommon.ParseContent(emList, content));
        }
    }
}

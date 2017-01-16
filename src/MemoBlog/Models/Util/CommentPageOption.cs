using MemoBlog.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models.Util
{
    public class CommentPageOption
    {
        public int PageCount { get; set; }
        public int PageSize { get; set; } = 20;
        public int Page { get; set; }

        public List<CommentView> Data { get; set; }
    }
}

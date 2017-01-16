using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models.Util
{
    public class PageOption
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageCount { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary Routes { get; set; }

        public void AddPageCount(int v)
        {
            this.PageCount = (int)Math.Ceiling(v / (double)PageSize);
        }
    }
}

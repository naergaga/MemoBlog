using MemoBlog.Models.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Components
{
    public class PageViewComponent : ViewComponent
    {
        public PageViewComponent()
        {

        }

        public IViewComponentResult Invoke(PageOption pageOption)
        {
            return View(pageOption);
        }
    }
}

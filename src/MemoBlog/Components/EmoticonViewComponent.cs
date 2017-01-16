using MemoBlog.Data;
using MemoBlog.Models.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Components
{
    public class EmoticonViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public EmoticonViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewData["emList"] = _context.Emoticons.ToList();
            return View();
        }
    }
}

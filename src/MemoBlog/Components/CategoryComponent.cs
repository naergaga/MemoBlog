using MemoBlog.Data;
using MemoBlog.Models.emoji;
using MemoBlog.Models.Util;
using MemoBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public CategoryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string userName)
        {
            var list = _context.Category.ToList();
            RouteValueDictionary routes = new RouteValueDictionary();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                routes.Add("userName", userName);
            }
            routes.Add("pagenum", null);
            ViewBag.Routes = routes;
            return View(list);
        }
    }
}

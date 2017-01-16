using MemoBlog.Data;
using MemoBlog.Models.emoji;
using MemoBlog.Models.Util;
using MemoBlog.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Components
{
    public class SongViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public SongViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            int maxId = _context.Songs.Max(t=>t.Id);
            Random rand1 = new Random();
            var item = _context.Songs.FirstOrDefault(t=>t.Id>=rand1.Next(maxId));
            return View(item);
        }
    }
}

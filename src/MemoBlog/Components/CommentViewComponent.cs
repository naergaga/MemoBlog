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
    public class CommentViewComponent : ViewComponent
    {
        private CommentService commentService;
        private ApplicationDbContext _context;

        public CommentViewComponent(CommentService commentService, ApplicationDbContext context)
        {
            this.commentService = commentService;
            _context = context;
        }

        public IViewComponentResult Invoke(int postId)
        {
            var list = commentService.GetListByPost(postId);
            ViewBag.emList = _context.Emoticons.ToList();
            return View(list);
        }
    }
}

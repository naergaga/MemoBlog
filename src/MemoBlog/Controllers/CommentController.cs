using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemoBlog.Models;
using MemoBlog.Services;
using MemoBlog.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using MemoBlog.Common;
using MemoBlog.Models.Util;
using MemoBlog.Models.View;

namespace MemoBlog.Controllers
{
    public class CommentController : Controller
    {
        CommentService service;
        UserService userService;
        private ApplicationDbContext _context;

        public CommentController(CommentService service, UserService userService, ApplicationDbContext context)
        {
            this.service = service;
            this.userService = userService;
            _context = context;
        }

        [HttpPost]
        public IActionResult Add(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreateTime = DateTime.Now;
                comment.UserId = userService.GetByUserName(User.Identity.Name).Id;
                service.Add(comment);
            }
            return RedirectToAction("Item", "Blog", new { id = comment.PostId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = service.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Comment comment)
        {
            var item = service.Get(comment.Id);
            if (item == null) return NotFound();
            item.Content = comment.Content;
            service.Edit(item);
            return RedirectToAction("MyComment", "Blog");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = service.Delete(id, User.Identity.Name);
            return Json(new { result = true });
        }

        [HttpPost]
        public JsonResult GetComments(int postId, int page)
        {
            int count = _context.Comments.Where(t => t.PostId == postId && t.Pid == 0).Count();
            int pageSize = 20;
            int totalPage = (int)Math.Ceiling(count / (double)pageSize);
            if (page < 1 || page > totalPage)
            {
                //!ERROR
            }

            CommentPageOption co = new CommentPageOption
            {
                PageCount = totalPage,
                PageSize = pageSize,
                Page = page,
            };

            var list = _context.Comments.FromSql("with temp (Id,Pid,Content,PostId,CreateTime,UserId) as(select Id,Pid,Content,PostId,CreateTime,UserId from Comments c where Pid=0 and c.PostId=@postId order by c.CreateTime offset @skipNum rows fetch next @takeNum row only¡¡union all¡¡select a.Id,a.Pid,a.Content,a.PostId,a.CreateTime,a.UserId from Comments a¡¡inner join temp on a.Pid=temp.Id)select * from temp", new SqlParameter("@postId", postId), new SqlParameter("@skipNum", (page - 1) * pageSize), new SqlParameter("@takeNum", pageSize)).ToList();

            foreach (var item in list)
            {
                item.User = _context.Users.SingleOrDefault(u => u.Id == item.UserId);
            }

            List<CommentView> list2 = CommentCommon.ParseList(list);

            co.Data = list2;
            return Json(co);
        }

        [HttpPost]
        public JsonResult GetEmoji()
        {
            return Json(_context.Emoticons.ToList());
        }
    }
}
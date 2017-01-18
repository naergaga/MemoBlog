using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MemoBlog.Services;
using MemoBlog.Models;
using MemoBlog.Models.Util;
using Microsoft.AspNetCore.Routing;
using MemoBlog.Common;
using MemoBlog.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MemoBlog.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private PostService postService;
        private UserService userService;
        private TagService tagService;
        private CommentService cService;
        private ApplicationDbContext _context;

        public BlogController(PostService postService, UserService userService, TagService tagService, CommentService cService, ApplicationDbContext context)
        {
            this.postService = postService;
            this.userService = userService;
            this.tagService = tagService;
            this.cService = cService;
            _context = context;
        }

        [Route("i", Order = 1)]
        [Route("i/page{pagenum}", Order = 0)]
        public IActionResult Index(int? pagenum)
        {
            var userName = User.Identity.Name;
            PageOption po = new PageOption
            {
                ActionName = "Index",
                ControllerName = "Blog",
                CurrentPage = pagenum ?? 1
            };
            po.AddPageCount(postService.CountByUser(userName));
            //po.Routes = new RouteValueDictionary();

            var list = postService.GetPagePostViewList(userName, po);
            ViewData["postList"] = list;
            ViewData["userTags"] = tagService.getListByUser(userName);
            ViewData["pageOption"] = po;
            return View();
        }

        [Route("i/tag/{id}", Order = 1)]
        [Route("i/tag/{id}/page{pagenum}", Order = 0)]
        public IActionResult TagIndex(int id, int? pagenum)
        {
            var userName = User.Identity.Name;
            PageOption po = new PageOption
            {
                ActionName = "TagIndex",
                ControllerName = "Blog",
                CurrentPage = pagenum ?? 1
            };
            po.AddPageCount(postService.CountByTagAndUser(userName, id));
            //po.Routes = new RouteValueDictionary();
            //po.Routes.Add("id", id);

            var list = postService.GetPagePostViewListByTag(userName, id, po);
            ViewData["postList"] = list;
            ViewData["userTags"] = tagService.getListByUser(userName);
            ViewData["pageOption"] = po;
            return View("Index");
        }

        [Route("c/{name}", Order = 3)]
        [Route("c/{name}/page{pagenum}", Order = 2)]
        [Route("u/{userName}/c/{name}", Order = 1)]
        [Route("u/{userName}/c/{name}/page{pagenum}", Order = 0)]
        [AllowAnonymous]
        public IActionResult CategoryIndex(string name, string userName, int? pagenum)
        {
            var user = _context.Users.SingleOrDefault(t => t.UserName == userName);
            PageOption po = new PageOption
            {
                ActionName = "CategoryIndex",
                ControllerName = "Blog",
                CurrentPage = pagenum ?? 1
            };
            var cateItem = _context.Category.FirstOrDefault(t => t.Name == name);
            if (cateItem == null)
            {
                return NotFound();
            }

            //linq数据
            Expression<Func<Post, bool>> exp;
            if (user != null)
            {

                if (user.UserName == User.Identity.Name)
                {
                    exp = t => t.CategoryId == cateItem.Id && t.UserId == user.Id;
                }
                else
                {
                    exp = t=> t.CategoryId == cateItem.Id && t.UserId == user.Id && t.IsPublic==true;
                }

            }
            else
            {
                exp=t => t.CategoryId == cateItem.Id && t.IsPublic == true;
            }
            //分页Count
            po.AddPageCount(_context.Posts.Count(exp));
            //验证po是否正确
            if (!po.Vaild())
            {
                return NotFound();
            }
            //分页链接生成
            //po.Routes = new RouteValueDictionary();
            //po.Routes.Add("name", name);
            //if (user != null)
            //{
            //    po.Routes.Add("userName", user.UserName);
            //}
            //分页数据
            var postList = _context.Posts.Where(exp).OrderBy(t => t.CreateTime)
                .Skip((po.CurrentPage - 1) * po.PageSize)
                .Take(po.PageSize)
                .ToList();
            var list = postService.GetPostView(postList);
            ViewData["postList"] = list;
            ViewData["pageOption"] = po;
            return View("CategoryIndex");
        }

        [HttpGet]
        public IActionResult Add()
        {
            var list = _context.Category.ToList();
            var selectList = new SelectList(list, "Id", "Name");
            ViewBag.Category = selectList;
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            string userId = userService.GetIdByUserName(User.Identity.Name);
            if (!postService.IsPostByUserId(id, userId))
                return NotFound();
            var item = postService.GetPostView(id);
            //category list
            var list = _context.Category.ToList();
            ViewBag.Category = new SelectList(list, "Id", "Name", item.CategoryId);
            return View(item);
        }

        [HttpPost]
        public IActionResult Add(Post post, string tags)
        {
            if (ModelState.IsValid)
            {
                var user = userService.GetByUserName(User.Identity.Name);
                if (user == null)
                    throw new Exception("not get user");
                post.UserId = user.Id;
                post.CreateTime = DateTime.Now;
                if (post.Content == null) post.Content = string.Empty;
                else
                {
                    post.Content = PostCommon.ParseImageSrc(HttpContext.Request.PathBase.Value, post.Content);
                }
                postService.Add(post, tags);
                return RedirectToAction("index");
            }
            return View(post);
        }

        [HttpPost]
        public IActionResult Edit(Post post, string tags)
        {
            if (ModelState.IsValid)
            {
                string userId = userService.GetIdByUserName(User.Identity.Name);
                if (!postService.IsPostByUserId(post.Id, userId))
                    return NotFound();
                var item = postService.Get(post.Id);
                if (!string.IsNullOrWhiteSpace(post.Content))
                {
                    item.Content = post.Content;
                }
                item.Title = post.Title;
                item.IsPublic = post.IsPublic;
                item.CategoryId = post.CategoryId;
                postService.Edit(item, tags);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var item = postService.Get(id);
            bool result = false;
            string userId = userService.GetIdByUserName(User.Identity.Name);
            if (postService.IsPostByUserId(id, userId))
            {
                result = postService.Delete(item);
            }
            return Json(new
            {
                result = result
            });
        }

        [AllowAnonymous]
        public IActionResult Item(int id)
        {
            var postItem = _context.Posts.Include(p=>p.User).SingleOrDefault(p=>p.Id==id);
            if (postItem == null || (!postItem.IsPublic && User.Identity.Name != postItem.User.UserName))
            {
                return NotFound();
            }
            if (postItem == null) { return NotFound(); }
            ViewData["Comments"] = cService.GetListByPost(id);
            return View(postItem);
        }

        public IActionResult MyComment()
        {
            var list = cService.GetListByUserName(User.Identity.Name);
            return View(list);
        }
    }
}
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

namespace MemoBlog.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private PostService postService;
        private UserService userService;
        private TagService tagService;
        private CommentService cService;

        public BlogController(PostService postService, UserService userService, TagService tagService, CommentService cService)
        {
            this.postService = postService;
            this.userService = userService;
            this.tagService = tagService;
            this.cService = cService;
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
            po.Routes = new RouteValueDictionary();

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
            po.Routes = new RouteValueDictionary();
            po.Routes.Add("id", id);

            var list = postService.GetPagePostViewListByTag(userName, id, po);
            ViewData["postList"] = list;
            ViewData["userTags"] = tagService.getListByUser(userName);
            ViewData["pageOption"] = po;
            return View("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            string userId = userService.GetIdByUserName(User.Identity.Name);
            if (!postService.IsPostByUserId(id, userId))
                return NotFound();
            var item = postService.GetPostView(id);
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
            var postItem = postService.Get(id);
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
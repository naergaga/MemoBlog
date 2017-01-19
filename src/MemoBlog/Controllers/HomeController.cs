using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemoBlog.Services;
using MemoBlog.Models.Util;
using Microsoft.AspNetCore.Routing;
using MemoBlog.Common;
using MemoBlog.Data;
using Microsoft.AspNetCore.Identity;
using MemoBlog.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace MemoBlog.Controllers
{
    public class HomeController : Controller
    {
        private PostService postService;
        private UserService userService;
        private TagService tagService;
        private ApplicationDbContext _context;
        private IHostingEnvironment _appEnv;
        private RoleManager<AppRole> _rm;
        private UserManager<ApplicationUser> _um;

        public HomeController(ApplicationDbContext context, PostService postService, UserService userService, TagService tagService, IHostingEnvironment appEnv, RoleManager<AppRole> rm, UserManager<ApplicationUser> um)
        {
            _context = context;
            this.postService = postService;
            this.userService = userService;
            this.tagService = tagService;
            _appEnv = appEnv;
            _rm = rm;
            _um = um;
        }

        [Route("", Order = 1)]
        [Route("/page{pagenum}", Order = 0)]
        public IActionResult Index(int? pagenum)
        {
            PageOption po = new PageOption
            {
                ActionName = "Index",
                ControllerName = "Blog",
                CurrentPage = pagenum ?? 1
            };
            po.AddPageCount(postService.CountByPublic());
            //po.Routes = new RouteValueDictionary();

            var list = postService.GetPagePostViewListInPublic(po);
            ViewData["postList"] = list;
            ViewData["pageOption"] = po;
            return View();
        }

        [Route("/u/{author}", Order = 1)]
        [Route("/u/{author}/page{pagenum}", Order = 0)]
        public IActionResult Index(string author, int? pagenum)
        {
            PageOption po = new PageOption
            {
                ActionName = "Index",
                ControllerName = "Blog",
                CurrentPage = pagenum ?? 1
            };
            po.AddPageCount(postService.CountByPublic(author));
            //po.Routes = new RouteValueDictionary();

            ViewBag.Author = author;
            var list = postService.GetPagePostViewListInPublic(po, author);
            ViewData["postList"] = list;
            ViewData["pageOption"] = po;
            return View();
        }

        public IActionResult Search(string s, int? pageNum, int? pageSize)
        {
            var po = new PageOption
            {
                PageSize = pageSize ?? 50,
                CurrentPage = pageNum ?? 1,
            };
            var skipNum = po.PageSize * (po.CurrentPage - 1);

            var query1 = _context.Posts.FromSql("SELECT DISTINCT [p].[Id], [p].[CategoryId], [p].[Content], [p].[CreateTime]," 
                + "[p].[IsPublic], [p].[Title], [p].[UserId]"
                + " FROM[Posts] AS[p]"
                + " left JOIN[PostTags] AS[pt] ON[p].[Id] = [pt].[PostId]"
                + " left JOIN[Tags] AS[t] ON[pt].[TagId] = [t].[Id]"
                + " INNER JOIN[AspNetUsers] AS[u] ON[p].[UserId] = [u].[Id]"
                + " WHERE([p].[Title] LIKE(N'%' + {0}) + N'%' OR[t].[Name] LIKE(N'%' + {1}) + N'%')"
                 +" AND(([p].[IsPublic] = 1) OR[u].[UserName] ={2})"
                , s,s,User.Identity.Name);

            po.AddPageCount(query1.Count());
            if (!po.Vaild())
            {
                return NotFound();
            } 

            ViewBag.PageOption = po;
            var postList = query1.OrderByDescending(p => p.CreateTime).Skip(skipNum).Take(po.PageSize).ToList();
            var list = postService.GetPostView(postList);
            return View(list);
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        public async Task<IActionResult> Config()
        {
            string[] arrs = new string[] { "admin", "user" };
            foreach (var item in arrs)
            {
                if (!await _rm.RoleExistsAsync(item))
                {
                    await _rm.CreateAsync(new AppRole { Name = item });
                }
            }

            var user = _context.Users.First();
            if (!await _um.IsInRoleAsync(user, "admin"))
            {
                await _um.AddToRoleAsync(user, "admin");
            }
            ViewBag.RoleList = _context.Roles.ToList();
            return View();
        }
    }
}

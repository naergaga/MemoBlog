using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MemoBlog.Data;
using MemoBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MemoBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private ApplicationDbContext _context;
        private RoleManager<AppRole> _roleManager;

        public RoleController(ApplicationDbContext context, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: Role
        public ActionResult Index()
        {
            var list = _context.Roles.ToList();
            return View(list);
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name")]AppRole role)
        {
            try
            {
                await _roleManager.CreateAsync(role);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Edit/5
        [HttpGet("Role/Edit/{name}")]
        public ActionResult Edit(string name)
        {
            var role = _context.Roles.SingleOrDefault(t => t.Name == name);
            return View(role);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Name")]AppRole role)
        {
            try
            {
                var item = _context.Roles.SingleOrDefault(t => t.Id == role.Id);
                await _roleManager.SetRoleNameAsync(item, role.Name);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Delete/5
        [HttpGet("Role/Delete/{name}")]
        public ActionResult Delete(string name)
        {
            var role = _context.Roles.SingleOrDefault(t => t.Name == name);
            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                var item = _context.Roles.SingleOrDefault(t=>t.Id==id);
                await _roleManager.DeleteAsync(item);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
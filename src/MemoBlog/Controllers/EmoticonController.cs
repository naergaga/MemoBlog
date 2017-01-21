using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemoBlog.Models.emoji;
using Microsoft.EntityFrameworkCore;
using MemoBlog.Data;
using MemoBlog.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MemoBlog.Controllers
{
    [Authorize(Roles ="Admin")]
    public class EmoticonController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnv;

        public EmoticonController(ApplicationDbContext context, IHostingEnvironment appEnv)
        {
            _context = context;
            _appEnv = appEnv;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var emojiList = _context.Emoticons.ToList();
            var fileNameList = EmoticonCommon.GetFileList(_appEnv.WebRootPath, emojiList);
            ViewData["fileList"] = fileNameList;

            return View(emojiList);
        }

        [HttpGet]
        public IActionResult ImportAll()
        {
            var emojiList = _context.Emoticons.ToList();
            var fileNameList = EmoticonCommon.GetFileList(_appEnv.WebRootPath, emojiList);
            int num = _context.Emoticons.Count();

            foreach (var item in fileNameList)
            {
                Emoticon em = new Emoticon
                {
                    Title = "表情" + ++num,
                    Path = item
                };
                _context.Emoticons.Add(em);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExistTitle(string title)
        {
            var result = _context.Emoticons.Any(t => t.Title == title);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string Title, IFormFile image)
        {
            if (image == null)
            {
                return View();
            }
            var emoticon = new Emoticon { Title = Title };
            emoticon.Path = Title + Path.GetExtension(image.FileName);
            image.CopyTo(new FileStream(EmoticonCommon.GetImagePath(_appEnv.WebRootPath)+"/" + emoticon.Path, FileMode.CreateNew));
            _context.Add(emoticon);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Emoticons.SingleOrDefaultAsync(e => e.Id == id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title")]Emoticon emoticon)
        {
            var item = _context.Emoticons.SingleOrDefault(t => t.Id == id);
            if (id != emoticon.Id || item == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    item.Title = emoticon.Title;
                    _context.Update(item);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Emoticons
                .SingleOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var em = await _context.Emoticons.SingleOrDefaultAsync(m => m.Id == id);
            _context.Emoticons.Remove(em);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}

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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MemoBlog.Controllers
{
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

            return View();
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
                    Title = "表情" + num++,
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
        [ValidateAntiForgeryToken]
        public IActionResult Add(Emoticon emoticon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emoticon);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Emoticons.SingleOrDefaultAsync(e => e.Id == id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Path")]Emoticon emoticon)
        {
            if (id != emoticon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emoticon);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(emoticon);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MemoBlog.Data;
using MemoBlog.Models.Memo;
using Microsoft.AspNetCore.Authorization;

namespace MemoBlog.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Note
        public async Task<IActionResult> Index()
        {
            return View(await (
                from n in _context.Note
                join u in _context.Users on n.UserId equals u.Id
                where u.UserName == User.Identity.Name
                select n).ToListAsync());
        }

        // GET: Note/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var note = (from n in _context.Note
                        join u in _context.Users on n.UserId equals u.Id
                        where u.UserName == User.Identity.Name && n.Id == (int)id
                        select n).SingleOrDefault();

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Note/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,State")] Note note)
        {
            if (ModelState.IsValid)
            {
                note.CreateTime = DateTime.Now;
                note.UserId = _context.Users.SingleOrDefault(u => u.UserName == User.Identity.Name).Id;
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // GET: Note/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.SingleOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,State")] Note note)
        {
            var note1 = (from n in _context.Note
                               join u in _context.Users on n.UserId equals u.Id
                               where u.UserName == User.Identity.Name && n.Id == id
                               select n).SingleOrDefault();
            if (id != note.Id || note1==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    note1.Content = note.Content;
                    note1.State = note.State;
                    _context.Update(note1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // GET: Note/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .SingleOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (IsNoteAndUser(id, User.Identity.Name))
            {
                var note = await _context.Note.SingleOrDefaultAsync(m => m.Id == id);
                _context.Note.Remove(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }

        private bool IsNoteAndUser(int id, string userName)
        {
            return (from n in _context.Note
                    join u in _context.Users on n.UserId equals u.Id
                    where u.UserName == userName && n.Id == id
                    select n).Count() == 1;
        }
    }
}

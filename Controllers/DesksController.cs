using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeskJockey.Data;
using DeskJockey.Models;

namespace DeskJockey.Controllers
{
    public class DesksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Desks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Desks.ToListAsync());
        }

        // GET: Desks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var desk = await _context.Desks
                .FirstOrDefaultAsync(m => m.DeskId == id);
            if (desk == null)
            {
                return NotFound();
            }

            return View(desk);
        }

        // GET: Desks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Desks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeskId,DeskNumber,Status")] Desk desk)
        {
            if (ModelState.IsValid)
            {
                _context.Add(desk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(desk);
        }

        // GET: Desks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var desk = await _context.Desks.FindAsync(id);
            if (desk == null)
            {
                return NotFound();
            }
            return View(desk);
        }

        // POST: Desks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeskId,DeskNumber,Status")] Desk desk)
        {
            if (id != desk.DeskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(desk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeskExists(desk.DeskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(desk);
        }

        // GET: Desks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var desk = await _context.Desks
                .FirstOrDefaultAsync(m => m.DeskId == id);
            if (desk == null)
            {
                return NotFound();
            }

            return View(desk);
        }

        // POST: Desks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var desk = await _context.Desks.FindAsync(id);
            if (desk != null)
            {
                _context.Desks.Remove(desk);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeskExists(int id)
        {
            return _context.Desks.Any(e => e.DeskId == id);
        }
    }
}

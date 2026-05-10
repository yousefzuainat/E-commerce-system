using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecommerce_system.Data;
using ecommerce_system.Models;

namespace ecommerce_system.Controllers
{
    public class ProudectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProudectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.proudects.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proudect = await _context.proudects
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proudect == null)
            {
                return NotFound();
            }

            return View(proudect);
        }

        // GET: Proudects/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descrption,Img,Price,CategoryId")] Proudect proudect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proudect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", proudect.CategoryId);
            return View(proudect);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proudect = await _context.proudects.FindAsync(id);
            if (proudect == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", proudect.CategoryId);
            return View(proudect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descrption,Img,Price,CategoryId")] Proudect proudect)
        {
            if (id != proudect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proudect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProudectExists(proudect.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", proudect.CategoryId);
            return View(proudect);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proudect = await _context.proudects
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proudect == null)
            {
                return NotFound();
            }

            return View(proudect);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proudect = await _context.proudects.FindAsync(id);
            if (proudect != null)
            {
                _context.proudects.Remove(proudect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProudectExists(int id)
        {
            return _context.proudects.Any(e => e.Id == id);
        }
    }
}

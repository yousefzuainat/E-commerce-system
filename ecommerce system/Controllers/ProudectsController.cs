using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecommerce_system.Data;
using ecommerce_system.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using ecommerce_system.ViewModel;

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
            ViewData["CategoryId"] = new SelectList(_context.categories, dataValueField: "Id", dataTextField: "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProudectCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? imgPath = null;
                if (model.Upload != null && model.Upload.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Upload.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Upload.CopyToAsync(stream);
                    }

                    imgPath = "/images/products/" + fileName;
                }

                var proudect = new Proudect
                {
                    Name = model.Name,
                    Descrption = model.Descrption,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Img = imgPath
                };

                _context.Add(proudect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, dataValueField: "Id", dataTextField: "Name", selectedValue: model.CategoryId);
            return View(model);
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

            var model = new ProudectEditViewModel
            {
                Id = proudect.Id,
                Name = proudect.Name,
                Descrption = proudect.Descrption,
                Price = proudect.Price,
                CategoryId = proudect.CategoryId,
                ExistingImg = proudect.Img
            };

            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", proudect.CategoryId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProudectEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var proudect = await _context.proudects.FindAsync(model.Id);
                    if (proudect == null)
                    {
                        return NotFound();
                    }

                    proudect.Name = model.Name;
                    proudect.Descrption = model.Descrption;
                    proudect.Price = model.Price;
                    proudect.CategoryId = model.CategoryId;

                    if (model.Upload != null && model.Upload.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Upload.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Upload.CopyToAsync(stream);
                        }

                        proudect.Img = "/images/products/" + fileName;
                    }

                    _context.Update(proudect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProudectExists(model.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", model.CategoryId);
            return View(model);
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

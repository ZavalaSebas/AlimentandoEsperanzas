using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlimentandoEsperanzas.Models;

namespace AlimentandoEsperanzas.Controllers
{
    public class ItemcategoriesController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public ItemcategoriesController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Itemcategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Itemcategories.ToListAsync());
        }

        // GET: Itemcategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemcategory = await _context.Itemcategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemcategory == null)
            {
                return NotFound();
            }

            return View(itemcategory);
        }

        // GET: Itemcategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Itemcategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] Itemcategory itemcategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(itemcategory);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Categoria creada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                await LogError($"{ex}");
            }
            return View(itemcategory);
        }

        // GET: Itemcategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemcategory = await _context.Itemcategories.FindAsync(id);
            if (itemcategory == null)
            {
                return NotFound();
            }
            return View(itemcategory);
        }

        // POST: Itemcategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] Itemcategory itemcategory)
        {
            if (id != itemcategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemcategory);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Categoria actualizada exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    await LogError("Error al actualizar la categoría del item");
                    if (!ItemcategoryExists(itemcategory.Id))
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
            return View(itemcategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Itemcategory itemcategory = _context.Itemcategories.Where(m => m.Id == id).FirstOrDefault();
            if (itemcategory == null)
            {
                return NotFound();
            }

            return PartialView("_ItemCategoryDelete", itemcategory);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Itemcategory itemcategory)
        {
            try
            {
                if (itemcategory == null)
                {
                    return NotFound();
                }

                var items = _context.Items.Where(i => i.Id == itemcategory.Id).ToList();

                if (items.Any())
                {
                    TempData["ErrorMessage"] = "No se puede eliminar la categoría de ítems porque hay ítems asociados.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Itemcategories.Remove(itemcategory);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Se ha eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                await LogError($"{ex}");
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LogError(string ex)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var errorlog = new Errorlog
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex,
                    UserId = userId.Value
                };

                _context.Errorlogs.Add(errorlog);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("No se pudo obtener el ID de usuario de la sesión");
            }
        }

        private bool ItemcategoryExists(int id)
        {
            return _context.Itemcategories.Any(e => e.Id == id);
        }
    }
}

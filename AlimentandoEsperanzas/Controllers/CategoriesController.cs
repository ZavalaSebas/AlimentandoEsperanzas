using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlimentandoEsperanzas.Models;
using System.Data;

namespace AlimentandoEsperanzas.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public CategoriesController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = _context.Categories.Where(m => m.CategoryId == id).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }

            return PartialView("_CategoriesDetails", category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Category1")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Se ha agregado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await LogError($"{ex}");
                    return PartialView("_CategoriesCreate", category);
                }
            }
            return View(category);
            
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = _context.Categories.Where(m => m.CategoryId == id).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Se ha actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    await LogError("Error al actualizar la categoría");
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = _context.Categories.Where(m => m.CategoryId == id).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }

            return PartialView("_CategoriesDelete", category);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Category category)
        {
            try
            {
                if (category == null)
                {
                    return NotFound();
                }

                var donations = _context.Donations.Where(d => d.CategoryId == category.CategoryId).ToList();

                if (donations.Any())
                {
                    TempData["ErrorMessage"] = "No se puede eliminar la categoría porque tiene donaciones asociadas.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Categories.Remove(category);
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


        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}

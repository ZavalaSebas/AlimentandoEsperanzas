using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlimentandoEsperanzas.Models;
using OfficeOpenXml;

namespace AlimentandoEsperanzas.Controllers
{
    public class ItemsController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public ItemsController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var alimentandoesperanzasContext = _context.Items.Include(i => i.CategoryNavigation);
            return View(await alimentandoesperanzasContext.ToListAsync());
        }


        // Acción para exportar ítems a Excel
        public IActionResult ExportItemsToExcel()
        {
            var items = _context.Items.ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Debes tener instalado EPPlus para usar esta funcionalidad

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Items");
                worksheet.Cells.LoadFromCollection(items, true);

                // Headers
                var properties = items.FirstOrDefault()?.GetType().GetProperties();
                if (properties != null)
                {
                    for (int i = 1; i <= properties.Length; i++)
                    {
                        worksheet.Cells[1, i].Value = properties[i - 1].Name;
                    }
                }


                // Guardar el archivo Excel en la memoria
                var stream = new MemoryStream(package.GetAsByteArray());

                // Devolver el archivo Excel como un archivo para descargar
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Items.xlsx");
            }
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.CategoryNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.Itemcategories, "Id", "Description");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Quantity,Category")] Item item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(item);
                await _context.SaveChangesAsync();
                await LogAction($"Registro del producto {item.Description}","Inventario");
                TempData["Mensaje"] = "Producto agregado exitosamente";
                return RedirectToAction(nameof(Index));
                 }
                ViewData["Category"] = new SelectList(_context.Itemcategories, "Id", "Id", item.Category);
                return View(item);
            }
            catch (Exception ex)
            {
                return View(item);
            }
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_context.Itemcategories, "Id", "Description", item.Category);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Quantity,Category")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                    await LogAction($"Actualización del producto {item.Description}", "Inventario");
                    TempData["Mensaje"] = "Producto actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["Category"] = new SelectList(_context.Itemcategories, "Id", "Id", item.Category);
            return View(item);
        }
    
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Item item = _context.Items.Include(i => i.CategoryNavigation).Where(m => m.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            return PartialView("_ItemDelete", item);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Item item)
        {
            try
            {
                if (item != null)
                {
                    await LogAction($"Eliminación de producto {item.Description}", "Inventario");
                    _context.Items.Remove(item);
                }

                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Producto eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await LogError($"{ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LogAction(string action, string document)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var actionLog = new Actionlog
                {
                    Action = action,
                    Date = DateTime.Now,
                    Document = document,
                    UserId = userId.Value
                };

                _context.Actionlogs.Add(actionLog);
                await _context.SaveChangesAsync();
            }
            else
            {
                await LogError("No se pudo obtener el ID de usuario de la sesión");
                throw new InvalidOperationException("No se pudo obtener el ID de usuario de la sesión");
            }
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

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}

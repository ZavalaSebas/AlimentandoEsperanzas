using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlimentandoEsperanzas.Models;

namespace AlimentandoEsperanzas.Controllers
{
    public class DonorsController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public DonorsController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // Acción para exportar donantes a Excel
        public IActionResult ExportDonorsToExcel()
        {
            var donors = _context.Donors.ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Debes tener instalado EPPlus para usar esta funcionalidad

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Donantes");
                worksheet.Cells.LoadFromCollection(donors, true);

                // Headers
                for (int i = 1; i <= donors.Count(); i++)
                {
                    worksheet.Cells[1, i].Value = donors[i - 1].GetType().GetProperties()[i - 1].Name;
                }

                // Guardar el archivo Excel en la memoria
                var stream = new MemoryStream(package.GetAsByteArray());

                // Devolver el archivo Excel como un archivo para descargar
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Donantes.xlsx");
            }
        }

        // GET: Donors
        public async Task<IActionResult> Index()
        {
            var alimentandoesperanzasContext = _context.Donors.Include(d => d.IdentificationTypeNavigation);
            return View(await alimentandoesperanzasContext.ToListAsync());
        }

        // GET: Donors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors
                .Include(d => d.IdentificationTypeNavigation)
                .FirstOrDefaultAsync(m => m.DonorId == id);
            if (donor == null)
            {
                return NotFound();
            }

            return View(donor);
        }

        // GET: Donors/Create
        public IActionResult Create()
        {
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Description");
            return View();
        }

        // POST: Donors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonorId,Name,LastName,Email,IdNumber,IdentificationType,PhoneNumber,Date,Comments")] Donor donor)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Add(donor);
                    await _context.SaveChangesAsync();
                    await LogAction($"Registro del donador {donor.IdNumber}", "Donadores");
                    TempData["Mensaje"] = "Se ha agregado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    return View(donor);
                }
                
            //}
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Description", donor.IdentificationType);
            return View(donor);
        }

        // GET: Donors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors.FindAsync(id);
            if (donor == null)
            {
                return NotFound();
            }
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Description", donor.IdentificationType);
            return View(donor);
        }

        // POST: Donors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonorId,Name,LastName,Email,IdNumber,IdentificationType,PhoneNumber,Date,Comments")] Donor donor)
        {
            if (id != donor.DonorId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(donor);
                    await _context.SaveChangesAsync();
                    await LogAction($"Actualización del donador {donor.IdNumber}", "Donadores");
                    TempData["Mensaje"] = "Se ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonorExists(donor.DonorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Id", donor.IdentificationType);
            return View(donor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Donor donor = _context.Donors.Include(d => d.IdentificationType).Where(m => m.DonorId == id).FirstOrDefault();
            if (donor == null)
            {
                return NotFound();
            }

            return PartialView("_DonorDelete", donor);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Donor donor)
        {
            if (donor == null)
            {
                return NotFound();
            }

            var donations = _context.Donations.Where(d => d.DonorId == donor.DonorId).ToList();

            if (donations.Any())
            {
                TempData["ErrorMessage"] = "No se puede eliminar el donante porque hay donaciones asociadas.";
                return RedirectToAction(nameof(Index));
            }

            await LogAction($"Eliminación de donador {donor.IdNumber}", "Donadores");
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Se ha eliminado exitosamente.";
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


        private bool DonorExists(int id)
        {
            return _context.Donors.Any(e => e.DonorId == id);
        }
    }
}

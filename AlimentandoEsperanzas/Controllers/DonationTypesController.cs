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
    public class DonationTypesController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public DonationTypesController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: DonationTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donationtypes.ToListAsync());
        }

        // GET: DonationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donationtype = await _context.Donationtypes
                .FirstOrDefaultAsync(m => m.DonationTypeId == id);
            if (donationtype == null)
            {
                return NotFound();
            }

            return PartialView("_DonationtypeDetails", donationtype);
        }

        // GET: DonationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationTypeId,DonationType1")] Donationtype donationtype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(donationtype);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Se ha agregado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await LogError($"{ex}");
                    return View(donationtype);
                }

            }
            return View(donationtype);
        }

        // GET: DonationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donationtype = await _context.Donationtypes.FindAsync(id);
            if (donationtype == null)
            {
                return NotFound();
            }
            return View(donationtype);
        }

        // POST: DonationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationTypeId,DonationType1")] Donationtype donationtype)
        {
            if (id != donationtype.DonationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donationtype);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Se ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    await LogError("Error al actualizar tipo de donación");
                    if (!DonationtypeExists(donationtype.DonationTypeId))
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
            return View(donationtype);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Donationtype donationtype = _context.Donationtypes.Where(m => m.DonationTypeId == id).FirstOrDefault();
            if (donationtype == null)
            {
                return NotFound();
            }

            return PartialView("_DonationtypeDelete", donationtype);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Donationtype donationtype)
        {
            try
            {
                if (donationtype == null)
                {
                    return NotFound();
                }

                var donations = _context.Donations.Where(d => d.DonationTypeId == donationtype.DonationTypeId).ToList();

                if (donations.Any())
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el tipo de donación porque hay donaciones asociadas.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Donationtypes.Remove(donationtype);
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


        private bool DonationtypeExists(int id)
        {
            return _context.Donationtypes.Any(e => e.DonationTypeId == id);
        }
    }
}

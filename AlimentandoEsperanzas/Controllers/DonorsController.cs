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
    public class DonorsController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public DonorsController(AlimentandoesperanzasContext context)
        {
            _context = context;
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
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(donor);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Se ha agregado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    return View(donor);
                }
                
            }
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Id", donor.IdentificationType);
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
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Id", donor.IdentificationType);
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donor);
                    await _context.SaveChangesAsync();
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
            }
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Id", donor.IdentificationType);
            return View(donor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Donor donor = _context.Donors.Include(d => d.IdentificationTypeNavigation).Where(m => m.DonorId == id).FirstOrDefault();
            if (donor == null)
            {
                return NotFound();
            }

            return PartialView("_DonorDelete", donor);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Donor donor)
        {

            if (donor != null)
            {
                _context.Donors.Remove(donor);
            }

            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Se ha eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }


        private bool DonorExists(int id)
        {
            return _context.Donors.Any(e => e.DonorId == id);
        }
    }
}

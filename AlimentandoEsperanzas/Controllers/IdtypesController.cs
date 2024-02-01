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
    public class IdtypesController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public IdtypesController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Idtypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Idtypes.ToListAsync());
        }

        // GET: Idtypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idtype = await _context.Idtypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idtype == null)
            {
                return NotFound();
            }

            return View(idtype);
        }

        // GET: Idtypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Idtypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] Idtype idtype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(idtype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(idtype);
        }

        // GET: Idtypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idtype = await _context.Idtypes.FindAsync(id);
            if (idtype == null)
            {
                return NotFound();
            }
            return View(idtype);
        }

        // POST: Idtypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] Idtype idtype)
        {
            if (id != idtype.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(idtype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdtypeExists(idtype.Id))
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
            return View(idtype);
        }

        // GET: Idtypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idtype = await _context.Idtypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idtype == null)
            {
                return NotFound();
            }

            return View(idtype);
        }

        // POST: Idtypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idtype = await _context.Idtypes.FindAsync(id);
            if (idtype != null)
            {
                _context.Idtypes.Remove(idtype);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdtypeExists(int id)
        {
            return _context.Idtypes.Any(e => e.Id == id);
        }
    }
}

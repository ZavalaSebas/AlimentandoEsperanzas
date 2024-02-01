﻿using System;
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
            if (ModelState.IsValid)
            {
                _context.Add(itemcategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                }
                catch (DbUpdateConcurrencyException)
                {
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

        // GET: Itemcategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Itemcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemcategory = await _context.Itemcategories.FindAsync(id);
            if (itemcategory != null)
            {
                _context.Itemcategories.Remove(itemcategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemcategoryExists(int id)
        {
            return _context.Itemcategories.Any(e => e.Id == id);
        }
    }
}

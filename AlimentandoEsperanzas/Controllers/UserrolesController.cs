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
    public class UserrolesController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public UserrolesController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Userroles
        public async Task<IActionResult> Index()
        {
            var alimentandoesperanzasContext = _context.Userroles.Include(u => u.Role).Include(u => u.User);
            return View(await alimentandoesperanzasContext.ToListAsync());
        }

        // GET: Userroles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userrole = await _context.Userroles
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserRolesId == id);
            if (userrole == null)
            {
                return NotFound();
            }

            return View(userrole);
        }

        // GET: Userroles/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Userroles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserRolesId,RoleId,UserId")] Userrole userrole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userrole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userrole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userrole.UserId);
            return View(userrole);
        }

        // GET: Userroles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userrole = await _context.Userroles.FindAsync(id);
            if (userrole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userrole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userrole.UserId);
            return View(userrole);
        }

        // POST: Userroles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserRolesId,RoleId,UserId")] Userrole userrole)
        {
            if (id != userrole.UserRolesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userrole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserroleExists(userrole.UserRolesId))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userrole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userrole.UserId);
            return View(userrole);
        }

        // GET: Userroles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userrole = await _context.Userroles
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserRolesId == id);
            if (userrole == null)
            {
                return NotFound();
            }

            return View(userrole);
        }

        // POST: Userroles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userrole = await _context.Userroles.FindAsync(id);
            if (userrole != null)
            {
                _context.Userroles.Remove(userrole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserroleExists(int id)
        {
            return _context.Userroles.Any(e => e.UserRolesId == id);
        }
    }
}

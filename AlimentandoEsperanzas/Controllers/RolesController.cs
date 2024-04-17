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
    public class RolesController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public RolesController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return PartialView("_RoleDetails", role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,Role1")] Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(role);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Rol agregado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                await LogError($"{ex}");
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,Role1")] Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Rol actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    await LogError("Error al actualizar el rol");
                    if (!RoleExists(role.RoleId))
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
            return View(role);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Role role = _context.Roles.Where(m => m.RoleId == id).FirstOrDefault();
            if (role == null)
            {
                return NotFound();
            }

            return PartialView("_RoleDelete", role);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Role role)
        {

            try
            {
                if (role != null)
                {
                    _context.Roles.Remove(role);
                }

                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Rol eliminado exitosamente.";
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

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}

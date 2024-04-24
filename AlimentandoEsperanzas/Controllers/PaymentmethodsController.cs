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
    public class PaymentmethodsController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public PaymentmethodsController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Paymentmethods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paymentmethods.ToListAsync());
        }

        // GET: Paymentmethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentmethod = await _context.Paymentmethods
                .FirstOrDefaultAsync(m => m.PaymentMethodId == id);
            if (paymentmethod == null)
            {
                return NotFound();
            }

            return PartialView("_PaymentDetails", paymentmethod);
        }

        // GET: Paymentmethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Paymentmethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentMethodId,PaymentMethod1")] Paymentmethod paymentmethod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(paymentmethod);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Se ha agregado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) 
                {
                    await LogError($"{ex}");
                    return View(paymentmethod);
                }
            }
            return View(paymentmethod);
        }

        // GET: Paymentmethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentmethod = await _context.Paymentmethods.FindAsync(id);
            if (paymentmethod == null)
            {
                return NotFound();
            }
            return View(paymentmethod);
        }

        // POST: Paymentmethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentMethodId,PaymentMethod1")] Paymentmethod paymentmethod)
        {
            if (id != paymentmethod.PaymentMethodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentmethod);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Se ha actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    await LogError("Error al actualizar el método de pago");
                    if (!PaymentmethodExists(paymentmethod.PaymentMethodId))
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
            return View(paymentmethod);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Paymentmethod paymentmethod = _context.Paymentmethods.Where(m => m.PaymentMethodId == id).FirstOrDefault();
            if (paymentmethod == null)
            {
                return NotFound();
            }

            return PartialView("_PaymentMethodDelete", paymentmethod);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Paymentmethod paymentmethod)
        {
            try
            {
                if (paymentmethod == null)
                {
                    return NotFound();
                }

                var donations = _context.Donations.Where(d => d.PaymentMethodId == paymentmethod.PaymentMethodId).ToList();

                if (donations.Any())
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el método de pago porque hay donaciones asociadas.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Paymentmethods.Remove(paymentmethod);
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

        private bool PaymentmethodExists(int id)
        {
            return _context.Paymentmethods.Any(e => e.PaymentMethodId == id);
        }
    }
}

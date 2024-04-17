using OfficeOpenXml;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlimentandoEsperanzas.Models;
using System.ComponentModel;

namespace AlimentandoEsperanzas.Controllers
{
    public class DonationsController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public DonationsController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ExportToExcel(string searchTerm)
        {
            IQueryable<Donation> donationsQuery = _context.Donations
                .Include(d => d.Category)
                .Include(d => d.DonationType)
                .Include(d => d.Donor)
                .Include(d => d.PaymentMethod);

            // Aplicar el filtro de búsqueda si se proporciona un término de búsqueda
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Intentar convertir el término de búsqueda a un número
                if (decimal.TryParse(searchTerm, out decimal amount))
                {
                    // Si el término de búsqueda es un número, buscar en la cantidad
                    donationsQuery = donationsQuery.Where(d => (decimal)d.Amount == amount);
                }
                else
                {
                    // Si el término de búsqueda no es un número, buscar en los campos de texto
                    donationsQuery = donationsQuery.Where(d =>
                        d.Category.Category1.Contains(searchTerm) ||
                        d.DonationType.DonationType1.Contains(searchTerm) ||
                        d.Donor.Name.Contains(searchTerm) ||
                        d.PaymentMethod.PaymentMethod1.Contains(searchTerm));
                }
            }

            var donations = await donationsQuery.ToListAsync();

            // Crear el archivo Excel
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Aquí se especifica el espacio de nombres completo
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Donaciones");
                worksheet.Cells.LoadFromCollection(donations, true);

                // Guardar el archivo en memoria
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Devolver el archivo Excel como una descarga
                string excelName = $"Donaciones_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }



        // GET: Donations
        public async Task<IActionResult> Index()
        {
            var alimentandoesperanzasContext = _context.Donations.Include(d => d.Category).Include(d => d.DonationType).Include(d => d.Donor).Include(d => d.PaymentMethod);
            return View(await alimentandoesperanzasContext.ToListAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.Category)
                .Include(d => d.DonationType)
                .Include(d => d.Donor)
                .Include(d => d.PaymentMethod)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return PartialView("_DonationDetails", donation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            var donors = _context.Donors.Select(d => new {
                DonorId = d.DonorId,
                Name = $"{d.Name} {d.LastName} - {d.IdNumber}"
            }).ToList();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category1");
            ViewData["DonationTypeId"] = new SelectList(_context.Donationtypes, "DonationTypeId", "DonationType1");
            ViewData["DonorId"] = new SelectList(donors, "DonorId", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethods, "PaymentMethodId", "PaymentMethod1");
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationId,DonorId,Amount,DonationTypeId,Date,PaymentMethodId,CategoryId,Comments")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(donation);
                    await _context.SaveChangesAsync();

                    // Obtener la información del donante asociado a la donación
                    var donor = await _context.Donors.FirstOrDefaultAsync(d => d.DonorId == donation.DonorId);

                    if (donor != null)
                    {
                        // Crear el servicio de correo electrónico
                        var emailService = new EmailService();

                        // Construir el mensaje de correo electrónico
                        string subject = "Recordatorio de Donación Mensual";
                        string body = $"Hola {donor.Name},\n\nEste es un recordatorio amistoso de que tu donación mensual ha sido procesada. ¡Gracias por tu apoyo continuo!\n\nAtentamente,\nAlimentando Esperanzas";


                        // Enviar el correo electrónico
                        await emailService.SendEmailAsync(donor.Email, body);


                        TempData["Mensaje"] = "Se ha agregado exitosamente y se ha enviado un recordatorio al donador.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No se pudo enviar el correo electrónico de recordatorio porque no se encontró la información del donador asociado a la donación.";
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await LogError($"{ex}");
                    return View(donation);
                }
            }

            // Si hay errores de validación, se vuelve a mostrar el formulario de creación
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category1", donation.CategoryId);
            ViewData["DonationTypeId"] = new SelectList(_context.Donationtypes, "DonationTypeId", "DonationTypeId", donation.DonationTypeId);
            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "DonorId", donation.DonorId);
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethods, "PaymentMethodId", "PaymentMethodId", donation.PaymentMethodId);
            return View(donation);
        }



        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            var donors = _context.Donors.Select(d => new {
                DonorId = d.DonorId,
                Name = $"{d.Name} {d.LastName} - {d.IdNumber}"
            }).ToList();


            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category1", donation.CategoryId);
            ViewData["DonationTypeId"] = new SelectList(_context.Donationtypes, "DonationTypeId", "DonationType1", donation.DonationTypeId);
            ViewData["DonorId"] = new SelectList(donors, "DonorId", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethods, "PaymentMethodId", "PaymentMethod1", donation.PaymentMethodId);
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationId,DonorId,Amount,DonationTypeId,Date,PaymentMethodId,CategoryId,Comments")] Donation donation)
        {
            if (id != donation.DonationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                    await LogAction($"Actualización de la donación {donation.DonationId}", "Donaciones");
                    TempData["Mensaje"] = "Se ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    await LogError("Error al actualizar la donación");
                    if (!DonationExists(donation.DonationId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", donation.CategoryId);
            ViewData["DonationTypeId"] = new SelectList(_context.Donationtypes, "DonationTypeId", "DonationTypeId", donation.DonationTypeId);
            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "DonorId", donation.DonorId);
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethods, "PaymentMethodId", "PaymentMethodId", donation.PaymentMethodId);
            return View(donation);
        }

        public IActionResult GetDonationChartData()
        {
            var donationData = _context.Donations
                .GroupBy(d => d.Date.Month)
                .Select(g => new { Month = g.Key, TotalAmount = g.Sum(d => d.Amount) })
                .OrderBy(g => g.Month)
                .ToList();

            return Json(donationData);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Donation donation = _context.Donations.Include(d => d.Category)
                .Include(d => d.DonationType)
                .Include(d => d.Donor)
                .Include(d => d.PaymentMethod)
                .Where(m => m.DonationId == id).FirstOrDefault();
            if (donation == null)
            {
                return NotFound();
            }

            return PartialView("_DonationDelete", donation);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Donation donation)
        {

            try
            {
                if (donation != null)
                {
                    await LogAction($"Eliminación de donación {donation.DonationId}", "Donaciones");
                    _context.Donations.Remove(donation);
                }

                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Se ha eliminado exitosamente.";
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

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.DonationId == id);
        }
    }
}

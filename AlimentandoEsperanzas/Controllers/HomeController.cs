using AlimentandoEsperanzas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization; // Agrega este using para obtener nombres de meses

namespace AlimentandoEsperanzas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AlimentandoesperanzasContext _context; // Agrega el contexto de tu base de datos

        public HomeController(ILogger<HomeController> logger, AlimentandoesperanzasContext context)
        {
            _logger = logger;
            _context = context; // Inicializa el contexto
        }

        public async Task<IActionResult> Index()
        {
            // Obtener datos de donaciones
            var donationData = await _context.Donations
                .GroupBy(d => d.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(d => d.Amount)
                })
                .OrderBy(g => g.Month)
                .ToListAsync();

            // Convertir los datos de donaciones en un formato adecuado para el gráfico
            var donationLabels = donationData.Select(d => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Month)).ToArray();
            var donationAmounts = donationData.Select(d => d.TotalAmount).ToArray();

            ViewBag.DonationLabels = donationLabels;
            ViewBag.DonationAmounts = donationAmounts;

            // Obtener datos de los donantes
            var donorData = await _context.Donors
                .GroupBy(d => d.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    DonorCount = g.Count()
                })
                .OrderBy(g => g.Month)
                .ToListAsync();

            // Convertir los datos de donantes en un formato adecuado para el gráfico
            var donorLabels = donorData.Select(d => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Month)).ToArray();
            var donorCounts = donorData.Select(d => d.DonorCount).ToArray();

            ViewBag.DonorLabels = donorLabels;
            ViewBag.DonorCounts = donorCounts;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

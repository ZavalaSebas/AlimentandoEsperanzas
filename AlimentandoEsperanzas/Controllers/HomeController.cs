using AlimentandoEsperanzas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization; //  Obtener nombres de meses

namespace AlimentandoEsperanzas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AlimentandoesperanzasContext _context; 

        public HomeController(ILogger<HomeController> logger, AlimentandoesperanzasContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            var donationData = await _context.Donations
                .GroupBy(d => d.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(d => d.Amount)
                })
                .OrderBy(g => g.Month)
                .ToListAsync();

            // Convertir los datos en un formato adecuado para el gráfico
            var labels = donationData.Select(d => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Month)).ToArray();
            var amounts = donationData.Select(d => d.TotalAmount).ToArray();

            ViewBag.Labels = labels;
            ViewBag.Amounts = amounts;

            var numDonations = await _context.Donations.CountAsync();
            var numDonors = await _context.Donors.CountAsync();

            ViewBag.TotalDonations = numDonations;
            ViewBag.TotalDonors = numDonors;

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

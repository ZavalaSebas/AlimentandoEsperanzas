using AlimentandoEsperanzas.Models;
using AlimentandoEsperanzas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlimentandoEsperanzas.Controllers
{
    public class HomeController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public HomeController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }


        public IActionResult DonationSummary()
        {
            DateTime StartDate = DateTime.Now;
            StartDate = StartDate.AddDays(-365);

            List<VMDonations> ListD = (from TDonation in _context.Donations
                                       where TDonation.Date >= StartDate.Date
                                       group TDonation by TDonation.Date into grupD
                                       select new VMDonations
                                       {
                                           Date = grupD.Key,
                                           Amount = grupD.Count(),
                                       }).ToList();

            return StatusCode(StatusCodes.Status200OK, ListD);
        }

        public IActionResult ItemSummary()
        {
            List<VMItems> ListD = (from TItems in _context.Items
                                   group TItems by TItems.Description into grupD
                                   orderby grupD.Max(item => item.Quantity) descending
                                   select new VMItems
                                   {
                                       Description = grupD.Key,
                                       Quantity = grupD.Max(item => item.Quantity),
                                   }).Take(4).ToList();

            return StatusCode(StatusCodes.Status200OK, ListD);
        }

        //public IActionResult ItemSummary()
        //{

        //    List<VMItems> ListD = (from TItems in _context.Items
        //                               group TItems by TItems.Description into grupD
        //                               orderby grupD.Count() descending
        //                               select new VMItems
        //                               {
        //                                   Description = grupD.Key,
        //                                   Quantity = grupD.Count(),
        //                               }).Take(4).ToList();

        //    return StatusCode(StatusCodes.Status200OK, ListD);
        //}


        public IActionResult Index()
        {
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

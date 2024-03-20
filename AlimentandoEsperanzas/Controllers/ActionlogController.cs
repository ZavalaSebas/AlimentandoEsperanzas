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
    public class ActionlogController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public ActionlogController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Actionlog
        public async Task<IActionResult> Index()
        {
            var alimentandoesperanzasContext = _context.Actionlogs.Include(a => a.User);
            return View(await alimentandoesperanzasContext.ToListAsync());
        }

        // GET: Actionlog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Actionlog actionlog = _context.Actionlogs.Include(i => i.User).Where(m => m.ActionLogId == id).FirstOrDefault();
            if (actionlog == null)
            {
                return NotFound();
            }

            return PartialView("_ActionlogDetails", actionlog);
        }


        private bool ActionlogExists(int id)
        {
            return _context.Actionlogs.Any(e => e.ActionLogId == id);
        }
    }
}

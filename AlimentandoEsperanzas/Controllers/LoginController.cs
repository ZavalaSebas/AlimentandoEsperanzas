using System.Linq;
using AlimentandoEsperanzas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlimentandoEsperanzas.Controllers
{
    public class LoginController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public LoginController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Iniciar sesión
                // Por ejemplo, puedes establecer una cookie de autenticación
                // HttpContext.SignInAsync(user);

                return RedirectToAction("Index", "Home"); // Redireccionar al usuario a la página de inicio
            }

            ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            return View();
        }
    }
}

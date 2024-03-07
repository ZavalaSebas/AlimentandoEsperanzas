using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AlimentandoEsperanzas.Models;

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
        var user = _context.Users
                           .Include(u => u.RoleNavigation)
                           .FirstOrDefault(u => u.Email == email && u.Password == password);

        if (user != null)
        {
            HttpContext.Session.SetString("UserRole", user.RoleNavigation.Role1);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Credenciales inválidas");
        return View();
    }
}

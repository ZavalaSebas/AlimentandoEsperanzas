using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AlimentandoEsperanzas.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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

    //[HttpPost]
    //public IActionResult Index(string email, string password)
    //{
    //    var user = _context.Users
    //                       .Include(u => u.RoleNavigation)
    //                       .FirstOrDefault(u => u.Email == email && u.Password == password);

    //    if (user != null)
    //    {
    //        HttpContext.Session.SetInt32("UserId", user.UserId);
    //        HttpContext.Session.SetString("UserRole", user.RoleNavigation.Role1);
    //        return RedirectToAction("Index", "Home");
    //    }

    //    ModelState.AddModelError(string.Empty, "Credenciales inválidas");
    //    return View();
    //}

    [HttpPost]
    public IActionResult Index(string email, string password)
    {
        // Buscar el usuario por su correo electrónico
        var user = _context.Users
                           .Include(u => u.RoleNavigation)
                           .FirstOrDefault(u => u.Email == email);

        if (user != null)
        {
            // Extraer el hash y el salt almacenados en la base de datos
            string[] passwordParts = user.Password.Split(':');
            string hashedPassword = passwordParts[0];
            byte[] salt = Convert.FromBase64String(passwordParts[1]);

            // Calcular el hash de la contraseña ingresada utilizando el mismo salt
            string hashedEnteredPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Verificar si los hashes coinciden
            if (hashedEnteredPassword == hashedPassword)
            {
                // Las contraseñas coinciden, iniciar sesión correctamente
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserRole", user.RoleNavigation.Role1);
                return RedirectToAction("Index", "Home");
            }
        }

        // Si no se encuentra el usuario o la contraseña es incorrecta, mostrar un mensaje de error
        ModelState.AddModelError(string.Empty, "Credenciales inválidas");
        return View();
    }

}

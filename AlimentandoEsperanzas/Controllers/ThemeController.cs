using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AlimentandoEsperanzas.Controllers
{
    public class ThemeController : Controller
    {
        public IActionResult ChangeTheme(string theme)
        {
            // Almacenar la preferencia del usuario en una cookie
            Response.Cookies.Append("theme", theme, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7) // Cookie válida por 7 días
            });

            return RedirectToAction("Index", "Home"); // Redirige a la página de inicio u otra página según tu estructura
        }
    }
}

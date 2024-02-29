using Microsoft.AspNetCore.Mvc;

namespace AlimentandoEsperanzas.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string usuario)
        {
            ViewBag.usuario = usuario;
            return View();
        }
    }
}

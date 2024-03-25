using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlimentandoEsperanzas.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace AlimentandoEsperanzas.Controllers
{
    public class UsersController : Controller
    {
        private readonly AlimentandoesperanzasContext _context;

        public UsersController(AlimentandoesperanzasContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var alimentandoesperanzasContext = _context.Users.Include(u => u.IdentificationTypeNavigation).Include(u => u.RoleNavigation);
            return View(await alimentandoesperanzasContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.IdentificationTypeNavigation)
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Description");
            ViewData["Role"] = new SelectList(_context.Roles, "RoleId", "Role1");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,LastName,Email,Password,IdNumber,IdentificationType,PhoneNumber,Date,Role,ConfirmPassword")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Encrypt
                    user.Password = HashPassword(user.Password);
                    user.ConfirmPassword = HashPassword(user.ConfirmPassword);

                    _context.Add(user);
                await _context.SaveChangesAsync();
                await LogAction($"Registro del usuario {user.Email}", "Usuarios");
                TempData["Mensaje"] = "Usuario agregado exitosamente";
                return RedirectToAction(nameof(Index));
                }
                ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Description", user.IdentificationType);
                ViewData["Role"] = new SelectList(_context.Roles, "RoleId", "Role1", user.Role);
                return View(user);
            }
            catch
            {
                return View(user);
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Description", user.IdentificationType);
            ViewData["Role"] = new SelectList(_context.Roles, "RoleId", "Role1", user.Role);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,LastName,Email,Password,IdNumber,IdentificationType,PhoneNumber,Date,Role,ConfirmPassword")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        user.Password = HashPassword(user.Password);
                        user.ConfirmPassword = HashPassword(user.ConfirmPassword);
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    await LogAction($"Actualización del usuario {user.Email}", "Usuarios");
                    TempData["Mensaje"] = "Usuario actualizado exitosamente";
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Id", user.IdentificationType);
            ViewData["Role"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.Role);
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = _context.Users.Include(u => u.IdentificationTypeNavigation).Include(u => u.RoleNavigation).Where(m => m.UserId == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return PartialView("_UserDelete", user);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(User user)
        {

            if (user != null)
            {
                await LogAction($"Eliminación de usuario {user.Email}", "Usuarios");
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Usuario eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UserProfile()
        {
            
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = await _context.Users
                .Include(u => u.IdentificationTypeNavigation)
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(user);
        
        }

        public async Task<IActionResult> EditProfile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Description", user.IdentificationType);
            ViewData["Role"] = new SelectList(_context.Roles, "RoleId", "Role1", user.Role);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // Verificar si se ha proporcionado una nueva contraseña
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        // Encriptar la nueva contraseña antes de guardarla
                        user.Password = HashPassword(user.Password);
                        user.ConfirmPassword = HashPassword(user.ConfirmPassword);
                    }


                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Usuario actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserProfile));
            }
            ViewData["IdentificationType"] = new SelectList(_context.Idtypes, "Id", "Id", user.IdentificationType);
            ViewData["Role"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.Role);
            return View(user);
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


        private string HashPassword(string password)
        {
            // Generar un salt aleatorio
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Calcular el hash de la contraseña usando el salt
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Retornar el hash de la contraseña concatenado con el salt
            return $"{hashedPassword}:{Convert.ToBase64String(salt)}";
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMOC20250324.AppWebMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EMOC20250324.AppWebMVC.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]

    public class UserController : Controller
    {
        private readonly Test20250324DbContext _context;

        public UserController(Test20250324DbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index(string userName, int topRegistro = 10)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(s => s.Username.ToLower().Contains(userName.ToLower()));
            }

            if (topRegistro > 0)
            {
                query = query.Take(topRegistro);
            }

            return View(await query.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Password,Role,Notes")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = CalcularHashMD5(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> CerrarSession()
        {
            // Hola mundo
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            user.Password = CalcularHashMD5(user.Password);
            var usuarioAuth = await _context.
                Users.
                FirstOrDefaultAsync(s => s.Email == user.Email && s.Password == user.Password);
            if (usuarioAuth != null && usuarioAuth.Id > 0 && usuarioAuth.Email == user.Email)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, usuarioAuth.Email),
                    new Claim("Id", usuarioAuth.Id.ToString()),
                     new Claim("Nombre", usuarioAuth.Username),
                    new Claim(ClaimTypes.Role, usuarioAuth.Role)
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "El email o contraseña estan incorrectos");
                return View();
            }
        }

        // GET: User/Edit/5
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
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Role,Notes")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            var usuarioUpdate = await _context.Users.FirstOrDefaultAsync(m => m.Id == user.Id);
            {
                try
                {
                    usuarioUpdate.Username = user.Username;
                    usuarioUpdate.Email = user.Email;
                    usuarioUpdate.Role = user.Role;
                    _context.Update(usuarioUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return View(user);
                    }
                }

            }

        }
        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

     
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Perfil()
        {

            var idStr = User.FindFirst("Id")?.Value;
            int id = int.Parse(idStr);
            var usuario = await _context.Users.FindAsync(id);
            return View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Perfil(int id, [Bind("UserId,Username,Email,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            var usuarioUpdate = await _context.Users.FirstOrDefaultAsync(m => m.Id == user.Id);
            if (usuarioUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(usuarioUpdate, "",
                u => u.Username, u => u.Email, u => u.Role))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al actualizar. Inténtelo de nuevo.");
                    }
                }
            }

            return View(user);
        }

        private string CalcularHashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" convierte el byte en una cadena hexadecimal de dos caracteres.
                }
                return sb.ToString();
            }
        }
    }
}






using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Petshop_SAEP.Models;
using System.Security.Claims;

namespace Petshop_SAEP.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbPetTutorContext _context;

        public AccountController(DbPetTutorContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Pet");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View(new LoginViewModel());
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();

            var tutor = await _context.Tutor
                .FirstOrDefaultAsync(t => t.Email == email);

            // Comparação compatível com os registros atuais em texto puro.
            // Para produção, utilize hashes de senha.
            if (tutor == null || tutor.Senha != model.Senha)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "E-mail ou senha incorretos.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    tutor.Codigo.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    tutor.Nome),

                new Claim(
                    ClaimTypes.Email,
                    tutor.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = model.LembrarMe,
                AllowRefresh = true
            };

            if (model.LembrarMe)
            {
                properties.ExpiresUtc =
                    DateTimeOffset.UtcNow.AddDays(7);
            }
            else
            {
                properties.ExpiresUtc =
                    DateTimeOffset.UtcNow.AddMinutes(30);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Pet");
        }

        // POST: Account/Logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        // GET: Account/AccessDenied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
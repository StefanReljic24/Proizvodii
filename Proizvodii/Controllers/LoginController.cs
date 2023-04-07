using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proizvodii.Data;
using Proizvodii.Models;
using System.Security.Claims;

namespace Proizvodii.Controllers
{
    public class LoginController : Controller
    {
        EFDataContext _context;
        public LoginController(EFDataContext context)
        {
            _context = context;
        }

        public IActionResult Index([FromQuery] string returnUrl)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Index(LoginModel data)
        {

            var user = _context.User
                .Include(p => p.UserRole)
                .ThenInclude(p => p.Role)
                .FirstOrDefault(p => p.Active &&
                p.Username.ToLower() == data.Username.ToLower()
                && p.Password == data.Password);


            if (user == null)
            {

                ModelState.AddModelError("", "Prijava nije uspešna");
                return View(data);
            }


            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                    new Claim("userId", user.UserId.ToString())
                };


            foreach (var r in user.UserRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, r.Role.Name));
            }
            var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdenties);


            Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal,
                new AuthenticationProperties() { IsPersistent = data.RememberMe });

            if (string.IsNullOrEmpty(data.ReturnUrl))
                data.ReturnUrl = "/";


            return Redirect(data.ReturnUrl);
        }

        public IActionResult Logout()
        {

            Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProyectoModulo.Models;

namespace ProyectoModulo.Controllers;

public class AdministradorController : Controller
{
    private readonly GenericRepositorie<Administrador> _repositorie;

    public AdministradorController(GenericRepositorie<Administrador> repositorie)
    {
        _repositorie = repositorie;
    }

    // Metodo para mostrar vista login
    public IActionResult Login()
    {
        var userClaim = HttpContext.User;

        if (userClaim.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        return View();
    }

    // Metodo para procesar solicitud iniciar sesión
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Administrador usuario)
    {
        var user = await _repositorie.GetAsync(u => u.UserName == usuario.UserName);
        if (user == null || usuario.Password != user.Password)
        {
            ViewBag.ValidateMessage = "El usuario no fue encontrado";
            return View(usuario);
        }

        var claims = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserName)
    };

        var claimsIdentity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var properties = new AuthenticationProperties()
        {
            AllowRefresh = true,
            IsPersistent = usuario.KeepLoggedIn
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            properties
        );

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }
}

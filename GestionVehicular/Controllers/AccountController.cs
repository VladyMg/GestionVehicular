
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GestionVehiculos.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly ApplicationDbContext _context;

    public AccountController(ILogger<AccountController> logger, ApplicationDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login user)
    {
        // Implementar lógica de autenticación
        if (ValidarCredenciales(user.Cedula, user.Contrasenia))
        {
            var userInt = _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Cedula == user.Cedula);

            //AUTENTIFICACION
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
            //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
            //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
            identity.AddClaim(new Claim("cedula", userInt.Cedula));
            identity.AddClaim(new Claim(ClaimTypes.Name, $"{userInt.Nombre} {userInt.Apellido}"));
            identity.AddClaim(new Claim(ClaimTypes.Role, userInt.Rol.Nombre));

            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddMinutes(45)
            });


            return RedirectToAction("Index", "Home");

        }

        // Credenciales inválidas
        ViewData["ErrorMessage"] = "Las credenciales de inicio de sesión son inválidas.";

        return View("Login");
    }

    public bool ValidarCredenciales(string cedula, string contrasenia)
    {
        // Obtener el usuario por la cédula
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Cedula == cedula);

        // Verificar si el usuario existe y si la contraseña es correcta
        if (usuario != null && BCrypt.Net.BCrypt.Verify(contrasenia.Trim(), usuario.Contrasenia))
        {
            return true;
        }

        return false;
    }

    // Todo: create logout action
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

}
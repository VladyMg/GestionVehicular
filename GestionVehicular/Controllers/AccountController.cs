
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

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
            //AUTENTIFICACION
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
            //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
            //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
            Claim claimCedula = new Claim("cedula", user.Cedula);

            identity.AddClaim(claimCedula);

            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddMinutes(45)
            });


            return RedirectToAction("Index", "Home");

        }

        // Credenciales inválidas
        ModelState.AddModelError("", "Las credenciales de inicio de sesión son inválidas.");
        return View("Login");
    }

    public bool ValidarCredenciales(string cedula, string contrasenia)
    {
        // Obtener el usuario por la cédula
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Cedula == cedula);

        // Verificar si el usuario existe y si la contraseña es correcta
        if (usuario != null && usuario.Contrasenia == contrasenia)
        {
            return true;
        }

        return false;
    }

}
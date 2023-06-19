using GestionVehiculos.Context;
using GestionVehiculos.Models;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Login(Usuario user)
    {
        // Aquí iría la lógica para validar las credenciales del usuario.
        // Como este es un ejemplo simple, vamos a omitirla.

        // Redireccionar al inicio (HomeController, action Index)
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
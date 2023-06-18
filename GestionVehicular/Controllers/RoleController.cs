using GestionVehiculos.Context;
using GestionVehiculos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionVehiculos.Controllers;

public class RoleController : Controller
{
    private readonly ILogger<RoleController> _logger;
    private ApplicationDbContext _context;

    public RoleController(ILogger<RoleController> logger, ApplicationDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public IActionResult Index()
    {
        var roles = _context.Roles.ToList();

        return View(roles);
    }

    // GET: Roles/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Roles/Create
    [HttpPost]
    public ActionResult Create(Role role)
    {
        if (ModelState.IsValid)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(role);
    }

    // GET: Roles/Edit/5
    public ActionResult Edit(int id)
    {
        var role = _context.Roles.SingleOrDefault(r => r.RolId == id);

        if (role == null)
            return null;

        return View(role);
    }

    // POST: Roles/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, Role role)
    {
        if (ModelState.IsValid)
        {
            var roleInDb = _context.Roles.Single(r => r.RolId == id);
            roleInDb.Nombre = role.Nombre;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(role);
    }

    // GET: Roles/Delete/5
    public ActionResult Delete(int id)
    {
        var role = _context.Roles.SingleOrDefault(r => r.RolId == id);

        if (role == null)
            return null;

        return View(role);
    }

    // POST: Roles/Delete/5
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var role = _context.Roles.Single(r => r.RolId == id);
        _context.Roles.Remove(role);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
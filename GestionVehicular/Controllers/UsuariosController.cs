
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionVehicular.Controllers;

public class UsuariosController : Controller
{
    private readonly ILogger<UsuariosController> _logger;
    private ApplicationDbContext _context;

    public UsuariosController(ILogger<UsuariosController> logger, ApplicationDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


    // GET: Usuarios
    public ActionResult Index()
    {
        var usuarios = _context.Usuarios.ToList();
        return View(usuarios);
    }

    // GET: Usuarios/Create
    public ActionResult Create()
    {
        var roles = _context.Roles.ToList();
        ViewBag.Roles = new SelectList(roles, "Id", "Name");
        return View();
    }

    // POST: Usuarios/Create
    [HttpPost]
    public ActionResult Create(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(usuario);
    }

    // GET: Usuarios/Edit/5
    public ActionResult Edit(int id)
    {
        var usuario = _context.Usuarios.SingleOrDefault(u => u.UsuarioId == id);

        if (usuario == null)
            return null;

        var roles = _context.Roles.ToList();
        ViewBag.Roles = new SelectList(roles, "Id", "Name", usuario.RolId);

        return View(usuario);
    }

    // POST: Usuarios/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            var usuarioInDb = _context.Usuarios.Single(u => u.UsuarioId == id);

            // Aquí debes hacer las asignaciones necesarias para actualizar las propiedades del usuario

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(usuario);
    }

    // GET: Usuarios/Delete/5
    public ActionResult Delete(int id)
    {
        var usuario = _context.Usuarios.SingleOrDefault(u => u.UsuarioId == id);

        if (usuario == null)
            return null;

        return View(usuario);
    }

    // POST: Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var usuario = _context.Usuarios.Single(u => u.UsuarioId == id);
        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsuariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Usuarios
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Usuarios.Include(u => u.Rol).Include(u => u.Subcircuito);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Usuarios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Usuarios == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Subcircuito)
            .FirstOrDefaultAsync(m => m.UsuarioId == id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // GET: Usuarios/Create
    public IActionResult Create()
    {
        ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre");
        ViewData["SubcircuitoId"] = new SelectList(_context.Set<Subcircuito>(), "SubcircuitoId", "CodSubcircuito");
        return View();
    }

    // POST: Usuarios/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UsuarioId,Cedula,Contrasenia,Nombre,Apellido,FechaNacimiento,TipoSangre,Telefono,CiudadNacimiento,Rango,EsActivo,RolId,SubcircuitoId,FechaCreacion")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            usuario.EsActivo = true;
            usuario.FechaCreacion = DateTime.Now;
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
        ViewData["SubcircuitoId"] = new SelectList(_context.Set<Subcircuito>(), "SubcircuitoId", "CodSubcircuito", usuario.SubcircuitoId);
        return View(usuario);
    }

    // GET: Usuarios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Usuarios == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
        ViewData["SubcircuitoId"] = new SelectList(_context.Set<Subcircuito>(), "SubcircuitoId", "CodSubcircuito", usuario.SubcircuitoId);
        return View(usuario);
    }

    // POST: Usuarios/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Cedula,Contrasenia,Nombre,Apellido,FechaNacimiento,TipoSangre,Telefono,CiudadNacimiento,Rango,EsActivo,RolId,SubcircuitoId,FechaCreacion")] Usuario usuario)
    {
        if (id != usuario.UsuarioId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.UsuarioId))
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
        ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
        ViewData["SubcircuitoId"] = new SelectList(_context.Set<Subcircuito>(), "SubcircuitoId", "CodSubcircuito", usuario.SubcircuitoId);
        return View(usuario);
    }

    // GET: Usuarios/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Usuarios == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Subcircuito)
            .FirstOrDefaultAsync(m => m.UsuarioId == id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // POST: Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Usuarios == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Usuarios'  is null.");
        }
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.UsuarioId == id);
    }
}

﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

[Authorize]
public class AbastecimientosController : Controller
{
    private readonly ApplicationDbContext _context;

    public AbastecimientosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Abastecimientos
    public async Task<IActionResult> Index()
    {
        var abastecimientos = await _context.Abastecimientos
        .Include(a => a.Usuario)
        .Include(a => a.Vehiculo)
        .ToListAsync();

        var aprobaciones = await _context.Aprobaciones
        .Where(x => x.AbastecimientoId > 0)
        .ToListAsync();

        var query = from abastecimiento in abastecimientos
                    join aprobacion in aprobaciones
                    on abastecimiento.AbastecimientoId equals aprobacion.AbastecimientoId
                    select new Abastecimiento
                    {
                        AbastecimientoId = abastecimiento.AbastecimientoId,
                        Fecha = abastecimiento.Fecha,
                        Gasolinera = abastecimiento.Gasolinera,
                        HoraLlegada = abastecimiento.HoraLlegada,
                        HoraSalida = abastecimiento.HoraSalida,
                        Combustible = abastecimiento.Combustible,
                        KilometrosSalida = abastecimiento.KilometrosSalida,
                        UsuarioId = abastecimiento.UsuarioId,
                        VehiculoId = abastecimiento.VehiculoId,
                        EsActivo = abastecimiento.EsActivo,
                        FechaCreacion = abastecimiento.FechaCreacion,
                        Usuario = abastecimiento.Usuario,
                        Vehiculo = abastecimiento.Vehiculo,
                        Aprobacion = aprobacion
                    };

        return View(query.OrderByDescending(x => x.FechaCreacion).ToList());
    }

    // GET: Abastecimientos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Abastecimientos == null)
        {
            return NotFound();
        }

        var abastecimiento = await _context.Abastecimientos
            .Include(a => a.Usuario)
            .Include(a => a.Vehiculo)
            .FirstOrDefaultAsync(m => m.AbastecimientoId == id);

        if (abastecimiento == null)
        {
            return NotFound();
        }

        var aprovacion = await _context.Aprobaciones
        .FirstOrDefaultAsync(x => x.AbastecimientoId == abastecimiento.AbastecimientoId);

        abastecimiento.Aprobacion = aprovacion;

        return View(abastecimiento);
    }

    // GET: Abastecimientos/Create
    public IActionResult Create()
    {
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula");
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa");
        return View();
        
    }

    // POST: Abastecimientos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AbastecimientoId,Fecha,Gasolinera,HoraLlegada,HoraSalida,Combustible,KilometrosSalida,UsuarioId,VehiculoId,EsActivo,FechaCreacion")] Abastecimiento abastecimiento)
    {
        if (ModelState.IsValid)
        {
            abastecimiento.EsActivo = true;
            abastecimiento.FechaCreacion = DateTime.Now;
            _context.Add(abastecimiento);

            await _context.SaveChangesAsync();

            var aprobacion = new Aprobaciones
            {
                Tipo = "Abastecimiento",
                Estado = "Pendiente",
                AbastecimientoId = abastecimiento.AbastecimientoId,
                UsuarioId = abastecimiento.UsuarioId,
                FechaCreacion = DateTime.Now,
                EsActivo = true
            };

            _context.Add(aprobacion);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", abastecimiento.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", abastecimiento.VehiculoId);
        return View(abastecimiento);
    }

    // GET: Abastecimientos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Abastecimientos == null)
        {
            return NotFound();
        }

        var abastecimiento = await _context.Abastecimientos.FindAsync(id);
        if (abastecimiento == null)
        {
            return NotFound();
        }
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", abastecimiento.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", abastecimiento.VehiculoId);
        return View(abastecimiento);
    }

    // POST: Abastecimientos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AbastecimientoId,Fecha,Gasolinera,HoraLlegada,HoraSalida,Combustible,KilometrosSalida,UsuarioId,VehiculoId,EsActivo,FechaCreacion")] Abastecimiento abastecimiento)
    {
        if (id != abastecimiento.AbastecimientoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(abastecimiento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbastecimientoExists(abastecimiento.AbastecimientoId))
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
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", abastecimiento.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", abastecimiento.VehiculoId);
        return View(abastecimiento);
    }

    // GET: Abastecimientos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Abastecimientos == null)
        {
            return NotFound();
        }

        var abastecimiento = await _context.Abastecimientos
            .Include(a => a.Usuario)
            .Include(a => a.Vehiculo)
            .FirstOrDefaultAsync(m => m.AbastecimientoId == id);
        if (abastecimiento == null)
        {
            return NotFound();
        }

        return View(abastecimiento);
    }

    // POST: Abastecimientos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Abastecimientos == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Abastecimientos'  is null.");
        }
        var abastecimiento = await _context.Abastecimientos.FindAsync(id);
        if (abastecimiento != null)
        {
            _context.Abastecimientos.Remove(abastecimiento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AbastecimientoExists(int id)
    {
        return _context.Abastecimientos.Any(e => e.AbastecimientoId == id);
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

[Authorize]
public class MovilizacionesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MovilizacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Movilizaciones
    public async Task<IActionResult> Index()
    {
        var movilizacions = await _context.Movilizaciones
        .Include(m => m.Usuario)
        .Include(m => m.Vehiculo)
        .ToListAsync();

        var aprobaciones = await _context.Aprobaciones
        .Where(x => x.MovilizacionId > 0)
        .ToListAsync();

        var query = from movilizacion in movilizacions
                    join aprobacion in aprobaciones
                    on movilizacion.MovilizacionId equals aprobacion.MovilizacionId
                    select new Movilizacion
                    {
                        MovilizacionId = movilizacion.MovilizacionId,
                        Fecha = movilizacion.Fecha,
                        HoraLlegada = movilizacion.HoraLlegada,
                        HoraSalida = movilizacion.HoraSalida,
                        KilometrosSalida = movilizacion.KilometrosSalida,
                        KilometrajeLlegada = movilizacion.KilometrajeLlegada,
                        Observacion = movilizacion.Observacion,
                        UsuarioId = movilizacion.UsuarioId,
                        VehiculoId = movilizacion.VehiculoId,
                        EsActivo = movilizacion.EsActivo,
                        FechaCreacion = movilizacion.FechaCreacion,
                        Usuario = movilizacion.Usuario,
                        Vehiculo = movilizacion.Vehiculo,
                        Aprobacion = aprobacion
                    };

        return View(query.OrderByDescending(x => x.FechaCreacion).ToList());
    }

    // GET: Movilizaciones/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Movilizaciones == null)
        {
            return NotFound();
        }

        var movilizacion = await _context.Movilizaciones
            .Include(m => m.Usuario)
            .Include(m => m.Vehiculo)
            .FirstOrDefaultAsync(m => m.MovilizacionId == id);

        if (movilizacion == null)
        {
            return NotFound();
        }

        var aprovacion = await _context.Aprobaciones
        .FirstOrDefaultAsync(x => x.MovilizacionId == movilizacion.MovilizacionId);

        movilizacion.Aprobacion = aprovacion;

        return View(movilizacion);
    }

    // GET: Movilizaciones/Create
    public IActionResult Create()
    {
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula");
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa");
        return View();
    }

    // POST: Movilizaciones/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovilizacionId,Fecha,HoraSalida,HoraLlegada,KilometrosSalida,KilometrajeLlegada,Observacion,UsuarioId,VehiculoId,EsActivo,FechaCreacion")] Movilizacion movilizacion)
    {
        if (ModelState.IsValid)
        {
            movilizacion.EsActivo = true;
            movilizacion.FechaCreacion = DateTime.Now;
            _context.Add(movilizacion);
            
            await _context.SaveChangesAsync();

            var aprobacion = new Aprobaciones
            {
                Tipo = "Movilizacion",
                Estado = "Pendiente",
                MovilizacionId = movilizacion.MovilizacionId,
                UsuarioId = movilizacion.UsuarioId,
                FechaCreacion = DateTime.Now,
                EsActivo = true
            };

            _context.Add(aprobacion);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", movilizacion.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", movilizacion.VehiculoId);
        return View(movilizacion);
    }

    // GET: Movilizaciones/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Movilizaciones == null)
        {
            return NotFound();
        }

        var movilizacion = await _context.Movilizaciones.FindAsync(id);
        if (movilizacion == null)
        {
            return NotFound();
        }
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", movilizacion.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", movilizacion.VehiculoId);
        return View(movilizacion);
    }

    // POST: Movilizaciones/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MovilizacionId,Fecha,HoraSalida,HoraLlegada,KilometrosSalida,KilometrajeLlegada,Observacion,UsuarioId,VehiculoId,EsActivo,FechaCreacion")] Movilizacion movilizacion)
    {
        if (id != movilizacion.MovilizacionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movilizacion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovilizacionExists(movilizacion.MovilizacionId))
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
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", movilizacion.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", movilizacion.VehiculoId);
        return View(movilizacion);
    }

    // GET: Movilizaciones/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Movilizaciones == null)
        {
            return NotFound();
        }

        var movilizacion = await _context.Movilizaciones
            .Include(m => m.Usuario)
            .Include(m => m.Vehiculo)
            .FirstOrDefaultAsync(m => m.MovilizacionId == id);
        if (movilizacion == null)
        {
            return NotFound();
        }

        return View(movilizacion);
    }

    // POST: Movilizaciones/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Movilizaciones == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Movilizaciones'  is null.");
        }
        var movilizacion = await _context.Movilizaciones.FindAsync(id);
        if (movilizacion != null)
        {
            _context.Movilizaciones.Remove(movilizacion);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovilizacionExists(int id)
    {
        return _context.Movilizaciones.Any(e => e.MovilizacionId == id);
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers
{
    public class MantenimientosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MantenimientosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mantenimientos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Mantenimientos.Include(m => m.TipoMantenimiento).Include(m => m.Usuario).Include(m => m.Vehiculo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Mantenimientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mantenimientos == null)
            {
                return NotFound();
            }

            var mantenimiento = await _context.Mantenimientos
                .Include(m => m.TipoMantenimiento)
                .Include(m => m.Usuario)
                .Include(m => m.Vehiculo)
                .FirstOrDefaultAsync(m => m.MantenimientoId == id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return View(mantenimiento);
        }

        // GET: Mantenimientos/Create
        public IActionResult Create()
        {
            ViewData["TipoMantenimientoId"] = new SelectList(_context.TiposMantenimiento, "TipoMantenimientoId", "Nombre");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa");
            return View();
        }

        // POST: Mantenimientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MantenimientoId,Nombre,Kilometraje,Observacion,UsuarioId,VehiculoId,TipoMantenimientoId,EsActivo,FechaCreacion")] Mantenimiento mantenimiento)
        {
            if (ModelState.IsValid)
            {
                mantenimiento.EsActivo = true;
                mantenimiento.FechaCreacion = DateTime.Now;
                _context.Add(mantenimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoMantenimientoId"] = new SelectList(_context.TiposMantenimiento, "TipoMantenimientoId", "Nombre", mantenimiento.TipoMantenimientoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Apellido", mantenimiento.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "CapacidadCarga", mantenimiento.VehiculoId);
            return View(mantenimiento);
        }

        // GET: Mantenimientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mantenimientos == null)
            {
                return NotFound();
            }

            var mantenimiento = await _context.Mantenimientos.FindAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }
            ViewData["TipoMantenimientoId"] = new SelectList(_context.TiposMantenimiento, "TipoMantenimientoId", "Nombre", mantenimiento.TipoMantenimientoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Apellido", mantenimiento.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "CapacidadCarga", mantenimiento.VehiculoId);
            return View(mantenimiento);
        }

        // POST: Mantenimientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MantenimientoId,Nombre,Kilometraje,Observacion,UsuarioId,VehiculoId,TipoMantenimientoId,EsActivo,FechaCreacion")] Mantenimiento mantenimiento)
        {
            if (id != mantenimiento.MantenimientoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mantenimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MantenimientoExists(mantenimiento.MantenimientoId))
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
            ViewData["TipoMantenimientoId"] = new SelectList(_context.TiposMantenimiento, "TipoMantenimientoId", "Nombre", mantenimiento.TipoMantenimientoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Apellido", mantenimiento.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "CapacidadCarga", mantenimiento.VehiculoId);
            return View(mantenimiento);
        }

        // GET: Mantenimientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mantenimientos == null)
            {
                return NotFound();
            }

            var mantenimiento = await _context.Mantenimientos
                .Include(m => m.TipoMantenimiento)
                .Include(m => m.Usuario)
                .Include(m => m.Vehiculo)
                .FirstOrDefaultAsync(m => m.MantenimientoId == id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return View(mantenimiento);
        }

        // POST: Mantenimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mantenimientos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Mantenimientos'  is null.");
            }
            var mantenimiento = await _context.Mantenimientos.FindAsync(id);
            if (mantenimiento != null)
            {
                _context.Mantenimientos.Remove(mantenimiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MantenimientoExists(int id)
        {
            return _context.Mantenimientos.Any(e => e.MantenimientoId == id);
        }
    }
}

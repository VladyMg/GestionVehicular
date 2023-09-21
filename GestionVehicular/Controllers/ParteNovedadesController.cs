using System.Text.Json;
using System.Text.Json.Serialization;
using GestionVehicular.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

[Authorize]
public class ParteNovedadesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ParteNovedadesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ParteNovedades
    public async Task<IActionResult> Index()
    {
        var partes = await _context.ParteNovedades
        .Include(p => p.Usuario)
        .Include(p => p.Vehiculo)
        .ToListAsync();

        var aprobaciones = await _context.Aprobaciones
        .Where(x => x.PartesNovedadesId > 0)
        .ToListAsync();

        var query = from parte in partes
                    join aprobacion in aprobaciones
                    on parte.ParteNovedadId equals aprobacion.PartesNovedadesId
                    select new ParteNovedad
                    {
                        ParteNovedadId = parte.ParteNovedadId,
                        Nombre = parte.Nombre,
                        Nota = parte.Nota,
                        UsuarioId = parte.UsuarioId,
                        VehiculoId = parte.VehiculoId,
                        EsActivo = parte.EsActivo,
                        FechaCreacion = parte.FechaCreacion,
                        Usuario = parte.Usuario,
                        Vehiculo = parte.Vehiculo,
                        Aprobacion = aprobacion
                    };

        return View(query.OrderByDescending(x => x.FechaCreacion).ToList());
    }

    // GET: ParteNovedades/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.ParteNovedades == null)
        {
            return NotFound();
        }

        var parteNovedad = await _context.ParteNovedades
            .Include(p => p.Usuario)
            .Include(p => p.Vehiculo)
            .FirstOrDefaultAsync(m => m.ParteNovedadId == id);

        if (parteNovedad == null)
        {
            return NotFound();
        }

        var aprovacion = await _context.Aprobaciones
        .FirstOrDefaultAsync(x => x.PartesNovedadesId == parteNovedad.ParteNovedadId);

        parteNovedad.Aprobacion = aprovacion;

        return View(parteNovedad);
    }

    // GET: ParteNovedades/Create
    public IActionResult Create()
    {
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula");
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa");

        var parteNovedad = GetObjectFromSession<ParteNovedad>(nameof(ParteNovedad)) ?? new ParteNovedad();

        if (parteNovedad.ParteNovedadId > 0)
        {
            parteNovedad = new ParteNovedad();
            SetObjectToSession(nameof(ParteNovedad), parteNovedad);
        }

        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        if (repuestos.Any(x => x.ParteNovedadId > 0))
        {
            repuestos = new List<Repuesto>();
            SetObjectToSession(nameof(Repuesto), repuestos);
        }

        return View(new ParteNovedadRepuestoViewModel
        {
            ParteNovedad = parteNovedad,
            Repuesto = new(),
            Repuestos = repuestos
        });
    }

    // POST: ParteNovedades/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ParteNovedadId,Nombre,Nota,UsuarioId,VehiculoId,EsActivo,FechaCreacion")] ParteNovedad parteNovedad)
    {
        if (ModelState.IsValid)
        {
            parteNovedad.FechaCreacion = DateTime.Now;
            parteNovedad.EsActivo = true;

            _context.Add(parteNovedad);
            await _context.SaveChangesAsync();

            var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

            foreach (var repuesto in repuestos)
            {
                repuesto.ParteNovedadId = parteNovedad.ParteNovedadId;
                _context.Add(repuesto);
            }

            var aprobacion = new Aprobaciones
            {
                Tipo = "ParteNovedad",
                Estado = "Pendiente",
                PartesNovedadesId = parteNovedad.ParteNovedadId,
                UsuarioId = parteNovedad.UsuarioId,
                FechaCreacion = DateTime.Now,
                EsActivo = true
            };

            _context.Add(aprobacion);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", parteNovedad.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", parteNovedad.VehiculoId);

        var respuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        return View(new ParteNovedadRepuestoViewModel
        {
            ParteNovedad = parteNovedad,
            Repuesto = new(),
            Repuestos = respuestos
        });
    }

    // GET: ParteNovedades/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.ParteNovedades == null)
        {
            return NotFound();
        }

        var parteNovedad = await _context.ParteNovedades.FindAsync(id);
        if (parteNovedad == null)
        {
            return NotFound();
        }
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", parteNovedad.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", parteNovedad.VehiculoId);

        var repuestos = await _context.Repuestos.Where(x => x.ParteNovedadId == parteNovedad.ParteNovedadId).ToListAsync();

        SetObjectToSession(nameof(Repuesto), repuestos);

        SetObjectToSession(nameof(ParteNovedad), parteNovedad);

        return View(new ParteNovedadRepuestoViewModel
        {
            ParteNovedad = parteNovedad,
            Repuesto = new(),
            Repuestos = repuestos
        });
    }

    // POST: ParteNovedades/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ParteNovedadId,Nombre,Nota,UsuarioId,VehiculoId,EsActivo,FechaCreacion")] ParteNovedad parteNovedad)
    {
        if (id != parteNovedad.ParteNovedadId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(parteNovedad);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParteNovedadExists(parteNovedad.ParteNovedadId))
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
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", parteNovedad.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", parteNovedad.VehiculoId);

        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        return View(new ParteNovedadRepuestoViewModel
        {
            ParteNovedad = parteNovedad,
            Repuesto = new(),
            Repuestos = repuestos
        });
    }

    // GET: ParteNovedades/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.ParteNovedades == null)
        {
            return NotFound();
        }

        var parteNovedad = await _context.ParteNovedades
            .Include(p => p.Usuario)
            .Include(p => p.Vehiculo)
            .FirstOrDefaultAsync(m => m.ParteNovedadId == id);
        if (parteNovedad == null)
        {
            return NotFound();
        }

        return View(parteNovedad);
    }

    // POST: ParteNovedades/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.ParteNovedades == null)
        {
            return Problem("Entity set 'ApplicationDbContext.ParteNovedades'  is null.");
        }
        var parteNovedad = await _context.ParteNovedades.FindAsync(id);

        if (parteNovedad != null)
        {
            _context.ParteNovedades.Remove(parteNovedad);

            var repuestos = await _context.Repuestos.Where(x => x.ParteNovedadId == parteNovedad.ParteNovedadId).ToListAsync();

            foreach (var repuesto in repuestos)
                _context.Repuestos.Remove(repuesto);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("AddRepuesto")]
    [ValidateAntiForgeryToken]
    public IActionResult AddRepuesto([Bind("RepuestoId,Nombre,Descripcion,Razon,Cost,ParteNovedadId,ParteNovedadId")] Repuesto repuesto)
    {
        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        var parteNovedad = GetObjectFromSession<ParteNovedad>(nameof(ParteNovedad)) ?? new ParteNovedad();

        if (ModelState.IsValid)
        {
            if (parteNovedad.ParteNovedadId > 0)
                repuesto.ParteNovedadId = parteNovedad.ParteNovedadId;

            repuestos.Add(repuesto);

            SetObjectToSession(nameof(Repuesto), repuestos);

            if (repuesto.ParteNovedadId > 0)
            {
                _context.Add(repuesto);
                _context.SaveChanges();
            }

        }

        if (parteNovedad != null && parteNovedad.ParteNovedadId > 0)
            return RedirectToAction("Edit", new { id = parteNovedad.ParteNovedadId });
        else
            return RedirectToAction("Create");
    }

    [HttpPost, ActionName("RemoveRepuesto")]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveRepuesto(int index)
    {
        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        var parteNovedad = GetObjectFromSession<ParteNovedad>(nameof(ParteNovedad)) ?? new ParteNovedad();

        if (repuestos[index] != null)
        {
            var repuesto = repuestos[index];

            if (repuesto.RepuestoId > 0)
            {
                _context.Repuestos.Remove(repuesto);
                _context.SaveChanges();
            }

            repuestos.RemoveAt(index);
        }

        SetObjectToSession(nameof(Repuesto), repuestos);

        if (parteNovedad != null && parteNovedad.ParteNovedadId > 0)
            return RedirectToAction("Edit", new { id = parteNovedad.ParteNovedadId });
        else
            return RedirectToAction("Create");
    }

    private T GetObjectFromSession<T>(string name)
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var sessionObject = HttpContext.Session.GetString(name);

        if (string.IsNullOrEmpty(sessionObject))
            return default;

        return JsonSerializer.Deserialize<T>(sessionObject, options);
    }

    private void SetObjectToSession<T>(string name, T @object)
    {

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var json = JsonSerializer.Serialize(@object, options);

        HttpContext.Session.SetString(name, json);
    }

    private bool ParteNovedadExists(int id)
    {
        return _context.ParteNovedades.Any(e => e.ParteNovedadId == id);
    }
}

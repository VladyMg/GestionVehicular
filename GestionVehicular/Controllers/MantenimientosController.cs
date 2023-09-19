using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GestionVehicular.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using OfficeOpenXml;
using System.Net.Mime;
using GestionVehiculos.Context;
using GestionVehicular.Models;

namespace GestionVehicular.Controllers;

[Authorize]
public class MantenimientosController : Controller
{
    private readonly ApplicationDbContext _context;

    public MantenimientosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Mantenimientos
    public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin)
    {
        var applicationDbContext = _context.Mantenimientos.AsQueryable();

            if (fechaInicio.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.FechaCreacion>= fechaInicio);
            }

            if (fechaFin.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.FechaCreacion <= fechaFin);
            }

            applicationDbContext = applicationDbContext.Include(s => s.TipoMantenimiento).Include(s => s.Usuario).Include(s => s.Vehiculo);

        var mantenimientos = await _context.Mantenimientos
        .Include(m => m.TipoMantenimiento)
        .Include(m => m.Usuario)
        .Include(m => m.Vehiculo)
        .ToListAsync();

        var aprobaciones = await _context.Aprobaciones
        .Where(x => x.MantenimientoId > 0)
        .ToListAsync();

        var query = from mantenimiento in mantenimientos
                    join aprobacion in aprobaciones
                    on mantenimiento.MantenimientoId equals aprobacion.MantenimientoId
                    select new Mantenimiento
                    {
                        MantenimientoId = mantenimiento.MantenimientoId,
                        Nombre = mantenimiento.Nombre,
                        Kilometraje = mantenimiento.Kilometraje,
                        Observacion = mantenimiento.Observacion,
                        UsuarioId = mantenimiento.UsuarioId,
                        VehiculoId = mantenimiento.VehiculoId,
                        TipoMantenimientoId = mantenimiento.TipoMantenimientoId,
                        EsActivo = mantenimiento.EsActivo,
                        FechaCreacion = mantenimiento.FechaCreacion,
                        Aprobacion = aprobacion
                    };

        /* return View(query.OrderByDescending(x => x.FechaCreacion).ToList()); */
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

        var aprovacion = await _context.Aprobaciones
        .FirstOrDefaultAsync(x => x.MantenimientoId == mantenimiento.MantenimientoId);

        mantenimiento.Aprobacion = aprovacion;

        return View(mantenimiento);
    }

    // GET: Mantenimientos/Create
    public IActionResult Create()
    {
        ViewData["TipoMantenimientoId"] = new SelectList(_context.TiposMantenimiento, "TipoMantenimientoId", "Nombre");
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula");
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa");

        var mantenimiento = GetObjectFromSession<Mantenimiento>(nameof(Mantenimiento)) ?? new Mantenimiento();

        if (mantenimiento.MantenimientoId > 0)
        {
            mantenimiento = new Mantenimiento();
            SetObjectToSession(nameof(Mantenimiento), mantenimiento);
        }

        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        if (repuestos.Any(x => x.MantenimientoId > 0))
        {
            repuestos = new List<Repuesto>();
            SetObjectToSession(nameof(Repuesto), repuestos);
        }

        return View(new MantenimientoRepuestoViewModel
        {
            Mantenimiento = mantenimiento,
            Repuesto = new(),
            Repuestos = repuestos
        });
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

            var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

            foreach (var repuesto in repuestos)
            {
                repuesto.MantenimientoId = mantenimiento.MantenimientoId;
                _context.Add(repuesto);
            }

            var aprobacion = new Aprobaciones
            {
                Tipo = "Mantenimiento",
                Estado = "Pendiente",
                MantenimientoId = mantenimiento.MantenimientoId,
                UsuarioId = mantenimiento.UsuarioId,
                FechaCreacion = DateTime.Now,
                EsActivo = true
            };

            _context.Add(aprobacion);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["TipoMantenimientoId"] = new SelectList(_context.TiposMantenimiento, "TipoMantenimientoId", "Nombre", mantenimiento.TipoMantenimientoId);
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", mantenimiento.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", mantenimiento.VehiculoId);

        var respuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        return View(new MantenimientoRepuestoViewModel
        {
            Mantenimiento = mantenimiento,
            Repuesto = new(),
            Repuestos = respuestos
        });
    }
// POST: Manteniminento/ExportarExcel        
        public async Task<IActionResult> ExportarExcel(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var applicationDbContext = _context.Mantenimientos.AsQueryable();

            if (fechaInicio.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.FechaCreacion >= fechaInicio);
            }

            if (fechaFin.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.FechaCreacion <= fechaFin);
            }

            applicationDbContext = applicationDbContext.Include(s => s.TipoMantenimiento).Include(s => s.Usuario).Include(s => s.Vehiculo);

            var mantenimientos = await applicationDbContext.ToListAsync();

            var report = (from s in mantenimientos
                          group s by new
                          {    
                              TipoMantenimiento = s.TipoMantenimiento.Nombre,
                              Usuario = s.Usuario.Cedula,
                              Vehiculo = s.Vehiculo.Placa,
                              /* TipoSugerenciaNombre = s.TipoSugerencia.Nombre,
                              CircuitoNombre = s.Circuito.Nombre,
                              SubcircuitoNombre = s.Subcircuito.Nombre, */
                          } into g
                          select new
                          {
                              FechaInicio = fechaInicio.ToString(),
                              FechaFin = fechaFin.ToString(),
                              Cantidad = g.Count(),
                              Tipo = g.Key.TipoMantenimiento,
                              Usuario = g.Key.Usuario,
                              Vehiculo = g.Key.Vehiculo,
                          }
                          ).ToList();

            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Lista Mantenimientos");

            worksheet.Cells["A1"].LoadFromCollection(report, PrintHeaders: true);

            for (var col = 1; col < report.Count + 1; col++)
                worksheet.Column(col).AutoFit();

            // Convertir el paquete de Excel a un array de bytes
            byte[] fileBytes = package.GetAsByteArray();

            // Devolver el archivo de Excel al usuario
            return File(fileBytes, MediaTypeNames.Application.Octet, "mantenimientos.xlsx");
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
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", mantenimiento.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", mantenimiento.VehiculoId);

        var repuestos = await _context.Repuestos.Where(x => x.MantenimientoId == mantenimiento.MantenimientoId).ToListAsync();

        SetObjectToSession(nameof(Repuesto), repuestos);

        SetObjectToSession(nameof(Mantenimiento), mantenimiento);

        return View(new MantenimientoRepuestoViewModel
        {
            Mantenimiento = mantenimiento,
            Repuesto = new(),
            Repuestos = repuestos
        });
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
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", mantenimiento.UsuarioId);
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", mantenimiento.VehiculoId);

        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        return View(new MantenimientoRepuestoViewModel
        {
            Mantenimiento = mantenimiento,
            Repuesto = new(),
            Repuestos = repuestos
        });
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

            var repuestos = await _context.Repuestos.Where(x => x.MantenimientoId == mantenimiento.MantenimientoId).ToListAsync();

            foreach (var repuesto in repuestos)
                _context.Repuestos.Remove(repuesto);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("AddRepuesto")]
    [ValidateAntiForgeryToken]
    public IActionResult AddRepuesto([Bind("RepuestoId,Nombre,Descripcion,Razon,Cost,MantenimientoId,ParteNovedadId")] Repuesto repuesto)
    {
        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        var mantenimiento = GetObjectFromSession<Mantenimiento>(nameof(Mantenimiento)) ?? new Mantenimiento();

        if (ModelState.IsValid)
        {
            if (mantenimiento.MantenimientoId > 0)
                repuesto.MantenimientoId = mantenimiento.MantenimientoId;

            repuestos.Add(repuesto);

            SetObjectToSession(nameof(Repuesto), repuestos);

            if (repuesto.MantenimientoId > 0)
            {
                _context.Add(repuesto);
                _context.SaveChanges();
            }

        }

        if (mantenimiento != null && mantenimiento.MantenimientoId > 0)
            return RedirectToAction("Edit", new { id = mantenimiento.MantenimientoId });
        else
            return RedirectToAction("Create");
    }

    [HttpPost, ActionName("RemoveRepuesto")]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveRepuesto(int index)
    {
        var repuestos = GetObjectFromSession<List<Repuesto>>(nameof(Repuesto)) ?? new List<Repuesto>();

        var mantenimiento = GetObjectFromSession<Mantenimiento>(nameof(Mantenimiento)) ?? new Mantenimiento();

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

        if (mantenimiento != null && mantenimiento.MantenimientoId > 0)
            return RedirectToAction("Edit", new { id = mantenimiento.MantenimientoId });
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

    private bool MantenimientoExists(int id)
    {
        return _context.Mantenimientos.Any(e => e.MantenimientoId == id);
    }
}


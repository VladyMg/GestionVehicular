using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionVehicular.Models;
using GestionVehiculos.Context;

namespace GestionVehicular.Controllers
{
    public class AprobacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AprobacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aprobaciones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Aprobaciones.Include(a => a.Abastecimiento).Include(a => a.Mantenimiento).Include(a => a.Movilizacion).Include(a => a.ParteNovedad).Include(a => a.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Aprobaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Aprobaciones == null)
            {
                return NotFound();
            }

            var aprobaciones = await _context.Aprobaciones
                .Include(a => a.Abastecimiento)
                .Include(a => a.Mantenimiento)
                .Include(a => a.Movilizacion)
                .Include(a => a.ParteNovedad)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.AprobacionId == id);
            if (aprobaciones == null)
            {
                return NotFound();
            }

            return View(aprobaciones);
        }

        // GET: Aprobaciones/Create
        public IActionResult Create()
        {
            ViewData["AbastecimientoId"] = new SelectList(_context.Abastecimientos, "AbastecimientoId", "Gasolinera");
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre");
            ViewData["MovilizacionId"] = new SelectList(_context.Movilizaciones, "MovilizacionId", "Observacion");
            ViewData["PartesNovedadesId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Apellido");
            return View();
        }

        // POST: Aprobaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AprobacionId,Estado,MantenimientoId,PartesNovedadesId,AbastecimientoId,UsuarioId,MovilizacionId,FechaCreacion,EsActivo")] Aprobaciones aprobaciones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aprobaciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AbastecimientoId"] = new SelectList(_context.Abastecimientos, "AbastecimientoId", "Gasolinera", aprobaciones.AbastecimientoId);
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre", aprobaciones.MantenimientoId);
            ViewData["MovilizacionId"] = new SelectList(_context.Movilizaciones, "MovilizacionId", "Observacion", aprobaciones.MovilizacionId);
            ViewData["PartesNovedadesId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre", aprobaciones.PartesNovedadesId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Apellido", aprobaciones.UsuarioId);
            return View(aprobaciones);
        }

        // GET: Aprobaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Aprobaciones == null)
            {
                return NotFound();
            }

            var aprobaciones = await _context.Aprobaciones.FindAsync(id);
            if (aprobaciones == null)
            {
                return NotFound();
            }
            ViewData["AbastecimientoId"] = new SelectList(_context.Abastecimientos, "AbastecimientoId", "Gasolinera", aprobaciones.AbastecimientoId);
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre", aprobaciones.MantenimientoId);
            ViewData["MovilizacionId"] = new SelectList(_context.Movilizaciones, "MovilizacionId", "Observacion", aprobaciones.MovilizacionId);
            ViewData["PartesNovedadesId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre", aprobaciones.PartesNovedadesId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Apellido", aprobaciones.UsuarioId);
            return View(aprobaciones);
        }

        // POST: Aprobaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AprobacionId,Estado,MantenimientoId,PartesNovedadesId,AbastecimientoId,UsuarioId,MovilizacionId,FechaCreacion,EsActivo")] Aprobaciones aprobaciones)
        {
            if (id != aprobaciones.AprobacionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aprobaciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AprobacionesExists(aprobaciones.AprobacionId))
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
            ViewData["AbastecimientoId"] = new SelectList(_context.Abastecimientos, "AbastecimientoId", "Gasolinera", aprobaciones.AbastecimientoId);
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre", aprobaciones.MantenimientoId);
            ViewData["MovilizacionId"] = new SelectList(_context.Movilizaciones, "MovilizacionId", "Observacion", aprobaciones.MovilizacionId);
            ViewData["PartesNovedadesId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre", aprobaciones.PartesNovedadesId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Apellido", aprobaciones.UsuarioId);
            return View(aprobaciones);
        }

        // GET: Aprobaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Aprobaciones == null)
            {
                return NotFound();
            }

            var aprobaciones = await _context.Aprobaciones
                .Include(a => a.Abastecimiento)
                .Include(a => a.Mantenimiento)
                .Include(a => a.Movilizacion)
                .Include(a => a.ParteNovedad)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.AprobacionId == id);
            if (aprobaciones == null)
            {
                return NotFound();
            }

            return View(aprobaciones);
        }

        // POST: Aprobaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Aprobaciones == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Aprobaciones'  is null.");
            }
            var aprobaciones = await _context.Aprobaciones.FindAsync(id);
            if (aprobaciones != null)
            {
                _context.Aprobaciones.Remove(aprobaciones);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AprobacionesExists(int id)
        {
          return _context.Aprobaciones.Any(e => e.AprobacionId == id);
        }
    }
}

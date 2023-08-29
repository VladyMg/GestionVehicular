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
    public class RepuestosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepuestosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repuestos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Repuestos.Include(r => r.Mantenimiento).Include(r => r.ParteNovedad);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Repuestos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Repuestos == null)
            {
                return NotFound();
            }

            var repuesto = await _context.Repuestos
                .Include(r => r.Mantenimiento)
                .Include(r => r.ParteNovedad)
                .FirstOrDefaultAsync(m => m.RepuestoId == id);
            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }

        // GET: Repuestos/Create
        public IActionResult Create()
        {
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre");
            ViewData["ParteNovedadId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre");
            return View();
        }

        // POST: Repuestos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepuestoId,Nombre,Descripcion,Razon,Cost,MantenimientoId,ParteNovedadId")] Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repuesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre", repuesto.MantenimientoId);
            ViewData["ParteNovedadId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre", repuesto.ParteNovedadId);
            return View(repuesto);
        }

        // GET: Repuestos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Repuestos == null)
            {
                return NotFound();
            }

            var repuesto = await _context.Repuestos.FindAsync(id);
            if (repuesto == null)
            {
                return NotFound();
            }
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre", repuesto.MantenimientoId);
            ViewData["ParteNovedadId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre", repuesto.ParteNovedadId);
            return View(repuesto);
        }

        // POST: Repuestos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepuestoId,Nombre,Descripcion,Razon,Cost,MantenimientoId,ParteNovedadId")] Repuesto repuesto)
        {
            if (id != repuesto.RepuestoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repuesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepuestoExists(repuesto.RepuestoId))
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
            ViewData["MantenimientoId"] = new SelectList(_context.Mantenimientos, "MantenimientoId", "Nombre", repuesto.MantenimientoId);
            ViewData["ParteNovedadId"] = new SelectList(_context.ParteNovedades, "ParteNovedadId", "Nombre", repuesto.ParteNovedadId);
            return View(repuesto);
        }

        // GET: Repuestos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Repuestos == null)
            {
                return NotFound();
            }

            var repuesto = await _context.Repuestos
                .Include(r => r.Mantenimiento)
                .Include(r => r.ParteNovedad)
                .FirstOrDefaultAsync(m => m.RepuestoId == id);
            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }

        // POST: Repuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Repuestos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Repuestos'  is null.");
            }
            var repuesto = await _context.Repuestos.FindAsync(id);
            if (repuesto != null)
            {
                _context.Repuestos.Remove(repuesto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepuestoExists(int id)
        {
          return _context.Repuestos.Any(e => e.RepuestoId == id);
        }
    }
}

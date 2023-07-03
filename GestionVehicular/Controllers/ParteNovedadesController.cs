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
            var applicationDbContext = _context.ParteNovedades.Include(p => p.Usuario).Include(p => p.Vehiculo);
            return View(await applicationDbContext.ToListAsync());
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

            return View(parteNovedad);
        }

        // GET: ParteNovedades/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa");
            return View();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Cedula", parteNovedad.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", parteNovedad.VehiculoId);
            return View(parteNovedad);
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
            return View(parteNovedad);
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
            return View(parteNovedad);
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParteNovedadExists(int id)
        {
            return _context.ParteNovedades.Any(e => e.ParteNovedadId == id);
        }
    }
}

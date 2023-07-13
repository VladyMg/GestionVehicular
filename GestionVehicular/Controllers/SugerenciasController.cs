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
    public class SugerenciasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SugerenciasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sugerencias
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sugerencias.Include(s => s.Circuito).Include(s => s.Subcircuito).Include(s => s.TipoSugerencia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sugerencias/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sugerencias == null)
            {
                return NotFound();
            }

            var sugerencia = await _context.Sugerencias
                .Include(s => s.Circuito)
                .Include(s => s.Subcircuito)
                .Include(s => s.TipoSugerencia)
                .FirstOrDefaultAsync(m => m.SugerenciaId == id);
            if (sugerencia == null)
            {
                return NotFound();
            }

            return View(sugerencia);
        }

        // GET: Sugerencias/Create
        public IActionResult Create()
        {
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito");
            ViewData["SubcircuitoId"] = new SelectList(_context.Subcircuitos, "SubcircuitoId", "CodSubcircuito");
            ViewData["TipoSugerenciaId"] = new SelectList(_context.TipoSugerencias, "TipoSugerenciaId", "Nombre");
            return View();
        }

        // POST: Sugerencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SugerenciaId,Nombres,Apellidos,Contacto,Detalle,Fecha,TipoSugerenciaId,CircuitoId,SubcircuitoId")] Sugerencia sugerencia)
        {
            if (ModelState.IsValid)
            {
                sugerencia.Fecha = DateTime.Now;
                _context.Add(sugerencia);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito", sugerencia.CircuitoId);
            ViewData["SubcircuitoId"] = new SelectList(_context.Subcircuitos, "SubcircuitoId", "CodSubcircuito", sugerencia.SubcircuitoId);
            ViewData["TipoSugerenciaId"] = new SelectList(_context.TipoSugerencias, "TipoSugerenciaId", "Nombre", sugerencia.TipoSugerenciaId);
            return View(sugerencia);
        }

        // GET: Sugerencias/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sugerencias == null)
            {
                return NotFound();
            }

            var sugerencia = await _context.Sugerencias.FindAsync(id);
            if (sugerencia == null)
            {
                return NotFound();
            }
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito", sugerencia.CircuitoId);
            ViewData["SubcircuitoId"] = new SelectList(_context.Subcircuitos, "SubcircuitoId", "CodSubcircuito", sugerencia.SubcircuitoId);
            ViewData["TipoSugerenciaId"] = new SelectList(_context.TipoSugerencias, "TipoSugerenciaId", "Nombre", sugerencia.TipoSugerenciaId);
            return View(sugerencia);
        }

        // POST: Sugerencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("SugerenciaId,Nombres,Apellidos,Contacto,Detalle,Fecha,TipoSugerenciaId,CircuitoId,SubcircuitoId")] Sugerencia sugerencia)
        {
            if (id != sugerencia.SugerenciaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sugerencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SugerenciaExists(sugerencia.SugerenciaId))
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
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito", sugerencia.CircuitoId);
            ViewData["SubcircuitoId"] = new SelectList(_context.Subcircuitos, "SubcircuitoId", "CodSubcircuito", sugerencia.SubcircuitoId);
            ViewData["TipoSugerenciaId"] = new SelectList(_context.TipoSugerencias, "TipoSugerenciaId", "Nombre", sugerencia.TipoSugerenciaId);
            return View(sugerencia);
        }

        // GET: Sugerencias/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sugerencias == null)
            {
                return NotFound();
            }

            var sugerencia = await _context.Sugerencias
                .Include(s => s.Circuito)
                .Include(s => s.Subcircuito)
                .Include(s => s.TipoSugerencia)
                .FirstOrDefaultAsync(m => m.SugerenciaId == id);
            if (sugerencia == null)
            {
                return NotFound();
            }

            return View(sugerencia);
        }

        // POST: Sugerencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sugerencias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sugerencias'  is null.");
            }
            var sugerencia = await _context.Sugerencias.FindAsync(id);
            if (sugerencia != null)
            {
                _context.Sugerencias.Remove(sugerencia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SugerenciaExists(int id)
        {
            return _context.Sugerencias.Any(e => e.SugerenciaId == id);
        }
    }
}

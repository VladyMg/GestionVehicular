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
    public class SubcircuitosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubcircuitosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subcircuitos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Subcircuito.Include(s => s.Circuito);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Subcircuitos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subcircuito == null)
            {
                return NotFound();
            }

            var subcircuito = await _context.Subcircuito
                .Include(s => s.Circuito)
                .FirstOrDefaultAsync(m => m.SubcircuitoId == id);
            if (subcircuito == null)
            {
                return NotFound();
            }

            return View(subcircuito);
        }

        // GET: Subcircuitos/Create
        public IActionResult Create()
        {
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito");
            return View();
        }

        // POST: Subcircuitos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubcircuitoId,Nombre,NoSubcircuito,CodSubcircuito,CircuitoId,EsActivo,FechaCreacion")] Subcircuito subcircuito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subcircuito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito", subcircuito.CircuitoId);
            return View(subcircuito);
        }

        // GET: Subcircuitos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subcircuito == null)
            {
                return NotFound();
            }

            var subcircuito = await _context.Subcircuito.FindAsync(id);
            if (subcircuito == null)
            {
                return NotFound();
            }
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito", subcircuito.CircuitoId);
            return View(subcircuito);
        }

        // POST: Subcircuitos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubcircuitoId,Nombre,NoSubcircuito,CodSubcircuito,CircuitoId,EsActivo,FechaCreacion")] Subcircuito subcircuito)
        {
            if (id != subcircuito.SubcircuitoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subcircuito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcircuitoExists(subcircuito.SubcircuitoId))
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
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "CodCircuito", subcircuito.CircuitoId);
            return View(subcircuito);
        }

        // GET: Subcircuitos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subcircuito == null)
            {
                return NotFound();
            }

            var subcircuito = await _context.Subcircuito
                .Include(s => s.Circuito)
                .FirstOrDefaultAsync(m => m.SubcircuitoId == id);
            if (subcircuito == null)
            {
                return NotFound();
            }

            return View(subcircuito);
        }

        // POST: Subcircuitos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subcircuito == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subcircuito'  is null.");
            }
            var subcircuito = await _context.Subcircuito.FindAsync(id);
            if (subcircuito != null)
            {
                _context.Subcircuito.Remove(subcircuito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubcircuitoExists(int id)
        {
          return _context.Subcircuito.Any(e => e.SubcircuitoId == id);
        }
    }
}

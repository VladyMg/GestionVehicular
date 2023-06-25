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
    public class CircuitosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CircuitosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Circuitos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Circuitos.Include(c => c.Distrito);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Circuitos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Circuitos == null)
            {
                return NotFound();
            }

            var circuito = await _context.Circuitos
                .Include(c => c.Distrito)
                .FirstOrDefaultAsync(m => m.CircuitoId == id);
            if (circuito == null)
            {
                return NotFound();
            }

            return View(circuito);
        }

        // GET: Circuitos/Create
        public IActionResult Create()
        {
            ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "CodDistrito");
            return View();
        }

        // POST: Circuitos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CircuitoId,Nombre,NoCircuito,CodCircuito,DistritoId,EsActivo,FechaCreacion")] Circuito circuito)
        {
            if (ModelState.IsValid)
            {
                circuito.EsActivo = true;
                circuito.FechaCreacion = DateTime.Now;
                _context.Add(circuito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "CodDistrito", circuito.DistritoId);
            return View(circuito);
        }

        // GET: Circuitos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Circuitos == null)
            {
                return NotFound();
            }

            var circuito = await _context.Circuitos.FindAsync(id);
            if (circuito == null)
            {
                return NotFound();
            }
            ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "CodDistrito", circuito.DistritoId);
            return View(circuito);
        }

        // POST: Circuitos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CircuitoId,Nombre,NoCircuito,CodCircuito,DistritoId,EsActivo,FechaCreacion")] Circuito circuito)
        {
            if (id != circuito.CircuitoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(circuito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CircuitoExists(circuito.CircuitoId))
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
            ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "CodDistrito", circuito.DistritoId);
            return View(circuito);
        }

        // GET: Circuitos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Circuitos == null)
            {
                return NotFound();
            }

            var circuito = await _context.Circuitos
                .Include(c => c.Distrito)
                .FirstOrDefaultAsync(m => m.CircuitoId == id);
            if (circuito == null)
            {
                return NotFound();
            }

            return View(circuito);
        }

        // POST: Circuitos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Circuitos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Circuitos'  is null.");
            }
            var circuito = await _context.Circuitos.FindAsync(id);
            if (circuito != null)
            {
                _context.Circuitos.Remove(circuito);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CircuitoExists(int id)
        {
            return _context.Circuitos.Any(e => e.CircuitoId == id);
        }
    }
}

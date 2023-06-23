using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

public class SubcircuitosController : Controller
    {
        
    private readonly ApplicationDbContext _context;

    public SubcircuitosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Subcircuito
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Subcircuitos.Include(u=> u.Circuito);
        return View(await applicationDbContext.ToListAsync());
    }

  // GET: Subcircuito/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Subcircuitos == null)
        {
            return NotFound();
        }

        var subcircuito = await _context.Subcircuitos
            .Include(u => u.Circuito)
            .FirstOrDefaultAsync(m => m.SubcircuitoId == id);
        if (subcircuito == null)
        {
            return NotFound();
        }

        return View(subcircuito);
    }
    // GET: Subcircuito/Create
    public IActionResult Create()
    {
        ViewData["Circuitoid"] = new SelectList(_context.Circuitos, "Circuitoid", "Nombre");
        return View();
    }

    // POST: Subcircuito/Create
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
        ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "Nombre", subcircuito.CircuitoId);
        return View(subcircuito);
    }
     // GET: Subcircuito/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Subcircuitos == null)
        {
            return NotFound();
        }

        var subcircuito = await _context.Subcircuitos.FindAsync(id);
        if (subcircuito == null)
        {
            return NotFound();
        }
        ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "Nombre", subcircuito.CircuitoId);
        return View(subcircuito);
    }

    // POST: Subcircuito/Edit/5
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
                if (!SubCircuitoExists(subcircuito.SubcircuitoId))
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
        ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "Nombre", subcircuito.CircuitoId);
        return View(subcircuito);
    }

    // GET: SubCircuito/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Subcircuitos == null)
        {
            return NotFound();
        }

        var subcircuito = await _context.Subcircuitos
            .Include(u => u.Circuito)
            .FirstOrDefaultAsync(m => m.SubcircuitoId == id);
        if (subcircuito == null)
        {
            return NotFound();
        }

        return View(subcircuito);
    }

    // POST: SubCircuito/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Subcircuitos == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Subcircuito'  is null.");
        }
        var subcircuito = await _context.Subcircuitos.FindAsync(id);
        if (subcircuito != null)
        {
            _context.Subcircuitos.Remove(subcircuito);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubCircuitoExists(int id)
    {
        return _context.Subcircuitos.Any(e => e.SubcircuitoId == id);
    }
}
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

public class CircuitosController : Controller
    {
        
    private readonly ApplicationDbContext _context;

    public CircuitosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Circuito
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Circuitos.Include(u=> u.Distrito);
        return View(await applicationDbContext.ToListAsync());
    }

  // GET: Circuito/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Circuitos == null)
        {
            return NotFound();
        }

        var circuito = await _context.Circuitos
            .Include(u => u.Distrito)
            .FirstOrDefaultAsync(m => m.CircuitoId == id);
        if (circuito == null)
        {
            return NotFound();
        }

        return View(circuito);
    }
    // GET: Circuito/Create
    public IActionResult Create()
    {
        ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "Nombre");
        return View();
    }

    // POST: Circuito/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CircuitoId,Nombre,NoCircuito,CodCircuito,DistritoId,Distrito,EsActivo,FechaCreacion")] Circuito Circuito)
    {
        if (ModelState.IsValid)
        {
            _context.Add(Circuito);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "Nombre", Circuito.DistritoId);
        return View(Circuito);
    }
     // GET: Circuito/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Circuitos == null)
        {
            return NotFound();
        }

        var Circuito = await _context.Circuitos.FindAsync(id);
        if (Circuito == null)
        {
            return NotFound();
        }
        ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "Nombre", Circuito.DistritoId);
        return View(Circuito);
    }

    // POST: Roles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("RolId,Nombre,EsActivo,FechaCreacion")] Circuito circuito)
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
        ViewData["DistritoId"] = new SelectList(_context.Distritos, "DistritoId", "Nombre", circuito.DistritoId);
        return View(circuito);
    }

    // GET: Circuito/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Circuitos == null)
        {
            return NotFound();
        }

        var Circuito = await _context.Circuitos
            .Include(u => u.Distrito)
            .FirstOrDefaultAsync(m => m.CircuitoId == id);
        if (Circuito == null)
        {
            return NotFound();
        }

        return View(Circuito);
    }

    // POST: Circuito/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Circuitos == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Circuito'  is null.");
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
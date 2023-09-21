using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

[Authorize]
public class GasolinerasController : Controller
{
    private readonly ApplicationDbContext _context;

    public GasolinerasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Gasolineras
    public async Task<IActionResult> Index()
    {
        return View(await _context.Gasolineras.ToListAsync());
    }

    // GET: Gasolineras/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Gasolineras == null)
        {
            return NotFound();
        }

        var gasolinera = await _context.Gasolineras
            .FirstOrDefaultAsync(m => m.GasolineraId == id);
        if (gasolinera == null)
        {
            return NotFound();
        }

        return View(gasolinera);
    }

    // GET: Gasolineras/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Gasolineras/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("GasolineraId,Nombre,Direccion,Telefono,EsActivo")] Gasolinera gasolinera)
    {
        if (ModelState.IsValid)
        {
            gasolinera.EsActivo = true;            
            _context.Add(gasolinera);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(gasolinera);
    }

    // GET: Gasolineras/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Gasolineras == null)
        {
            return NotFound();
        }

        var gasolinera = await _context.Gasolineras.FindAsync(id);
        if (gasolinera == null)
        {
            return NotFound();
        }
        return View(gasolinera);
    }

    // POST: Gasolineras/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("GasolineraId,Nombre,Direccion,Telefono,EsActivo")] Gasolinera gasolinera)
    {
        if (id != gasolinera.GasolineraId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(gasolinera);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GasolineraExists(gasolinera.GasolineraId))
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
        return View(gasolinera);
    }

    // GET: Gasolineras/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Gasolineras == null)
        {
            return NotFound();
        }

        var gasolinera = await _context.Gasolineras
            .FirstOrDefaultAsync(m => m.GasolineraId == id);
        if (gasolinera == null)
        {
            return NotFound();
        }

        return View(gasolinera);
    }

    // POST: Gasolineras/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Gasolineras == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Gasolineras'  is null.");
        }
        var gasolinera = await _context.Gasolineras.FindAsync(id);
        if (gasolinera != null)
        {
            _context.Gasolineras.Remove(gasolinera);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GasolineraExists(int id)
    {
        return _context.Gasolineras.Any(e => e.GasolineraId == id);
    }
}

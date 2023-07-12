using Microsoft.EntityFrameworkCore;

namespace GestionVehicular.Controllers;

[Authorize]
public class TiposSugerenciaController : Controller
{
    private readonly ApplicationDbContext _context;

    public TiposSugerenciaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TipoSugerencia
    public async Task<IActionResult> Index()
    {
        return View(await _context.TipoSugerencias.ToListAsync());
    }

    // GET: TipoSugerencia/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.TipoSugerencias == null)
        {
            return NotFound();
        }

        var tiposugerencia = await _context.TipoSugerencias
            .FirstOrDefaultAsync(m => m.TipoSugerenciaId == id);
        if (tiposugerencia == null)
        {
            return NotFound();
        }

        return View(tiposugerencia);
    }

    // GET: TipoSugerencia/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TipoSugerencia/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TipoSugerenciaId,Nombre,EsActivo")] TipoSugerencia TipoSugerencia)
    {
        if (ModelState.IsValid)
        {
            TipoSugerencia.EsActivo = true;
            _context.Add(TipoSugerencia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(TipoSugerencia);
    }

    // GET: Sugerencia/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.TipoSugerencias == null)
        {
            return NotFound();
        }

        var tiposugerencia = await _context.TipoSugerencias.FindAsync(id);
        if (tiposugerencia == null)
        {
            return NotFound();
        }
        return View(tiposugerencia);
    }

    // POST: TipoSugerencia/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TipoSugerenciaId,Nombre,EsActivo")] TipoSugerencia TipoSugerencia)
    {
        if (id != TipoSugerencia.TipoSugerenciaId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(TipoSugerencia);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoSugerenciaExists(TipoSugerencia.TipoSugerenciaId))
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
        return View(TipoSugerencia);
    }

    // GET: TipoSugerencia/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.TipoSugerencias == null)
        {
            return NotFound();
        }

        var tiposugerencia = await _context.TipoSugerencias
            .FirstOrDefaultAsync(m => m.TipoSugerenciaId == id);
        if (tiposugerencia == null)
        {
            return NotFound();
        }

        return View(tiposugerencia);
    }

    // POST: TipoSugerencia/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.TipoSugerencias == null)
        {
            return Problem("Entity set 'ApplicationDbContext.TipoSugerencia'  is null.");
        }
        var tiposugerencia = await _context.TipoSugerencias.FindAsync(id);
        if (tiposugerencia != null)
        {
            _context.TipoSugerencias.Remove(tiposugerencia);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TipoSugerenciaExists(int id)
    {
        return _context.TipoSugerencias.Any(e => e.TipoSugerenciaId == id);
    }
}

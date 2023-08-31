using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionVehicular.Models;
using GestionVehiculos.Context;
using GestionVehicular.Helpers;

namespace GestionVehicular.Controllers;

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
        var applicationDbContext = _context.Aprobaciones
        .Include(a => a.Abastecimiento)
        .Include(a => a.Mantenimiento)
        .Include(a => a.Movilizacion)
        .Include(a => a.ParteNovedad)
        .Include(a => a.Usuario)
        .OrderByDescending(x => x.FechaCreacion)
        .ThenByDescending(x => x.Estado);

        return View(await applicationDbContext.ToListAsync());
    }

    [HttpPost, ActionName("ApprobeRequest")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprobeRequest(int id)
    {

        var aprobacion = await _context.Aprobaciones.FirstOrDefaultAsync(x => x.AprobacionId == id);

        if (aprobacion != null)
        {
            aprobacion.Estado = "Aprobado";
            _context.Update(aprobacion);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("RejectedRequest")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectedRequest(int id)
    {

        var aprobacion = await _context.Aprobaciones.FirstOrDefaultAsync(x => x.AprobacionId == id);

        if (aprobacion != null)
        {
            aprobacion.Estado = "Rechazado";
            _context.Update(aprobacion);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}

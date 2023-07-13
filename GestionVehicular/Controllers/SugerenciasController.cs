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
    }
}

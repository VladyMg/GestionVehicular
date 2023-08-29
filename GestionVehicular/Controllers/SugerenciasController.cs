using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionVehicular.Models;
using GestionVehiculos.Context;
using OfficeOpenXml;
using System.Net.Mime;

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
        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var applicationDbContext = _context.Sugerencias.AsQueryable();

            if (fechaInicio.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Fecha >= fechaInicio);
            }

            if (fechaFin.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Fecha <= fechaFin);
            }

            applicationDbContext = applicationDbContext.Include(s => s.Circuito).Include(s => s.Subcircuito).Include(s => s.TipoSugerencia);

            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Sugerencias/Create
        public IActionResult Create()
        {
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "Nombre");
            ViewData["SubcircuitoId"] = new SelectList(_context.Subcircuitos, "SubcircuitoId", "Nombre");
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
            ViewData["CircuitoId"] = new SelectList(_context.Circuitos, "CircuitoId", "Nombre", sugerencia.CircuitoId);
            ViewData["SubcircuitoId"] = new SelectList(_context.Subcircuitos, "SubcircuitoId", "Nombre", sugerencia.SubcircuitoId);
            ViewData["TipoSugerenciaId"] = new SelectList(_context.TipoSugerencias, "TipoSugerenciaId", "Nombre", sugerencia.TipoSugerenciaId);
            return View(sugerencia);
        }

        // POST: Sugerencias/ExportarExcel        
        public async Task<IActionResult> ExportarExcel(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var applicationDbContext = _context.Sugerencias.AsQueryable();

            if (fechaInicio.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Fecha >= fechaInicio);
            }

            if (fechaFin.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Fecha <= fechaFin);
            }

            applicationDbContext = applicationDbContext.Include(s => s.Circuito).Include(s => s.Subcircuito).Include(s => s.TipoSugerencia);

            var sugerencias = await applicationDbContext.ToListAsync();

            var report = (from s in sugerencias
                          group s by new
                          {
                              TipoSugerenciaNombre = s.TipoSugerencia.Nombre,
                              CircuitoNombre = s.Circuito.Nombre,
                              SubcircuitoNombre = s.Subcircuito.Nombre,
                          } into g
                          select new
                          {
                              FechaInicio = fechaInicio.ToString(),
                              FechaFin = fechaFin.ToString(),
                              Cantidad = g.Count(),
                              Tipo = g.Key.TipoSugerenciaNombre,
                              Circuito = g.Key.CircuitoNombre,
                              Subcircuito = g.Key.SubcircuitoNombre,
                          }
                          ).ToList();

            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("SugerenciasReclamos");

            worksheet.Cells["A1"].LoadFromCollection(report, PrintHeaders: true);

            for (var col = 1; col < report.Count + 1; col++)
                worksheet.Column(col).AutoFit();

            // Convertir el paquete de Excel a un array de bytes
            byte[] fileBytes = package.GetAsByteArray();

            // Devolver el archivo de Excel al usuario
            return File(fileBytes, MediaTypeNames.Application.Octet, "sugerencias.xlsx");
        }
    }
}

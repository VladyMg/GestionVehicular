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
   public class ReporteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReporteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reporte vehiculo
        [Authorize]
        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var applicationDbContext = _context.Repuestos.AsQueryable();

            if (fechaInicio.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Mantenimiento.FechaCreacion >= fechaInicio);
            }

            if (fechaFin.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Mantenimiento.FechaCreacion <= fechaFin);
            }

            applicationDbContext = applicationDbContext.Include(s => s.Mantenimiento.TipoMantenimiento).Include(s => s.Mantenimiento.Usuario).Include(s => s.Mantenimiento.Vehiculo);

            return View(await applicationDbContext.ToListAsync());
            
        }


        
        // POST: Sugerencias/ExportarExcel        
        /* public async Task<IActionResult> ExportarExcel(DateTime? fechaInicio, DateTime? fechaFin)
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
        }*/
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionVehiculos.Context;
using GestionVehiculos.Models;

namespace GestionVehicular.Controllers
{
    public class DetailsModel : PageModel
    {
        private readonly GestionVehiculos.Context.ApplicationDbContext _context;

        public DetailsModel(GestionVehiculos.Context.ApplicationDbContext context)
        {
            _context = context;
        }

      public Usuario Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }
            else 
            {
                Usuario = usuario;
            }
            return Page();
        }
    }
}

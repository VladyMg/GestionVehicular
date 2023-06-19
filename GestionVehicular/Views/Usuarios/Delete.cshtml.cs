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
    public class DeleteModel : PageModel
    {
        private readonly GestionVehiculos.Context.ApplicationDbContext _context;

        public DeleteModel(GestionVehiculos.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                Usuario = usuario;
                _context.Usuarios.Remove(Usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

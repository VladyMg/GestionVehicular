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
    public class IndexModel : PageModel
    {
        private readonly GestionVehiculos.Context.ApplicationDbContext _context;

        public IndexModel(GestionVehiculos.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuario { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Usuarios != null)
            {
                Usuario = await _context.Usuarios.ToListAsync();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace GestionVehiculos.Context;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Distrito> Distritos { get; set; }

    public DbSet<Circuito> Circuitos { get; set; }

    public DbSet<Subcircuito> Subcircuitos { get; set; }
}
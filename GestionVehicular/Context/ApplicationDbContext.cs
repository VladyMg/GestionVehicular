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

    public DbSet<Vehiculo> Vehiculos { get; set; }

    public DbSet<TipoMantenimiento> TiposMantenimiento { get; set; }

    public DbSet<Mantenimiento> Mantenimientos { get; set; }

    public DbSet<ParteNovedad> ParteNovedades { get; set; }

    public DbSet<Abastecimiento> Abastecimientos { get; set; }

    public DbSet<Movilizacion> Movilizaciones { get; set; }

    public DbSet<Aprobaciones> Aprobaciones { get; set; }

    public DbSet<TipoSugerencia> TipoSugerencias { get; set; }

    public DbSet<Sugerencia> Sugerencias { get; set; }

    public DbSet<Repuesto> Repuestos { get; set; }


}
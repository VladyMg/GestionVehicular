using GestionVehiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionVehiculos.Context;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; }

    // public DbSet<User> Users { get; set; }
}
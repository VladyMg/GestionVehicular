

namespace GestionVehiculos.Models;
// table name
[Table("Roles")]
public class Role
{
    [Key]
    public int RolId { get; set; }
    public string Nombre { get; set; }
    public bool? EsActivo { get; set; }

    // Propiedad de navegación
    public ICollection<Usuario> Usuarios { get; set; }
}

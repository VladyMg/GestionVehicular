using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionVehiculos.Models;
// table name
[Table("Roles")]
public class Role
{
    [Key]
    public int RolId { get; set; }
    public string Nombre { get; set; }
    public bool? EsActivo { get; set; }
}

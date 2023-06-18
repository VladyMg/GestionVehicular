using System.ComponentModel.DataAnnotations;

namespace GestionVehiculos.Models;

public class Role
{
    [Key]
    public int RolId { get; set; }
    public string Nombre { get; set; }
    public bool? EsActivo { get; set; }
}

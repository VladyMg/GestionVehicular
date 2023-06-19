namespace GestionVehiculos.Models;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }
    public string Cedula { get; set; }
    public string Contrasenia { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string TipoSangre { get; set; }
    public string Telefono { get; set; }
    public string CiudadNacimiento { get; set; }
    public string Rango { get; set; }
    public bool? EsActivo { get; set; }
    public int? SubcircuitoId { get; set; }
    public string Password { get; set; }

    // Propiedad de navegación

    public int? RolId { get; set; }
    public Role Role { get; set; }
}
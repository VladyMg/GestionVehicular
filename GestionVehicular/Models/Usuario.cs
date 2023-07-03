namespace GestionVehicular.Models;

[Table("Usuario")]
public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }

    [Required]
    [MaxLength(10)]
    [MinLength(10)]
    public string Cedula { get; set; }

    [Required]
    [MaxLength(100)]
    public string Contrasenia { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(50)]
    public string Apellido { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }


    [Required]
    [MaxLength(10)]
    public string TipoSangre { get; set; }

    [Required]
    [MaxLength(10)]
    public string Telefono { get; set; }

    [Required]
    [MaxLength(50)]
    public string CiudadNacimiento { get; set; }

    [Required]
    [MaxLength(100)]
    public string Rango { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    [ForeignKey("Rol")]
    public int? RolId { get; set; }
    public virtual Role Rol { get; set; }

    [ForeignKey("Subcircuito")]
    public int? SubcircuitoId { get; set; }
    public virtual Subcircuito Subcircuito { get; set; }


}
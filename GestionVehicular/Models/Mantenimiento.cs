namespace GestionVehicular.Models;

[Table("Mantenimiento")]
public class Mantenimiento
{
    [Key]
    public int MantenimientoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; }

    [Required]
    public int Kilometraje { get; set; }

    [Required]
    [MaxLength(500)]
    public string Observacion { get; set; }

    [ForeignKey("Usuario")]
    public int? UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; }

    [ForeignKey("Vehiculo")]
    public int? VehiculoId { get; set; }
    public virtual Vehiculo Vehiculo { get; set; }

    [ForeignKey("TipoMantenimiento")]
    public int? TipoMantenimientoId { get; set; }
    public virtual TipoMantenimiento TipoMantenimiento { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    public string Estado { get; set; }

    // Navigation properties
    public virtual ICollection<Repuesto> Repuestos { get; set; }

    public virtual Aprobaciones Aprobacion { get; set; }
}
namespace GestionVehicular.Models;

[Table("TiposMantenimiento")]
public class TipoMantenimiento
{
    [Key]
    public int TipoMantenimientoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    // Navigation properties
    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; }
}
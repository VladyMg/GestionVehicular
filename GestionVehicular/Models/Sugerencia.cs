namespace GestionVehicular.Models;

[Table("Sugerencia")]
public class Sugerencia
{
    [Key]
    public int SugerenciaId { get; set; }

    [Required]
    [MaxLength(200)]    
    public string Nombres { get; set; }

    [Required]
    [MaxLength(200)]
    public string Apellidos { get; set; }

    [Required]
    [MaxLength(20)]
    public int Contacto { get; set; }

    [Required]
    [MaxLength(500)]
    public string Detalle { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [ForeignKey("TipoSugerencia")]
    public int? TipoSugerenciaId { get; set; }
    public virtual TipoSugerencia TipoSugerencia { get; set; }

    [ForeignKey("Circuito")]
    public int? CircuitoId { get; set; }
    public virtual Circuito Circuito { get; set; }

    [ForeignKey("Subcircuito")]
    public int? SubcircuitoId { get; set; }
    public virtual Subcircuito Subcircuito { get; set; }


}
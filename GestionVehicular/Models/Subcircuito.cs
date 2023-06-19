namespace GestionVehicular.Models;

[Table("Subcircuito")]
public class Subcircuito
{
    [Key]
    public int SubcircuitoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; }

    [Required]
    public int NoSubcircuito { get; set; }

    [Required]
    [MaxLength(1)]
    public string CodSubcircuito { get; set; }

    [ForeignKey("Circuito")]
    public int? CircuitoId { get; set; }
    public virtual Circuito Circuito { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }
}
namespace GestionVehicular.Models;

[Table("Circuito")]
public class Circuito
{
    [Key]
    public int CircuitoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; }

    [Required]
    public int NoCircuito { get; set; }

    [Required]
    [MaxLength(50)]
    public string CodCircuito { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    // Navigation Properties

    [ForeignKey("Distrito")]
    public int? DistritoId { get; set; }
    public virtual Distrito Distrito { get; set; }
    
    // SubCircuito
    public virtual ICollection<Subcircuito> Subcircuitos { get; set; }
    // Sugerencia
    public virtual ICollection<Sugerencia> Sugerencias { get; set; }

}
namespace GestionVehicular.Models;

// table name
[Table("TipoSugerencia")]

public class TipoSugerencia
{
    [Key]
    public int TipoSugerenciaId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    // Navigation properties
    public virtual ICollection<Sugerencia> Sugerencia { get; set; }

}
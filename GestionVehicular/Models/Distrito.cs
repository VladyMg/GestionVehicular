namespace GestionVehicular.Models;

[Table("Distrito")]

public class Distrito
{
    [Key]
    public int DistritoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(200)]
    public string Provincia { get; set; }

    [Required]
    public int NoDistrito { get; set; }

    [Required]
    [MaxLength(200)]
    public string Parroquia { get; set; }

    [Required]
    [MaxLength(50)]
    public string CodDistrito { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    // Navigation properties
    public virtual ICollection<Circuito> Circuitos { get; set; }
}
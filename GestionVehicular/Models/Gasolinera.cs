namespace GestionVehicular.Models;

[Table("Gasolinera")]

public class Gasolinera
{
    [Key]
    public int GasolineraId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(200)]
    public string Direccion { get; set; }

    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; }
    
    [Required]
    public bool EsActivo { get; set; }

    // Navigation properties
    public virtual ICollection<Abastecimiento> Abastecimientos { get; set; }
}
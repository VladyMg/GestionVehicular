namespace GestionVehicular.Models;

[Table("ParteNovedad")]
public class ParteNovedad
{
    [Key]
    public int ParteNovedadId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; }

    [Required]
    public string Nota { get; set; }

    [ForeignKey("Usuario")]
    public int? UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; }

    [ForeignKey("Vehiculo")]
    public int? VehiculoId { get; set; }
    public virtual Vehiculo Vehiculo { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    // Navigation properties
    public virtual ICollection<Repuesto> Repuestos { get; set; }
}
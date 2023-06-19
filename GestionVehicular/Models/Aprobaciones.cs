namespace GestionVehicular.Models;


[Table("Aprobaciones")]
public class Aprobaciones
{
    [Key]
    public int AprobacionId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Estado { get; set; }

    [ForeignKey("Mantenimiento")]
    public int? MantenimientoId { get; set; }
    public virtual Mantenimiento Mantenimiento { get; set; }

    [ForeignKey("ParteNovedad")]
    public int? PartesNovedadesId { get; set; }
    public virtual ParteNovedad ParteNovedad { get; set; }

    [ForeignKey("Abastecimiento")]
    public int? AbastecimientoId { get; set; }
    public virtual Abastecimiento Abastecimiento { get; set; }

    [ForeignKey("Usuario")]
    public int? UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; }

    [ForeignKey("Movilizacion")]
    public int? MovilizacionId { get; set; }
    public virtual Movilizacion Movilizacion { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    [Required]
    public bool EsActivo { get; set; }
}
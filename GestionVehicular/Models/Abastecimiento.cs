namespace GestionVehicular.Models;

[Table("Abastecimiento")]
public class Abastecimiento
{
    [Key]
    public int AbastecimientoId { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [MaxLength(200)]
    public string Gasolinera { get; set; }

    [Required]
    public TimeSpan HoraLlegada { get; set; }

    [Required]
    public TimeSpan HoraSalida { get; set; }

    [Required]
    public int Combustible { get; set; }

    [Required]
    public int KilometrosSalida { get; set; }

    [ForeignKey("Usuario")]
    public int? UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; }

    [ForeignKey("Vehiculo")]
    public int? VehiculoId { get; set; }
    public virtual Vehiculo Vehiculo { get; set; }

    [ForeignKey("Gasolinera")]
    public int? GasolineraId { get; set; }
    public virtual Gasolinera Gasolineras { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    public virtual Aprobaciones Aprobacion { get; set; }
}
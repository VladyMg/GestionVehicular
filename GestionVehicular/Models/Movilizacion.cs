namespace GestionVehicular.Models;

[Table("Movilizacion")]
public class Movilizacion
{
    [Key]
    public int MovilizacionId { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public TimeSpan HoraSalida { get; set; }

    [Required]
    public TimeSpan HoraLlegada { get; set; }

    [Required]
    public int KilometrosSalida { get; set; }

    [Required]
    public int KilometrajeLlegada { get; set; }

    [Required]
    [MaxLength(400)]
    public string Observacion { get; set; }

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

    public virtual Aprobaciones Aprobacion { get; set; }
}
namespace GestionVehicular.Models;


[Table("Vehiculo")]
public class Vehiculo
{
    [Key]
    public int VehiculoId { get; set; }

    [Required]
    [MaxLength(7)]
    public string Placa { get; set; }

    [Required]
    [MaxLength(100)]
    public string NumeroChasis { get; set; }

    [Required]
    [MaxLength(100)]
    public string NumeroMotor { get; set; }

    [Required]
    [MaxLength(1)]
    public string Modelo { get; set; }

    [Required]
    [MaxLength(100)]
    public string Marca { get; set; }

    [Required]
    public int Kilometraje { get; set; }

    [Required]
    [MaxLength(50)]
    public string Cilindraje { get; set; }

    [Required]
    [MaxLength(20)]
    public string CapacidadCarga { get; set; }

    [Required]
    public int CapacidadPasajero { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [ForeignKey("Subcircuito")]
    public int? SubcircuitoId { get; set; }
    public virtual Subcircuito Subcircuito { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }
}
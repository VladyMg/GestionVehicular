﻿namespace GestionVehicular.Models;

public class Repuesto
{
    [Key]
    public int RepuestoId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(500)]
    public string Descripcion { get; set; }

    [Required]
    [MaxLength(500)]
    public string Razon { get; set; }

    [Required]
    public decimal? Cost { get; set; }

    [ForeignKey("Mantenimiento")]
    public int? MantenimientoId { get; set; }
    public virtual Mantenimiento Mantenimiento { get; set; }

    [ForeignKey("ParteNovedad")]
    public int? ParteNovedadId { get; set; }
    public virtual ParteNovedad ParteNovedad { get; set; }
}
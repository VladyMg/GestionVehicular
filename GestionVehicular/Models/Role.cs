

namespace GestionVehicular.Models;

// table name
[Table("Role")]

public class Role
{
    [Key]
    public int RolId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }

    [Required]
    public bool EsActivo { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    // Navigation properties
    public virtual ICollection<Usuario> Usuarios { get; set; }
}
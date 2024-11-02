// mopdel Propeitarios
namespace api.Models;

[Table("propietario")]
public class Propietario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Required]
    [Column("apellido")]
    [StringLength(100)]
    public string Apellido { get; set; } = null!;

    [Required]
    [Column("dni")]
    [StringLength(20)]
    public string Dni { get; set; } = null!;

    [Required]
    [Column("email")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Column("telefono")]
    [StringLength(15)]
    public string Telefono { get; set; } = null!;

    //    [Required]
    [Column("password")]
    //[NotMapped]
    public string? Password { get; set; } = null!;

    [Column("avatar")]
    public string Avatar { get; set; } = null!;

    [Required]
    [Column("borrado")]
    public bool Borrado { get; set; }
    [Column("pass_restore")]
    public string? PassRestore { get; set; }
    // realcionws        
    public virtual ICollection<Inmueble> Inmuebles { get; set; } = new HashSet<Inmueble>();
    public virtual ICollection<Contrato> Contratos { get; set; } = new HashSet<Contrato>();

    public string toString()

    {
        return $"{Id} {Nombre} {Apellido} {Dni} {Email} {Password} {Avatar} {Borrado}";
    }

    //    public virtual ICollection<Inmueble> Inmuebles { get; set; } = new HashSet<Inmueble>();
}


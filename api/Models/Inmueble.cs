namespace api.Models;

[Table("inmueble")]
public class Inmueble
{
 [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("direccion")]
    public string Direccion { get; set; } = null!;

    [Column("uso")]
    public string Uso { get; set; } = null!;

    [Column("ciudad")]
    public string Ciudad { get; set; }=null!;    

    [Column("ambientes")]
    public sbyte Ambientes { get; set; }

    [Column("coordenadas")]
    public string? Coordenadas { get; set; }

    [Column("precio")]
    public decimal? Precio { get; set; }

    [ForeignKey("Propietario")]
    [Column("id_propietario")]
    public int PropietarioId { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = "Retirado";    

   [Required]
    [Column("borrado")]
    public bool Borrado { get; set; } = false;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("url_img")]
    public string? UrlImg { get; set; }
    [Column("tipo")]
    public string? Tipo { get; set; }


    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

}


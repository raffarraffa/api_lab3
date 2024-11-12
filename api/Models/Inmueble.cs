namespace api.Models;

[Table("inmueble")]
public class Inmueble
{
 [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("direccion")]
    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [JsonPropertyName("direccion")]
    public string Direccion { get; set; } = null!;

    [Column("uso")]
    [JsonPropertyName("uso")]
    public string Uso { get; set; } = null!;

    [Column("ciudad")]
    [JsonPropertyName("ciudad")]
    public string Ciudad { get; set; }=null!;    

    [Column("ambientes")]
    [JsonPropertyName("ambientes")]
    public sbyte Ambientes { get; set; }

    [Column("coordenadas")]
    public string? Coordenadas { get; set; }

    [Column("precio")]
    [JsonPropertyName("precio")]
    public decimal Precio { get; set; }

    [ForeignKey("Propietario")]
    [Column("id_propietario")]
    public int? PropietarioId { get; set; }

    [Column("estado")]    
    public string? Estado { get; set; } = "Retirado";    

   [Required]
    [Column("borrado")]
    public bool? Borrado { get; set; } = false;

    [Column("descripcion")]
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }

    [Column("url_img")]
    public string? UrlImg { get; set; } ="default.jpg";
    [Column("tipo")]
    public string? Tipo { get; set; }


    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public override string ToString()
{
    return $"Id: {Id}, Direccion: {Direccion}, Uso: {Uso}, Ciudad: {Ciudad}, Ambientes: {Ambientes}, " +
           $"Coordenadas: {Coordenadas}, Precio: {Precio.ToString() ?? "N/A"}, PropietarioId: {PropietarioId}, " +
           $"Estado: {Estado}, Borrado: {Borrado}, Descripcion: {Descripcion}, UrlImg: {UrlImg}, Tipo: {Tipo}";
}


}



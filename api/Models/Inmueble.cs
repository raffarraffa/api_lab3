using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
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

        [ForeignKey("TipoInmueble")]
        [Column("id_tipo")]
        public int IdTipo { get; set; }

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
        public string Estado { get; set; } = null!;

        [ForeignKey("Ciudad")]
        [Column("id_ciudad")]
        public int IdCiudad { get; set; }

        [ForeignKey("Zona")]
        [Column("id_zona")]
        public int IdZona { get; set; }
        [Required]
        [Column("borrado")]
        public bool Borrado { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("url_img")]
        public string? UrlImg { get; set; }

        // atrib  navegación 
        public virtual TipoInmueble TipoInmueble { get; set; } = null!;
        //        public virtual Propietario Propietario { get; set; } = null!;
        public virtual Ciudad Ciudad { get; set; } = null!;
        public virtual Zona Zona { get; set; } = null!;

        // del uno -> muchos
        public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    }
}

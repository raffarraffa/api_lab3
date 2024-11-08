
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("contrato")]
    public class Contrato
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Inquilino")]
        [Column("id_inquilino")]
        public int IdInquilino { get; set; }

        [ForeignKey("Inmueble")]
        [Column("id_inmueble")]
        public int IdInmueble { get; set; }

        [Column("fecha_inicio")]
        public DateOnly? FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateOnly? FechaFin { get; set; }

        [Column("fecha_efectiva")]
        public DateOnly? FechaEfectiva { get; set; }

        [Column("monto")]
        public decimal? Monto { get; set; }

        [Column("borrado")]
        public bool? Borrado { get; set; }

        [Column("creado_fecha")]
        public DateTime CreadoFecha { get; set; }

        [Column("creado_usuario")]
        public int CreadoUsuario { get; set; }

        [Column("cancelado_fecha")]
        public DateTime? CanceladoFecha { get; set; }

        [Column("cancelado_usuario")]
        public int CanceladoUsuario { get; set; }

        [Column("editado_usuario")]
        public int? EditadoUsuario { get; set; }

        [Column("editado_fecha")]
        public DateTime? EditadoFecha { get; set; }

        [Column("propietario_id")]
        public int PropietarioId  {get; set; }

        // attribe navegacion
        public virtual Inquilino Inquilino { get; set; } = null!;
        public virtual Inmueble Inmueble { get; set; } = null!;

        // realcion uno ->muchos
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("pago")]
    public class Pago
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("id_contrato")]
        public int IdContrato { get; set; }

        [Required]
        [Column("fecha_pago")]
        public DateOnly FechaPago { get; set; }

        [Required]
        [Column("importe")]
        [Range(0, double.MaxValue)]
        public decimal Importe { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Required]
        [Column("numero_pago")]
        public uint NumeroPago { get; set; }
        [Required]
        [Column("detalle")]
        public string Detalle { get; set; } = null!;

        [Required]
        [Column("creado_fecha")]
        public DateTime CreadoFecha { get; set; }

        [Required]
        [Column("creado_usuario")]
        public int CreadoUsuario { get; set; }

        [Column("editado_usuario")]
        public int? EditadoUsuario { get; set; }

        [Column("editado_fecha")]
        public DateTime? EditadoFecha { get; set; }
        // atributo d enavegacion
        //    [ForeignKey("IdContrato")]
        //        public virtual Contrato Contrato { get; set; }
    }
}

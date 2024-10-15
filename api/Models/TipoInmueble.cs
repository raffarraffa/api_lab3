using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("tipo_inmueble")]
    public class TipoInmueble
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tipo")]
        [StringLength(100)]
        public string Tipo { get; set; } = null!;

        [Required]
        [Column("borrado")]
        public bool Borrado { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("ciudad")]
    public class Ciudad
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ciudad")]
        public string NombreCiudad { get; set; } = null!;
        // inmuebles -> ciudad
        //    public virtual ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    }
}

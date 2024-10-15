using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("zona")]
    public class Zona
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("zona")]
        public string Zona1 { get; set; } = null!;

        //  uno -> muuchos inmuebles
        public virtual ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    }
}

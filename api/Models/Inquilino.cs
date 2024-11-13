using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

    [Table("inquilino")]
    public class Inquilino
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column("apellido")]
        public string Apellido { get; set; } = null!;

        [Required]
        [Column("dni")]
        public string Dni { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Required]
        [Column("telefono")]
        public string Telefono { get; set; } = null!;

        [Required]
        [Column("borrado")]
        public bool Borrado { get; set; }
        // atrib navegaion uno->muchos
      //  public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
    }


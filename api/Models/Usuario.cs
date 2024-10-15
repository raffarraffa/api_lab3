using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("usuario")]
    public class Usuario
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
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [Column("password")]
        public string Password { get; set; } = null!;

        [Required]
        [Column("rol")]
        [StringLength(20)]
        public string Rol { get; set; } = null!;

        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [Column("borrado")]
        [Required]
        public bool Borrado { get; set; }

        [Column("update_at")]
        public DateTime UpdateAt { get; set; }
    }
}

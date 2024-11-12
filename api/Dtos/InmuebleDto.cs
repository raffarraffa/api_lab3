using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class InmuebleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "La dirección debe tener al menos 3 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El uso es obligatorio.")]
        public string Uso { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string Ciudad { get; set; }

        [Range(1, 12, ErrorMessage = "El número de ambientes debe estar entre 1 y 12.")]
        public sbyte Ambientes { get; set; }

        public string? Coordenadas { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un número positivo.")]
        public decimal Precio { get; set; }

        public int PropietarioId { get; set; } = 0;

        public string? Estado { get; set; } = "Retirado";

        [Required(ErrorMessage = "El campo borrado es obligatorio.")]
        public bool? Borrado { get; set; } = false;

        public string? Descripcion { get; set; } = "N/D";
        public string? UrlImg { get; set; } = "no_img.png";
        public string? Tipo { get; set; } = "Casa";

        public override string ToString()
        {
            return @$"
                Direccion: {Direccion},
                Uso: {Uso},
                Ciudad: {Ciudad},
                Ambientes: {Ambientes},
                Precio: {Precio.ToString() ?? "N/A"},
                PropietarioId: {PropietarioId},
                Estado: {Estado},
                Descripcion: {Descripcion},
                UrlImg: {UrlImg},
                Tipo: {Tipo}
            ";
        }
    }
}

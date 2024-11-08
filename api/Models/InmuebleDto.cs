using System.Text.Json.Serialization;

namespace api.Models
{
    public class InmuebleDto
    {
        

        [JsonPropertyName("ambientes")]
        public sbyte Ambientes { get; set; }

        [JsonPropertyName("ciudad")]
        public string Ciudad { get; set; } = null!;

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("direccion")]
        public string Direccion { get; set; } = null!;

        [JsonPropertyName("precio")]
        public decimal? Precio { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = null!;

        [JsonPropertyName("uso")]
        public string Uso { get; set; } = null!;

        public string? UrlImg { get; set; }
        public int? PropietarioId { get; set; }

        // Método ToString() para representar el objeto como cadena
        public override string ToString()
        {
            return $"Propietario: {PropietarioId} Inmueble: {Tipo}, Dirección: {Direccion}, Ciudad: {Ciudad}, " +
                   $"Ambientes: {Ambientes}, Uso: {Uso}, Precio: {Precio?.ToString("C")}, " +
                   $"Descripción: {Descripcion}, URL Imagen: {UrlImg}";
        }
    }
}

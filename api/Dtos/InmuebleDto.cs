namespace api.Dtos;
    public class InmuebleDto
    {
        public int Id { get; set; }
        public string Direccion { get; set; }
        public string Uso { get; set; }
        public string Ciudad { get; set; }
        public sbyte Ambientes { get; set; }
        public string? Coordenadas { get; set; }
        public decimal? Precio { get; set; }
        public int? PropietarioId { get; set; }
        public string? Estado { get; set; } = "Retirado";
        public bool? Borrado { get; set; } = false;
        public string? Descripcion { get; set; }
        public string? UrlImg { get; set; }
        public string? Tipo { get; set; }
    }


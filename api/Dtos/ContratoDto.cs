using System;

namespace api.Dtos;
    public class ContratoDto
    {
        public int Id { get; set; }

        public int InquilinoId { get; set; }

        public int InmuebleId { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime? FechaEfectiva { get; set; }

        public decimal? Monto { get; set; }

        public bool? Borrado { get; set; }

        public DateTime CreadoFecha { get; set; }

        public int CreadoUsuario { get; set; }

        public DateTime? CanceladoFecha { get; set; }

        public int CanceladoUsuario { get; set; }

        public int? EditadoUsuario { get; set; }

        public DateTime? EditadoFecha { get; set; }

        public int PropietarioId { get; set; }
         public InquilinoDto Inquilino { get; set; } = null!;

        public InmuebleDto Inmueble { get; set; } = null!;

        public ICollection<PagoDto> Pagos { get; set; } = new List<PagoDto>();
    }

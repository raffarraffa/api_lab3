using System.Text.Json;
namespace api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContratoController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly AuthService _authService;
    public ContratoController(ApiDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
[HttpGet("listar")]
public async Task<ActionResult<List<ContratoDto>>> ObtenerContratos()
    {
        // Obtener el userId del token o claims
        var userId = Convert.ToInt32(_authService.GetUserClaims(User).GetValueOrDefault("UserId"));
        var fechaActual = DateTime.Now;        
        // Obtener los contratos relacionados con el usuario (propietario o inquilino)
        var contratos = await _context.Contratos
                                //     .Where(c => c.PropietarioId == userId || c.InquilinoId == userId)
                                .Where(c => c.PropietarioId == userId && c.FechaInicio <= fechaActual && c.FechaFin >= fechaActual)
                                    .Where(c => c.Borrado == false) 
                                    .Include(c => c.Inmueble) 
                                    .Include(c => c.Inquilino)
                                    .Include(c => c.Pagos.OrderBy(p => p.Id)) 
                                    .ToListAsync();
        var contratosDto = contratos.Select(c => new ContratoDto
        {
            Id = c.Id,
            FechaInicio = c.FechaInicio,
            FechaFin = c.FechaFin,
            Monto = c.Monto,
            Borrado = c.Borrado,
            Inmueble = new InmuebleDto
            {
                Id = c.Inmueble.Id,
                Direccion = c.Inmueble.Direccion,
                Uso = c.Inmueble.Uso,
                Ciudad = c.Inmueble.Ciudad,
                Precio = c.Inmueble.Precio,
                Estado = c.Inmueble.Estado,
                Descripcion = c.Inmueble.Descripcion,
                UrlImg = c.Inmueble.UrlImg,
                Tipo = c.Inmueble.Tipo
            },
            Inquilino = new InquilinoDto
            {
                Id = c.Inquilino.Id,
                Nombre = c.Inquilino.Nombre,
                Apellido = c.Inquilino.Apellido,
                Email = c.Inquilino.Email,
                Telefono = c.Inquilino.Telefono
            },
            Pagos = c.Pagos.Select(p => new PagoDto
            {
                Id = p.Id,
                Importe = p.Importe,
                FechaPago = p.FechaPago,
                Estado = p.Estado
            }).ToList()
        }).ToList();

        // Retornar la lista de contratos en formato JSON
        return Ok(contratosDto);
    }

}
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
    
    // Obtener los contratos relacionados con el usuario (propietario o inquilino)
    var contratos = await _context.Contratos
                                  .Where(c => c.PropietarioId == userId || c.InquilinoId == userId) // Filtra por propietario o inquilino
                                  .Where(c => c.Borrado == false) // Solo contratos activos
                                  .Include(c => c.Inmueble) // Incluir inmueble
                                  .Include(c => c.Inquilino) // Incluir inquilino
                                  .Include(c => c.Pagos) // Incluir pagos (si existe la relación)
                                  .ToListAsync();

    // Convertir los contratos a DTOs
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
            Email = c.Inquilino.Email
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



   /*
        var inmueblesContratos = _context.Contratos
                                  .Where(c => c.PropietarioId == userId && c.FechaInicio <= fechaActual && c.FechaFin >= fechaActual)
                                  .Include(c => c.Pagos )
                                  .Include(c => c.Inquilino)
                                  .ToList();  
    */                              


        /*var inmueblesContratos = _context.Inmuebles
                                    .Where(i => i.PropietarioId == 4)
                                    .Where(i=> i.Contratos.Any(c=> c.FechaInicio <= fechaActual && c.FechaFin >= fechaActual))
                                    .Select(i => new
                                    {
                                        InmuebleId = i.Id,
                                        Direccion =i.Direccion,                                      
                                    ContratosActivos = i.Contratos
                                        .Where(c=> c.FechaInicio <= fechaActual && c.FechaFin>=fechaActual)
                                        .Select(C=> new
                                        {
                                            Contratoid=C.Id,
                                            FechaInicio=C.FechaInicio,
                                            FechaFin = C.FechaFin
                                        })
                                    .ToList()
                                    })
                                    .ToList();
                                    */

    
//        return Ok(inmueblesContratos);
 //   }
    
}
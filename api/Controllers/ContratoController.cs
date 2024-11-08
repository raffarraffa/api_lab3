using System.Text.Json;
namespace api.Controllers;
//[Authorize]
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
    public ActionResult<List<Contrato>> ObtenerTodos(int? id )
    {
        var user = Convert.ToInt32(_authService.GetUserClaims(User).GetValueOrDefault("UserId"));
        var fechaActual = DateOnly.FromDateTime(DateTime.Now);
        var inmueblesContratos = _context.Contratos
                                  .Where(c => c.PropietarioId == id && c.FechaInicio <= fechaActual && c.FechaFin >= fechaActual)
                                  .Include(c => c.Pagos )
                                  .Include(c => c.Inquilino)
                                  .ToList();  


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

    
        return Ok(inmueblesContratos);
    }
    
}
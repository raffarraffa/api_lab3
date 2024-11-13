using System.Text.Json;
namespace api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InmuebleController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly AuthService _authService;
    public InmuebleController(ApiDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    [HttpGet("listar")]
    public ActionResult<List<Inmueble>> ObtenerTodos()
    {
        //    var user = _authService.GetUserClaims(User).GetValueOrDefault("UserId");

        var user = Convert.ToInt32(_authService.GetUserClaims(User).GetValueOrDefault("UserId"));
                Console.WriteLine("----------"+user);
        var inmuebles = _context.Inmuebles                
                .Where(i => i.PropietarioId == user )
              //  .Where(i => i.PropietarioId == user && i.Estado == "Disponible")
                .OrderBy(i => i.Id)              
                .ToList();
        return Ok(inmuebles);
    }

    [HttpDelete("{id:int?}")]
    public ActionResult<bool> EliminadoLogico(int id)
    {
        var user = Convert.ToInt32(_authService.GetUserClaims(User).GetValueOrDefault("UserId"));
        var inmueble = _context.Inmuebles
            .FirstOrDefault(i => i.Id == id && i.PropietarioId == user);
        if (inmueble == null)
        {
            return NotFound();
        }
        inmueble.Borrado = true;
        _context.SaveChanges();
        return Ok(true);
    }


    [HttpGet("{id}")]
    public ActionResult<Inmueble> ObtenerPorId(int id)
    {
        var user = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
        if (!int.TryParse(user, out int userId))
        {
            return BadRequest("El UserId debe ser un número entero.");
        }
        Console.WriteLine($"{{ ----- {userId} ---- -}}");

        var inmueble = _context.Inmuebles.FirstOrDefault(i => i.Id == id && i.PropietarioId == userId);
        if (inmueble == null)
        {
            Console.WriteLine("Inmueble no encontrado");
            return NotFound();
        }
        return Ok(inmueble);
    }
    
[HttpPost("new")]
public async Task<ActionResult> GuardarInmueble([FromForm] string inmueble, [FromForm] IFormFile imagen)
{  
   
    var user = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
    if (!int.TryParse(user, out int userId))
        return BadRequest("El Usuario no está identificado.");

    if (inmueble == null)
        return BadRequest(new { message = "No se ha enviado el inmueble." });    

     if (imagen == null)
         return BadRequest(new { message = "No se ha enviado la imagen." });    

    // validar si el archivo es una imagen
    if (!Utils.IsImageValid(imagen, 5 * 1024 * 1024))
        return BadRequest("El archivo enviado no es una imagen válida.");

    // deserializar el JSON del inmueble
      var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true 
        };
     var inmuebleDto = JsonSerializer.Deserialize<InmuebleDto>(inmueble, options); 
        
        if(inmuebleDto.Direccion.IsNullOrEmpty() || inmuebleDto.Ciudad.IsNullOrEmpty() || inmuebleDto.Descripcion.IsNullOrEmpty()|| inmuebleDto.Tipo.IsNullOrEmpty() || inmuebleDto.Uso.IsNullOrEmpty() || inmuebleDto.Precio==0)
            return BadRequest("Alguin dato sesta fuera de rango");
        try
        {
            //  nuevo nombre para el archivo de imagen
            var newFileName = Utils.renameFile(imagen);        
            // dir para guardar los archivos
            var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "files","inmuebles" );
            // directorio si no existe
                if (!Directory.Exists(pathDir)) 
                    Directory.CreateDirectory(pathDir);  
            // Guardar la imagen en el disco
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), pathDir, newFileName);    
            using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);    
                }
            // nombre imagen al DTO
            inmuebleDto.UrlImg = newFileName;
            // porpeirtario id al dto
            inmuebleDto.PropietarioId=userId;
            var inmuebleNew = MapInmuebleDtoToInmueble(inmuebleDto);
            
            //inmueble a  base de datos
            _context.Inmuebles.Add(inmuebleNew);
            await _context.SaveChangesAsync();        
            return Ok(new { message = "Inmueble guardado correctamente", inmueble });
        } 
        catch (UnauthorizedAccessException ex)
        {
            // ermisos de acceso
            Console.WriteLine($"Error de permisos: {ex.Message}");
        }
        catch (IOException ex)
        {
            //  error de i/o
                Console.WriteLine($"Error de entrada/salida: {ex.Message}");
            }
        catch (Exception ex)
            {    
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }   
        return StatusCode(500, new{message="Error de servidor"});       
    

}
  

[HttpPatch("changeEstado/{id}/{estado}")]
public  ActionResult<Inmueble> changeEstado(string estado, int id)
{
    
    var user = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
    if (!int.TryParse(user, out int userId))
        return BadRequest("El Usuario no está identificado.");
    string estadoCap = char.ToUpper(estado[0]) + estado.Substring(1).ToLower();        
    if (string.IsNullOrEmpty(estadoCap) || (estadoCap != "Retirado" && estadoCap !="Disponible" ))
        return BadRequest("Petición incorecta en datos");
    
    Inmueble? inmueble = _context.Inmuebles.FirstOrDefault(i => i.Id == id && i.PropietarioId == userId);
    if (inmueble == null)
        return NotFound($"Inmueble con ID {id} no encontrado.");       
    inmueble.Estado = estado;
    try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
        
            return StatusCode(500, $"Ocurrió un error al guardar los cambios: {ex.Message}");
        }      
       
    return Ok(inmueble);
}
    private InmuebleDto MapInmuebleToInmuebleDto(Inmueble inmueble)
    {
        return new InmuebleDto
        {
            Ambientes = inmueble.Ambientes,
            Ciudad = inmueble.Ciudad,
            Descripcion = inmueble.Descripcion,
            Direccion = inmueble.Direccion,
            Precio = inmueble.Precio,
            Tipo = inmueble.Tipo,
            Uso = inmueble.Uso,
            UrlImg = inmueble.UrlImg
        };
    }
    private Inmueble MapInmuebleDtoToInmueble(InmuebleDto inmuebleDto)
    {
        return new Inmueble
        {
            Ambientes = inmuebleDto.Ambientes,
            Ciudad = inmuebleDto.Ciudad.Trim(),
            Descripcion = inmuebleDto.Descripcion.Trim(),
            Direccion = inmuebleDto.Direccion.Trim(),
            Precio = inmuebleDto.Precio,
            Tipo = inmuebleDto.Tipo.Trim(),
            Uso = inmuebleDto.Uso.Trim(),
            UrlImg = inmuebleDto.UrlImg,
            PropietarioId= inmuebleDto.PropietarioId
        };
    }
}
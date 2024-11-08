using System.Text.Json;
namespace api.Controllers;
//[Authorize]
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
    
    [HttpPost("crear")]
public ActionResult<Inmueble> Crear([FromBody] Inmueble inmueble)
{
    Console.WriteLine(inmueble);
    var user = Convert.ToInt32(_authService.GetUserClaims(User).GetValueOrDefault("UserId"));
    inmueble.PropietarioId = user;

    if (ModelState.IsValid)
    {
        _context.Inmuebles.Add(inmueble);

        try
        {
            _context.SaveChanges();
            
             Console.WriteLine($"Inmueble creado con Id: {inmueble.Id}");

            // Devuelve el objeto creado con un código 201 (Creado)
            return CreatedAtAction(nameof(ObtenerPorId), new { id = inmueble.Id }, inmueble);
               //return StatusCode(201, inmueble);
        }
        catch (MySqlConnector.MySqlException mysqlEx) when (mysqlEx.Message.Contains("Data truncated"))
        {
            Console.WriteLine(inmueble);
            Console.WriteLine(inmueble);
            // Mensaje específico para errores de truncamiento en columnas ENUM
            return BadRequest("Uno de los valores enviados no coincide con los valores permitidos en la base de datos. Verifica los campos ENUM.");
        }
        catch (Exception ex)
        {
            // Captura de cualquier otro tipo de error
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    return BadRequest(ModelState);
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
public async Task<ActionResult> GuardarInmueble([FromForm] string? inmueble, [FromForm] IFormFile? imagen)
{
    var user = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
    if (!int.TryParse(user, out int userId))
        return BadRequest("El Usuario no está identificado.");

    if (imagen == null)
        return BadRequest(new { message = "No se ha enviado la imagen." });

    // validar si el archivo es una imagen
    if (!Utils.IsImageValid(imagen, 2 * 1024 * 1024))
        return BadRequest("El archivo enviado no es una imagen válida.");

    // deserializar el JSON del inmueble
    var inmuebleDto = JsonSerializer.Deserialize<InmuebleDto>(inmueble);    
 
    //  nuevo nombre para el archivo de imagen
    var newFileName = Utils.renameFile(imagen);        

    // imagen al DTO
    inmuebleDto.UrlImg = newFileName;
    inmuebleDto.PropietarioId=userId;
    var inmuebleNew = MapInmuebleDtoToInmueble(inmuebleDto);
    Console.WriteLine(inmuebleNew.ToString()); 

   // dir para guardar los archivos
    var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "files", userId.ToString());
    // directorio si no existe
        if (!Directory.Exists(pathDir)) 
            Directory.CreateDirectory(pathDir);  
    // Guardar la imagen en el disco
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), pathDir, newFileName);    
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await imagen.CopyToAsync(stream);
    }

        // Guardar el inmueble en la base de datos
        _context.Inmuebles.Add(inmuebleNew);
        await _context.SaveChangesAsync();

        // Retornar una respuesta adecuada
        return Ok(new { message = "Inmueble guardado correctamente", inmueble });
    

}



   

    [HttpPost("new2")] 
    public ActionResult GuardarFile2(
        [FromForm(Name = "inmueble")] string inmuebleJson,
        [FromForm(Name = "imagen")]  IFormFile? imagen)
        {
            if (imagen == null)
                return BadRequest(new { message = "No se ha enviado la imagen." });
        try
        {
            Inmueble inmueble2 = JsonSerializer.Deserialize<Inmueble>(inmuebleJson);
            Console.WriteLine(inmueble2.Ambientes);
            Console.WriteLine(Path.GetExtension(imagen.FileName));
            return Ok(new { message = "éxasdtfgfito" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "Ocurrió un error en el servidor", error = ex.Message });
        }
    }
         /*
        var headers = Request.Headers;
    Console.WriteLine("Encabezados de la solicitud:");
    foreach (var header in headers)
    {
        Console.WriteLine($"{header.Key}: {header.Value}");
    }
        // verifuicar  enviado un archivo
        if (imagen == null || imagen.Length == 0){
            Console.WriteLine("mierda 109");
            return BadRequest("No se ha enviado ningún archivo.");
        }

        // validar imagen
        if (!Utils.IsImageValid(imagen, 5 * 1024 * 1024)){
            Console.WriteLine("mierda 115");
            return BadRequest("El archivo enviado no es una imagen válida.");      
        }
        var _userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");

        if (!int.TryParse(_userId, out int userId))
            return BadRequest("Usuario inválido");
                
        Inmueble inmueble = _context.Inmuebles.FirstOrDefault(i => i.Id == id && i.PropietarioId == userId);

        // verifica inmueble y si propietario es elquien sube la imagen
        if (inmueble == null)
            return BadRequest("Usuario o Inmueble inválido");

        // dir para guardar los archivos
        var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "files", userId.ToString(), "inmuebles");

        // directorio si no existe
        if (!Directory.Exists(pathDir)) 
            Directory.CreateDirectory(pathDir);  

        // nuewvo nombre de imagen con un UUID
        var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";              
        var newFilePath = Path.Combine(pathDir, newFileName);       

        try
        {
            // savenuevo archivo
            using (var stream = new FileStream(newFilePath, FileMode.Create))
                imagen.CopyTo(stream);  
            
            // d3elete archivo viejo solo si s guarda nuevo
            if (inmueble.UrlImg != null)
            {
                var pathOldFile = Path.Combine(pathDir, inmueble.UrlImg);
                if (System.IO.File.Exists(pathOldFile))                 
                    System.IO.File.Delete(pathOldFile);                    
            }

            // saveUrlImg en el inmueble
            inmueble.UrlImg = newFileName;        
            _context.SaveChanges();

            return Ok(new { message = "Archivo subido con éxito", newFilePath, newFileName });
        }
        catch (Exception e)
        {            
            return StatusCode(500, new { message = "Ocurrió un error al guardar el archivo.", error = e.Message });
        } 
        */
   

// Métodos de mapeo manual
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
            Ciudad = inmuebleDto.Ciudad,
            Descripcion = inmuebleDto.Descripcion,
            Direccion = inmuebleDto.Direccion,
            Precio = inmuebleDto.Precio,
            Tipo = inmuebleDto.Tipo,
            Uso = inmuebleDto.Uso,
            UrlImg = inmuebleDto.UrlImg,
            PropietarioId= inmuebleDto.PropietarioId
        };
    }
}
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
        var inmuebles = _context.Inmuebles
                .Include(i => i.Ciudad)
                .Where(i => i.PropietarioId == user && i.Estado == "Disponible")
                .OrderBy(i => i.Id)
                .Select(i => new
                {
                    i.Id,
                    i.Ambientes,
                    i.Borrado,
                    i.Coordenadas,
                    i.Descripcion,
                    i.Direccion,
                    i.Estado,
                    i.IdCiudad,
                    i.IdTipo,
                    i.IdZona,
                    i.Precio,
                    i.PropietarioId,
                    i.UrlImg,
                    i.Uso,
                    CiudadNombre = i.Ciudad.NombreCiudad
                })
                .ToList();

        return Ok(inmuebles);
    }
    [HttpPost("create")]
    public ActionResult<Inmueble> Crear([FromBody] Inmueble inmueble)
    {
        if (ModelState.IsValid)
        {
            _context.Inmuebles.Add(inmueble);
            _context.SaveChanges();

            // Devuelve el objeto creado con un código 201 (Creado)
            return CreatedAtAction(nameof(ObtenerPorId), new { id = inmueble.Id }, inmueble);
        }

        // Si el modelo no es válido, devuelve un 400 (Solicitud incorrecta) con los errores
        return BadRequest(ModelState);
        // var user = Convert.ToInt32(_authService.GetUserClaims(User).GetValueOrDefault("UserId"));
        // inmueble.PropietarioId = user;        
        // _context.Inmuebles.Add(inmueble);
        // _context.SaveChanges();
        // return Ok(inmueble);
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

}
// public class InmuebleController : ControllerBase, IController<Inmueble>
// {
//     private readonly ApiDbContext _context;
//     private readonly AuthService _authService;
//     public InmuebleController(ApiDbContext context, AuthService authService)
//     {
//         _context = context;
//         _authService = authService;
//     }
//     [HttpPost("create")]
//     public ActionResult<Inmueble> Crear([FromBody] Inmueble Inmueble)
//     {
//         _context.Inmuebles.Add(Inmueble);
//         _context.SaveChanges();
//         return Ok(Inmueble);
//     }

//     [HttpGet("todos")]
//     public ActionResult<List<Inmueble>> ObtenerTodos()
//     {
//         return Ok(_context.Inmuebles.ToList());
//     }
//     [HttpGet("todosactivos")]
//     public ActionResult<List<Inmueble>> ObtenerActivos()
//     {
//         List<Inmueble> Inmuebles = _context.Inmuebles.ToList();
//         return Ok(Inmuebles);

//     }

//     [HttpGet("{id:int?}")]
//     public ActionResult<Inmueble> Obtener(int? id)
//     {

//         var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");


//         if (!int.TryParse(userId, out int UserIdInt))
//         {
//             //    return BadRequest("El UserId debe ser un número entero.");
//         }
//         if (id <= 0)
//         {
//             return BadRequest("El ID debe ser un número entero positivo.");
//         }

//         var inmueble = _context.Inmuebles.FirstOrDefaultAsync(i => i.Id == id);

//         return Ok(inmueble);
//         if (inmueble == null)
//         {
//             Console.WriteLine("Inmueble no encontrado");
//             return NotFound();
//         }
//         return Ok(inmueble);
//     }
//     [HttpGet("perfil")]
//     public ActionResult<Inmueble> PerfilSolo()
//     {
//         var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
//         if (!int.TryParse(userId, out int Id))
//         {
//             return BadRequest("El UserId debe ser un número entero.");
//         }
//         var Inmueble = _context.Inmuebles.FirstOrDefault(p => p.Id == Id);
//         if (Inmueble == null)
//         {
//             Console.WriteLine("Inmueble no encontrado");
//             return NotFound();
//         }
//         return Ok(Inmueble);
//     }

//     [HttpPatch("update")]
//     public ActionResult<Inmueble> Actualizar([FromBody] Inmueble Inmueble)
//     {
//         var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
//         Console.WriteLine(userId);
//         Console.WriteLine(Inmueble);
//         if (!int.TryParse(userId, out int Id))
//         {
//             return BadRequest();
//         }
//         Inmueble propDb = _context.Inmuebles.FirstOrDefault(p => p.Id == Id);
//         if (propDb == null)
//         {
//             return NotFound();
//         }

//         _context.SaveChanges();
//         return Ok(propDb);
//     }
//     [HttpPost("avatar")]
//     [HttpDelete]
//     public ActionResult<bool> EliminadoLogico([FromBody] int id)
//     {
//         var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
//         if (!int.TryParse(userId, out int Id))
//         {
//             return BadRequest();
//         }
//         Inmueble propDb = _context.Inmuebles.FirstOrDefault(p => p.Id == Id);
//         if (propDb == null)
//         {
//             return NotFound();
//         }
//         propDb.Borrado = true;
//         _context.SaveChanges();
//         return Ok(true);
//     }
//     [HttpPost]
//     public ActionResult GuardarFile([FromForm] IFormFile file)
//     {
//         var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");

//         Console.WriteLine(userId);
//         if (!int.TryParse(userId, out int Id))
//         {
//             return BadRequest();
//         }


//         if (file == null || file.Length == 0)
//             return BadRequest("No se ha enviado ningún archivo.");
//         // valido imgen    
//         if (!Utils.IsImageValid(file, 5 * 1024 * 1024))
//             return BadRequest("El archivo enviado no es una imagen.");

//         // dir  donde guardar los archivos
//         var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "files\\" + Id.ToString() + "\\avatares");

//         // ceo  directorio si no existe
//         if (!Directory.Exists(pathDir))
//         {
//             Directory.CreateDirectory(pathDir);
//         }
//         else
//         {
//             // delete archivos existentes en el directorio
//             var existingFiles = Directory.GetFiles(pathDir);
//             foreach (var existingFile in existingFiles)
//             {
//                 System.IO.File.Delete(existingFile);
//             }
//         }
//         // cambio nombre y pongo un uuid para evitar el cacheo si la cambiaron

//         var newFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
//         //var snakeFileName = $"{Path.GetFileNameWithoutExtension(file.newFileName).Replace(' ', '_')}"; // Reemplaza espacios con guiones bajos
//         string snakeFileName = newFileName.Replace(' ', '_');

//         //var newFileName = $"avatar_{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
//         var filePath = Path.Combine(pathDir, snakeFileName);

//         using (var stream = new FileStream(filePath, FileMode.Create))
//         {
//             file.CopyTo(stream);
//         }
//         Inmueble Inmueble = _context.Inmuebles.FirstOrDefault(p => p.Id == Id);
//         if (Inmueble == null)
//         {
//             return NotFound("Inmueble no encontrado.");
//         }

//         _context.SaveChanges();
//         return Ok(new { message = "Archivo subido con éxito", filePath });
//     }

// }


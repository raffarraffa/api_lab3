namespace api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PropietarioController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly AuthService _authService;
    public PropietarioController(ApiDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    [HttpGet("perfil")]
    public ActionResult<Propietario> Perfil()
    {
        var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
        if (!int.TryParse(userId, out int Id))
        {
            return BadRequest("El UserId debe ser un número entero.");
        }
        var propietario = _context.Propietarios
                                .Include(p => p.Inmuebles)
                                .FirstOrDefault(p => p.Id == Id);
        if (propietario == null)
        {
            Console.WriteLine("Propietario no encontrado");
            return NotFound();
        }
        return Ok(propietario);
    }
    [HttpGet("perfilonly")]
    public ActionResult<Propietario> PerfilSolo()
    {
        var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
        if (!int.TryParse(userId, out int Id))
        {
            return BadRequest("El UserId debe ser un número entero.");
        }
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Id == Id);
        if (propietario == null)
        {
            Console.WriteLine("Propietario no encontrado");
            return NotFound();
        }
        return Ok(propietario);
    }

    [HttpPatch("update")]
    public ActionResult<Propietario> Update([FromBody] Propietario propietario)
    {
        var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
        if (!int.TryParse(userId, out int Id))
        {
            return BadRequest();
        }
        Propietario propDb = _context.Propietarios.FirstOrDefault(p => p.Id == Id);
        if (propDb == null)
        {
            return NotFound();
        }
        propDb.Nombre = propietario.Nombre ?? propDb.Nombre;
        propDb.Apellido = propietario.Apellido ?? propDb.Apellido;
        propDb.Dni = propietario.Dni ?? propDb.Dni;
        propDb.Email = propietario.Email ?? propDb.Email;
        propDb.Telefono = propietario.Telefono ?? propDb.Telefono;
        propDb.Password = propietario.Password ?? propDb.Password;
        propDb.Avatar = propietario.Avatar ?? propDb.Avatar;
        _context.SaveChanges();
        return Ok(propDb);
    }
    [HttpPost("avatar")]

    public ActionResult AvatarImg([FromForm] IFormFile file)
    {
        var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");

        Console.WriteLine(userId);
        if (!int.TryParse(userId, out int Id))
        {
            return BadRequest();
        }


        if (file == null || file.Length == 0)
            return BadRequest("No se ha enviado ningún archivo.");
        // valido imgen    
        if (!Utils.IsImageValid(file, 5 * 1024 * 1024))
            return BadRequest("El archivo enviado no es una imagen.");

        // dir  donde guardar los archivos
        var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "files\\" + Id.ToString() + "\\avatares");

        // ceo  directorio si no existe
        if (!Directory.Exists(pathDir))
        {
            Directory.CreateDirectory(pathDir);
        }
        else
        {
            // delete archivos existentes en el directorio
            var existingFiles = Directory.GetFiles(pathDir);
            foreach (var existingFile in existingFiles)
            {
                System.IO.File.Delete(existingFile);
            }
        }
        // cambio nombre y pongo un uuid para evitar el cacheo si la cambiaron

        var newFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //var snakeFileName = $"{Path.GetFileNameWithoutExtension(file.newFileName).Replace(' ', '_')}"; // Reemplaza espacios con guiones bajos
        string snakeFileName = newFileName.Replace(' ', '_');

        //var newFileName = $"avatar_{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(pathDir, snakeFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        Propietario propietario = _context.Propietarios.FirstOrDefault(p => p.Id == Id);
        if (propietario == null)
        {
            return NotFound("Propietario no encontrado.");
        }
        propietario.Avatar = snakeFileName;
        _context.SaveChanges();
        return Ok(new { message = "Archivo subido con éxito", filePath });
    }

}


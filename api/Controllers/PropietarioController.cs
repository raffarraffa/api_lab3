namespace api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PropietarioController : ControllerBase, IController<Propietario>
{
    private readonly ApiDbContext _context;
    private readonly AuthService _authService;
    public PropietarioController(ApiDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    [HttpGet("perfil")]
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
        propietario.Password = null;
        //propietario.Avatar = propietario.Id + "/avatares/" + propietario.Avatar;
        return Ok(propietario);
    }
    [HttpPatch("update")]
        public ActionResult<Propietario> Actualizar([FromBody] Propietario propietario)
        {
            var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
            if (!int.TryParse(userId, out int Id))
            {
                return BadRequest();
            }

     
            Propietario? propDb = _context.Propietarios.FirstOrDefault(p => p.Id == Id);
            
            if (propDb == null)
            {
                return NotFound();
            }

            
            if (!string.IsNullOrEmpty(propietario.Nombre) && propDb.Nombre != propietario.Nombre)
            {
                propDb.Nombre = propietario.Nombre;
            }

            if (!string.IsNullOrEmpty(propietario.Apellido) && propDb.Apellido != propietario.Apellido)
            {
                propDb.Apellido = propietario.Apellido;
            }

            if (!string.IsNullOrEmpty(propietario.Dni) && propDb.Dni != propietario.Dni)
            {
                propDb.Dni = propietario.Dni;
            }

            if (!string.IsNullOrEmpty(propietario.Email) && propDb.Email != propietario.Email)
            {
                propDb.Email = propietario.Email;
            }

            if (!string.IsNullOrEmpty(propietario.Telefono) && propDb.Telefono != propietario.Telefono)
            {
                propDb.Telefono = propietario.Telefono;
            }
            
            propietario.Password = propietario.Password?.Trim();
            if (!string.IsNullOrEmpty(propietario.Password) && propietario.Password.Length > 3)
            {
                string hashedPassword = HashPassword.HashingPassword(propietario.Password);
                if (propDb.Password != hashedPassword)
                {
                    propDb.Password = hashedPassword;
                }
            }          
            
            _context.SaveChanges();

            return Ok(propDb);
        }
       
   [HttpPatch("avatar")]
    public async Task<ActionResult> GuardarFile([FromForm] IFormFile imagen)
    {
        var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
        Console.WriteLine(userId);        
        if (!int.TryParse(userId, out int Id))        
            return BadRequest("User no valido");
        Propietario propietario = _context.Propietarios.FirstOrDefault(p => p.Id == Id);
        if (propietario == null)
                return NotFound("Propietario no encontrado.");
        
        if (imagen == null || imagen.Length == 0)
            return BadRequest("No se ha enviado ningún archivo.");

        if (!Utils.IsImageValid(imagen, 5 * 1024 * 1024))
            return BadRequest("El archivo enviado no es una imagen Valida.");
        try{    
            // dir  donde guardar los archivos
            var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "files","avatares" );
            var oldAvatar= propietario.Avatar;         
            // ceo  directorio si no existe
            if (!Directory.Exists(pathDir))
                Directory.CreateDirectory(pathDir);
            //  nuevo nombre para el archivo de imagen
            var newFileName = Utils.renameFile(imagen);
            // Guardar la imagen en el disco
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), pathDir, newFileName);    
            using (var stream = new FileStream(filePath, FileMode.Create))
                 {
                    await imagen.CopyToAsync(stream);    
                 } 
        
        propietario.Avatar = newFileName;
         if (!string.IsNullOrEmpty(oldAvatar))
        {
            var oldFilePath = Path.Combine(pathDir, oldAvatar);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
        }

        _context.SaveChanges();
        
        return Ok(new { message = "Archivo subido con éxito", filePath });
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

    [HttpGet("todo")]
     public ActionResult<List<Propietario>> GetTodos()
     {
    var userId = _authService.GetUserClaims(User).GetValueOrDefault("UserId");
    var propietariosConContratosYPagos = _context.Propietarios
    .Where(p => p.Id == 4)  
    .Include(p => p.Inmuebles)
        .ThenInclude(i => i.Contratos)
            .ThenInclude(c => c.Pagos)
    .Include(p => p.Inmuebles)
        .ThenInclude(i => i.Contratos)
            .ThenInclude(c => c.Inquilino)
    .ToList();
    return Ok(propietariosConContratosYPagos);
     }

}


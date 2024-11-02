using System.Text.Json;

namespace api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly SendMailService _sendMailService;
    private readonly ApiDbContext _context;
    public LoginController(IConfiguration configuration, ApiDbContext context, SendMailService sendMailService)
    {
        var jwtConfig = configuration.GetSection("JWTAuthentication");
        string secretKey = jwtConfig["SecretKey"] ?? throw new ArgumentNullException("La clave JWT no está configurada.");
        string issuer = jwtConfig["Issuer"] ?? "localhost";
        string audience = jwtConfig["Audience"] ?? "localhost";
        _authService = new AuthService(secretKey, issuer, audience);
        _context = context;
        _sendMailService = sendMailService;
    }


    [HttpPost("")]

    public IActionResult Login([FromBody] LoginDto loginDto)

    {


        //docs suo DTo pra tenr un modelo parcial de propietario
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == loginDto.Email);
        Console.WriteLine(HashPassword.HashingPassword(loginDto.Password));
        Console.WriteLine(propietario?.Id);
        if (propietario == null || !HashPassword.isValidPassword(loginDto.Password, propietario.Password))
        {
            Console.WriteLine($" login:{loginDto.Email} pass:{loginDto.Password} passProp: {propietario?.Password}");

            return Unauthorized();
        }


        var claims = new List<Claim>
            {
            //    new Claim(JwtRegisteredClaimNames.Sub, propietario.Email),
                // Claim(ClaimTypes.Email, propietario.Email),
                new Claim(ClaimTypes.Name, propietario.Nombre),
//                new Claim(JwtRegisteredClaimNames.Sub, propietario.Id.ToString()),
                new Claim("UserId", propietario.Id.ToString()),
                new Claim("Email", propietario.Email)

                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var jtoken = _authService.GetToken(claims);
        loginDto.Token = jtoken;
        //    loginDto.Propietario = propietario;
        Console.WriteLine(loginDto);
        return Ok(jtoken);
        //return Ok(loginDto);
    }
    [HttpPost("passwordlost")]
    public IActionResult PasswordLostdddd([FromBody] LoginDto loginDto)
    {
        if (loginDto.Password == null || loginDto.Email == null)
        {
            return BadRequest("user o pass vacios");
        }
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == loginDto.Email);
        if (propietario == null)
        {
            return NotFound();
        }
        // creo json con  la contraseña y el timestamp
        var restorePass = new
        {
            pass = HashPassword.HashingPassword(loginDto.Password),
            timestamp = ((DateTimeOffset)DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds()
        };
        Console.WriteLine(restorePass);
        Console.WriteLine(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
        long lastSixDigits = restorePass.timestamp % 1000000;
        Console.WriteLine(lastSixDigits);
        Console.WriteLine(loginDto.Email);
        propietario.PassRestore = JsonSerializer.Serialize(restorePass);
        _context.SaveChanges();
        // string email = propietario.Email;
        // if (string.IsNullOrEmpty(email))
        // {
        //     return BadRequest(new { message = "El correo electrónico es requerido." });
        // }
        string body = $"Restaure su cuenta: use el password ingresado en la aplicacion, \n Luego le solicitara este Token de  6 digitos: {lastSixDigits} \n Si Ud. no solicito este token, ignore este correo, Ignorelo, su cuenta esta seguro. ";
        _sendMailService.SendMail(loginDto.Email, "Recupero password", body);
        return Ok(restorePass);
    }
    [HttpPost("acceptrestore")]
    public IActionResult AcceptRestore([FromBody] LoginDto loginDto)
    {
        string body = loginDto.Email + "-" + loginDto.Password;
        return Ok(new { msg = body });

        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == loginDto.Email);
        if (propietario == null)
        {
            return NotFound();
        }
        propietario.PassRestore = null;
        _context.SaveChanges();
        //return Ok(propietario);
    }

    [HttpPost("passwordrestore")]
    public IActionResult PasswordRestore([FromBody] LoginDto loginDto)
    {
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == loginDto.Email);
        if (propietario == null)
        {
            return NotFound();
        }
        // creo json con  la contraseña y el timestamp
        var restorePass = new
        {
            pass = HashPassword.HashingPassword(loginDto.Password),
            timestamp = ((DateTimeOffset)DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds()
        };
        Console.WriteLine(restorePass);
        Console.WriteLine(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
        long lastSixDigits = restorePass.timestamp % 1000000;
        Console.WriteLine(lastSixDigits);
        Console.WriteLine(loginDto.Email);

        // if (string.IsNullOrEmpty(email))
        // {
        //     return BadRequest(new { message = "El correo electrónico es requerido." });
        // }
        string body = @$"Restaure su cuenta: use el password ingresado en la aplicacion, y luego le solicitara este Token de  6 digitos : {lastSixDigits} \n 
                        Si Ud. no solcito este token, ignore este correo, Ignorelo, su cuenta esta seguro. 
                        Para restaurar su cuenta,  siga este link desde el dispositivo que utilizo para pedido la  restauracion http://localhost:8104/api/login/restore  ";

        _sendMailService.SendMail(loginDto.Email, "Recupero password", body);
        string mensaje = $"Se envió código de acceso a {loginDto.Email}. \n Verifique en su bandeja de entrada o en la carpeta Correo No deseado.";
        Console.WriteLine(mensaje);
        return Ok(new { msg = body });
        // return Content(mensaje);
        //   return Content(mensaje, "text/plain");
    }



}


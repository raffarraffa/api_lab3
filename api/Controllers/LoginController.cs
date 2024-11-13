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

                new Claim(ClaimTypes.Name, propietario.Nombre),
                new Claim("UserId", propietario.Id.ToString()),
                new Claim("Email", propietario.Email)
            };

        var jtoken = _authService.GetToken(claims);
        loginDto.Token = jtoken;
        Console.WriteLine(loginDto);
        return Ok(jtoken);
    }

    [HttpPost("passwordrestore")]
    public IActionResult PasswordRestore([FromBody] LoginDto loginDto)
    {
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == loginDto.Email);
        if (propietario == null)
           return NotFound();

      var claims = new List<Claim>
           {
              new Claim(ClaimTypes.Name, propietario.Nombre),
              new Claim("UserId", propietario.Id.ToString()),
              new Claim("Email", propietario.Email)
           };

        var jtoken = _authService.GetToken(claims);


        
        Random random = new Random();
        int otpAleatorio = random.Next(100000, 1000000);
        var otpHash= HashPassword.HashingPassword(otpAleatorio.ToString());
         propietario.PassRestore = otpHash;
        _context.SaveChanges();       
        string body = @$"
                <p>Restaure su cuenta: use el password ingresado en la aplicación y luego introduzca este token de 6 dígitos: {otpAleatorio}.</p>
                <p>Si usted no solicitó este token, ignore este correo. Su cuenta está segura.</p>
                <p>Para restaurar su cuenta, siga este <a href='http://localhost:8104/api/login/restore?token={jtoken}'> enlace de restauración</a> desde el dispositivo utilizado para la solicitud.</p>";
        _sendMailService.SendMail(loginDto.Email, "Recupero password", body);
        string mensaje = $"Se envió código de acceso a {loginDto.Email}. \n Verifique en su bandeja de entrada o en la carpeta Correo No deseado.";
        return Ok(new { msg = body });
    }
  
    [Authorize]
    [HttpPost("acceptrestore")]
    public IActionResult AcceptRestore([FromBody] LoginDto loginDto)
    {
       
        var user = _authService.GetUserClaims(User).GetValueOrDefault("UserId");         
        if (!int.TryParse(user, out int userId))
            return BadRequest("El Usuario no está identificado.");
        Console.WriteLine(106);

        var propietario = _context.Propietarios.FirstOrDefault(p => p.Id == userId);
        Console.WriteLine(userId);   
        if(propietario==null)
            return NotFound();
        Console.WriteLine(111);            
        
        if(propietario.PassRestore==null)
            return UnprocessableEntity("'Otp' nulo.");
        if(!HashPassword.isValidPassword(loginDto.Otp.ToString(), propietario.PassRestore))
             return Unauthorized(new {msg="otp invalida"});        
        Console.WriteLine(119);
         propietario.PassRestore = null;
        _context.SaveChanges();     
        return Ok(propietario);
    }


}


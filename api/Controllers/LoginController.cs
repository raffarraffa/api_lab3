namespace api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ApiDbContext _context;
    public LoginController(IConfiguration configuration, ApiDbContext context)
    {
        var jwtConfig = configuration.GetSection("JWTAuthentication");
        string secretKey = jwtConfig["SecretKey"] ?? throw new ArgumentNullException("La clave JWT no está configurada.");
        string issuer = jwtConfig["Issuer"] ?? "localhost";
        string audience = jwtConfig["Audience"] ?? "localhost";
        _authService = new AuthService(secretKey, issuer, audience);
        _context = context;
    }


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto) //suo DTo pra tenr un modelo parcial de propietario
    {
        var propietario = _context.Propietarios.FirstOrDefault(p => p.Email == loginDto.Email);
        Console.WriteLine(propietario.Id);
        if (propietario == null || !HashPassword.isValidPassword(loginDto.Password, propietario.Password))
        {
            Console.WriteLine($" login:{loginDto.Email} pass:{loginDto.Password}");
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

        return Ok(new { Token = jtoken });
    }
}


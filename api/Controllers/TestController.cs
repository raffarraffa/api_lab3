namespace api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly AuthService _authService;

    // Inyección de IConfiguration en el constructor
    public TestController(IConfiguration configuration)
    {
        // Leer la configuración desde appsettings.json
        var jwtConfig = configuration.GetSection("JWTAuthentication");
        string secretKey = jwtConfig["SecretKey"] ?? throw new ArgumentNullException("La clave JWT no está configurada.");
        string issuer = jwtConfig["Issuer"] ?? "localhost";
        string audience = jwtConfig["Audience"] ?? "localhost";
        // Inicializar AuthService con la configuración leída
        _authService = new AuthService(secretKey, issuer, audience);
    }

    [HttpGet("token")]
    public IActionResult GenerateToken()
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "nombreUsuafsdfffffffrio"),
                new Claim(JwtRegisteredClaimNames.Email, "email@ejemplo.com"),

            };

        // Generar el token
        var jtoken = _authService.GetToken(claims);
        // Devolver el token como respuesta
        return Ok(new { Token = jtoken });
    }
}


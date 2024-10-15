namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class Test2Controller : ControllerBase
    {
        private readonly AuthService _authService;

        public Test2Controller(AuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("user-info")]
        public IActionResult GetUserInfo()
        {
            // Usar el servicio para extraer los claims
            var claims = _authService.GetUserClaims(User);

            var userId = claims["UserId"];
            var email = claims["Email"];

            if (email == null || userId == null)
            {
                return Unauthorized();
            }

            return Ok(new { Email = email, UserId = userId });
        }
    }
}

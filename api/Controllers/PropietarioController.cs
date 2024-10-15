namespace api.Controllers
{
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

        [HttpGet]
        public ActionResult<IEnumerable<Propietario>> GetAll()
        {
            var propietarios = _context.Propietarios
            .Include(p => p.Inmuebles)
            // .Include(p => p.Contratos)
            // .ThenInclude(c => c.Pagos)
            .ToList();
            return Ok(propietarios);
        }
        [HttpGet("id")]
        public ActionResult<Propietario> GetById()
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

    }
}

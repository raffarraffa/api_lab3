
namespace api.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class PropietarioController : ControllerBase, IController<Propietario>
{
    private readonly ApiDbContext _context;

    public PropietarioController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Propietario>> GetAll()
    {
        var propietarios = _context.Propietarios
        .Include(p => p.Inmuebles)
        .ToList();
        return Ok(propietarios);
    }

    [HttpGet("{id}")]
    public ActionResult<Propietario> GetById(int id)
    {
        var propietario = _context.Propietarios
        .Include(p => p.Inmuebles)
        .Include(p => p.Contratos)
        .ThenInclude(c => c.Pagos)
        .FirstOrDefault(p => p.Id == id);

        if (propietario == null)
        {
            return NotFound();
        }
        return Ok(propietario);
    }

    [HttpPost]
    public ActionResult<Propietario> Post(Propietario entity)
    {
        _context.Propietarios.Add(entity);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Propietario entity)
    {
        _context.Propietarios.Update(entity);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var propietario = _context.Propietarios.Find(id);
        if (propietario == null)
        {
            return NotFound();
        }
        _context.Propietarios.Remove(propietario);
        _context.SaveChanges();
        return NoContent();
    }


}



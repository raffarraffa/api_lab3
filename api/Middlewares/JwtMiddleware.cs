// using Microsoft.AspNetCore.Http;
// using Microsoft.EntityFrameworkCore; 
// using Microsoft.IdentityModel.JsonWebTokens;
// using System.Security.Claims;
// using System.Threading.Tasks;

namespace api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiDbContext _context;

        public JwtMiddleware(RequestDelegate next, ApiDbContext context)
        {
            _next = next;
            _context = context;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            Console.WriteLine($"Buscando propietario con email 23");
            if (context.User.Identity.IsAuthenticated)

            {
                Console.WriteLine($"Buscando propietario con email 27");

                var emailClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (!string.IsNullOrEmpty(emailClaim))
                {

                    var propietario = await _context.Propietarios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Email == emailClaim);

                    if (propietario != null)
                    {

                        context.Items["Propietario"] = new
                        {
                            propietario.Email,
                            propietario.Nombre,
                            propietario.Id
                        };
                        Console.WriteLine(propietario.toString());
                    }
                }
                else
                {
                    Console.WriteLine("no hay email");
                }

            }

            // Llama al siguiente middleware en la cadena
            Console.WriteLine("Llamando al siguiente middleware...");
            await _next(context);
        }
    }
}


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ConnectionProduction");
var configuration = builder.Configuration;
var jwtConfig = configuration.GetSection("JWTAuthentication");
var issuer = jwtConfig.GetValue<string>("Issuer") ?? "localhost";
var audience = jwtConfig.GetValue<string>("Audience") ?? "localhost";
var secretKey = jwtConfig.GetValue<string>("SecretKey") ?? throw new ArgumentNullException("La clave JWT no está configurada.");
builder.WebHost.UseUrls("http://*:8104");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Definir el esquema de seguridad para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Habilitar la seguridad global para las operaciones protegidas
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// servicios 

builder.Services.AddDbContext<ApiDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() // Permitir cualquier origen
                   .AllowAnyMethod() // Permitir cualquier método (GET, POST, etc.)
                   .AllowAnyHeader(); // Permitir cualquier encabezado
        });
});
builder.Services.AddSingleton(new AuthService(issuer, audience, secretKey));
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var app = builder.Build();
app.Use(async (context, next) =>
{
    // Registrar la solicitud
    var requestPath = context.Request.Path;
    var requestMethod = context.Request.Method;
    var requestTime = DateTime.UtcNow;

    // Obtener el token de autorización del encabezado
    var authToken = context.Request.Headers["Authorization"].ToString();

    Console.WriteLine($"Solicitud: {requestMethod} {requestPath} a las {requestTime}");

    // Imprimir el token si está presente
    if (!string.IsNullOrEmpty(authToken))
    {
        Console.WriteLine($"Token de Autorización: {authToken}");
    }
    else
    {
        Console.WriteLine("No se encontró el token de autorización.");
    }
    await next.Invoke(); // Llama al siguiente middleware

    // Registrar la respuesta
    var responseTime = DateTime.UtcNow;
    var responseStatusCode = context.Response.StatusCode;

    Console.WriteLine($"Respuesta: {responseStatusCode} a las {responseTime}");
});
//docs  swager en producción /developer 
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
//app.UseMiddleware<JwtMiddleware>();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
       Path.Combine(builder.Environment.ContentRootPath, "files")),
    RequestPath = "/archivos"
});
app.UseAuthorization();
app.MapControllers();

app.Run();

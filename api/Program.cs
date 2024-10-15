

using api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ConnectionProduction");
var configuration = builder.Configuration;
var jwtConfig = configuration.GetSection("JWTAuthentication");
var issuer = jwtConfig.GetValue<string>("Issuer") ?? "localhost";
var audience = jwtConfig.GetValue<string>("Audience") ?? "localhost";
var secretKey = jwtConfig.GetValue<string>("SecretKey") ?? throw new ArgumentNullException("La clave JWT no está configurada.");
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
builder.WebHost.UseUrls("http://localhost:8104", "http://*:8104");
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
//docs  swager en producción /developer 
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
//app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

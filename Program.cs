using ApiSalvarVidas.Compartidos.Interfaces;
using ApiSalvarVidas.Data;
using ApiSalvarVidas.Models;
using ApiSalvarVidas.Repositories;
using ApiSalvarVidas.Repositories.Interfaces;
using ApiSalvarVidas.Services;
using ApiSalvaVidas.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// DATABASE
// ==========================
builder.Services.AddDbContext<PaginaparaSalvarVidasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================
// IDENTITY
// ==========================
builder.Services
    .AddIdentity<AspNetUser, AspNetRole>()
    .AddEntityFrameworkStores<PaginaparaSalvarVidasContext>()
    .AddDefaultTokenProviders();

// ==========================
// JWT
// ==========================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = builder.Configuration["Jwt:Issuer"],
        ValidAudience            = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// ==========================
// CORS
// ==========================
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ==========================
// REPOSITORIOS
// ==========================
builder.Services.AddScoped<IAnimalRepository,           AnimalRepository>();
builder.Services.AddScoped<IAdopcionRepository,         AdopcionRepository>();
builder.Services.AddScoped<IComunitarioRepository,      ComunitarioRepository>();
builder.Services.AddScoped<ITransitoRepository,         TransitoRepository>();
builder.Services.AddScoped<IPerdidoEncontradoRepository,PerdidoEncontradoRepository>();
builder.Services.AddScoped<IFamiliaRepository,          FamiliaRepository>();
builder.Services.AddScoped<IAuthRepository,             AuthRepository>();
builder.Services.AddScoped<ISolicitudRepository,        SolicitudRepository>();
builder.Services.AddScoped<CloudinaryRepository>();


// ==========================
// SERVICIOS
// ==========================
builder.Services.AddScoped<IAnimalService,           AnimalService>();
builder.Services.AddScoped<IAdopcionService,         AdopcionService>();
builder.Services.AddScoped<IComunitarioService,      ComunitarioService>();
builder.Services.AddScoped<ITransitoService,         TransitoService>();
builder.Services.AddScoped<IPerdidoEncontradoService,PerdidoEncontradoService>();
builder.Services.AddScoped<IFamiliaService,          FamiliaService>();
builder.Services.AddScoped<IAuthService,             AuthService>();
builder.Services.AddScoped<ISolicitudService,        SolicitudService>();
builder.Services.AddScoped<CloudinaryService>();


// ==========================
// CONTROLLERS + SWAGGER
// ==========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SalvarVidas API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name        = "Authorization",
        Type        = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme      = "Bearer",
        BearerFormat = "JWT",
        In          = Microsoft.OpenApi.Models.ParameterLocation.Header
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {{
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        Array.Empty<string>()
    }});
});
var app = builder.Build();

// ==========================
// SEMILLA  — ahora usa UserManager/RoleManager correctamente
// ==========================
using (var scope = app.Services.CreateScope())
{
    await Semilla.Inicializar(scope.ServiceProvider);
}

// ==========================
// PIPELINE
// ==========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

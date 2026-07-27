using Microsoft.EntityFrameworkCore;
using EcoSystem.Data;
using EcoSystem.Business.Interfaces;
using EcoSystem.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EcoSystem.Data.Models; // Reconocimiento de la clase JwtSettings

var builder = WebApplication.CreateBuilder(args);

// --- NUEVO: Leer y registrar la configuración de JWT como servicio inyectable ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configurar la conexión a PostgreSQL (Supabase)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SupabasePostgres")));

// --- NUEVO: Configurar autenticación JWT Bearer ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

// --- NUEVO: Habilitar Autorización como servicio ---
builder.Services.AddAuthorization();

builder.Services.AddScoped<IProductoService, ProductoService>();

// Habilitar el uso de controladores
builder.Services.AddControllers();

// Habilitar Swagger (Interfaz Gráfica)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline para usar Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- NUEVO: Habilitar los Middlewares de Seguridad en el Pipeline HTTP ---
// Es vital que vayan en este orden y antes de MapControllers
app.UseAuthentication();
app.UseAuthorization();

// Mapear los endpoints de los controladores
app.MapControllers();

app.Run();
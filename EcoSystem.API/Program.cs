using Microsoft.EntityFrameworkCore;
using EcoSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar la conexión a PostgreSQL (Supabase)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SupabasePostgres")));

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

// Mapear los endpoints de los controladores
app.MapControllers();

app.Run();
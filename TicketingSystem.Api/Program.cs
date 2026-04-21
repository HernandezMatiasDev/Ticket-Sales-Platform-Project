using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.Queries;
using TicketingSystem.Infrastructure.Persistence;
using TicketingSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Swagger/OpenAPI (Opcional pero recomendado para probar)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Contexto de Base de Datos
// Asegúrate de tener la conexión "DefaultConnection" en tu appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Registro de Repositorios e Interfaces
builder.Services.AddScoped<IEventRepository, EventRepository>();

// 4. Registro de tus Queries (CQRS)
builder.Services.AddScoped<GetEventsQuery>();
builder.Services.AddScoped<GetSeatMapQuery>();

// 5. Soporte para Controladores (Importante para que funcione tu EventsController)
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 6. Mapeo de Controladores
app.MapControllers();

app.Run();
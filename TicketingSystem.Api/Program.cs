using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.UseCases.Handlers;

// NOTA: El nombre del DbContext debe ser consistente. Usaremos "TicketingDbContext" como está en la mayoría de los repositorios.
// Asegúrate que la clase se llame así en el archivo TicketingSystem.Infrastructure/Persistence/TicketingDbContext.cs
// y que contenga los DbSet para todas las entidades (Events, Seats, Reservations, AuditLogs).
using TicketingSystem.Infrastructure.Persistence;
using TicketingSystem.Infrastructure.Repositories;
using TicketingSystem.Infrastructure.Workers;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Swagger/OpenAPI (Opcional pero recomendado para probar)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Contexto de Base de Datos
// Asegúrate de tener la conexión "DefaultConnection" en tu appsettings.json
builder.Services.AddDbContext<TicketingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Registro de Repositorios (Puertos y Adaptadores)
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>(); // NOTA: La implementación SeatRepository.cs no fue provista, pero debe ser creada y registrada.
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

// 4. Unidad de Trabajo (Unit of Work)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 5. Registro de Casos de Uso y Queries (Capa de Aplicación)
builder.Services.AddScoped<GetEventsQuery>();
builder.Services.AddScoped<GetSeatMapQuery>();
//builder.Services.AddScoped<ReserveSeatUseCase>();
builder.Services.AddScoped<ICommandHandler<CreateEventCommand, int>, CreateEventHandler>();


// 6. Registro de Servicios en Segundo Plano (Workers)
builder.Services.AddHostedService<ExpiredReservationWorker>();

// 7. Soporte para Controladores
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// 8. Mapeo de Controladores
app.MapControllers();

app.Run();
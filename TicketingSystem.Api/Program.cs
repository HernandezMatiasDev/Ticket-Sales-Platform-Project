using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.UseCases;
using TicketingSystem.Application.UseCases.Handlers;

// NOTA: El nombre del DbContext debe ser consistente. Usaremos "TicketingDbContext" como está en la mayoría de los repositorios.
// Asegúrate que la clase se llame así en el archivo TicketingSystem.Infrastructure/Persistence/TicketingDbContext.cs
// y que contenga los DbSet para todas las entidades (Events, Seats, Reservations, AuditLogs).
using TicketingSystem.Infrastructure.Persistence;
using TicketingSystem.Infrastructure.Repositories;
using TicketingSystem.Infrastructure.Workers;
using TicketingSystem.Infrastructure.Identity;
using TicketingSystem.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Swagger/OpenAPI (Opcional pero recomendado para probar)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Contexto de Base de Datos
// Asegúrate de tener la conexión "DefaultConnection" en tu appsettings.json
builder.Services.AddDbContext<TicketingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<TicketingDbContext>()
    .AddDefaultTokenProviders();

// Configuración de Autenticación JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "ClaveParaJWTSuperSecretaDeDesarrollo12345!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

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
builder.Services.AddScoped<ReserveSeatUseCase>();
builder.Services.AddScoped<ICommandHandler<CreateEventCommand, int>, CreateEventHandler>();
// 5. Registro de Casos de Uso y Queries
builder.Services.AddScoped<IQueryHandler<GetEventsQuery, IEnumerable<Event>>, GetEventsHandler>();
builder.Services.AddScoped<IQueryHandler<GetSeatMapQuery, IEnumerable<Seat>>, GetSeatMapHandler>();
builder.Services.AddScoped<IQueryHandler<GetEventSectorsQuery, IEnumerable<Sector>>, GetEventSectorsHandler>();


// Registro del Servicio de Autenticación
builder.Services.AddScoped<IAuthService, AuthService>();

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

// Middlewares de Seguridad (Orden importante: siempre AuthN antes de AuthZ y antes del Mapeo)
app.UseAuthentication();
app.UseAuthorization();

// 8. Mapeo de Controladores
app.MapControllers();

app.Run();
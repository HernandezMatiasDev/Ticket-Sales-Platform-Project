using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TicketingSystem.Infrastructure.Identity;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Infrastructure.Persistence.Configurations;
// NOTA: Para que el código de OnModelCreating funcione, debes crear las clases de configuración
// (ej. ReservationConfiguration.cs) en una carpeta como "Persistence/Configurations"
// y luego descomentar las líneas en OnModelCreating.

namespace TicketingSystem.Infrastructure.Persistence;

// Se cambia el nombre a TicketingDbContext para unificarlo en todo el proyecto.
public class TicketingDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options) { }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuraciones predeterminadas de IdentityDbContext ya cubiertas por el base.OnModelCreating

        // === PRECARGA DE DATOS (SEEDING) ===

        // 1. Crear el Evento con fecha fija
        var eventId = 1;
        modelBuilder.Entity<Event>().HasData(
            new { Id = eventId, Name = "Concierto de Rock", EventDate = new DateTime(2025, 12, 31, 20, 0, 0, DateTimeKind.Utc), Venue = "Estadio Municipal", Status = "Active" }
        );

        // 2. Crear los Sectores (VIP y General)
        var vipSectorId = 1;
        var generalSectorId = 2;
        
        modelBuilder.Entity<Sector>().HasData(
            new { Id = vipSectorId, EventId = eventId, Name = "VIP", Price = 5000m },
            new { Id = generalSectorId, EventId = eventId, Name = "General", Price = 2000m }
        );

        // 3. Crear 50 butacas para cada sector usando objetos anónimos y Guids deterministas
        var seats = new List<object>();
        for (int i = 1; i <= 50; i++)
        {
            // Guids que siempre serán los mismos en cada compilación gracias al formato
            seats.Add(new { Id = new Guid($"11111111-1111-1111-1111-{i:D12}"), Number = i, Row = "V", SectorId = vipSectorId, Status = SeatStatus.Available, Version = 1 });
            seats.Add(new { Id = new Guid($"22222222-2222-2222-2222-{i:D12}"), Number = i, Row = "G", SectorId = generalSectorId, Status = SeatStatus.Available, Version = 1 });
        }
        
        modelBuilder.Entity<Seat>().HasData(seats.ToArray());





        // El código que has proporcionado es la forma correcta de configurar las entidades
        // usando Fluent API en clases separadas.

        // Configuraciones de tu compañero
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new SectorConfiguration());
        modelBuilder.ApplyConfiguration(new SeatConfiguration()); // ¡AQUÍ ES DONDE ÉL PONE EL CONCURRENCY TOKEN!

        // Tus configuraciones
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
    }
}
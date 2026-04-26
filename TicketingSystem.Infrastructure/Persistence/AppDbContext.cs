using Microsoft.EntityFrameworkCore;
using TicketingSystem.Domain.Entities;
// NOTA: Para que el código de OnModelCreating funcione, debes crear las clases de configuración
// (ej. ReservationConfiguration.cs) en una carpeta como "Persistence/Configurations"
// y luego descomentar las líneas en OnModelCreating.

namespace TicketingSystem.Infrastructure.Persistence;

// Se cambia el nombre a TicketingDbContext para unificarlo en todo el proyecto.
public class TicketingDbContext : DbContext
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



// para crear 1 evento con 2 sectores
        // modelBuilder.Entity<Event>().HasData(new Event("Concierto de Rock", DateTime.Now.AddDays(30), "Estadio Municipal") 
        // { 
        //     Id = 1, 
        //     Status = "Active" 
        // });

        // // 2. Definir Sectores (2 sectores de 50 butacas cada uno)
        // modelBuilder.Entity<Sector>().HasData(
        //     new { Id = 1, EventId = 1, Name = "Vip", Price = 5000m, Capacity = 50 },
        //     new { Id = 2, EventId = 1, Name = "General", Price = 2000m, Capacity = 50 }
        // );





        // El código que has proporcionado es la forma correcta de configurar las entidades
        // usando Fluent API en clases separadas.

        // Configuraciones de tu compañero
        // modelBuilder.ApplyConfiguration(new EventConfiguration());
        // modelBuilder.ApplyConfiguration(new SectorConfiguration());
        // modelBuilder.ApplyConfiguration(new SeatConfiguration()); // ¡AQUÍ ES DONDE ÉL PONE EL CONCURRENCY TOKEN!

        // Tus configuraciones
        // modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        // modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
    }
}
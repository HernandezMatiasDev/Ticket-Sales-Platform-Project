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
    // Se añaden los DbSet que faltaban para Reservation y AuditLog
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
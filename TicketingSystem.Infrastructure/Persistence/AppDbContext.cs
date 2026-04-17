using Microsoft.EntityFrameworkCore;
using TicketingSystem.Domain.Entities;
using System.Reflection;

namespace TicketingSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<Seat> Seats => Set<Seat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Esto aplica automáticamente todas las configuraciones en la carpeta Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}


// {
//     public DbSet<Event> Events => Set<Event>();
//     public DbSet<Sector> Sectors => Set<Sector>();
//     public DbSet<Seat> Seats => Set<Seat>();

//     public AppDbContext(DbContextOptions<AppDbContext> options)
//         : base(options)
//     {
//     }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);

//         modelBuilder.Entity<Event>(entity =>
//         {
//             entity.HasKey(e => e.Id);

//             entity.Property(e => e.Name)
//                   .IsRequired()
//                   .HasMaxLength(200);

//             entity.HasMany(e => e.Sectors)
//                   .WithOne(s => s.Event)
//                   .HasForeignKey(s => s.EventId);
//         });

//         modelBuilder.Entity<Sector>(entity =>
//         {
//             entity.HasKey(s => s.Id);

//             entity.Property(s => s.Name)
//                   .IsRequired()
//                   .HasMaxLength(100);

//             entity.HasMany(s => s.Seats)
//                   .WithOne(se => se.Sector)
//                   .HasForeignKey(se => se.SectorId);
//         });

//         modelBuilder.Entity<Seat>(entity =>
//         {
//             entity.HasKey(s => s.Id);

//             entity.Property(s => s.Number)
//                   .IsRequired()
//                   .HasMaxLength(10);
//         });
//     }
// }
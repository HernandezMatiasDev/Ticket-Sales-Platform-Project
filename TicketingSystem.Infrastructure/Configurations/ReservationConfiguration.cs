using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            // Nombre de la tabla según tu diagrama
            builder.ToTable("RESERVATION");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .ValueGeneratedNever(); // El Guid se genera en el caso de uso

            builder.Property(r => r.UserId)
                   .IsRequired();

            builder.Property(r => r.SeatId)
                   .IsRequired();

            // Mapeo del Enum a string en la base de datos
            builder.Property(r => r.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(r => r.ReservedAt)
                   .IsRequired();

            builder.Property(r => r.ExpiresAt)
                   .IsRequired();
        }
    }
}
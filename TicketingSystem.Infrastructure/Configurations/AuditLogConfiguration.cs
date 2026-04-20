using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Infrastructure.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad AuditLog.
    /// Define cómo se mapea la clase del dominio a la tabla de la base de datos,
    /// estableciendo claves, longitudes máximas y obligatoriedad de campos.
    /// </summary>
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        /// <summary>
        /// Configura las propiedades de la entidad AuditLog usando el Fluent API.
        /// </summary>
        /// <param name="builder">El constructor de entidades de EF Core.</param>
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            // Nombre de la tabla según tu diagrama
            builder.ToTable("AUDIT_LOG");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedNever();

            // UserId es opcional (int?) porque puede ser una acción del sistema (Worker)
            builder.Property(a => a.UserId)
                   .IsRequired(false);

            builder.Property(a => a.Action)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(a => a.EntityType)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(a => a.EntityId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(a => a.Details)
                   .HasMaxLength(1000) 
                   .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                   .IsRequired();
        }
    }
}
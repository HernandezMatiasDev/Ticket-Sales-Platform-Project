using System.Threading.Tasks;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Infrastructure.Persistence;

namespace TicketingSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de auditoría usando Entity Framework Core.
    /// Se encarga de guardar los logs de éxito o error en las tablas correspondientes.
    /// </summary>
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly TicketingDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia del repositorio de auditoría.
        /// </summary>
        /// <param name="context">El contexto de base de datos inyectado por el contenedor de dependencias.</param>
        public AuditLogRepository(TicketingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega un log de auditoría al ChangeTracker de Entity Framework.
        /// Nota: Requiere llamar a SaveChanges() o Commit() en el UnitOfWork para impactar en la BD.
        /// </summary>
        /// <param name="auditLog">El registro de auditoría a guardar.</param>
        public async Task AddAsync(AuditLog auditLog)
        {
            await _context.Set<AuditLog>().AddAsync(auditLog);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TicketingSystem.Application.Ports;

namespace TicketingSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del patrón Unit of Work para manejar las transacciones 
    /// de Entity Framework Core de forma centralizada y agnóstica a los repositorios.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketingDbContext _context;
        private IDbContextTransaction _currentTransaction;

        /// <summary>
        /// Inicializa la Unidad de Trabajo con el DbContext del sistema.
        /// </summary>
        public UnitOfWork(TicketingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Inicia una nueva transacción en la base de datos si no existe una activa.
        /// Útil para agrupar múltiples operaciones de repositorios (ej. actualizar butaca, crear reserva y log).
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }
            // Inicia la transacción a nivel de base de datos
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Impacta todos los cambios en la base de datos y confirma (comitea) la transacción activa.
        /// Si ocurre un conflicto de concurrencia optimista, Entity Framework lanzará 
        /// una excepción (DbUpdateConcurrencyException) aquí.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            try
            {
                // Guarda los cambios primero (aquí salta el Optimistic Locking si hay conflicto)
                await _context.SaveChangesAsync();
                
                if (_currentTransaction != null)
                {
                    // Si todo fue bien, consolida la transacción
                    await _currentTransaction.CommitAsync();
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// Deshace todos los cambios realizados durante la transacción activa si ocurre algún error,
        /// dejando la base de datos en su estado original.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    // Revierte todo si hubo un fallo
                    await _currentTransaction.RollbackAsync();
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
                
                // Limpiamos la memoria de EF Core para descartar las entidades modificadas/agregadas que fallaron.
                // Esto permite ejecutar operaciones compensatorias (como logs) en un lienzo en blanco.
                _context.ChangeTracker.Clear();
            }
        }

        /// <summary>
        /// Guarda los cambios en el contexto sin requerir la confirmación de una 
        /// transacción explícita (útil para operaciones individuales rápidas).
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
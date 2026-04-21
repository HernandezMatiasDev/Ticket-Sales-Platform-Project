using System.Threading.Tasks;

namespace TicketingSystem.Application.Interfaces
{
    /// <summary>
    /// Puerto para implementar el patrón Unit of Work (Unidad de Trabajo).
    /// Garantiza la atomicidad de las operaciones: o se guardan todos los cambios
    /// de los distintos repositorios juntos, o no se guarda ninguno (Rollback).
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Inicia una nueva transacción en la base de datos.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual, impactando todos los cambios de los repositorios 
        /// en la base de datos de manera atómica.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Deshace todos los cambios realizados en memoria y en la transacción actual,
        /// revirtiendo la base de datos a su estado anterior.
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Persiste los cambios actuales en el contexto sin necesidad de manejar 
        /// explícitamente una transacción. Útil para operaciones simples o compensatorias.
        /// </summary>
        Task SaveChangesAsync();
    }
}

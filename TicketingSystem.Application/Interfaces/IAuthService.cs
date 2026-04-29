using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketingSystem.Application.Interfaces;

public interface IAuthService
{
    // Retorna el JWT Token como string si es exitoso
    Task<string> LoginAsync(string email, string password);
    
    Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(string email, string password);
}
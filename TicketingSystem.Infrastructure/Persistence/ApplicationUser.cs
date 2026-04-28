using Microsoft.AspNetCore.Identity;

namespace TicketingSystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    // Aquí puedes agregar propiedades extendidas en el futuro si lo necesitas (ej: FirstName, LastName)
}
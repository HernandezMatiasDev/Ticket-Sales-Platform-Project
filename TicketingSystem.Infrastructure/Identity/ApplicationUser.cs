using Microsoft.AspNetCore.Identity;

namespace TicketingSystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    // Datos personales
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    
    
    // Auditoría básica
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
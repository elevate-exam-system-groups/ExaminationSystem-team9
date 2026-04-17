using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Entities.Authentication;

public class ApplicationRole : IdentityRole<Guid> 
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; } = false;
}

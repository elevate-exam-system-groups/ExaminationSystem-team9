namespace ExaminationSystem.Domain.Entities.Authentication;

public class PasswordResetToken : BaseEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }

    public ApplicationUser User { get; set; } = default!;
}

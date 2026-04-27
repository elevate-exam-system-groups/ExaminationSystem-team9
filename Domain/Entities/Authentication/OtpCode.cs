namespace ExaminationSystem.Domain.Entities.Authentication;

public class OtpCode : BaseEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = default!;
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; }
    public int AttemptCount { get; set; } = 0;
    public int ResendCount { get; set; } = 0;

    public ApplicationUser User { get; set; } = default!;
}

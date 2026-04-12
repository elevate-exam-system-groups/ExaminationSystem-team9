using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class QuizAttempt : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid QuizId { get; set; }
    public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;
    public DateTime StartTime { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public decimal? Score { get; set; }
    public bool? Passed { get; set; }

    public ApplicationUser Student { get; set; } = default!; 
    public Quiz Quiz { get; set; } = default!;
    public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();
}

using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class Quiz : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DiplomaId { get; set; }
    public string Title { get; set; } = default!;
    public string? Instructions { get; set; }
    public int DurationMinutes { get; set; }
    public double PassScore { get; set; } = 60.00;
    public int? MaxAttempts { get; set; }
    public QuizStatus Status { get; set; } = QuizStatus.Draft;
    public DateTime? PublishedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Diploma Diploma { get; set; } = default!;
    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
}

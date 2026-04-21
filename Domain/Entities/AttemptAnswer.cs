namespace ExaminationSystem.Domain.Entities;

public class AttemptAnswer : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuizAttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public bool? IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
    public QuizAttempt QuizAttempt { get; set; } = default!;
    public Question Question { get; set; } = default!;
    public Option? SelectedOption { get; set; }
}

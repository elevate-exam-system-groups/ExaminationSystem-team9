namespace ExaminationSystem.Domain.Entities;

public class Option : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuestionId { get; set; }
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }
    public Question Question { get; set; } = default!;
    public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = []; 
}

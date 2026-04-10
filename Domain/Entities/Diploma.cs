using ExaminationSystem.Domain.Entities.Common;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class Diploma : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DiplomaStatus Status { get; set; } = DiplomaStatus.Draft;
    public DateTime? DeletedAt { get; set; }
    public ICollection<Quiz> Quizzes { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}

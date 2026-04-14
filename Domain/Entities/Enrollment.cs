using ExaminationSystem.Domain.Entities.Authentication;

namespace ExaminationSystem.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid DiplomaId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public ApplicationUser Student { get; set; } = default!;
    public Diploma Diploma { get; set; } = default!;
}

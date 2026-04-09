using System;

public class Enrollments
{
    public class Enrollment
    {
        public int Id { get; set; }

        public Guid StudentId { get; set; }

        public Guid DiplomaId { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;


        
        public User Student { get; set; } = null!;

        public Diploma Diploma { get; set; } = null!;
    }
}

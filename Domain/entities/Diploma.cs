using System;

public class Diploma
{
    public class Diploma
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Status { get; set; } = "draft";

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Quiz> Quizzes { get; set; }
    }
}

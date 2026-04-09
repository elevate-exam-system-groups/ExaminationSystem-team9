using System;

public class Quiz
{
    public class Quiz
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid DiplomaId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Instructions { get; set; }

        public int DurationMinutes { get; set; }

        public decimal PassScore { get; set; } = 60.0m;

        public int? MaxAttempts { get; set; }

        public QuizStatus Status { get; set; } = QuizStatus.Draft;

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted { get; set; } = false;


        
        public Diploma Diploma { get; set; } = null!;
    }
}

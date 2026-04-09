using System;

public class QuizAttempt
{
    public class QuizAttempt
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public Guid QuizId { get; set; }

        public string Status { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public decimal? Score { get; set; }
        public bool? Passed { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public User Student { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<AttemptAnswer> Answers { get; set; }
    }
}

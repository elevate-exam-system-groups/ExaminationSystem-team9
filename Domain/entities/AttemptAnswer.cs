using System;

public class AttemptAnswer
{
    public class AttemptAnswer
    {
        public int Id { get; set; }

        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }

        public bool? IsCorrect { get; set; }

        public DateTime AnsweredAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public QuizAttempt Attempt { get; set; }
        public Question Question { get; set; }
        public Option SelectedOption { get; set; }
    }
}
